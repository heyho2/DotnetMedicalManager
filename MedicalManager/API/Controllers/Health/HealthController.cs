using GD.Common;
using GD.Common.Helper;
using GD.Dtos.Health;
using GD.Manager.Consumer;
using GD.Manager.Health;
using GD.Manager.Utility;
using GD.Models.Consumer;
using GD.Models.Utility;
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
    /// 健康档案控制器
    /// </summary>
    public class HealthController : HealthBaseController
    {
        /// <summary>
        /// 注册会员
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto)), AllowAnonymous]
        public async Task<IActionResult> CreateConsumerHealthInfo([FromBody]CreateConsumerRequestDto request)
        {

            if (request.Informations.Count <= 0)
            {
                return Failed(ErrorCode.Empty, "基础信息未提交");
            }

            if (request.Informations.Any(d => string.IsNullOrEmpty(d.InformationGuid)))
            {
                return Failed(ErrorCode.Empty, "基础信息未提交");
            }

            var userBiz = new UserBiz();

            var user = await userBiz.GetByPnoneAsync(request.Phone);

            if (user != null)
            {
                return Failed(ErrorCode.Empty, $"该手机号【{request.Phone}】已注册，请直接在会员列表搜索");
            }

            var userGuid = Guid.NewGuid().ToString("N");

            var pwd = request.Phone.Substring(request.Phone.Length - 6);

            var userModel = new UserModel()
            {
                Phone = request.Phone,
                UserGuid = userGuid,
                UserName = string.IsNullOrWhiteSpace(request.UserName) ? userGuid.Substring(0, 6) : request.UserName,//userGuid.Substring(0, 6),
                Password = CryptoHelper.AddSalt(userGuid, CryptoHelper.Md5(pwd)),
                NickName = userGuid.Substring(0, 6),
                Gender = string.IsNullOrWhiteSpace(request.Gender) ? "M" : request.Gender,
                Birthday = request.Birthday,
                IdentityNumber = request.IdentityNumber,
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
                OrgGuid = ""
            };

            var consumerModel = new ConsumerModel()
            {
                ConsumerGuid = userGuid,
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
                OrgGuid = ""
            };

            var infos = request.Informations.Select(d => new ConsumerHealthInfoModel()
            {
                InfoRecordGuid = Guid.NewGuid().ToString("N"),
                UserGuid = userGuid,
                InformationGuid = d.InformationGuid,
                InformationType = d.InformationType?.ToString(),
                OptionGuids = JsonConvert.SerializeObject(d.OptionGuids),
                ResultValue = d.ResultValue,
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
                OrgGuid = ""
            }).ToList();

            var consumerBiz = new ConsumerBiz();

            var result = await consumerBiz.CreateConsumerHealthInfo(userModel, consumerModel, infos);
            if (!result)
            {
                return Failed(ErrorCode.Empty, "注册失败，请稍后重试");
            }

            return Success();
        }

        /// <summary>
        /// 会员列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetConsumerListResponseDto>))]
        public async Task<IActionResult> GetConsumersPageList([FromQuery]
         GetConsumerListRequestDto request)
        {
            var consumerBiz = new ConsumerBiz();

            return Success(await consumerBiz.GetConsumersPageList(request));
        }

        /// <summary>
        /// 会员问卷列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetConsumerQuestionnairesPageListResponseDto>))]
        public async Task<IActionResult> GetConsumerQuestionnairesPageList([FromBody]
         GetConsumerQuestionnairesPageListRequestDto request)
        {
            var healthBiz = new HealthBiz();

            return Success(await healthBiz.GetConsumerQuestionnairesPageList(request));
        }

        #region 会员检验报告
        /// <summary>
        /// 会员检验报告列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetConsumerHealthReportPageListResponseDto>))]
        public async Task<IActionResult> GetConsumerHealthReportPageList([FromQuery]
         GetConsumerHealthReportPageListRequestDto request)
        {
            var healthBiz = new HealthBiz();

            return Success(await healthBiz.GetConsumerHealthReportPageList(request));
        }

        /// <summary>
        /// 获取会员指定报告详情
        /// </summary>
        /// <param name="reportGuid"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetConsumerHealthReportResponseDto>))]
        public async Task<IActionResult> GetConsumerHealthReport(string reportGuid)
        {
            if (string.IsNullOrEmpty(reportGuid))
            {
                return Failed(ErrorCode.Empty, "指定报告参数未提供");
            }

            var healthBiz = new HealthBiz();

            return Success(await healthBiz.GetConsumerHealthReportDetail(reportGuid));
        }

        /// <summary>
        /// 删除会员指定报告
        /// </summary>
        /// <param name="reportGuid"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<bool>))]
        public async Task<IActionResult> DeleteConsumerHealthReport(string reportGuid)
        {
            if (string.IsNullOrEmpty(reportGuid))
            {
                return Failed(ErrorCode.Empty, "指定报告参数未提供");
            }

            var healthBiz = new HealthBiz();

            return Success(await healthBiz.DeleteConsumerHealthReport(reportGuid));
        }

        /// <summary>
        /// 获取会员昵称和手机号码
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<bool>))]
        public async Task<IActionResult> GetConsumerBasicInfo(string userGuid)
        {
            if (string.IsNullOrEmpty(userGuid))
            {
                return Failed(ErrorCode.Empty, "请指定上传会员");
            }

            var userBiz = new UserBiz();
            var model = await userBiz.GetAsync(userGuid);

            return Success(model == null ? (null) : new { model.NickName, model.Phone });
        }

        /// <summary>
        /// 上传检验报告
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<bool>))]
        public async Task<IActionResult> UploadHealthReport([FromBody] CreateOrUpdateHealthReportRequestDto request)
        {
            var healthBiz = new HealthBiz();

            if (string.IsNullOrEmpty(request.UserGuid))
            {
                return Failed(ErrorCode.Empty, "请指定检验报告所属会员");
            }

            var userBiz = new UserBiz();
            var userModel = await userBiz.GetAsync(request.UserGuid);
            if (userModel is null || !userModel.Enable)
            {
                return Failed(ErrorCode.Empty, "指定会员不存在");
            }

            if (string.IsNullOrEmpty(request.Suggestion) && request.Suggestion.Length > 1000)
            {
                return Failed(ErrorCode.Empty, "报告建议超过最大长度限制");
            }

            if (request.Attachments is null || request.Attachments.Count <= 0)
            {
                return Failed(ErrorCode.Empty, "请上传报告附件");
            }

            if (await healthBiz.CheckReportName(request.ReportName))
            {
                return Failed(ErrorCode.Empty, $"报告名称“{request.ReportName}”已存在");
            }

            var model = new ConsumerHealthReportModel()
            {
                ReportGuid = Guid.NewGuid().ToString("N"),
                ReportName = request.ReportName,
                UserGuid = request.UserGuid,
                Suggestion = request.Suggestion,
                LastUpdatedBy = UserID,
                CreatedBy = UserID
            };

            (List<ConsumerHealthReportDetailModel> detailModels, string errorMsg) = FilterReportAttachments(request.Attachments, model.ReportGuid);

            if (detailModels is null && !string.IsNullOrEmpty(errorMsg))
            {
                return Failed(ErrorCode.Empty, errorMsg);
            }

            var result = await healthBiz.CreateHealthReport(model, detailModels.ToList());

            return result ? Success() : Failed(ErrorCode.DataBaseError, "上传检验报告失败");
        }

        /// <summary>
        /// 编辑检验报告
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<bool>))]
        public async Task<IActionResult> UpdateHealthReport([FromBody] CreateOrUpdateHealthReportRequestDto request)
        {
            var healthBiz = new HealthBiz();

            if (string.IsNullOrEmpty(request.UserGuid))
            {
                return Failed(ErrorCode.Empty, "请指定检验报告所属会员");
            }

            var userBiz = new UserBiz();
            var userModel = await userBiz.GetAsync(request.UserGuid);
            if (userModel is null || !userModel.Enable)
            {
                return Failed(ErrorCode.Empty, "指定会员不存在");
            }

            if (string.IsNullOrEmpty(request.ReportGuid))
            {
                return Failed(ErrorCode.Empty, "请指定编辑报告");
            }

            if (string.IsNullOrEmpty(request.Suggestion) && request.Suggestion.Length > 1000)
            {
                return Failed(ErrorCode.Empty, "报告建议超过最大长度限制");
            }

            if (request.Attachments is null || request.Attachments.Count <= 0)
            {
                return Failed(ErrorCode.Empty, "请上传报告附件");
            }

            if (await healthBiz.CheckReportName(request.ReportName, request.ReportGuid))
            {
                return Failed(ErrorCode.Empty, $"报告名称“{request.ReportName}”已存在");
            }

            var model = await healthBiz.GetConsumerHealthReport(request.ReportGuid);

            model.ReportName = request.ReportName;
            model.Suggestion = request.Suggestion;
            model.LastUpdatedBy = UserID;
            model.LastUpdatedDate = DateTime.Now;

            (List<ConsumerHealthReportDetailModel> detailModels, string errorMsg) = FilterReportAttachments(request.Attachments, model.ReportGuid);

            if (detailModels is null && !string.IsNullOrEmpty(errorMsg))
            {
                return Failed(ErrorCode.Empty, errorMsg);
            }

            var result = await healthBiz.UpdateHealthReport(model, detailModels.ToList());

            return result ? Success() : Failed(ErrorCode.DataBaseError, "编辑检验报告失败");
        }

        /// <summary>
        /// 过滤附件
        /// </summary>
        /// <param name="attachments"></param>
        /// <param name="reportGuid"></param>
        /// <returns></returns>
        (List<ConsumerHealthReportDetailModel>, string errorMeeage) FilterReportAttachments(List<UploadReportAttachment> attachments, string reportGuid)
        {
            var details = new List<ConsumerHealthReportDetailModel>();

            var removeNameDuplicates = new List<string>();

            foreach (var attachment in attachments)
            {
                if (string.IsNullOrEmpty(attachment.ReportDetailGuid))
                {
                    return (null, "附件为空");
                }

                if (string.IsNullOrEmpty(attachment.Name?.Trim()))
                {
                    return (null, "附件名称为空");
                }

                if (attachment.Name.Length > 255)
                {
                    return (null, $"附件名称【{attachment.Name}】超过最大长度限制");
                }

                if (removeNameDuplicates.Contains(attachment.Name.Trim()))
                {
                    return (null, $"附件名称【{attachment.Name}】有重复，请检查");
                }
                removeNameDuplicates.Add(attachment.Name.Trim());

                details.Add(new ConsumerHealthReportDetailModel()
                {
                    ReportDetailGuid = Guid.NewGuid().ToString("N"),
                    AccessoryName = attachment.Name,
                    CreatedBy = UserID,
                    LastUpdatedBy = UserID,
                    ReportGuid = reportGuid,
                    AccessoryGuid = attachment.ReportDetailGuid
                });
            }

            return (details, null);
        }
        #endregion
    }
}
