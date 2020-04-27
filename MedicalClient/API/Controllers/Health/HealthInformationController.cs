using GD.Common;
using GD.Consumer;
using GD.Dtos.Health;
using GD.Health;
using GD.Models.CommonEnum;
using GD.Models.Consumer;
using GD.Models.Utility;
using GD.Utility;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.API.Controllers.Health
{
    /// <summary>
    /// 健康基础信息控制器
    /// </summary>
    public class HealthInformationController : HealthBaseController
    {
        /// <summary>
        /// 获取用户基础信息
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetUserHealthInformationResponseDto>))]
        public async Task<IActionResult> GetHealthInformationAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                userId = UserID;
            }
            GetUserHealthInformationResponseDto respon = new GetUserHealthInformationResponseDto();
            //删除用户改变的所有基础信息数据
            var resultModelList = await new HealthInformationBiz().DeleteUserHealthInformation(userId);
            var result = await new HealthInformationBiz().GetHealthInformationList(userId);
            var userBiz = new UserBiz();
            var model = userBiz.GetUser(userId);
            if (model != null)
            {
                UserResponseDto userInfo = new UserResponseDto
                {
                    UserName = model.UserName,
                    Birthday = model.Birthday,
                    Gender = model.Gender,
                    IdentityNumber = model.IdentityNumber
                };

                respon.UserInfo = userInfo;
            }
            if (result == null)
            {
                return Success(respon);
            }
            foreach (var item in result)
            {
                if (item.InformationType != HealthInformationEnum.Decimal.ToString() && item.InformationType != HealthInformationEnum.String.ToString())
                {
                    //查询选项数据
                    item.OptionList = (await new HealthInformationOptionBiz().GetHealthInformationOptionAsync(item.InformationGuid)).Select(s => new HealthInformationOptionResponse
                    {
                        OptionGuid = s.OptionGuid,
                        OptionLabel = s.OptionLabel,
                        IsDefault = s.IsDefault,
                        Sort = s.Sort
                    }).OrderBy(s => s.Sort).ToList();
                    item.OptionValue = (await new ConsumerHealthInfoBiz().GetConsumerHealthInfoAsync(item.InformationGuid, userId))?.OptionGuids;
                }
            }
            respon.HealthInformationList = result;
            return Success(respon);
        }
        /// <summary>
        /// 健康基础数据更新
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> UpdateHealthInformationAsync([FromBody]GetHealthInformationRequestDto requestDto)
        {
            if (requestDto.UpdateType == 0)
            {
                var healthInfoModel = await new HealthInformationBiz().GetAsync(requestDto.InformationGuid);
                if (healthInfoModel == null)
                {
                    return Failed(ErrorCode.DataBaseError, "健康基础信息不存在");
                }
                ConsumerHealthInfoModel consumerHealthInfoModel = new ConsumerHealthInfoModel()
                {
                    InfoRecordGuid = Guid.NewGuid().ToString("N"),
                    InformationGuid = requestDto.InformationGuid,
                    InformationType = healthInfoModel.InformationType,
                    UserGuid = UserID,
                    CreatedBy = UserID,
                    CreationDate = DateTime.Now
                };

                if (healthInfoModel.InformationType == HealthInformationEnum.Decimal.ToString())
                {
                    //数值类型校验
                    decimal value = 0;
                    if (!Decimal.TryParse(requestDto.ResultValue, out value))
                    {
                        return Failed(ErrorCode.DataBaseError, "请填写数值类型数据");
                    }
                    consumerHealthInfoModel.ResultValue = requestDto.ResultValue;
                    consumerHealthInfoModel.OptionGuids = null;
                }
                else if (healthInfoModel.InformationType == HealthInformationEnum.Array.ToString() || healthInfoModel.InformationType == HealthInformationEnum.Enum.ToString())
                {
                    //差集
                    var healthInformationOptionList = await new HealthInformationOptionBiz().GetHealthInformationOptionAsync(requestDto.InformationGuid);
                    var infoResult = requestDto.OptionGuids.Except(healthInformationOptionList.Select(s => s.OptionGuid))?.ToList();
                    if (infoResult != null && infoResult.Count != 0)
                    {
                        return Failed(ErrorCode.DataBaseError, "答案Id不存在");
                    }
                    if (requestDto.OptionGuids == null || requestDto.OptionGuids.Count == 0)
                    {
                        return Failed(ErrorCode.DataBaseError, "请选择答案");
                    }
                    if (healthInfoModel.InformationType == HealthInformationEnum.Bool.ToString() || healthInfoModel.InformationType == HealthInformationEnum.Enum.ToString())
                    {
                        //单选和判断题选择值只能为一个答案
                        if (requestDto.OptionGuids.Count != 1)
                        {
                            return Failed(ErrorCode.DataBaseError, "请选择一种答案");
                        }
                    }
                    consumerHealthInfoModel.OptionGuids = JsonConvert.SerializeObject(requestDto.OptionGuids);
                    consumerHealthInfoModel.ResultValue = null;
                }
                else
                {
                    //暂不做校验
                    consumerHealthInfoModel.OptionGuids = null;
                    consumerHealthInfoModel.ResultValue = requestDto.ResultValue;
                }
                bool result = false;
                ConsumerHealthInfoBiz consumerHealthInfoBiz = new ConsumerHealthInfoBiz();
                var consumerHealthInfo = await consumerHealthInfoBiz.GetConsumerHealthInfoAsync(requestDto.InformationGuid, UserID);
                if (consumerHealthInfo != null)
                {
                    //存在即更新
                    consumerHealthInfoModel.InfoRecordGuid = consumerHealthInfo.InfoRecordGuid;
                    consumerHealthInfoModel.InformationGuid = consumerHealthInfo.InformationGuid;
                    consumerHealthInfoModel.CreatedBy = consumerHealthInfo.CreatedBy;
                    consumerHealthInfoModel.CreationDate = consumerHealthInfo.CreationDate;
                    consumerHealthInfoModel.LastUpdatedBy = UserID;
                    consumerHealthInfoModel.LastUpdatedDate = DateTime.Now;
                    result = await consumerHealthInfoBiz.UpdateAsync(consumerHealthInfoModel);
                }
                else
                {
                    result = await consumerHealthInfoBiz.InsertAsync(consumerHealthInfoModel);
                }
                return result ? Success() : Failed(ErrorCode.DataBaseError, "更新健康基础信息失败");
            }
            else
            {
                //固定五项数据更新
                var userBiz = new UserBiz();
                var model = userBiz.GetUser(UserID);
                if (model == null)
                {
                    return Failed(ErrorCode.DataBaseError, "用户不存在");
                }
                model.IdentityNumber = requestDto.IdentityNumber;
                model.LastUpdatedBy = UserID;
                model.LastUpdatedDate = DateTime.Now;
                model.UserName = requestDto.UserName;
                model.Birthday = requestDto.Birthday;
                model.Gender = requestDto.Gender;
                return userBiz.UpdateUser(model) ? Success() : Failed(ErrorCode.DataBaseError, "更新用户信息失败");
            }
        }
        /// <summary>
        /// 健康报告查询
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetHealthInformationArchivesPageListResponseDto>))]
        public async Task<IActionResult> GetHealthInformationArchivesPageListAsync([FromQuery]GetHealthInformationArchivesPageListRequestDto requestDto)
        {
            var response = await new ConsumerHealthReportBiz().GetHealthInformationArchivesListAsync(requestDto, UserID);
            return Success(response);
        }

        /// <summary>
        /// 健康报告附件查询
        /// </summary>
        /// <param name="reportGuid">报告id</param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<ConsumerHealthReportDetailResponse>>))]
        public async Task<IActionResult> GetHealthInformationArchivesDetailAsync([FromQuery]string reportGuid)
        {
            if (string.IsNullOrWhiteSpace(reportGuid))
            {
                return Failed(ErrorCode.DataBaseError, "报表Id不能为空");
            }
            var response = await new ConsumerHealthReportDetailBiz().GetConsumerHealthReportDetailAsync(reportGuid);
            return Success(response);
        }
        /// <summary>
        /// 健康基础数据批量更新
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> UpdateHealthInformationBatchAsync([FromBody]UpdateHealthInformationRequestDto request)
        {
            var userBiz = new UserBiz();
            var model = await userBiz.GetModelAsync(request.UserGuid, true);
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
    }
}
