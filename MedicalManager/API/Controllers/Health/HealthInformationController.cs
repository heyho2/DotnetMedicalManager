using GD.Common;
using GD.Dtos.Enum;
using GD.Dtos.Health;
using GD.Manager.Consumer;
using GD.Manager.Health;
using GD.Manager.Utility;
using GD.Models.CommonEnum;
using GD.Models.Consumer;
using GD.Models.Health;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GD.API.Controllers.Health
{
    /// <summary>
    /// 健康基础信息控制器
    /// </summary>
    public class HealthInformationController : HealthBaseController
    {
        /// <summary>
        /// 获取会员基础信息
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetUserHealthInformationResponseDto>))]
        public async Task<IActionResult> GetHealthInformationAsync(string userGuid)
        {
            if (string.IsNullOrEmpty(userGuid))
            {
                return Failed(ErrorCode.Empty, "请指定会员");
            }

            var userBiz = new UserBiz();
            var model = await userBiz.GetAsync(userGuid);
            if (model is null || !model.Enable)
            {
                return Failed(ErrorCode.Empty, "指定会员不存在");
            }

            var informationBiz = new HealthInformationBiz();
            var informations = await informationBiz.GetHealthInformationList(userGuid);
            GetUserHealthInformationResponseDto result = new GetUserHealthInformationResponseDto
            {
                UserName = model.UserName,
                Birthday = model.Birthday,
                Gender = model.Gender,
                IdentityNumber = model.IdentityNumber,
                HealthInformationList = informations
            };
            return Success(result);
        }

        /// <summary>
        /// 健康基础数据更新
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> UpdateHealthInformationAsync([FromBody]UpdateHealthInformationRequestDto request)
        {
            var userBiz = new UserBiz();
            var model = await userBiz.GetAsync(request.UserGuid);
            if (model is null || !model.Enable)
            {
                return Failed(ErrorCode.Empty, "指定会员不存在，请检查");
            }

            if (request.Items.Count == 0)
            {
                return Failed(ErrorCode.Empty, "会员基础信息为空，请检查");
            }

            var informationBiz = new HealthInformationBiz();

            var consumerHealthInfoBiz = new ConsumerHealthInfoBiz();

            var insertModels = new List<ConsumerHealthInfoModel>();
            var updateModels = new List<ConsumerHealthInfoModel>();

            var filterHealthInfos = new List<string>();

            foreach (var item in request.Items)
            {
                if (string.IsNullOrEmpty(item.InformationGuid))
                {
                    return Failed(ErrorCode.Empty, "健康基础信息项数据为空，请检查");
                }

                if (filterHealthInfos.Contains(item.InformationGuid))
                {
                    return Failed(ErrorCode.Empty, "健康基础信息存在重复项，请检查");
                }

                var healthInfoModel = await informationBiz.GetAsync(item.InformationGuid);

                if (healthInfoModel == null)
                {
                    return Failed(ErrorCode.DataBaseError, "健康基础信息不存在，请检查");
                }

                filterHealthInfos.Add(healthInfoModel.InformationGuid);

                var consumerHealthInfoModel = new ConsumerHealthInfoModel()
                {
                    InfoRecordGuid = Guid.NewGuid().ToString("N"),
                    InformationGuid = item.InformationGuid,
                    UserGuid = request.UserGuid,
                    CreatedBy = UserID,
                    LastUpdatedBy = UserID,
                    ResultValue = item.ResultValue
                };

                var selectedOptionGuids = item.OptionGuids;

                if (healthInfoModel.InformationType == HealthInformationEnum.Decimal.ToString())
                {
                    if (!string.IsNullOrEmpty(item.ResultValue))
                    {
                        if (!decimal.TryParse(item.ResultValue, out var value))
                        {
                            return Failed(ErrorCode.Empty, "数据类型不正确");
                        }
                    }
                }
                else if (healthInfoModel.InformationType == HealthInformationEnum.Enum.ToString()
                    || healthInfoModel.InformationType == HealthInformationEnum.Array.ToString())
                {
                    if (selectedOptionGuids.Count <= 0)
                    {
                        continue;
                    }

                    if (healthInfoModel.InformationType == HealthInformationEnum.Enum.ToString())
                    {
                        if (selectedOptionGuids.Count > 1)
                        {
                            return Failed(ErrorCode.Empty, $"题目【{healthInfoModel.SubjectName}】答案存在多个");
                        }
                    }

                    var options = await informationBiz.GetHealthInformationOptionAsync(item.InformationGuid);

                    var infoResult = selectedOptionGuids.Except(options.Select(s => s.OptionGuid));

                    if (infoResult != null && infoResult.Count() > 0)
                    {
                        return Failed(ErrorCode.Empty, $"题目【{healthInfoModel.SubjectName}】答案选项不存在");
                    }

                    consumerHealthInfoModel.OptionGuids = JsonConvert.SerializeObject(selectedOptionGuids);
                    consumerHealthInfoModel.ResultValue = null;

                }

                //用户没有填写任何信息，既不更新也不添加
                if (string.IsNullOrEmpty(consumerHealthInfoModel.OptionGuids) &&
                    string.IsNullOrEmpty(consumerHealthInfoModel.ResultValue?.Trim()))
                {
                    continue;
                }

                var consumerHealthInfo = await consumerHealthInfoBiz.GetConsumerHealthInfoAsync(item.InformationGuid, request.UserGuid);

                if (consumerHealthInfo != null)
                {
                    consumerHealthInfoModel.InfoRecordGuid = consumerHealthInfo.InfoRecordGuid;
                    consumerHealthInfoModel.InformationGuid = consumerHealthInfo.InformationGuid;
                    consumerHealthInfoModel.LastUpdatedBy = UserID;
                    consumerHealthInfoModel.LastUpdatedDate = DateTime.Now;

                    updateModels.Add(consumerHealthInfoModel);
                }
                else
                {
                    insertModels.Add(consumerHealthInfoModel);
                }
            }

            var result = await consumerHealthInfoBiz.CreateOrUpdateConsumerHealthInfo(insertModels, updateModels);

            return result ? Success() : Failed(ErrorCode.DataBaseError, "更新健康基础信息失败");
        }

        /// <summary>
        /// 保存健康基础信息配置项数据
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> SaveHealthInformationAsync([FromBody]SaveHealthInformationRequestDto requestDto)
        {
            var context = new SaveHealthInformationContext(requestDto);

            (var checkResult, var response) = CheckSaveHealthInformation(context);
            if (!checkResult)
            {
                return response;
            }

            int infoIndex = 0;
            int optionIndex = 0;
            context.RequestDto.Infos.ForEach(a =>
            {
                a.Sort = ++infoIndex;
                optionIndex = 0;
                if (a.Options == null)
                {
                    a.Options = new List<SaveHealthInformationRequestDto.HealthInfoOption>();
                }

                a.Options.ForEach(b =>
                {
                    b.Sort = ++optionIndex;
                });

            });

            context.AllInfos = (await new HealthInformationBiz().GetListAsync(true)).ToList();
            context.AllOptions = (await new HealthInformationOptionBiz().GetListAsync(true)).ToList();
            GenerateHealthInfoData(context);
            var result = await new HealthInformationBiz().SaveHealthInformationAsync(context);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "保存健康基础信息失败");
        }
        /// <summary>
        /// 组织待保存的健康信息数据
        /// </summary>
        /// <param name="context"></param>
        private void GenerateHealthInfoData(SaveHealthInformationContext context)
        {
            foreach (var item in context.RequestDto.Infos)
            {
                if (item.Options == null)
                {
                    item.Options = new List<SaveHealthInformationRequestDto.HealthInfoOption>();
                }
                var infoModel = new HealthInformationModel
                {
                    InformationType = item.InformationType.ToString(),
                    Sort = item.Sort,
                    SubjectName = item.SubjectName,
                    SubjectUnit = item.SubjectUnit,
                    SubjectPromptText = item.SubjectPromptText,
                    IsSingleLine = item.IsSingleLine
                };
                //选择题不需要单位、提示语、是否单行属性
                if (item.InformationType == HealthInformationTypeEnum.Enum
                    || item.InformationType == HealthInformationTypeEnum.Bool
                    || item.InformationType == HealthInformationTypeEnum.Array)
                {
                    infoModel.SubjectUnit = string.Empty;
                    infoModel.SubjectPromptText = string.Empty;
                    infoModel.IsSingleLine = false;
                }
                else if (item.InformationType == HealthInformationTypeEnum.String)
                {
                    infoModel.SubjectUnit = string.Empty;
                }
                //新增
                if (string.IsNullOrWhiteSpace(item.InformationGuid))
                {
                    //新增信息
                    infoModel.InformationGuid = Guid.NewGuid().ToString("N");
                    infoModel.CreatedBy = UserID;
                    infoModel.LastUpdatedBy = UserID;
                    context.AddInfos.Add(infoModel);
                    //新增信息选择项
                    context.AddOptions.AddRange(item.Options.Select(a => new HealthInformationOptionModel
                    {
                        OptionGuid = Guid.NewGuid().ToString("N"),
                        InformationGuid = infoModel.InformationGuid,
                        OptionLabel = a.OptionLabel,
                        IsDefault = a.IsDefault,
                        Sort = a.Sort,
                        CreatedBy = UserID,
                        LastUpdatedBy = UserID
                    }));
                }
                else//编辑
                {
                    var sourcesInfoModel = context.AllInfos.FirstOrDefault(a => a.InformationGuid == item.InformationGuid);
                    infoModel.InformationGuid = item.InformationGuid;
                    infoModel.CreatedBy = sourcesInfoModel.CreatedBy;
                    infoModel.CreationDate = sourcesInfoModel.CreationDate;
                    infoModel.LastUpdatedBy = UserID;
                    infoModel.LastUpdatedDate = DateTime.Now;
                    context.UpdateInfos.Add(infoModel);
                    var sourceOptions = context.AllOptions.Where(a => a.InformationGuid == item.InformationGuid);//信息原有的选择项列表
                    if (item.Options != null && item.Options.Any())
                    {
                        //信息编辑时新增的选项
                        context.AddOptions.AddRange(item.Options.Where(a => string.IsNullOrWhiteSpace(a.OptionGuid)).Select(a => new HealthInformationOptionModel
                        {
                            OptionGuid = Guid.NewGuid().ToString("N"),
                            InformationGuid = infoModel.InformationGuid,
                            OptionLabel = a.OptionLabel,
                            IsDefault = a.IsDefault,
                            Sort = a.Sort,
                            CreatedBy = UserID,
                            LastUpdatedBy = UserID
                        }));
                        //编辑时删除的选项
                        var deleteOptionModels = sourceOptions.GroupJoin(item.Options.Where(a => !string.IsNullOrWhiteSpace(a.OptionGuid)), l => l.OptionGuid, r => r.OptionGuid,
                            (l, gs) => new
                            {
                                left = l,
                                right = gs.FirstOrDefault()//一对一的关系，所以只会存在一条
                            }).Where(a => a.right == null)
                            .Select(a => a.left);
                        context.DeleteOptions.AddRange(deleteOptionModels);
                        //信息编辑时编辑的选项
                        var updateOptionModels = sourceOptions.Join(item.Options.Where(a => !string.IsNullOrWhiteSpace(a.OptionGuid)), a => a.OptionGuid, b => b.OptionGuid,
                            (a, b) => new HealthInformationOptionModel
                            {
                                OptionGuid = a.OptionGuid,
                                InformationGuid = a.InformationGuid,
                                OptionLabel = b.OptionLabel,
                                IsDefault = b.IsDefault,
                                Sort = b.Sort,
                                CreatedBy = a.CreatedBy,
                                CreationDate = a.CreationDate,
                                LastUpdatedBy = UserID,
                                LastUpdatedDate = DateTime.Now,
                                OrgGuid = a.OrgGuid
                            });
                        context.UpdateOptions.AddRange(updateOptionModels);

                    }
                    else
                    {
                        //编辑时删除的选项
                        //无选项列表，有可能是从选择题并未非选择题，需要删除原来的选项列表
                        context.DeleteOptions.AddRange(sourceOptions);
                    }
                }
            }
            //已删除的信息
            var deleteInfoModels = context.AllInfos.GroupJoin(context.RequestDto.Infos, l => l.InformationGuid, r => r.InformationGuid,
                (l, temp) => new
                {
                    left = l,
                    right = temp.FirstOrDefault()
                }).Where(a => a.right == null)
                .Select(a => a.left);
            context.DeleteInfos.AddRange(deleteInfoModels);
            //已删除的信息下的选择项
            context.DeleteOptions.AddRange(context.AllOptions.Join(deleteInfoModels, a => a.InformationGuid, b => b.InformationGuid, (a, b) => a));
        }

        /// <summary>
        /// 保存健康基础信息前的数据验证
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private (bool, ResponseDto) CheckSaveHealthInformation(SaveHealthInformationContext context)
        {
            if (context.RequestDto.Infos == null || !context.RequestDto.Infos.Any())
            {
                return (false, Failed(ErrorCode.UserData, "请传入至少一条健康信息"));
            }
            var checkInfoOptionRes = context.RequestDto.Infos.All(info =>
            {
                if (info.InformationType == HealthInformationTypeEnum.Decimal || info.InformationType == HealthInformationTypeEnum.String)
                {
                    return true;
                }
                if (info.Options == null || !info.Options.Any())
                {
                    return false;
                }
                return true;
            });
            if (!checkInfoOptionRes)
            {
                return (false, Failed(ErrorCode.UserData, "选择题和判断题需要有选项"));
            }
            return (true, null);
        }

        /// <summary>
        /// 获取健康基础信息列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Produces(typeof(ResponseDto<List<GetHealthInfoBasicDataResponseDto>>))]
        public async Task<IActionResult> GetHealthInfoBasicDataAsync()
        {
            var infos = await new HealthInformationBiz().GetListAsync(true);
            var options = await new HealthInformationOptionBiz().GetListAsync(true);
            var result = infos.Select(info => new GetHealthInfoBasicDataResponseDto
            {
                InformationGuid = info.InformationGuid,
                InformationType = Enum.Parse<HealthInformationTypeEnum>(info.InformationType),
                SubjectName = info.SubjectName,
                SubjectUnit = info.SubjectUnit,
                SubjectPromptText = info.SubjectPromptText,
                IsSingleLine = info.IsSingleLine,
                Sort = info.Sort,
                Options = options.Where(option => option.InformationGuid == info.InformationGuid).Select(option => new GetHealthInfoBasicDataResponseDto.HealthInfoOption
                {
                    OptionGuid = option.OptionGuid,
                    Sort = option.Sort,
                    OptionLabel = option.OptionLabel,
                    IsDefault = option.IsDefault
                }).OrderBy(b => b.Sort).ToList()
            }).OrderBy(a => a.Sort).ToList();
            return Success(result);
        }

        /// <summary>
        /// 改变问题序号
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Produces(typeof(ResponseDto))]
        public async Task<IActionResult> ChangeInfomationSortAsync([FromBody]ChangeInfomationSortRequestDto requestDto)
        {
            var biz = new HealthInformationBiz();
            var model = await biz.GetAsync(requestDto.InformationGuid);
            if (model == null)
            {
                return Failed(ErrorCode.UserData, "未找到此健康信息");
            }
            var newSort = requestDto.Sort;
            if (model.Sort == newSort)
            {
                return Failed(ErrorCode.UserData, "健康信息序号未产生变化，请核对");
            }

            // 一.其他健康信息为待变化健康信息顺次移动位置
            //  1.若向前移动，则原序号和新序号之间所有健康信息序号+1 (不包括待变化健康信息)
            //  2.若向后移动，则原序号和新序号之间所有健康信息序号-1 (不包括待变化健康信息)
            // 二.将待变化健康信息序号变为新序号
            var result = await biz.ChangeSortAsync(model, newSort, UserID);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "健康信息变化序号失败");
        }
    }
}
