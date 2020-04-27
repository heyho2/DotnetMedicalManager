using GD.Common;
using GD.Common.Helper;
using GD.Consumer;
using GD.Dtos.Common;
using GD.Dtos.Doctor.Hospital;
using GD.Dtos.Enum;
using GD.Dtos.Health;
using GD.Dtos.Health.HealthManager;
using GD.Health;
using GD.Manager.Health;
using GD.Models.CommonEnum;
using GD.Models.Consumer;
using GD.Models.Health;
using GD.Module.WeChat;
using GD.Utility;
using Microsoft.AspNetCore.Authorization;
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
    /// 健康管理师控制器
    /// </summary>
    public class HealthManagerController : HealthBaseController
    {
        /// <summary>
        /// 用于测试时获取token
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        public IActionResult GetToken(string id)
        {
            var response = new EnterPriseWeChatLoginResponseDto
            {
                UserId = id,
                UserName = "测试",
                Token = CreateToken(id, Common.EnumDefine.UserType.Unknown, 30),
            };
            return Success(response);
        }


        /// <summary>
        /// 企业微信授权登录
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost, AllowAnonymous, Produces(typeof(EnterPriseWeChatLoginResponseDto))]
        public async Task<IActionResult> EnterpriseWeChatLogin(string code)
        {
            var aToken = await EnterpriseWeChatApi.GetEnterpriseAccessToken(PlatformSettings.EnterpriseWeChatAppid, PlatformSettings.HealthManagerMobileSecret);
            if (string.IsNullOrWhiteSpace(aToken?.AccessToken))
            {
                Logger.Error($"健康管理师端企业微信授权登录获取token失败:{aToken.Errmsg} at {nameof(HealthManagerController)}.{nameof(EnterpriseWeChatLogin)}({code})");
                return Failed(ErrorCode.SystemException, "健康管理师端企业微信授权登录获取token失败");
            }
            var result = await EnterpriseWeChatApi.GetEnterpriseWeChatUserInfo(code, aToken.AccessToken);
            if (result == null)
            {
                return Failed(ErrorCode.Empty, "未找到对应用户信息");
            }
            if (result.extattr == null || result.extattr.attrs == null || result.extattr.attrs.Length == 0)
            {
                return Failed(ErrorCode.Empty, "未找到对应用户工号信息");
            }
            var jobNumber = result.extattr.attrs.FirstOrDefault(a => a.name == "工号");
            if (jobNumber == null || string.IsNullOrEmpty(jobNumber.value))
            {
                return Failed(ErrorCode.Empty, "用户工号信息为空");
            }

            //获取指定工号对应的健康管理师资料、验证合法性
            var model = await new HealthManagerBiz().GetByJobNumberAsync(jobNumber.value);
            if (model == null)
            {
                return Failed(ErrorCode.Unauthorized, "企业微信对应的工号的健康管理师不存在");
            }
            if (!string.IsNullOrWhiteSpace(result.userid) && result.userid != model.EnterpriseUserId)
            {
                model.EnterpriseUserId = result.userid;
                model.LastUpdatedBy = model.ManagerGuid;
                model.LastUpdatedDate = DateTime.Now;
                await new HealthManagerBiz().UpdateAsync(model);
            }
            var response = new EnterPriseWeChatLoginResponseDto
            {
                UserId = model.ManagerGuid,
                UserName = model.UserName,
                Token = CreateToken(model.ManagerGuid, Common.EnumDefine.UserType.Unknown, 30),
            };
            return Success(response);
        }
        /// <summary>
        /// 获取健康管理师企业微信应用相关配置信息
        /// </summary>
        /// <returns></returns>
        [HttpGet, AllowAnonymous, Produces(typeof(ResponseDto<GetEnterpriseWeChatConfigResponseDto>))]
        public IActionResult GetEnterpriseWeChatConfig()
        {
            var config = new GetEnterpriseWeChatConfigResponseDto
            {
                AppId = PlatformSettings.EnterpriseWeChatAppid,
                AgentId = PlatformSettings.HealthManagerMobileAgentid
            };
            return Success(config);
        }

        /// <summary>
        /// 获取健康管理师基础信息
        /// </summary>
        /// <param name="id">健康管理师id</param>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(ResponseDto<GetHealthManagerBasicInfoResponseDto>))]
        public async Task<IActionResult> GetHealthManagerBasicInfoAsync(string id)
        {
            var model = await new HealthManagerBiz().GetAsync(id);
            if (model == null)
            {
                return Failed(ErrorCode.UserData, "此健康管理师不存在");
            }
            var portrait = await new AccessoryBiz().GetAsync(model.PortraitGuid);
            var response = new GetHealthManagerBasicInfoResponseDto
            {
                Name = model.UserName,
                Portrait = $"{portrait?.BasePath}{portrait?.RelativePath}",
                Gender = model.Gender,
                Phone = model.Phone,
                IdentityNumber = model.IdentityNumber,
                OccupationGrade = model.OccupationGrade
            };
            return Success(response);
        }

        /// <summary>
        /// 修改健康管理师头像
        /// </summary>
        /// <param name="portraitGuid"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> UpdatePortraitAsync(string portraitGuid)
        {
            var biz = new HealthManagerBiz();
            var model = await biz.GetAsync(UserID);
            if (model == null)
            {
                return Failed(ErrorCode.UserData, "此健康管理师不存在");
            }
            model.PortraitGuid = portraitGuid;
            model.LastUpdatedBy = UserID;
            model.LastUpdatedDate = DateTime.Now;
            var result = await biz.UpdateAsync(model);
            if (!result)
            {
                return Failed(ErrorCode.DataBaseError, "修改头像失败");
            }
            return Success();
        }

        /// <summary>
        /// 获取当前登录的健康管理师名下的健康指标预警分页记录
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetIndicatorWarningPageListResponseDto>))]
        public async Task<IActionResult> GetIndicatorWarningPageListAsync(GetIndicatorWarningPageListRequestDto requestDto)
        {
            var result = await new IndicatorWarningBiz().GetIndicatorWarningPageListAsync(requestDto, UserID);
            return Success(result);
        }

        /// <summary>
        /// 获取预警记录详情
        /// </summary>
        /// <param name="warningId">预警id</param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetIndicatorWarningDetailsResponseDto>))]
        public async Task<IActionResult> GetIndicatorWarningDetailsAsync(string warningId)
        {
            var model = await new IndicatorWarningBiz().GetAsync(warningId);
            if (model == null)
            {
                return Failed(ErrorCode.UserData, "无此预警记录");
            }
            var result = new GetIndicatorWarningDetailsResponseDto
            {
                WarningGuid = model.WarningGuid,
                Name = model.Name,
                Age = model.Age,
                WarningDate = model.CreationDate,
                Phone = model.Phone,
                Description = model.Description,
                Status = Enum.Parse<IndicatorWarningStatusEnum>(model.Status)
            };
            return Success(result);
        }

        /// <summary>
        /// 获取当前登录的健康管理师待处理的日常指标预警数量
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<int>))]
        public async Task<IActionResult> GetPendingWarningNumberAsync()
        {
            var result = await new IndicatorWarningBiz().GetNumberByHealthManagerIdAsync(UserID);
            return Success(result);
        }

        /// <summary>
        /// 关闭日常指标预警
        /// </summary>
        /// <param name="warningId"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> CloseIndicatorWarningAsync(string warningId)
        {
            var biz = new IndicatorWarningBiz();
            var model = await biz.GetAsync(warningId);
            if (model == null)
            {
                return Failed(ErrorCode.UserData, "无此预警记录");
            }
            if (model.Status != IndicatorWarningStatusEnum.Pending.ToString())
            {
                return Failed(ErrorCode.UserData, "当前预警不是待处理状态，无需处理");
            }
            model.Status = IndicatorWarningStatusEnum.Closed.ToString();
            model.LastUpdatedBy = UserID;
            model.LastUpdatedDate = DateTime.Now;
            var result = await biz.UpdateAsync(model);
            if (!result)
            {
                return Failed(ErrorCode.DataBaseError, "关闭预警失败，请联系管理员");
            }
            return Success();
        }

        /// <summary>
        /// 获取随访记录分页列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetFollowupRecordPageListResponseDto>))]
        public async Task<IActionResult> GetFollowupRecordPageListAsync(GetFllowupRecordPageListRequestDto requestDto)
        {
            var result = await new FollowupRecordBiz().GetFllowupRecordPageListAsync(requestDto);
            return Success(result);
        }

        /// <summary>
        /// 新建随访记录
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddFollowupRecordAsync([FromBody]AddFollowupRecordRequestDto requestDto)
        {
            var userModel = await new UserBiz().GetModelAsync(requestDto.ConsumerGuid);
            if (userModel == null)
            {
                return Failed(ErrorCode.UserData, "无此会员信息");
            }
            var model = new FollowupRecordModel
            {
                FollowupGuid = Guid.NewGuid().ToString("N"),
                ConsumerGuid = requestDto.ConsumerGuid,
                Name = userModel.UserName,
                Phone = userModel.Phone,
                HealthManagerGuid = UserID,
                FollowupTime = requestDto.FollowupTime,
                Content = requestDto.Content,
                Suggestion = requestDto.Suggestion,
                CreatedBy = UserID,
                LastUpdatedBy = UserID
            };
            var result = await new FollowupRecordBiz().InsertAsync(model);
            if (!result)
            {
                return Failed(ErrorCode.DataBaseError, "新增随访记录失败，请联系管理员");
            }
            return Success();
        }
        /// <summary>
        /// 获取指定消费者下随访记录记录人列表
        /// key表示记录人id,value表示记录人姓名
        /// </summary>
        /// <param name="consumerId"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<KeyValueDto<string, string>>>))]
        public async Task<IActionResult> GetFollowupOperatorsOfTheConsumerAsync(string consumerId)
        {
            var result = await new FollowupRecordBiz().GetFollowupOperatorsOfTheConsumerAsync(consumerId);
            return Success(result);
        }
        /// <summary>
        /// 我的用户分页列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetHealthManagerConsumerListResponseDto>))]
        public async Task<IActionResult> GetHealthManagerConsumers([FromQuery]
        GetHealthManagerConsumerListRequestDto request)
        {
            request.UserId = UserID;
            var managerBiz = new HealthManagerBiz();
            var response = await managerBiz.GetHealthManagerConsumers(request);
            if (response != null)
            {
                foreach (var item in response.CurrentPage)
                {
                    if (item.ReportMaxDate > item.MaxDate)
                    {
                        item.MaxDate = item.ReportMaxDate;
                    }
                }
                response.CurrentPage = response.CurrentPage?.OrderByDescending(s => s.MaxDate).ThenByDescending(s => s.ManagerBindDate).ToList();
            }
            return Success(response);
        }
        /// <summary>
        /// 新增注册用户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto)), AllowAnonymous]
        public async Task<IActionResult> CreateConsumerHealthInfo([FromBody]CreateConsumerRequestDto request)
        {

            if (request.Informations.Count <= 0)
            {
                return Failed(ErrorCode.UserData, "基础信息未提交");
            }

            if (request.Informations.Any(d => string.IsNullOrEmpty(d.InformationGuid)))
            {
                return Failed(ErrorCode.UserData, "基础信息未提交");
            }

            var userBiz = new UserBiz();

            var user = userBiz.GetModelByPhoneAsync(request.Phone);

            if (user != null)
            {
                return Failed(ErrorCode.UserData, $"该手机号【{request.Phone}】已注册，请直接在会员列表搜索");
            }

            var userGuid = Guid.NewGuid().ToString("N");

            var pwd = request.Phone.Substring(request.Phone.Length - 6);

            var userModel = new GD.Models.Utility.UserModel()
            {
                Phone = request.Phone,
                UserGuid = userGuid,
                UserName = string.IsNullOrWhiteSpace(request.UserName) ? userGuid.Substring(0, 6) : request.UserName,//userGuid.Substring(0, 6),
                Password = CryptoHelper.AddSalt(userGuid, CryptoHelper.Md5(pwd)),
                NickName = userGuid.Substring(0, 6),
                Gender = request.Gender,
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
            if (request.IsBind)
            {
                consumerModel.HealthManagerGuid = UserID;
                consumerModel.ManagerBindDate = DateTime.Now;
            }
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
                return Failed(ErrorCode.UserData, "注册失败，请稍后重试");
            }

            return Success();
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
            var userModel = userBiz.GetUser(request.UserGuid);
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
            var userModel = userBiz.GetUser(request.UserGuid);
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
        /// <summary>
        /// 保存或更新指标预警值
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
            var consumer = await consumerBiz.GetModelAsync(request.ConsumerGuid);
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
                return Failed(ErrorCode.Empty, "该会员指定指标项数据不存在");
            }
            var response = new GetIndicatorWarningLimitResponseDto()
            {
                TurnOnWarning = limits.FirstOrDefault().TurnOnWarning,
                Limits = limits
            };
            return Success(response);
        }
        /// <summary>
        /// 获取绑定用户列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<DataUserInfo>>))]
        public async Task<IActionResult> GetBindConsumers([FromQuery]
        GetBindRequestDto request)
        {
            var response = new List<DataUserInfo>();
            var result = await new HealthManagerBiz().GetBindConsumers(request);
            if (result != null)
            {
                foreach (var item in result)
                {
                    if (!string.IsNullOrWhiteSpace(item.UserName))
                    {
                        item.Letter = NPinyin.Pinyin.GetInitials(item.UserName).Substring(0, 1);
                    }
                }
                var responseDetail = result.GroupBy(s => s.Letter).Select(r => new DataUserInfo
                {
                    Letter = r.Key,
                    data = r.ToList()
                }).OrderBy(a => a.Letter).ToList();
                if (responseDetail != null)
                {
                    //不包含26字母集合
                    string str = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
                    string[] array = str.Split(',');
                    var noContent = responseDetail.Where(s => !array.Contains(s.Letter)).ToList();
                    var content = responseDetail.Where(s => array.Contains(s.Letter)).ToList();
                    response.AddRange(content);
                    DataUserInfo data = new DataUserInfo
                    {
                        Letter = "*"
                    };
                    data.data = new List<GetBindResponseDto>();
                    if (noContent != null)
                    {
                        foreach (var item in noContent)
                        {
                            data.data.AddRange(item.data);
                        }
                    }
                    response.Add(data);
                }
            }
            return Success(response);
        }
        /// <summary>
        /// 批量会员绑定健康管理师
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> BatchBindHealthManager([FromBody] BatchUpdateConsumerBindMangerRequestDto request)
        {
            if (request.ConsumerGuids.Count <= 0)
            {
                return Failed(ErrorCode.Empty, "会员参数未提供，请检查");
            }

            var managerBiz = new HealthManagerBiz();
            var manager = await managerBiz.GetAsync(UserID);
            if (manager is null)
            {
                return Failed(ErrorCode.Empty, "健康管理师不存在，请检查");
            }
            request.ManagerGuid = UserID;
            var response = await managerBiz.BatchBindHealthManager(request);

            return Success(response);
        }
        /// 获取用户注册信息选项
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetHealthInformationResponseDto>>))]
        public async Task<IActionResult> GetRegisterHealthInformationAsync()
        {
            var result = await new HealthInformationBiz().GetHealthInformationList("");
            if (result == null)
            {
                return Success(result);
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
                }
            }
            return Success(result);
        }
        /// <summary>
        /// 评价用户填写的问卷
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Produces(typeof(ResponseDto))]
        public async Task<IActionResult> CommentConsumerQuestionnaireAsync([FromBody]CommentConsumerQuestionnaireRequestDto requestDto)
        {
            var biz = new QuestionnaireResultBiz();
            var questionnaireResult = await biz.GetAsync(requestDto.ResultGuid);
            if (questionnaireResult == null)
            {
                return Failed(ErrorCode.UserData, "用户问卷结果不存在");
            }
            if (!questionnaireResult.FillStatus)
            {
                return Failed(ErrorCode.UserData, "用户问卷未提交，不可评价");
            }
            questionnaireResult.Commented = true;
            questionnaireResult.Comment = requestDto.Comment;
            questionnaireResult.LastUpdatedBy = UserID;
            questionnaireResult.LastUpdatedDate = DateTime.Now;
            var result = await biz.UpdateAsync(questionnaireResult);
            if (result)
            {
                //rabbitMQ通知用户问卷结果被评价
                new HealthRabbitMQNotificationBiz().HealthRabbitMQNotification(new HealthMessageDto
                {
                    Title = "您填写的问卷得到医生回复啦",
                    Content = questionnaireResult.Comment,
                    HealthType = HealthMessageDto.HealthTypeEnum.Questionnaire,
                    ResultGuid = questionnaireResult.ResultGuid
                }, questionnaireResult.UserGuid);
            }
            return result ? Success() : Failed(ErrorCode.DataBaseError, "评价用户问卷结果失败");
        }
        /// <summary>
        /// 校验文件信息
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
    }
}
