using GD.Common;
using GD.Common.Base;
using GD.Common.Helper;
using GD.Dtos.Health;
using GD.Manager.Consumer;
using GD.Manager.Health;
using GD.Manager.Utility;
using GD.Models.Consumer;
using GD.Models.Health;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GD.API.Controllers.Health
{
    /// <summary>
    /// 日常健康指标控制器
    /// </summary>
    public class HealthIndicatorController : BaseController
    {
        #region 会员日常指标
        /// <summary>
        /// 获取用户日常指标列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetHealthIndicatorResponseDto>))]
        public async Task<IActionResult> GetHealthIndicatorList(string userGuid)
        {
            if (string.IsNullOrEmpty(userGuid))
            {
                return Failed(ErrorCode.Empty, "会员参数为空");
            }

            var health = new HealthIndicatorBiz();

            return Success(await health.GetHealthIndicatorList(userGuid));
        }

        /// <summary>
        /// 获取健康指标项数据
        /// </summary>
        /// <param name="indicatorGuid">健康指标id</param>
        /// <param name="userGuid">用户Guid</param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<ConsumerHealthIndicatorOption>>))]
        public async Task<IActionResult> GetHealthIndicatorOptions(string indicatorGuid,
            string userGuid)
        {
            if (string.IsNullOrEmpty(userGuid))
            {
                return Failed(ErrorCode.Empty, "会员参数为空");
            }

            if (string.IsNullOrEmpty(indicatorGuid))
            {
                return Failed(ErrorCode.Empty, "指标id不能为空");
            }

            var health = new HealthIndicatorBiz();

            var options = await health.GetHealthIndicatorOptionList(indicatorGuid, userGuid);

            return Success(options);
        }

        /// <summary>
        /// 获取健康指标项详情数据
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetHealthIndicatorRecordResponseDto>))]
        public async Task<IActionResult> GetHealthIndicatorDetails([FromQuery]GetHealthIndicatorDetailRequestDto requestDto)
        {
            var resultResponse = new GetHealthIndicatorRecordResponseDto();

            var result = await new HealthIndicatorBiz().GetHealthIndicatorDetailList(requestDto);

            if (result is null || result.Count <= 0)
            {
                return Success(resultResponse);
            }

            resultResponse.DateList = result.Select(a => a.CreationDate)
                .Distinct().OrderBy(a => a).ToList();

            var resultGroup = result.GroupBy(o => o.IndicatorOptionGuid).Select(d => new GetHealthIndicatorDetailResponseDto
            {
                OptionGuid = d.Key,
                OptionName = d.FirstOrDefault().OptionName,
                OptionUnit = d.FirstOrDefault().OptionUnit,
                MinValue = d.FirstOrDefault().MinValue,
                MaxValue = d.FirstOrDefault().MaxValue,
                OptionList = d.ToList().Select(s => new KeyValuePair<DateTime, decimal?>(s.CreationDate, s.IndicatorValue)).OrderBy(a => a.Key).ToList()
            }).ToList();
            //查询个人用户的范围值
            var indicatorList = await new IndicatorWarningLimitBiz().GetModelAsyncByUser(requestDto.UserGuid);
            if (indicatorList != null && indicatorList.Count > 0)
            {
                foreach (var item in resultGroup)
                {
                    var indicatorModel = indicatorList.FirstOrDefault(s => s.IndicatorOptionGuid == item.OptionGuid);
                    if (indicatorModel != null)
                    {
                        item.MaxValue = indicatorModel.MaxValue;
                        item.MinValue = indicatorModel.MinValue;
                    }
                }
            }
            resultResponse.TotalCount = await new HealthIndicatorBiz().GetHealthIndicatorDetailCount(requestDto.UserGuid, requestDto);
            resultResponse.DetailList = resultGroup;

            return Success(resultResponse);
        }

        /// <summary>
        /// 添加会员健康指标
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<bool>))]
        public async Task<IActionResult> CreateConsumerHealthIndicatorOptions([FromBody]
         CreateHealthIndicatorRequestDto request)
        {
            if (request is null)
            {
                return Failed(ErrorCode.Empty, "参数为空，请检查！");
            }

            var options = request.Options;
            if (options.Count == 0)
            {
                return Failed(ErrorCode.Empty, "对应健康指标项为空，请检查！");
            }

            var userBiz = new UserBiz();
            var model = await userBiz.GetAsync(request.UserGuid);
            if (model is null || !model.Enable)
            {
                return Failed(ErrorCode.Empty, "指定会员不存在");
            }

            var healthInDicatorBiz = new HealthIndicatorBiz();

            var healthIndicator = await healthInDicatorBiz.GetAsync(request.IndicatorGuid);

            if (healthIndicator == null)
            {
                return Failed(ErrorCode.Empty, "对应健康指标未找到，参数错误！");
            }

            if (!healthIndicator.IndicatorType && request.Options.All(d => !d.IndicatorValue.HasValue))
            {
                return Failed(ErrorCode.Empty, "单指标必须提供值，请检查！");
            }

            var groupOptions = options.GroupBy(s => s.OptionGuid);

            foreach (var groupOption in groupOptions.Select((x, i) => new { Index = i + 1, Value = x }))
            {
                if (groupOption.Value.Count() > 1)
                {
                    return Failed(ErrorCode.Empty, $"第【{groupOption.Index}】个指标项有重复，请检查!");
                }
            }

            var optionGuids = options.Select(s => s.OptionGuid).Distinct();

            //判断是否存在必填的选项
            var healthIndicatorOptions = await healthInDicatorBiz.GetHealthIndicatorOptionAsync(request.IndicatorGuid);

            if (healthIndicatorOptions is null || healthIndicatorOptions.Count <= 0)
            {
                return Failed(ErrorCode.DataBaseError, "健康指标选项不存在");
            }

            var dbOptionGuids = healthIndicatorOptions.Select(s => s.OptionGuid);

            //全局匹配差集
            var infoCheckResult = optionGuids.Except(dbOptionGuids).ToList();

            if (infoCheckResult != null && infoCheckResult.Count() > 0)
            {
                return Failed(ErrorCode.DataBaseError, "提交有不存在的健康指标项，请检查！");
            }

            //必须有的选项值
            var dbRequiredOptionGuids = healthIndicatorOptions.Where(s => s.Required)
                .Select(s => s.OptionGuid);

            //差集
            var infoResult = dbRequiredOptionGuids.Except(optionGuids).ToList();

            if (infoResult != null && infoResult.Count > 0)
            {
                return Failed(ErrorCode.Empty, "健康指标选项必填项不能为空");
            }

            var recordGuid = Guid.NewGuid().ToString("N");

            var indicatorModel = new ConsumerIndicatorModel()
            {
                IndicatorRecordGuid = recordGuid,
                IndicatorGuid = request.IndicatorGuid,
                UserGuid = request.UserGuid,
                CreatedBy = UserID,
                LastUpdatedBy = UserID
            };

            var indicatorDetailModels = request.Options.Select(s => new ConsumerIndicatorDetailModel
            {
                RecordDetailGuid = Guid.NewGuid().ToString("N"),
                IndicatorRecordGuid = recordGuid,
                IndicatorOptionGuid = s.OptionGuid,
                IndicatorValue = s.IndicatorValue,
                CreatedBy = UserID,
                CreationDate = DateTime.Now,
                LastUpdatedDate = DateTime.Now
            }).ToList();

            var result = await healthInDicatorBiz.CreateConsumerHealthIndicatorAsync(indicatorModel, indicatorDetailModels);

            return result ? Success() : Failed(ErrorCode.DataBaseError, "创建健康指标失败");
        }

        /// <summary>
        /// 获取指标预警值
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetIndicatorWarningLimitResponseDto>))]
        public async Task<IActionResult> GetIndicatorWarningLimit([FromQuery] GetIndicatorWarningLimitRequestDto request)
        {
            var warningLimitBiz = new IndicatorWarningLimitBiz();

            var limits = await warningLimitBiz.GetIndicatorWarningLimits(request);

            if (limits.Count <= 0)
            {
                return Failed(ErrorCode.Empty, "指标不存在");
            }

            var response = new GetIndicatorWarningLimitResponseDto()
            {
                TurnOnWarning = limits.FirstOrDefault().TurnOnWarning,
                Limits = limits
            };

            return Success(response);
        }

        /// <summary>
        /// 保存指标预警值
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> SaveIndicatorWarningLimit([FromBody] CreateIndicatorWarningLimitRequestDto request)
        {
            if (request.Limits.Count <= 0)
            {
                return Failed(ErrorCode.Empty, "指标项参数未提供，请检查");
            }

            if (request.Limits.Any(d => string.IsNullOrEmpty(d.OptionGuid)))
            {
                return Failed(ErrorCode.Empty, "存在指标项为空的参数，请检查");
            }

            var duplicateOptionExists = request.Limits
                .GroupBy(d => d.OptionGuid.Trim())
                .Any(g => g.Count() > 1);

            if (duplicateOptionExists)
            {
                return Failed(ErrorCode.Empty, "存在重复指标项的参数，请检查");
            }
            if (request.Limits.Any(d => !d.MinValue.HasValue || !d.MaxValue.HasValue))
            {
                return Failed(ErrorCode.Empty, "存在预警范围为空的参数，请检查");
            }
            else
            {

                if (request.Limits.Any(d => d.MaxValue.Value < d.MinValue.Value))
                {
                    return Failed(ErrorCode.Empty, "存在预警低值大于预警高值的参数，请检查");
                }
            }
            var consumerBiz = new ConsumerBiz();
            var consumer = await consumerBiz.GetAsync(request.ConsumerGuid);
            if (consumer is null || !consumer.Enable)
            {
                return Failed(ErrorCode.Empty, "指定会员不存在");
            }

            var warningLimiBiz = new IndicatorWarningLimitBiz();
            var dbLimits = await warningLimiBiz.GetLimits(request.ConsumerGuid);

            var createModels = new List<IndicatorWarningLimitModel>();
            var updateModels = new List<IndicatorWarningLimitModel>();

            if (dbLimits is null || dbLimits.Count <= 0)
            {
                createModels = request.Limits.Select(d => new IndicatorWarningLimitModel()
                {
                    LimitGuid = Guid.NewGuid().ToString(),
                    UserGuid = request.ConsumerGuid,
                    TurnOnWarning = request.TurnOnWarning,
                    IndicatorOptionGuid = d.OptionGuid,
                    MinValue = d.MinValue,
                    MaxValue = d.MaxValue,
                    CreatedBy = UserID,
                    LastUpdatedBy = UserID,
                    OrgGuid = ""
                }).ToList();
            }
            else
            {
                foreach (var limit in request.Limits)
                {
                    var dblimit = dbLimits.FirstOrDefault(d => d.IndicatorOptionGuid.Equals(limit.OptionGuid));

                    var model = new IndicatorWarningLimitModel()
                    {
                        UserGuid = request.ConsumerGuid,
                        TurnOnWarning = request.TurnOnWarning,
                        IndicatorOptionGuid = limit.OptionGuid,
                        MinValue = limit.MinValue,
                        MaxValue = limit.MaxValue,
                        LastUpdatedBy = UserID
                    };

                    if (dblimit != null)
                    {
                        model.LimitGuid = dblimit.LimitGuid;

                        updateModels.Add(model);
                    }
                    else
                    {
                        model.LimitGuid = Guid.NewGuid().ToString("N");
                        model.CreatedBy = UserID;
                        model.OrgGuid = "";

                        createModels.Add(model);
                    }
                }
            }

            var result = await warningLimiBiz.CreateOrUpdateLimits(createModels, updateModels);

            return result ? Success() : Failed(ErrorCode.Empty, "保存预警设置失败，请稍后重试");
        }

        /// <summary>
        /// 更新医生建议
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<bool>))]
        public async Task<IActionResult> UpdateConsumerHealthIndicatorSuggestion([FromBody]
        UpdateHealthIndicatorSuggestion request)
        {
            var userBiz = new UserBiz();
            var model = await userBiz.GetAsync(request.UserGuid);
            if (model is null || !model.Enable)
            {
                return Failed(ErrorCode.Empty, "指定会员不存在");
            }

            var suggestionModel = new ConsumerIndicatorSuggestionModel()
            {
                IndicatorSuggestionGHuid = Guid.NewGuid().ToString("N"),
                Suggestion = request.Suggestion,
                UserGuid = request.UserGuid,
                CreatedBy = UserID,
                LastUpdatedBy = UserID
            };

            var result = await new HealthIndicatorBiz().UpdateConsumerHealthIndicatorSuggestion(suggestionModel);

            if (result)
            {
                new HealthRabbitMQNotificationBiz().HealthRabbitMQNotification(new HealthMessageDto()
                {
                    HealthType = HealthMessageDto.HealthTypeEnum.HealthIndicator,
                    Content = suggestionModel.Suggestion,
                    Title = "您的日常健康指标医生建议有更新",

                }, request.UserGuid);
            }

            return result ? Success() : Failed(ErrorCode.DataBaseError, "更新医生建议失败");
        }

        #endregion

        #region 日常健康指标设置

        /// <summary>
        /// 获取健康指标列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetHealthIndicatorListResponseDto>))]
        public async Task<IActionResult> GetHealthIndicators()
        {
            var healthInDicatorBiz = new HealthIndicatorBiz();

            return Success(await healthInDicatorBiz.GetHealthIndicators());
        }

        /// <summary>
        /// 保存日常指标设置
        /// </summary>
        /// <param name="requset"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<bool>))]
        public async Task<IActionResult> SaveHealthIndicators([FromBody]
        CreateOrUpdateHealthIndicatorRequestDto requset)
        {
            var items = requset.Items;
            if (requset is null || items.Count == 0)
            {
                return Failed(ErrorCode.Empty, "提交参数为空，请检查");
            }

            Logger.Info($"request_indicator_parameter ：{JsonConvert.SerializeObject(requset)}");

            var context = new HealthIndicatorContext();

            var healthInDicatorBiz = new HealthIndicatorBiz();

            var indicators = (await healthInDicatorBiz.GetListAsync()).ToList();

            var indicatorGuids = (List<string>)null;

            if (indicators != null && indicators.Count > 0)
            {
                indicatorGuids = indicators.Select(d => d.IndicatorGuid).ToList();
            }

            if (items.Any(d => d is null))
            {
                return Failed(ErrorCode.Empty, "存在为空的指标项，请检查");
            }

            if (items.Any(d => string.IsNullOrEmpty(d.IndicatorName?.Trim())))
            {
                return Failed(ErrorCode.Empty, "存在名称空的指标项，请检查");
            }

            var duplicateIndicatorNameExists = items.GroupBy(d => d.IndicatorName?.Trim())
                .Any(g => g.Count() > 1);

            if (duplicateIndicatorNameExists)
            {
                return Failed(ErrorCode.Empty, "指标名称存在重复项，请检查");
            }

            var itemContext = new HealthIndicatorItemContext()
            {
                Context = context
            };

            for (var i = 0; i < items.Count; i++)
            {
                var item = items[i];
                item.Sort = i + 1;

                itemContext.Item = item;

                if (item.IndicatorName.Length > 10)
                {
                    return Failed(ErrorCode.Empty, $"指标名称【{item.IndicatorName}】超过最大长度限制，请检查");
                }

                if (item.Options.Count <= 0)
                {
                    return Failed(ErrorCode.Empty, $"指标名称【{item.IndicatorName}】指标项为空，请检查");
                }

                if (item.IndicatorType == 0 && item.Options.Count > 1)
                {
                    return Failed(ErrorCode.Empty, $"【{item.IndicatorName}】指标类型为单个值但选项存在多个，请检查");
                }
                else if (item.IndicatorType == 1 && item.Options.Count < 2)
                {
                    return Failed(ErrorCode.Empty, $"【{item.IndicatorName}】指标类型为多个值但选项没有多个，请检查");
                }

                if (item.Options.Any(o => string.IsNullOrEmpty(o.OptionName)))
                {
                    return Failed(ErrorCode.Empty, $"【{item.IndicatorName}】存在名称为空的指标项，请检查");
                }

                var duplicateOptionNameExists = item.Options.GroupBy(d => d.OptionName?.Trim())
                    .Any(g => g.Count() > 1);

                if (duplicateOptionNameExists)
                {
                    return Failed(ErrorCode.Empty, $"指标名称【{item.IndicatorName}】存在重复指标项，请检查");
                }

                if (string.IsNullOrEmpty(item.IndicatorGuid))
                {
                    var checkResult = CreateIndicators(itemContext);

                    if (checkResult.Code != ErrorCode.Success)
                    {
                        return checkResult;
                    }
                }
                else
                {
                    if (indicators is null || indicators.Count <= 0)
                    {
                        return Success();
                    }

                    var indicator = indicators.FirstOrDefault(d => d.IndicatorGuid == item.IndicatorGuid);

                    if (indicator is null)
                    {
                        return Failed(ErrorCode.Empty, $"指标名称【{item.IndicatorName}】不存在，请检查");
                    }

                    if (indicators.Any(d => d.IndicatorName.Equals(indicator
                        .IndicatorName?.Trim()) && d.IndicatorGuid != indicator.IndicatorGuid))
                    {
                        return Failed(ErrorCode.Empty, $"指标名称【{item.IndicatorName}】已存在，请检查");
                    }

                    //移除已存在指标，最终该集合仍存在指标元素则需删除
                    indicatorGuids.Remove(indicator.IndicatorGuid);

                    var type = Convert.ToInt32(indicator.IndicatorType);

                    //当指标名称或指标类型发生修改时
                    if (!indicator.IndicatorName.Equals(item.IndicatorName.Trim()) ||
                        type != item.IndicatorType)
                    {
                        var checkResult = CreateIndicators(itemContext);
                        if (checkResult.Code != ErrorCode.Success)
                        {
                            return checkResult;
                        }

                        //对应指标和指标项将删除
                        context.DeleteIndicatorGuids.Add(indicator.IndicatorGuid);
                    }
                    else
                    {
                        #region 获取需要删除的指标项
                        var dbOptions = await healthInDicatorBiz.GetHealthIndicatorOptionAsync(indicator.IndicatorGuid);

                        if (dbOptions is null || dbOptions.Count <= 0)
                        {
                            return Failed(ErrorCode.Empty, $"指标名称【{item.IndicatorName}】数据出现异常，请联系技术支持");
                        }

                        var dbOptionGuids = dbOptions.Select(d => d.OptionGuid).ToList();

                        var currentOptionGuids = item.Options.Select(d => d.OptionGuid).ToList();

                        //若存在删除指标项或新建指标项则重新创建指标
                        if (currentOptionGuids.Any(d => string.IsNullOrEmpty(d)) || dbOptionGuids.Except(currentOptionGuids).Any())
                        {
                            var checkResult = CreateIndicators(itemContext);

                            if (checkResult.Code != ErrorCode.Success)
                            {
                                return checkResult;
                            }

                            //对应指标和指标项将删除
                            context.DeleteIndicatorGuids.Add(indicator.IndicatorGuid);
                        }
                        #endregion
                        else
                        {
                            #region 更新指标

                            //指标排序可能发生改变，需及时更新
                            indicator.Sort = (i + 1);
                            indicator.Display = item.Display;
                            indicator.LastUpdatedBy = UserID;
                            indicator.LastUpdatedDate = DateTime.Now;

                            context.UpdateIndicatorModels.Add(indicator);
                            #endregion

                            itemContext.DbOptions = dbOptions;

                            for (int j = 0; j < item.Options.Count; j++)
                            {
                                var option = item.Options[j];
                                option.Sort = j + 1;

                                itemContext.SubmitOption = option;

                                var checkOptionResult = UpdateIndicatorOption(itemContext);

                                if (checkOptionResult.Code != ErrorCode.Success)
                                {
                                    return checkOptionResult;
                                }
                            }
                        }
                    }
                }
            }

            if (indicatorGuids != null && indicatorGuids.Count > 0)
            {
                context.DeleteIndicatorGuids.AddRange(indicatorGuids);
            }

            var result = await healthInDicatorBiz.SaveHealthIndicators(context);

            return result ? Success() : Failed(ErrorCode.DataBaseError, "保存日常指标失败");
        }

        /// <summary>
        /// 添加指标
        /// </summary>
        /// <param name="itemContext"></param>
        ResponseDto CreateIndicators(HealthIndicatorItemContext itemContext)
        {
            var item = itemContext.Item;
            item.IndicatorGuid = Guid.NewGuid().ToString("N");

            itemContext.Context.InsertIndicatorModels.Add(new HealthIndicatorModel()
            {
                IndicatorGuid = item.IndicatorGuid,
                IndicatorName = item.IndicatorName,
                IndicatorType = Convert.ToBoolean(item.IndicatorType),
                Sort = item.Sort,
                Display = item.Display,
                CreatedBy = UserID,
                LastUpdatedBy = UserID
            });

            return CreateIndicatorOptions(itemContext.Context, item);
        }

        /// <summary>
        /// 添加指标项集合
        /// </summary>
        /// <param name="context"></param>
        /// <param name="item"></param>
        ResponseDto CreateIndicatorOptions(HealthIndicatorContext context, HealthIndicatorItem item)
        {
            for (int j = 0; j < item.Options.Count; j++)
            {
                var option = item.Options[j];

                if (!string.IsNullOrEmpty(option.OptionUnit?.Trim()) &&
                    option.OptionUnit.Length > 7)
                {
                    return Failed(ErrorCode.Empty, $"【{option.OptionName}】指标单位超过最大长度限制，请检查");
                }

                if (option.MinValue.HasValue && option.MaxValue.HasValue)
                {
                    if (option.MinValue.Value > option.MaxValue.Value)
                    {
                        return Failed(ErrorCode.Empty, $"【{option.OptionName}】参考范围最低值大于最高值，请检查");
                    }
                }

                context.InsertIndicatorOptionModels.Add(new HealthIndicatorOptionModel()
                {
                    OptionGuid = Guid.NewGuid().ToString("N"),
                    OptionName = option.OptionName,
                    OptionUnit = option.OptionUnit,
                    Required = option.Required,
                    IndicatorGuid = item.IndicatorGuid,
                    Sort = (j + 1),
                    MinValue = option.MinValue,
                    MaxValue = option.MaxValue,
                    LastUpdatedBy = UserID,
                    CreatedBy = UserID,
                });
            }

            return Success();
        }

        ResponseDto UpdateIndicatorOption(HealthIndicatorItemContext itemContext)
        {
            var option = itemContext.SubmitOption;
            var item = itemContext.Item;
            var dbOptions = itemContext.DbOptions;

            if (!string.IsNullOrEmpty(option.OptionUnit?.Trim()) &&
                     option.OptionUnit.Length > 7)
            {
                return Failed(ErrorCode.Empty, $"【{option.OptionName}】指标单位超过最大长度限制，请检查");
            }

            if (option.MinValue.HasValue && option.MaxValue.HasValue)
            {
                if (option.MinValue.Value > option.MaxValue.Value)
                {
                    return Failed(ErrorCode.Empty, $"【{option.OptionName}】参考范围最低值大于最高值，请检查");
                }
            }

            var dbOption = dbOptions.FirstOrDefault(d => d.OptionGuid.Equals(option.OptionGuid));

            if (dbOption is null)
            {
                return Failed(ErrorCode.Empty, $"【{option.OptionName}】的指标项不存在，请检查");
            }

            if (dbOptions.Any(d => d.OptionName.Equals(dbOption
                .OptionName?.Trim()) && d.OptionGuid != dbOption.OptionGuid))
            {
                return Failed(ErrorCode.Empty, $"【{dbOption.OptionName}】的指标项已存在，请检查");
            }

            //当指标选项名称发生修改，则重新生成新指标，旧的指标和指标项将删除
            if (!dbOption.OptionName.Equals(option.OptionName.Trim()))
            {
                itemContext.Context.DeleteIndicatorGuids.Add(dbOption.IndicatorGuid);

                var checkResult = CreateIndicators(itemContext);

                if (checkResult.Code != ErrorCode.Success)
                {
                    return checkResult;
                }
            }
            else
            {
                dbOption.OptionUnit = option.OptionUnit;
                dbOption.Sort = option.Sort;
                dbOption.MinValue = option.MinValue;
                dbOption.MaxValue = option.MaxValue;
                dbOption.Required = option.Required;
                dbOption.LastUpdatedBy = UserID;
                dbOption.LastUpdatedDate = DateTime.Now;

                itemContext.Context.UpdateIndicatorOptionModels.Add(dbOption);
            }

            return Success();
        }
        #endregion
    }
}
