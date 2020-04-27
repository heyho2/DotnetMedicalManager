using GD.API.Code;
using GD.AppSettings;
using GD.Common;
using GD.Common.EnumDefine;
using GD.Common.Helper;
using GD.Communication.XMPP;
using GD.Consumer;
using GD.Dtos.Doctor.Doctor;
using GD.Dtos.DtoIn;
using GD.Dtos.Mall.Product;
using GD.Dtos.Mall.Project;
using GD.Dtos.Merchant.Category;
using GD.Dtos.Merchant.Merchant;
using GD.Dtos.Merchant.Therapist;
using GD.Dtos.Utility.Utility;
using GD.Fushion.CompositeBiz;
using GD.Mall;
using GD.Manager;
using GD.Merchant;
using GD.Models.CommonEnum;
using GD.Models.Consumer;
using GD.Models.Mall;
using GD.Models.Manager;
using GD.Models.Merchant;
using GD.Models.Utility;
using GD.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static GD.Dtos.Consumer.Consumer.GetUserOrderProductCommentPageListItemDto;
using static GD.Dtos.Merchant.Merchant.GetMerchantOrderPageListRequestDto;
using static GD.Models.Mall.OrderModel;

namespace GD.API.Controllers.Merchant
{
    /// <inheritdoc />
    /// <summary>
    /// 商户控制器
    /// </summary>
    public class MerchantController : MerchantBaseController
    {
        /// <summary>
        /// xmpp Client
        /// </summary>
        private static readonly Client xmppClient;

        static MerchantController()
        {
            var settings = Factory.GetSettings("host.json");
            var xmppAccount = settings["XMPP:operationAccount"];
            var xmppPassword = settings["XMPP:operationPassword"];
            xmppClient = new Client(xmppAccount, xmppPassword, $"{nameof(MerchantController)}#@@@#{Guid.NewGuid().ToString("N")}");
            xmppClient.ConnectAsync();
        }

        #region 商户账号管理

        /// <summary>
        /// 商户登录
        /// </summary>
        /// <param name="loginRequestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<MerchantLoginResponseDto>)), AllowAnonymous]
        public async Task<IActionResult> MerchantLoginAsync([FromBody]MerchantLoginRequestDto loginRequestDto)
        {
            var merchantBiz = new MerchantBiz();

            var model = await merchantBiz.GetModelByAccountAsync(loginRequestDto.Account);

            if (model is null)
            {
                return Failed(ErrorCode.Unauthorized, "账号不存在或已禁用");
            }

            if (!model.Password.Equals(CryptoHelper.AddSalt(model.MerchantGuid, loginRequestDto.Password), StringComparison.OrdinalIgnoreCase))
            {
                return Failed(ErrorCode.InvalidIdPassword, "账号或密码错误");
            }

            var status = Client.QueryStatusAsync(model.MerchantGuid);
            status.Wait();

            // 检查用户IM账号是否存在,如果不存在，则注册该用户的IM账号
            if (status.Result == IMStatus.NotExist)
            {
                RegisterIM(model);
            }

            var merchantPicModel = await new AccessoryBiz().GetAsync(model.MerchantPicture);
            var settings = Factory.GetSettings("host.json");
            var domain = settings["XMPP:domain"];
            var httpBind = settings["XMPP:httpBind"];
            var response = new MerchantLoginResponseDto
            {
                MerchantGuid = model.MerchantGuid,
                MerchantName = model.MerchantName,
                MerchantPicture = $"{merchantPicModel?.BasePath}{merchantPicModel?.RelativePath}",
                Token = CreateToken(model.MerchantGuid, UserType.Merchant, 30),
                Domain = domain,
                Xmpp = httpBind
            };

            return Success(response);
        }

        /// <summary>
        /// 注册IM账号
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private bool RegisterIM(MerchantModel model)
        {
            var response = xmppClient.CreateUserAsync(model.MerchantGuid, model.MerchantGuid.Md5(), model.MerchantName);
            response.Wait();

            return !response.IsFaulted;
        }

        /// <summary>
        /// 商户修改密码 
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<bool>))]
        public async Task<IActionResult> ModifyPassword([FromBody]MerchantModifyPasswordRequestDto requestDto)
        {
            if (string.IsNullOrEmpty(UserID))
            {
                return Failed(ErrorCode.Unauthorized, "账号不存在或已禁用");
            }

            var merchantBiz = new MerchantBiz();

            var model = await merchantBiz.GetModelAsync(UserID);

            if (model is null)
            {
                return Failed(ErrorCode.Unauthorized, "账号不存在或已禁用");
            }

            var addSaltPwd = CryptoHelper.AddSalt(model.MerchantGuid, requestDto.Password);

            if (!model.Password.Equals(addSaltPwd, StringComparison.OrdinalIgnoreCase))
            {
                return Failed(ErrorCode.InvalidIdPassword, "账号或密码错误");
            }

            model.LastUpdatedBy = model.MerchantGuid;
            model.LastUpdatedDate = DateTime.Now;
            model.Password = CryptoHelper.AddSalt(model.MerchantGuid, requestDto.NewPassword);

            var result = await merchantBiz.UpdateAsync(model);

            return result ? Success() : Failed(ErrorCode.DataBaseError, "密码更新失败！");
        }
        #endregion


        /// <summary>
        /// 注册商户
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> RegisterMerchant([FromBody]MerchantDto merchantDto)
        {
            var merchantBiz = new MerchantBiz();
            var checkModel = await merchantBiz.GetModelAsync(UserID);

            bool isAdd = checkModel == null; ;//当前为更新操作还是新增操作
            var statusCheck = string.Equals(checkModel?.Status, StatusEnum.Submit.ToString(), StringComparison.OrdinalIgnoreCase) || string.Equals(checkModel?.Status, StatusEnum.Approved.ToString(), StringComparison.OrdinalIgnoreCase);
            if (checkModel != null && statusCheck && checkModel.Enable)
            {
                return Failed(ErrorCode.DataBaseError, "该用户已注册过商户！");
            }

            if (!merchantDto.Scopes.Any())
            {
                return Failed(ErrorCode.UserData, "经营范围数据为空！");
            }
            var userModel = await new UserBiz().GetModelAsync(UserID);
            userModel.UserName = merchantDto.UserName;
            userModel.IdentityNumber = merchantDto.IdentityNumber;
            userModel.Birthday = merchantDto.Birthday;
            userModel.Gender = merchantDto.Gender;
            var merchantModel = new MerchantModel();
            if (!isAdd)
            {
                merchantModel = checkModel;
            }
            //商户信息
            merchantModel.MerchantGuid = UserID;
            merchantModel.MerchantName = merchantDto.MerchantName;
            merchantModel.CreatedBy = UserID;
            merchantModel.SignatureGuid = merchantDto.SignatureGuid;
            merchantModel.Telephone = merchantDto.Telephone;
            merchantModel.Status = StatusEnum.Submit.ToString();
            merchantModel.OrgGuid = String.Empty;
            merchantModel.LastUpdatedBy = UserID;
            merchantModel.LastUpdatedDate = DateTime.Now;
            //商户经营范围信息
            var lstScope = new List<ScopeModel>();
            if (merchantDto.Scopes.Any())
            {
                lstScope = merchantDto.Scopes.Select(scope => new ScopeModel
                {
                    ScopeGuid = Guid.NewGuid().ToString("N"),
                    ScopeDicGuid = scope.ScopeDicGuid,
                    MerchantGuid = merchantModel.MerchantGuid,
                    PictureGuid = scope.AccessoryGuid,
                    CreatedBy = UserID,
                    OrgGuid = "",
                    LastUpdatedBy = UserID
                }).ToList();
            }
            //商户配置项证书信息 & 配置项证书附件信息
            var accessoryBiz = new AccessoryBiz();
            var lstCertificate = new List<CertificateModel>();
            var lstAccessory = new List<AccessoryModel>();
            if (merchantDto.Certificates.Any())
            {
                foreach (var certificate in merchantDto.Certificates)
                {
                    var certificateModel = new CertificateModel
                    {
                        CertificateGuid = Guid.NewGuid().ToString("N"),
                        PictureGuid = certificate.AccessoryGuid,
                        OwnerGuid = merchantModel.MerchantGuid,
                        DicGuid = certificate.DicGuid,
                        CreatedBy = UserID,
                        OrgGuid = "",
                        LastUpdatedBy = UserID
                    };
                    lstCertificate.Add(certificateModel);
                    var accModel = await accessoryBiz.GetAsync(certificate.AccessoryGuid);
                    if (accModel != null)
                    {
                        accModel.OwnerGuid = certificateModel.CertificateGuid;
                        accModel.LastUpdatedDate = DateTime.Now;
                        lstAccessory.Add(accModel);
                    }
                }
            }
            var merchantCompositeBiz = new MerchantCompositeBiz();
            var result = await merchantCompositeBiz.RegisterMerchant(merchantModel, lstScope, lstCertificate, lstAccessory, userModel, isAdd);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "商户注册数据插入不成功!");
        }

        /// <summary>
        /// 获取商户订单基本统计数据
        /// </summary>
        /// <param name="merchantGuid"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetMerchantOrderBasicStatisticsDataResponseDto>))]
        public async Task<IActionResult> GetMerchantBasicStatisticsDataAsync(string merchantGuid)
        {
            var result = await new OrderBiz().GetMerchantOrderBasicStatisticsDataAsync(merchantGuid);
            return Success(result);
        }

        /// <summary>
        /// 获取商户近期的订单数据(销售额、订单数量、商品数量)
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<List<GetMerchantOrderStatisticsDataSomeDaysResponseDto>>))]
        public async Task<IActionResult> GetMerchantOrderStatisticsDataSomeDaysAsync([FromBody]GetMerchantOrderStatisticsDataSomeDaysRequestDto requestDto)
        {
            var result = await new OrderBiz().GetMerchantOrderStatisticsDataSomeDaysAsync(requestDto.MerchantGuid, requestDto.StartDate, requestDto.EndDate);
            return Success(result);
        }

        /// <summary>
        /// 获取商户基础信息
        /// </summary>
        /// <param name="merchantGuid">商户guid</param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetMerchantBasicInfoResponseDto>))]
        public async Task<IActionResult> GetMerchantBasicInfoAsync(string merchantGuid)
        {
            if (string.IsNullOrEmpty(merchantGuid))
            {
                merchantGuid = UserID;
            }

            var model = await new MerchantBiz().GetModelAsync(merchantGuid);
            if (model == null)
            {
                return Failed(ErrorCode.Empty, "无此商户数据");
            }
            var response = model.ToDto<GetMerchantBasicInfoResponseDto>();
            var pic = await new AccessoryBiz().GetAsync(model.MerchantPicture);
            response.MerchantPictureUrl = $"{pic?.BasePath}{pic?.RelativePath}";
            return Success(response);
        }

        /// <summary>
        /// 查看当前用户是否注册商户,若注册提供注册状态
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetMerchantRegisterStateResponseDto>))]
        public async Task<IActionResult> GetMerchantRegisterState()
        {
            var merchantBiz = new MerchantBiz();
            var model = merchantBiz.GetModel(UserID);
            var result = await new ReviewRecordBiz().GetLatestReviewRecordByTargetGuidAsync(UserID, ReviewRecordModel.TypeEnum.Merchant.ToString());
            return Success(new GetMerchantRegisterStateResponseDto
            {
                WhetherRegister = model != null,
                RegisterState = model?.Status,
                ApprovalMessage = result?.RejectReason
            });
        }


        /// <summary>
        /// 获取经营范围基础数据(取自Dictionary表)
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetScopesResponseDto>>))]
        public IActionResult GetScopes()
        {
            var dictionaryBiz = new DictionaryBiz();
            var scopeMetadataDic = dictionaryBiz.GetModelById(DictionaryType.BusinessScopeDic);
            if (scopeMetadataDic == null)
            {
                return Failed(ErrorCode.DataBaseError, "缺乏经营范围元数据配置项");
            }
            var dics = dictionaryBiz.GetListByParentGuid(scopeMetadataDic.DicGuid);//.Where(a => a.ExtensionField == GD.Common.EnumDefine.PlatformType.CloudDoctor.ToString()).ToList();
            if (!dics.Any())
                return Failed(ErrorCode.Empty, "不存在配置项");
            var lstDic = dics.Select(dic => new GetScopesResponseDto
            {
                DicGuid = dic.DicGuid,
                ConfigCode = dic.ConfigCode,
                ConfigName = dic.ConfigName
            }).ToList();

            return Success(lstDic);
        }
        /// <summary>
        /// 获取商铺经营范围详情数据
        /// </summary>
        /// <param name="merchantGuid">商户guid</param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetMerchantScopeDetailResponseDto>>))]
        public async Task<IActionResult> GetMerchanScopesDetailAsync(string merchantGuid)
        {
            if (string.IsNullOrWhiteSpace(merchantGuid))
            {
                merchantGuid = UserID;
            }
            var merchantScopes = await new ScopeBiz().GetMerchantScopeDetailAsync(merchantGuid);
            return Success(merchantScopes);

        }

        /// <summary>
        /// 获取商户注册资料配置项
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetMerchantCertificateConfigResponseDto>>))]
        public IActionResult GetMerchantCertificateConfig()
        {
            var dictionaryBiz = new DictionaryBiz();
            var merchantDic = dictionaryBiz.GetModelById(DictionaryType.MerchantDicConfig);
            if (merchantDic == null)
            {
                return Failed(ErrorCode.Empty, "缺乏商户证书元数据配置项！");
            }
            var dics = dictionaryBiz.GetListByParentGuid(merchantDic.DicGuid);
            if (!dics.Any()) return Failed(ErrorCode.Empty, "不存在配置项！");
            var lstDic = dics.Select(dic => new GetMerchantCertificateConfigResponseDto
            {
                DicGuid = dic.DicGuid,
                ConfigCode = dic.ConfigCode,
                ConfigName = dic.ConfigName
            }).ToList();

            return Success(lstDic);
        }

        /// <summary>
        /// 获取商户注册资料配置项明细（所有配置项+配置项对应的值）
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<CertificateDetailDto>>))]
        public async Task<IActionResult> GetMerchantCertificateDetailAsync()
        {
            var response = await new CertificateBiz().GetCertificateDetailAsync(DictionaryType.MerchantDicConfig, UserID);
            return Success(response);
        }

        /// <summary>
        /// 审核商户注册信息
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public IActionResult AuditMerchantRegisterInfo([FromBody]AuditMerchantRegisterInfoRequestDto requestDto)
        {
            var merchantBiz = new MerchantBiz();
            var model = merchantBiz.GetModel(requestDto.MerchantGuid);
            if (model == null)
            {
                return Failed(ErrorCode.Empty, "未查到该商户数据");
            }
            model.Status = requestDto.Status.ToString();
            model.LastUpdatedBy = UserID;
            model.LastUpdatedDate = DateTime.Now;
            return merchantBiz.UpdateModel(model) ? Success() : Failed(ErrorCode.DataBaseError, "审核商户数据出现错误");

        }

        /// <summary>
        /// 判断用户是否是商户用户
        /// </summary>
        /// <param name="userGuid">用户Id</param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<bool>)), AllowAnonymous]
        public async Task<IActionResult> CheckIsMerchantUserAsync(string userGuid)
        {
            var result = await new MerchantBiz().GetModelAsync(userGuid);
            bool response = false;
            if (result != null && result.Status.ToLower() == StatusEnum.Approved.ToString().ToLower())
            {
                response = true;
            }
            return Success(response, null);
        }

        #region 商品分类接口

        /// <summary>
        /// 获取商户的商品一级分类
        /// </summary>
        /// <param name="merchantGuid">商户guid</param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<ProductCategoryDto>>))]
        public async Task<IActionResult> GetProductOneLevelCategorysOfMerchantAsync(string merchantGuid)
        {
            var result = await new ScopeBiz().GetScopeModelsByMerchantGuidAsync(merchantGuid);
            if (!result.Any())
            {
                return Failed(ErrorCode.Empty, "因商户未获取到经营范围数据，无法获取该商户的商品类别");
            }
            var scopes = result.Select(a => a.ScopeDicGuid).ToArray();
            var oneLevelCategory = await new DictionaryBiz().GetListByParentGuidsAsync(scopes);
            var productCategorys = oneLevelCategory.Select(a => new ProductCategoryDto
            {
                CategoryGuid = a.DicGuid,
                CategoryName = a.ConfigName
            }).ToList();
            return Success(productCategorys);
        }

        /// <summary>
        /// 获取商户的商品二级分类
        /// </summary>
        /// <param name="oneLevelCategoryGuid">商户guid</param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<ProductCategoryDto>>))]
        public async Task<IActionResult> GetProductTwoLevelCategorysOfMerchantByOneLevelCategoryGuidAsync(string oneLevelCategoryGuid)
        {
            var twoLevelCategory = await new DictionaryBiz().GetListByParentGuidAsync(oneLevelCategoryGuid);
            var productTwoCategorys = twoLevelCategory.Select(a => new ProductCategoryDto
            {
                CategoryGuid = a.DicGuid,
                CategoryName = a.ConfigName
            }).ToList();
            return Success(productTwoCategorys);
        }

        #endregion

        /// <summary>
        /// 获取商户的品牌数据(非分页)
        /// </summary>
        /// <param name="merchantGuid">商户guid</param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetBrandsOfMerchantResponseDto>>))]
        public async Task<IActionResult> GetBrandsOfMerchantAsync(string merchantGuid)
        {
            var models = await new BrandBiz().GetModelByMerchantIdAsync(merchantGuid);
            if (!models.Any())
            {
                return Failed(ErrorCode.Empty, "商户未配置品牌数据");
            }
            var response = models.Select(a => a.ToDto<GetBrandsOfMerchantResponseDto>()).ToList();
            return Success(response);
        }

        /// <summary>
        /// 商户创建品牌数据
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> CreateBrandForMerchantAsync([FromBody]CreateBrandForMerchantRequestDto requestDto)
        {
            var model = new BrandModel
            {
                BrandGuid = Guid.NewGuid().ToString("N"),
                BrandName = requestDto.BrandName,
                MerchantGuid = requestDto.MerchantGuid,
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
                OrgGuid = string.Empty,
                PictureGuid = requestDto.PictureGuid
            };
            var result = await new BrandBiz().AddAsync(model);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "创建品牌数据失败");
        }
        /// <summary>
        /// 移除品牌数据
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> RemoveBrandOfMerchantAsync([FromBody]RemoveBrandOfMerchantRequestDto requestDto)
        {
            var result = await new BrandBiz().RemoveAsync(requestDto.MerchantGuid, requestDto.BrandGuids);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "移除品牌数据失败");
        }
        /// <summary>
        /// 修改品牌数据
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> EditBrandAsync([FromBody]EditBrandRequestDto requestDto)
        {
            var brandBiz = new BrandBiz();
            var model = await brandBiz.GetAsync(requestDto.BrandGuid);
            if (model == null)
            {
                return Failed(ErrorCode.Empty, "未查到此品牌数据");
            }
            model.BrandName = requestDto.BrandName;
            model.PictureGuid = requestDto.PictureGuid;
            model.LastUpdatedBy = UserID;
            model.LastUpdatedDate = DateTime.Now;
            var result = await brandBiz.UpdateAsync(model);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "更新品牌数据失败");
        }

        #region 用户端预约
        /// <summary>
        /// 获取商户某天某项目的服务人员和排班详情(不分页)
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<List<GetTherapistsScheduleByProjectIdOneDayResponseDto>>))]
        public async Task<IActionResult> GetTherapistsScheduleByProjectIdOneDayAsync([FromBody]GetTherapistsScheduleByProjectIdOneDayRequestDto requestDto)
        {
            var project = await new ProjectBiz().GetAsync(requestDto.ProjectGuid);
            if (project == null)
            {
                return Failed(ErrorCode.Empty, "服务项目不存在");
            }

            //由于项目直接属于门店且无跨店逻辑，所以接口无需传入门店guid,直接从项目实体中取即可
            requestDto.MerchantGuid = project.MerchantGuid;

            var scheduleTemplate = await new ScheduleTemplateBiz().GetModelByDateAsync(requestDto.MerchantGuid, requestDto.ScheduleDate);

            if (scheduleTemplate == null)
            {
                return Failed(ErrorCode.Empty, "店铺当天未进行排班！");
            }

            var times = await GetMerchantMaxDurationTimeButtonsAsync(requestDto.MerchantGuid, scheduleTemplate.TemplateGuid);

            if (!times.Any())
            {
                return Failed(ErrorCode.Empty, "店铺未配置班次，请联系店铺处理！");
            }

            var result = await new TherapistBiz().GetTherapistsScheduleByProjectIdOneDayAsync(requestDto);

            if (result == null)
            {
                return Success();
            }

            var (currentDay, minAppointTime) = CheckCurrentDayGetMinAppointTime(requestDto.ScheduleDate);

            var groupDetails = result.ScheduleDetails.GroupBy(a => a.TherapistGuid);

            var response = new List<GetTherapistsScheduleByProjectIdOneDayResponseDto>();

            foreach (var item in result.Therapists)
            {
                var therapistDetails = (groupDetails.FirstOrDefault(a => a.Key == item.TherapistGuid))?.ToList();

                var responseItem = new GetTherapistsScheduleByProjectIdOneDayResponseDto
                {
                    TherapistGuid = item.TherapistGuid,
                    TherapistName = item.TherapistName,
                    PortraitUrl = item.PortraitUrl,
                    ScheduleGuid = therapistDetails?.FirstOrDefault()?.ScheduleGuid,
                    ScheduleDetails = new List<ScheduleTimeDetailDto>()
                };

                if (therapistDetails != null)
                {
                    foreach (var t in times)
                    {
                        var res = therapistDetails?.FirstOrDefault(a => a.StartTime != null && a.StartTime.CompareTo(t.StartTime) <= 0 && a.EndTime.CompareTo(t.StartTime) > 0);

                        var scheduleTimeDetail = new ScheduleTimeDetailDto
                        {
                            ScheduleDetailGuid = res?.ScheduleDetailGuid,
                            StartTime = t.StartTime,
                            EndTime = t.EndTime,
                            ConsumptionGuid = res?.ConsumptionGuid,
                            Occupy = res != null
                        };

                        if (currentDay && t.StartTime.CompareTo(minAppointTime) < 0)
                        {
                            scheduleTimeDetail.Occupy = true;
                        }

                        responseItem.ScheduleDetails.Add(scheduleTimeDetail);

                    }
                    response.Add(responseItem);
                }
            }
            return Success(response);
        }

        /// <summary>
        /// 获取服务人员某一天的排班详情（时间刻度列表），用于用户预约时选择服务人员的时间
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetTherapistScheduleDetailForOneDayAsyncResponseDto>))]
        public async Task<IActionResult> GetTherapistScheduleDetailForOneDayAsync([FromBody]GetTherapistScheduleDetailForOneDayRequestDto requestDto)
        {
            if (requestDto.ScheduleDate.Date < DateTime.Now.Date)
            {
                return Failed(ErrorCode.UserData, "查询的日期已过期");
            }

            var (currentDay, minAppointTime) = CheckCurrentDayGetMinAppointTime(requestDto.ScheduleDate);

            var therapistModel = await new TherapistBiz().GetAsync(requestDto.TherapistGuid);
            if (therapistModel == null)
            {
                return Failed(ErrorCode.Empty, "未查到此服务人员数据");
            }
            var scheduleTemplateModel = await new ScheduleTemplateBiz().GetModelByDateAsync(therapistModel.MerchantGuid, requestDto.ScheduleDate);
            if (scheduleTemplateModel == null)
            {
                return Failed(ErrorCode.Empty, "店铺当天未进行排班！");
            }
            List<TimeDto> lstTime = await GetMerchantMaxDurationTimeButtonsAsync(therapistModel.MerchantGuid, scheduleTemplateModel.TemplateGuid);
            if (!lstTime.Any())
            {
                return Failed(ErrorCode.Empty, "店铺未配置班次，请联系店铺处理！");
            }
            var checkModel = await new MerchantScheduleBiz().GetMerchantScheduleOfSomeOneAsync(therapistModel.MerchantGuid, requestDto.ScheduleDate, requestDto.TherapistGuid);
            bool hasSchedule = checkModel != null && checkModel.Any();
            var cheduleDetaiModels = await new MerchantScheduleDetailBiz().GetScheduleDetailByTargetGuid(requestDto.ScheduleDate, requestDto.TherapistGuid);

            var lstScheduleTimeDetail = cheduleDetaiModels.Select(a => a.ToDto<ScheduleTimeDetailDto>()).ToList();
            var timeButtons = new List<ScheduleTimeDetailDto>();
            if (hasSchedule)
            {
                foreach (var item in lstTime)
                {
                    var result = lstScheduleTimeDetail?.FirstOrDefault(a => a.StartTime != null && a.StartTime.CompareTo(item.StartTime) <= 0 && a.EndTime.CompareTo(item.StartTime) > 0);
                    var tb = new ScheduleTimeDetailDto
                    {
                        ScheduleDetailGuid = result?.ScheduleDetailGuid,
                        StartTime = item.StartTime,
                        EndTime = item.EndTime,
                        ConsumptionGuid = result?.ConsumptionGuid,
                        Occupy = result != null
                    };
                    if (currentDay && item.StartTime.CompareTo(minAppointTime) < 0)
                    {
                        tb.Occupy = true;
                    }
                    timeButtons.Add(tb);


                }
                var response = new GetTherapistScheduleDetailForOneDayAsyncResponseDto
                {
                    ScheduleGuid = checkModel?.FirstOrDefault()?.ScheduleGuid,
                    ScheduleDetails = timeButtons
                };
                return Success(response);
            }


            return Success();
        }

        /// <summary>
        /// 获取商户最大营业时间的时间刻度按钮
        /// </summary>
        /// <param name="merchantId">商户Id</param>
        /// <param name="templateGuid"></param>
        /// <param name="timeStep">时间刻度按钮间隔，默认为15分钟</param>
        /// <returns></returns>
        private async Task<List<TimeDto>> GetMerchantMaxDurationTimeButtonsAsync(string merchantId, string templateGuid, int timeStep = 15)
        {
            var maxDuration = await new MerchantWorkShiftDetailBiz().GetMaxDuration(merchantId, templateGuid);
            if (maxDuration == null)
            {
                return new List<TimeDto>();
            }
            string businessHoursStart = Convert.ToDateTime(maxDuration.StartTime).ToString("HH:mm");
            string businessHourEnd = Convert.ToDateTime(maxDuration.EndTime).ToString("HH:mm");
            DateTime dtStart = Convert.ToDateTime(businessHoursStart);
            DateTime dtEnd = Convert.ToDateTime(businessHourEnd);
            DateTime nextTime = dtStart;
            List<TimeDto> lstTime = new List<TimeDto>();
            while (nextTime < dtEnd)
            {
                lstTime.Add(new TimeDto { StartTime = nextTime.ToString("HH:mm"), EndTime = nextTime.AddMinutes(timeStep).ToString("HH:mm") });
                nextTime = nextTime.AddMinutes(timeStep);
            }
            return lstTime;

        }

        /// <summary>
        /// 检测当前排班是否是当天，并获取当天能够预约的最早时间
        /// </summary>
        /// <param name="scheduleDate"></param>
        /// <returns></returns>
        (bool currentDay, string minAppointTime) CheckCurrentDayGetMinAppointTime(DateTime scheduleDate)
        {
            var currentDate = DateTime.Now;

            var minAppointTime = currentDate.AddMinutes(30).ToString("HH:mm");

            if (currentDate.AddMinutes(30).Date > currentDate.Date)
            {
                minAppointTime = "23:59";
            }

            var isCurrentDay = scheduleDate.Date == currentDate.Date;

            return (isCurrentDay, minAppointTime);
        }
        #endregion

        #region 商户类别管理

        /// <summary>
        /// 获取服务型商品类别
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetServiceClassifysResponseDto>>))]
        public async Task<IActionResult> GetServiceClassifys()
        {
            var classify = await new DictionaryBiz().GetAsync(DictionaryType.ServiceClassifyGuid);

            return Success(new List<GetServiceClassifysResponseDto>() { new GetServiceClassifysResponseDto()
            {
                Id = classify.DicGuid,
                Name = classify.ConfigName
            } });
        }

        /// <summary>
        /// 获取指定商户服务类型下二级分类列表
        /// </summary>
        /// <param name="classifyGuid"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetServiceClassifysResponseDto>>))]
        public async Task<IActionResult> GetServiceTwoLevelClassifys(string classifyGuid)
        {
            var categoryBiz = new MerchantCategoryBiz();

            return Success(await categoryBiz.GetServiceTwoLevelClassifys(UserID, classifyGuid));
        }

        /// <summary>
        /// 获取服务类型二级分类列表(返回{id:'', name:''}集合)
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<object>>))]
        public async Task<IActionResult> GetClassifys()
        {
            var categoryBiz = new MerchantCategoryBiz();

            return Success(await categoryBiz.GetClassifies());
        }

        /// <summary>
        /// 获取指定商户平台二级分类列表(返回{id:'', name:''}集合)
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<object>>))]
        public async Task<IActionResult> GetMerchantClassifys()
        {
            var categoryBiz = new MerchantCategoryBiz();

            return Success(await categoryBiz.GetClassifies(UserID, true));
        }

        /// <summary>
        /// 获取商户指定二级分类下类别列表(返回{id:'', name:''}集合)
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<object>>))]
        public async Task<IActionResult> GetCategoriesByClassify(string classifyGuid)
        {
            var categoryBiz = new MerchantCategoryBiz();

            return Success(await categoryBiz.GetCategoriesByClassify(UserID, classifyGuid));
        }

        /// <summary>
        /// 获取类别详细信息
        /// </summary>
        /// <param name="categoryGuid"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetMerchantCategoryDetailInfoResponseDto>))]
        public async Task<IActionResult> GetMerchantCategoryDetailInfo(string categoryGuid)
        {
            if (string.IsNullOrEmpty(categoryGuid))
            {
                return Failed(ErrorCode.Empty, "参数不正确");
            }

            return Success(await new MerchantCategoryBiz().GetMerchantCategoryDetailInfo(UserID, categoryGuid));
        }

        /// <summary>
        /// 获取类别列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetMerchantCategoryListRequestDto>>))]
        public async Task<IActionResult> GetCategories([FromQuery]GetMerchantCategoryListRequestDto requestDto)
        {
            var categoryBiz = new MerchantCategoryBiz();

            requestDto.MerchantGuid = UserID;

            return Success(await categoryBiz.GetCategories(requestDto));
        }

        /// <summary>
        /// 创建类别
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<bool>))]
        public async Task<IActionResult> CreateCategory([FromBody]AddMerchantCategoryRequestDto category)
        {
            if (category.Address?.Count() <= 0)
            {
                return Failed(ErrorCode.FormatError, "地址未选择");
            }

            var regex = new Regex("^[0-9]+$");
            if (!regex.IsMatch(category.Telephone))
            {
                return Failed(ErrorCode.FormatError, "联系电话格式不正确");
            }

            if (category.Banners.Count <= 0)
            {
                return Failed(ErrorCode.FormatError, "未上传主图");
            }

            if (category.Banners.Any(d => string.IsNullOrEmpty(d.Url) || string.IsNullOrEmpty(d.Value)))
            {
                return Failed(ErrorCode.FormatError, "主图数据不正确");
            }

            if (category.LimitTime <= 0)
            {
                return Failed(ErrorCode.FormatError, "预约取消时间必须大于0");
            }

            if (category.LongLatitude.Count() <= 0)
            {
                return Failed(ErrorCode.FormatError, "经纬度必填");
            }

            var model = category.ToModel<CategoryExtensionModel>();
            model.Address = JsonConvert.SerializeObject(category.Address);

            if (category.LongLatitude.Length < 2)
            {
                return Failed(ErrorCode.FormatError, "经纬度格式不正确");
            }

            decimal.TryParse(category.LongLatitude[0], out var latitude);
            if (latitude <= 0)
            {
                return Failed(ErrorCode.FormatError, "维度格式不正确");
            }

            decimal.TryParse(category.LongLatitude[1], out var longtitude);
            if (longtitude <= 0)
            {
                return Failed(ErrorCode.FormatError, "经度格式不正确");
            }

            model.Longitude = longtitude;
            model.Latitude = latitude;

            var categoryBiz = new MerchantCategoryBiz();

            if (await categoryBiz.MerchantUnitqueClassify(UserID, category.ClassifyGuid))
            {
                return Failed(ErrorCode.FormatError, "已在同一平台大类下创建大类");
            }

            var exsitName = await categoryBiz.ExistCategoryName(UserID, category.CategoryName.Trim());

            if (exsitName)
            {
                return Failed(ErrorCode.FormatError, $"类别“{category.CategoryName}”已存在");
            }

            model.MerchantGuid = UserID;
            model.LimitTime = category.LimitTime;
            model.CategoryGuid = Guid.NewGuid().ToString("N");
            model.CreatedBy = UserID;
            model.LastUpdatedBy = UserID;
            model.OrgGuid = "";

            var banners = category.Banners.Select((d, i) => new BannerModel()
            {
                BannerGuid = Guid.NewGuid().ToString("N"),
                BannerName = "",
                OwnerGuid = model.CategoryGuid,
                TargetUrl = d.Url,
                PictureGuid = d.Value,
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
                OrgGuid = "",
                Sort = i
            }).ToList();

            var result = await categoryBiz.CreateCategoryAsync(model, banners);

            return result ? Success()
                : Failed(ErrorCode.DataBaseError, "类别创建失败！");
        }

        /// <summary>
        /// 更新类别
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<bool>))]
        public async Task<IActionResult> UpdateCategory([FromBody] AddMerchantCategoryRequestDto category)
        {
            if (string.IsNullOrEmpty(category.CategoryGuid))
            {
                return Failed(ErrorCode.UserData, "参数不正确");
            }

            var regex = new Regex("^[0-9]+$");
            if (!regex.IsMatch(category.Telephone))
            {
                return Failed(ErrorCode.FormatError, "联系电话格式不正确");
            }

            if (category.Address?.Count() <= 0)
            {
                return Failed(ErrorCode.FormatError, "地址未选择");
            }

            if (category.Banners.Count <= 0)
            {
                return Failed(ErrorCode.FormatError, "未上传主图");
            }

            if (category.Banners.Any(d => string.IsNullOrEmpty(d.Url) || string.IsNullOrEmpty(d.Value)))
            {
                return Failed(ErrorCode.FormatError, "主图数据不正确");
            }

            if (category.LimitTime <= 0)
            {
                return Failed(ErrorCode.FormatError, "预约取消时间必须大于0");
            }

            var categoryBiz = new MerchantCategoryBiz();

            if (await categoryBiz.MerchantUnitqueClassify(UserID, category.ClassifyGuid, category.CategoryGuid))
            {
                return Failed(ErrorCode.FormatError, "已在同一平台大类下创建大类");
            }

            var model = await categoryBiz.GetCategoryModelById(UserID, category.CategoryGuid);

            if (model is null)
            {
                return Failed(ErrorCode.UserData, $"类别“{category.CategoryName}”不存在");
            }

            var exsitName = await categoryBiz.ExistCategoryName(UserID, category.CategoryName.Trim(), category.CategoryGuid);

            if (exsitName)
            {
                return Failed(ErrorCode.UserData, $"类别“{category.CategoryName}”已存在,请修改");
            }

            decimal.TryParse(category.LongLatitude[0], out var latitude);
            if (latitude <= 0)
            {
                return Failed(ErrorCode.FormatError, "维度格式不正确");
            }

            decimal.TryParse(category.LongLatitude[1], out var longtitude);
            if (longtitude <= 0)
            {
                return Failed(ErrorCode.FormatError, "经度格式不正确");
            }

            model.Longitude = longtitude;
            model.Latitude = latitude;
            model.ClassifyName = category.ClassifyName;
            model.ClassifyGuid = category.ClassifyGuid;
            model.CoverGuid = category.CoverGuid;
            model.LimitTime = category.LimitTime;
            model.CategoryName = category.CategoryName;
            model.Telephone = category.Telephone;
            model.Introduction = category.Introduction;
            model.Address = JsonConvert.SerializeObject(category.Address);
            model.DetailAddress = category.DetailAddress;
            model.LastUpdatedBy = UserID;
            model.LastUpdatedDate = DateTime.Now;

            var banners = category.Banners.Select((d, i) => new BannerModel()
            {
                BannerGuid = Guid.NewGuid().ToString("N"),
                BannerName = "",
                OwnerGuid = model.CategoryGuid,
                TargetUrl = d.Url,
                PictureGuid = d.Value,
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
                OrgGuid = "",
                Sort = i
            }).ToList();

            var result = await categoryBiz.UpdateCategoryAsync(model, banners);

            return result ? Success() : Failed(ErrorCode.DataBaseError, "类别更新失败！");
        }
        /// <summary>
        /// 是否上线类别
        /// </summary>
        /// <param name="categoryGuid"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<bool>))]
        public async Task<IActionResult> OnlieCategory(string categoryGuid)
        {
            if (string.IsNullOrEmpty(categoryGuid))
            {
                return Failed(ErrorCode.UserData, "参数不正确");
            }

            var categoryBiz = new MerchantCategoryBiz();

            var model = await categoryBiz.GetCategoryModelById(UserID, categoryGuid);

            if (model is null)
            {
                return Failed(ErrorCode.UserData, "类别不存在");
            }

            model.Enable = !model.Enable;

            var result = await categoryBiz.UpdateAsync(model);

            return result ? Success() : Failed(ErrorCode.DataBaseError, "更新失败！");
        }
        #endregion

        #region 商户服务项目管理

        /// <summary>
        /// 获取服务项目列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetMerchantProjectResponseDto>>))]
        public async Task<IActionResult> GetMerchantProjects([FromQuery]GetMerchantProjectListRequestDto requestDto)
        {
            var projectBiz = new ProjectBiz();

            requestDto.MerchantGuid = UserID;
            return Success(await projectBiz.GetMerchantProjects(requestDto));
        }

        /// <summary>
        /// 创建服务项目
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<bool>))]
        public async Task<IActionResult> CreateMerchantProject([FromBody]
        AddMerchantProjectRequestDto project)
        {
            if (project.OperationTime <= 0)
            {
                return Failed(ErrorCode.UserData, "服务时长需大于0");
            }

            if (project.Price <= 0)
            {
                return Failed(ErrorCode.UserData, "价格需大于0");
            }

            var model = project.ToModel<ProjectModel>();

            var projectBiz = new ProjectBiz();

            var exsitName = await projectBiz.ExistMerchantProjectName(UserID, project.ProjectName.Trim());

            if (exsitName)
            {
                return Failed(ErrorCode.UserData, $"服务项目“{project.ProjectName}”已存在");
            }

            model.MerchantGuid = UserID;
            model.ProjectGuid = Guid.NewGuid().ToString("N");
            model.CreatedBy = UserID;
            model.LastUpdatedBy = UserID;
            model.OrgGuid = "";

            var result = await projectBiz.InsertAsync(model);

            return result ? Success()
                : Failed(ErrorCode.DataBaseError, "服务项目创建失败！");
        }

        /// <summary>
        /// 更新服务项目
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<bool>))]
        public async Task<IActionResult> UpdateMerchantProject([FromBody] AddMerchantProjectRequestDto project)
        {
            if (string.IsNullOrEmpty(project.ProjectGuid))
            {
                return Failed(ErrorCode.UserData, "参数不正确");
            }

            if (project.OperationTime <= 0)
            {
                return Failed(ErrorCode.UserData, "服务时长需大于0");
            }

            if (project.Price <= 0)
            {
                return Failed(ErrorCode.UserData, "价格需大于0");
            }

            var projectBiz = new ProjectBiz();

            var model = await projectBiz.GetMerchantPorjectModelById(UserID, project.ProjectGuid);

            if (model is null)
            {
                return Failed(ErrorCode.UserData, $"服务项目“{project.ProjectName}”不存在");
            }

            var exsitName = await projectBiz.ExistMerchantProjectName(UserID, project.ProjectName.Trim(), project.ProjectGuid);

            if (exsitName)
            {
                return Failed(ErrorCode.UserData, $"服务项目“{project.ProjectName}”已存在,请修改");
            }

            model.ClassifyName = project.ClassifyName;
            model.ClassifyGuid = project.ClassifyGuid;
            model.ProjectName = project.ProjectName;
            model.OperationTime = project.OperationTime;
            model.Price = project.Price;
            model.LastUpdatedBy = UserID;
            model.LastUpdatedDate = DateTime.Now;

            var result = await projectBiz.UpdateAsync(model);

            return result ? Success() : Failed(ErrorCode.DataBaseError, "服务项目更新失败！");
        }

        /// <summary>
        /// 删除服务项目
        /// </summary>
        /// <param name="projectGuid"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<bool>))]
        public async Task<IActionResult> DeleteMerchantProject(string projectGuid)
        {
            if (string.IsNullOrEmpty(projectGuid))
            {
                return Failed(ErrorCode.UserData, "参数不正确");
            }

            var projectBiz = new ProjectBiz();

            var model = await projectBiz.GetMerchantPorjectModelById(UserID, projectGuid);

            if (model is null)
            {
                return Failed(ErrorCode.UserData, "服务项目不存在");
            }

            model.Enable = false;

            var result = await projectBiz.DeleteMerchantProject(model);

            return result ? Success() : Failed(ErrorCode.DataBaseError, "服务项目删除失败！");
        }
        #endregion

        /// <summary>
        /// 服务人员锁定排班时间
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<bool>))]
        public async Task<IActionResult> LockScheduleDetailTimesAsync([FromBody]LockScheduleDetailTimesRequestDto requestDto)
        {
            var therapistModel = await new MerchantScheduleBiz().GetModelAsync(requestDto.ScheduleGuid);
            if (therapistModel == null)
            {
                return Failed(ErrorCode.Empty, "未查到此服务人员排班数据");
            }
            if (requestDto.LockTimes == null || !requestDto.LockTimes.Any())
            {
                return Failed();
            }
            var lstScheduleDetail = new List<MerchantScheduleDetailModel>();
            foreach (var item in requestDto.LockTimes)
            {
                lstScheduleDetail.Add(new MerchantScheduleDetailModel
                {
                    ScheduleDetailGuid = Guid.NewGuid().ToString("N"),
                    ScheduleGuid = requestDto.ScheduleGuid,
                    StartTime = item.StartTime,
                    EndTime = item.EndTime,
                    ConsumptionGuid = string.Empty,
                    CreatedBy = UserID,
                    LastUpdatedBy = UserID
                });
            }
            var response = await new MerchantScheduleDetailBiz().LockScheduleDetailTimesAsync(lstScheduleDetail, requestDto.ScheduleGuid);
            return Success(response);

        }

        /// <summary>
        /// 解锁服务人员某天的锁定时间
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<bool>))]
        public async Task<IActionResult> UnLockScheduleDetailTimesAsync([FromBody]UnLockScheduleDetailTimesRequestDto requestDto)
        {
            var scheduleModel = await new MerchantScheduleBiz().GetModelAsync(requestDto.ScheduleGuid);
            if (scheduleModel == null)
            {
                return Failed(ErrorCode.Empty, "未查到排班数据");
            }
            var merchantScheduleDetailBiz = new MerchantScheduleDetailBiz();
            var scheduleDetailModels = await merchantScheduleDetailBiz.GetModelsAsync(requestDto.ScheduleDetailGuids.ToArray(), requestDto.ScheduleGuid);
            if (scheduleDetailModels.Where(a => a.ConsumptionGuid.CompareTo("Lock") != 0).Count() > 0)
            {
                return Failed(ErrorCode.UserData, "不可以选择未锁定的时间进行解锁");
            }
            var response = await merchantScheduleDetailBiz.DeleteAsync(requestDto.ScheduleDetailGuids.ToArray(), requestDto.ScheduleGuid);
            return Success(response);
        }

        #region 商户班次模板管理

        /// <summary>
        /// 获取班次模板分页列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetWorkShiftTemplatePageListResponseDto>))]
        public async Task<IActionResult> GetWorkShiftTemplatePageListAsync([FromBody]GetWorkShiftTemplatePageListRequestDto requestDto)
        {
            requestDto.MerchantGuid = string.IsNullOrWhiteSpace(requestDto.MerchantGuid) ? UserID ?? "" : requestDto.MerchantGuid;
            if (string.IsNullOrWhiteSpace(requestDto.MerchantGuid))
            {
                return Failed(ErrorCode.UserData, "商户guid必填");
            }
            var response = await new MerchantWorkShiftTemplateBiz().GetWorkShiftTemplatePageListAsync(requestDto);
            return Success(response);
        }

        /// <summary>
        /// 获取班次模板下的班次详情
        /// </summary>
        /// <param name="templateGuid"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetWorkShiftDetailsOfTemplateGuidResponseDto>>)), AllowAnonymous]
        public async Task<IActionResult> GetWorkShiftDetailsOfTemplateGuidAsync(string templateGuid)
        {
            if (string.IsNullOrEmpty(templateGuid))
            {
                return Failed(ErrorCode.Empty, "参数不正确");
            }

            var workShiftTemplateBiz = new MerchantWorkShiftTemplateBiz();

            var templateModel = await workShiftTemplateBiz.GetModelAsync(templateGuid);
            if (templateModel is null)
            {
                return Failed(ErrorCode.Empty, "未查询到此班次模板数据");
            }

            var workShiftDetailBiz = new MerchantWorkShiftDetailBiz();

            var details = await workShiftDetailBiz.GetWorkShiftDetailsOfTemplateGuidAsync(templateGuid);

            if (details?.Count <= 0)
            {
                return Success(new List<GetWorkShiftDetailsOfTemplateGuidResponseDto>());
            }

            var groups = details.GroupBy(a => new { a.WorkShiftGuid, a.WorkShiftName });

            var response = groups.Select(item => new GetWorkShiftDetailsOfTemplateGuidResponseDto
            {
                WorkShiftGuid = item.Key.WorkShiftGuid,
                WorkShiftName = item.Key.WorkShiftName,
                WorkShiftDetailTimes = item.Select(t => new TimeDto
                {
                    StartTime = t.StartTime,
                    EndTime = t.EndTime
                }).OrderBy(t => t.StartTime).ToList()
            }).OrderBy(a => a.WorkShiftDetailTimes.FirstOrDefault().StartTime).ToList();

            return Success(response);
        }

        /// <summary>
        /// 新增班次模板
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> AddWorkShiftTemplateAsync([FromBody]AddWorkShiftTemplateRequestDto requestDto)
        {
            if (requestDto.WorkShifts?.Count <= 0)
            {
                return Failed(ErrorCode.Empty, "班次数据未提供");
            }

            requestDto.MerchantGuid = string.IsNullOrWhiteSpace(requestDto.MerchantGuid) ? UserID ?? "" : requestDto.MerchantGuid;

            //校验商户是否存在
            var merchantModel = await new MerchantBiz().GetModelAsync(requestDto.MerchantGuid);
            if (merchantModel is null)
            {
                return Failed(ErrorCode.Empty, "商户不存在");
            }

            var shiftTemplateBiz = new MerchantWorkShiftTemplateBiz();

            //校验商户模板名称是否存在
            if (await shiftTemplateBiz.IsExistTemplate(requestDto.MerchantGuid, requestDto.TemplateName))
            {
                return Failed(ErrorCode.Empty, $"模板名称“{requestDto.TemplateName}”已存在，请修改");
            }

            var templateModel = new MerchantWorkShiftTemplateModel
            {
                TemplateGuid = Guid.NewGuid().ToString("N"),
                TemplateName = requestDto.TemplateName,
                MerchantGuid = requestDto.MerchantGuid,
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
                OrgGuid = string.Empty
            };

            var workShiftModels = new List<MerchantWorkShiftModel>();

            var workShiftDetailModels = new List<MerchantWorkShiftDetailModel>();

            foreach (var workShift in requestDto.WorkShifts)
            {
                if (workShiftModels.Any(d => d.WorkShiftName == workShift.WorkShiftName))
                {
                    return Failed(ErrorCode.Empty, $"相同班次名称“{workShift.WorkShiftName}”已存在");
                }

                if (workShift?.WorkShiftDetailTimes?.Count <= 0)
                {
                    return Failed(ErrorCode.Empty, $"班次名称“{workShift.WorkShiftName}”班次时间段未提供");
                }

                var workShiftModel = new MerchantWorkShiftModel
                {
                    WorkShiftGuid = Guid.NewGuid().ToString("N"),
                    WorkShiftName = workShift.WorkShiftName,
                    TemplateGuid = templateModel.TemplateGuid,
                    MerchantGuid = requestDto.MerchantGuid,
                    CreatedBy = UserID,
                    LastUpdatedBy = UserID,
                    OrgGuid = string.Empty
                };

                workShiftModels.Add(workShiftModel);

                foreach (var workShiftDetail in workShift.WorkShiftDetailTimes)
                {
                    DateTime.TryParse(workShiftDetail.StartTime, out var startTime);
                    DateTime.TryParse(workShiftDetail.EndTime, out var endTime);

                    if (startTime == DateTime.MinValue)
                    {
                        return Failed(ErrorCode.Empty, $"班次名称“{workShift.WorkShiftName}”开始时间格式不正确");
                    }

                    if (endTime == DateTime.MinValue)
                    {
                        return Failed(ErrorCode.Empty, $"班次名称“{workShift.WorkShiftName}”结束时间格式不正确");
                    }

                    workShiftDetailModels.Add(new MerchantWorkShiftDetailModel()
                    {
                        WorkShiftDetailGuid = Guid.NewGuid().ToString("N"),
                        WorkShiftGuid = workShiftModel.WorkShiftGuid,
                        StartTime = startTime.ToString("HH:mm"),
                        EndTime = endTime.ToString("HH:mm"),
                        CreatedBy = UserID,
                        LastUpdatedBy = UserID,
                        OrgGuid = string.Empty
                    });
                }
            }

            var result = await shiftTemplateBiz.AddWorkShiftTemplateAsync(templateModel, workShiftModels, workShiftDetailModels);

            return result ? Success() : Failed(ErrorCode.DataBaseError, "新增商户班次模板数据失败！");
        }

        /// <summary>
        /// 修改班次模板
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> ModifyWorkShiftTemplateAsync([FromBody]ModifyWorkShiftTemplateRequestDto requestDto)
        {
            if (requestDto.WorkShifts?.Count <= 0)
            {
                return Failed(ErrorCode.Empty, "班次数据未提供");
            }

            var workShiftTemplateBiz = new MerchantWorkShiftTemplateBiz();

            var templateModel = await workShiftTemplateBiz.GetModelAsync(requestDto.TemplateGuid);
            if (templateModel is null)
            {
                return Failed(ErrorCode.Empty, "未查询到此班次模板数据");
            }

            templateModel.TemplateName = requestDto.TemplateName;

            //校验商户模板是否已应用于排班
            var check = await workShiftTemplateBiz.CheckTemplateUsed(requestDto.TemplateGuid);
            if (check)
            {
                return Failed(ErrorCode.UserData, $"模板“{requestDto.TemplateName}”已在排班中使用过，不能进行修改");
            }

            //校验商户模板名称是否存在
            if (await workShiftTemplateBiz.IsExistTemplate(UserID, requestDto.TemplateName, requestDto.TemplateGuid))
            {
                return Failed(ErrorCode.Empty, $"模板名称“{requestDto.TemplateName}”已存在，请修改");
            }

            var workShiftModels = new List<MerchantWorkShiftModel>();

            var workShiftDetailModels = new List<MerchantWorkShiftDetailModel>();

            foreach (var workShift in requestDto.WorkShifts)
            {
                if (workShiftModels.Any(d => d.WorkShiftName == workShift.WorkShiftName))
                {
                    return Failed(ErrorCode.Empty, $"相同班次名称“{workShift.WorkShiftName}”已存在");
                }

                if (workShift.WorkShiftDetailTimes?.Count <= 0)
                {
                    return Failed(ErrorCode.Empty, $"班次名称“{workShift.WorkShiftName}”班次时间段未提供");
                }

                var workShiftModel = new MerchantWorkShiftModel
                {
                    WorkShiftGuid = Guid.NewGuid().ToString("N"),
                    WorkShiftName = workShift.WorkShiftName,
                    TemplateGuid = templateModel.TemplateGuid,
                    MerchantGuid = templateModel.MerchantGuid,
                    CreatedBy = UserID,
                    LastUpdatedBy = UserID,
                    OrgGuid = string.Empty
                };

                workShiftModels.Add(workShiftModel);

                foreach (var workShiftDetail in workShift.WorkShiftDetailTimes)
                {
                    DateTime.TryParse(workShiftDetail.StartTime, out var startTime);
                    DateTime.TryParse(workShiftDetail.EndTime, out var endTime);

                    if (startTime == DateTime.MinValue)
                    {
                        return Failed(ErrorCode.Empty, $"班次名称“{workShift.WorkShiftName}”开始时间格式不正确");
                    }

                    if (endTime == DateTime.MinValue)
                    {
                        return Failed(ErrorCode.Empty, $"班次名称“{workShift.WorkShiftName}”结束时间格式不正确");
                    }

                    workShiftDetailModels.Add(new MerchantWorkShiftDetailModel()
                    {
                        WorkShiftDetailGuid = Guid.NewGuid().ToString("N"),
                        WorkShiftGuid = workShiftModel.WorkShiftGuid,
                        StartTime = startTime.ToString("HH:mm"),
                        EndTime = endTime.ToString("HH:mm"),
                        CreatedBy = UserID,
                        LastUpdatedBy = UserID,
                        OrgGuid = string.Empty
                    });
                }
            }

            var result = await workShiftTemplateBiz.ModifyWorkShiftTemplateAsync(templateModel, workShiftModels, workShiftDetailModels);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "修改商户班次模板数据失败！");

        }
        #endregion

        #region 商户排班管理

        /// <summary>
        /// 获取当月和下月排班周期数据
        /// </summary>
        /// <param name="merchantGuid">商户guid选填，默认为当前登录的商户guid</param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetCurrentMonthAndNexMonthSchduleTemplateButtonResponseDto>>))]
        public async Task<IActionResult> GetCurrentMonthAndNexMonthSchduleTemplateButtonAsync(string merchantGuid)
        {
            merchantGuid = string.IsNullOrWhiteSpace(merchantGuid) ? UserID ?? "" : merchantGuid;

            var scheduleTemplateBiz = new ScheduleTemplateBiz();
            var workShiftTemplateBiz = new MerchantWorkShiftTemplateBiz();
            var currentDate = DateTime.Now;
            var currentMonthFirstDayDate = new DateTime(currentDate.Year, currentDate.Month, 1);
            var nextMonthFirstDayDate = currentMonthFirstDayDate.AddMonths(1);
            //var nextMonthFirstDayDate = new DateTime(currentDate.Year, currentDate.Month + 1, 1);
            var currentScheduleTemplate = await scheduleTemplateBiz.GetModelByDateAsync(merchantGuid, currentMonthFirstDayDate.Date);
            var nextScheduleTemplate = await scheduleTemplateBiz.GetModelByDateAsync(merchantGuid, nextMonthFirstDayDate.Date);
            var currentTemplate = await workShiftTemplateBiz.GetModelAsync(currentScheduleTemplate?.TemplateGuid);
            var nextTemplate = await workShiftTemplateBiz.GetModelAsync(nextScheduleTemplate?.TemplateGuid);
            var response = new List<GetCurrentMonthAndNexMonthSchduleTemplateButtonResponseDto>
            {
                new GetCurrentMonthAndNexMonthSchduleTemplateButtonResponseDto
                {
                    MonthText = $"{currentMonthFirstDayDate.Month}月份",
                    StarDate = currentMonthFirstDayDate,
                    EndDate = currentMonthFirstDayDate.AddMonths(1).AddDays(-1),
                    ScheduleState = currentScheduleTemplate != null,
                    ScheduleTemplateGuid = currentScheduleTemplate?.ScheduleTemplateGuid,
                    TemplateName=currentTemplate?.TemplateName
                },
                new GetCurrentMonthAndNexMonthSchduleTemplateButtonResponseDto
                {
                    MonthText = $"{nextMonthFirstDayDate.Month}月份",
                    StarDate = nextMonthFirstDayDate,
                    EndDate = nextMonthFirstDayDate.AddMonths(1).AddDays(-1),
                    ScheduleState = nextScheduleTemplate != null,
                    ScheduleTemplateGuid = nextScheduleTemplate?.ScheduleTemplateGuid,
                    TemplateName=nextTemplate?.TemplateName
                }
            };
            return Success(response);

        }

        /// <summary>
        /// 获取商户指定模板周期的排班日历
        /// </summary>
        /// <param name="scheduleTemplateGuid">排班周期guid</param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetMerchantSchduleCalendarResponseDto>>))]
        public async Task<IActionResult> GetMerchantSchduleCalendarAsync(string scheduleTemplateGuid)
        {
            var details = await new MerchantScheduleBiz().GetMerchantSchduleCalendarDetailAsync(scheduleTemplateGuid);
            var groups = details.GroupBy(a => a.ScheduleDate).OrderBy(a => a.Key);
            List<GetMerchantSchduleCalendarResponseDto> scheduleModels = new List<GetMerchantSchduleCalendarResponseDto>();
            foreach (var item in groups)
            {
                var groupDetails = item.ToList().GroupBy(a => new { a.WorkShiftGuid, a.WorkShiftName, a.WorkShiftTimeDuration }).OrderBy(a => a.Key.WorkShiftTimeDuration);
                scheduleModels.Add(new GetMerchantSchduleCalendarResponseDto
                {
                    ScheduleDate = item.Key.Date,
                    Details = groupDetails.Select(a => new MerchantSchduleCalendarWorkShiftDto
                    {
                        WorkShiftGuid = a.Key.WorkShiftGuid,
                        WorkShiftName = a.Key.WorkShiftName,
                        WorkShiftTimeDuration = a.Key.WorkShiftTimeDuration,
                        Therapists = a.Where(b => b.TherapistGuid != null).OrderBy(b => b.WorkShiftTimeDuration).Select(b => new MerchantSchduleCalendarTherapistDto
                        {
                            TherapistGuid = b.TherapistGuid,
                            TherapistName = b.TherapistName
                        }).ToList()
                    }).ToList()
                });
            }
            return Success(scheduleModels);
        }

        /// <summary>
        /// 商户批次安排服务人员排班（一次提交一个周期；细节性排班）
        /// 商户对每一天每个班次进行排班调整
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> ScheduleTherapistsWorkShiftInBatchesAsync([FromBody]ScheduleTherapistsWorkShiftInBatchesResquestDto requestDto)
        {
            var scheduleTemplateModel = new ScheduleTemplateModel
            {
                ScheduleTemplateGuid = Guid.NewGuid().ToString("N"),
                MerchantGuid = UserID,
                StartDate = requestDto.StartDate.Date,
                EndDate = requestDto.EndDate.Date,
                TemplateGuid = requestDto.TemplateGuid,
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
                OrgGuid = string.Empty
            };

            var merchantWorkShiftDetialModels = await new MerchantWorkShiftDetailBiz().GetModelsByTemplateGuidAsync(requestDto.TemplateGuid);
            List<TimeDto> lstTime = await GetMerchantMaxDurationTimeButtonsAsync(UserID, requestDto.TemplateGuid);
            if (!lstTime.Any())
            {
                return Failed(ErrorCode.Empty, "店铺未配置班次！");
            }
            List<MerchantScheduleModel> schedulemodel = new List<MerchantScheduleModel>();
            List<MerchantScheduleDetailModel> scheduleDetails = new List<MerchantScheduleDetailModel>();

            foreach (var item in requestDto.TherapistWorkShifts)
            {
                var merchantScheduleModel = new MerchantScheduleModel
                {
                    ScheduleGuid = Guid.NewGuid().ToString("N"),
                    ScheduleDate = item.ScheduleDate.Date,
                    MerchantGuid = UserID,
                    TargetGuid = item.TherapistGuid,
                    TargetType = ScheduleTargetType.Therapist.ToString(),
                    ScheduleTemplateGuid = scheduleTemplateModel.ScheduleTemplateGuid,
                    WorkShiftGuid = item.WorkShiftGuid,
                    PlatformType = GD.Common.EnumDefine.PlatformType.LifeCosmetology.ToString(),
                    CreatedBy = UserID,
                    LastUpdatedBy = UserID,
                    OrgGuid = string.Empty
                };
                schedulemodel.Add(merchantScheduleModel);
                lstTime.ForEach(n =>
                {
                    if (merchantWorkShiftDetialModels.FirstOrDefault(a => a.WorkShiftGuid == item.WorkShiftGuid && a.StartTime.CompareTo(n.StartTime) <= 0 && a.EndTime.CompareTo(n.StartTime) > 0) == null)
                    {
                        scheduleDetails.Add(new MerchantScheduleDetailModel
                        {
                            ScheduleDetailGuid = Guid.NewGuid().ToString("N"),
                            ScheduleGuid = merchantScheduleModel.ScheduleGuid,
                            StartTime = n.StartTime,
                            EndTime = n.EndTime,
                            ConsumptionGuid = "Lock",
                            CreatedBy = UserID,
                            LastUpdatedBy = UserID,
                            OrgGuid = string.Empty
                        });
                    }
                });
            }
            var result = await new ScheduleTemplateBiz().ScheduleTherapistsWorkShiftInBatchesAsync(scheduleTemplateModel, schedulemodel, scheduleDetails);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "批量安排排班出错");

        }

        /// <summary>
        /// 商户批次安排服务人员（一次提交一个周期，将一天的排班复制到整个周期上）
        /// 商户只需安排一天的班次
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> ScheduleTherapistsWorkShiftInCopyBatchesAsync([FromBody]ScheduleTherapistsWorkShiftInCopyBatchesRequestDto requestDto)
        {
            var checkHasTherapist = requestDto.TherapistWorkShifts.FirstOrDefault(a => a.TherapistGuids.Count > 0);
            if (checkHasTherapist == null)
            {
                return Failed(ErrorCode.UserData, "未选择排班的服务人员，请先选择服务人员");
            }

            var scheduleTemplateBiz = new ScheduleTemplateBiz();
            var check = await scheduleTemplateBiz.CheckScheduleTemplateCrossAsync(UserID, requestDto.StartDate, requestDto.EndDate);
            if (check) return Failed(ErrorCode.UserData, "其实日期与其他排班周期重叠，请重新选择");
            var scheduleTemplateModel = new ScheduleTemplateModel
            {
                ScheduleTemplateGuid = Guid.NewGuid().ToString("N"),
                MerchantGuid = UserID,
                StartDate = requestDto.StartDate.Date,
                EndDate = requestDto.EndDate.Date,
                TemplateGuid = requestDto.TemplateGuid,
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
                OrgGuid = string.Empty
            };

            var merchantWorkShiftDetialModels = await new MerchantWorkShiftDetailBiz().GetModelsByTemplateGuidAsync(requestDto.TemplateGuid);
            List<TimeDto> lstTime = await GetMerchantMaxDurationTimeButtonsAsync(UserID, requestDto.TemplateGuid);
            if (!lstTime.Any())
            {
                return Failed(ErrorCode.Empty, "店铺未配置班次！");
            }
            List<MerchantScheduleModel> schedulemodel = new List<MerchantScheduleModel>();
            List<MerchantScheduleDetailModel> scheduleDetails = new List<MerchantScheduleDetailModel>();
            var scheduleDate = requestDto.StartDate.Date;

            while (scheduleDate <= requestDto.EndDate.Date)
            {
                foreach (var item in requestDto.TherapistWorkShifts)
                {
                    item.TherapistGuids.ForEach(t =>
                    {
                        var merchantScheduleModel = new MerchantScheduleModel
                        {
                            ScheduleGuid = Guid.NewGuid().ToString("N"),
                            ScheduleDate = scheduleDate.Date,
                            MerchantGuid = UserID,
                            TargetGuid = t,
                            TargetType = ScheduleTargetType.Therapist.ToString(),
                            ScheduleTemplateGuid = scheduleTemplateModel.ScheduleTemplateGuid,
                            WorkShiftGuid = item.WorkShiftGuid,
                            PlatformType = GD.Common.EnumDefine.PlatformType.LifeCosmetology.ToString(),
                            CreatedBy = UserID,
                            LastUpdatedBy = UserID,
                            OrgGuid = string.Empty
                        };
                        schedulemodel.Add(merchantScheduleModel);
                        lstTime.ForEach(n =>
                        {
                            if (merchantWorkShiftDetialModels.FirstOrDefault(a => a.WorkShiftGuid == item.WorkShiftGuid && a.StartTime.CompareTo(n.StartTime) <= 0 && a.EndTime.CompareTo(n.StartTime) > 0) == null)
                            {
                                scheduleDetails.Add(new MerchantScheduleDetailModel
                                {
                                    ScheduleDetailGuid = Guid.NewGuid().ToString("N"),
                                    ScheduleGuid = merchantScheduleModel.ScheduleGuid,
                                    StartTime = n.StartTime,
                                    EndTime = n.EndTime,
                                    ConsumptionGuid = "Lock",
                                    CreatedBy = UserID,
                                    LastUpdatedBy = UserID,
                                    OrgGuid = string.Empty
                                });
                            }
                        });

                    });
                }

                scheduleDate = scheduleDate.AddDays(1).Date;
            }

            var result = await scheduleTemplateBiz.ScheduleTherapistsWorkShiftInBatchesAsync(scheduleTemplateModel, schedulemodel, scheduleDetails);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "批量安排排班出错");
        }

        /// <summary>
        /// 删除商户安排的周期排班
        /// </summary>
        /// <param name="scheduleTemplateGuid">周期排班guid</param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> RemoveScheduleCycleAsync(string scheduleTemplateGuid)
        {
            var scheduleTemplateBiz = new ScheduleTemplateBiz();
            var scheduleTemplateModel = await scheduleTemplateBiz.GetModelAsync(scheduleTemplateGuid);
            if (scheduleTemplateModel == null)
            {
                return Failed(ErrorCode.Empty, "检测到该排班周期不存在！");
            }
            if (scheduleTemplateModel.MerchantGuid != UserID)
            {
                return Failed(ErrorCode.UserData, "非法操作，不能操作非本店的排班周期数据！");
            }
            var checkHasSchedule = await new MerchantScheduleDetailBiz().CheckConsumerScheduledOfSchedulingCycle(UserID, scheduleTemplateGuid);
            if (checkHasSchedule)
            {
                return Failed(ErrorCode.Empty, "当前周期中，已经有消费者预约过，不能删除周期排班数据！");
            }
            var result = await scheduleTemplateBiz.RemoveScheduleCycleAsync(scheduleTemplateModel);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "删除排班周期数据出错");
        }

        /// <summary>
        /// 商户批次修改服务人员排班（一次提交一个周期；细节性修改排班）
        /// 商户对每一天每个班次进行覆盖性排班调整
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> ModifyTherapistsWorkShiftInBatchesAsync([FromBody]ModifyTherapistsWorkShiftInBatchesRequestDto requestDto)
        {
            var scheduleTemplateBiz = new ScheduleTemplateBiz();
            var workShiftTemplateModel = await new MerchantWorkShiftTemplateBiz().GetModelAsync(requestDto.TemplateGuid);
            var scheduleTemplateModel = await scheduleTemplateBiz.GetModelAsync(requestDto.ScheduleTemplateGuid);
            if (scheduleTemplateModel == null)
            {
                return Failed(ErrorCode.Empty, "检测到该排班周期不存在！");
            }
            if (workShiftTemplateModel == null)
            {
                return Failed(ErrorCode.Empty, "检测到选择的班次模板不存在！");
            }
            if (scheduleTemplateModel.StartDate <= DateTime.Now.Date)
            {
                return Failed(ErrorCode.Empty, "当前日期处于待修改的排班周期内，不能全覆盖性修改已经包含当前日期的排班周期数据！");
            }
            var checkHasSchedule = await new MerchantScheduleDetailBiz().CheckConsumerScheduledOfSchedulingCycle(UserID, requestDto.ScheduleTemplateGuid);
            if (checkHasSchedule)
            {
                return Failed(ErrorCode.Empty, "当前周期中，已经有消费者预约过，不能全覆盖性修改周期排班数据！");
            }

            scheduleTemplateModel.TemplateGuid = requestDto.TemplateGuid;
            scheduleTemplateModel.LastUpdatedBy = UserID;
            scheduleTemplateModel.LastUpdatedDate = DateTime.Now;

            var merchantWorkShiftDetialModels = await new MerchantWorkShiftDetailBiz().GetModelsByTemplateGuidAsync(scheduleTemplateModel.TemplateGuid);
            List<TimeDto> lstTime = await GetMerchantMaxDurationTimeButtonsAsync(UserID, scheduleTemplateModel.TemplateGuid);
            if (!lstTime.Any())
            {
                return Failed(ErrorCode.Empty, "店铺未配置班次！");
            }
            List<MerchantScheduleModel> schedulemodel = new List<MerchantScheduleModel>();
            List<MerchantScheduleDetailModel> scheduleDetails = new List<MerchantScheduleDetailModel>();

            foreach (var item in requestDto.TherapistWorkShifts)
            {
                var merchantScheduleModel = new MerchantScheduleModel
                {
                    ScheduleGuid = Guid.NewGuid().ToString("N"),
                    ScheduleDate = item.ScheduleDate.Date,
                    MerchantGuid = UserID,
                    TargetGuid = item.TherapistGuid,
                    TargetType = ScheduleTargetType.Therapist.ToString(),
                    ScheduleTemplateGuid = scheduleTemplateModel.ScheduleTemplateGuid,
                    WorkShiftGuid = item.WorkShiftGuid,
                    PlatformType = GD.Common.EnumDefine.PlatformType.LifeCosmetology.ToString(),
                    CreatedBy = UserID,
                    LastUpdatedBy = UserID,
                    OrgGuid = string.Empty
                };
                schedulemodel.Add(merchantScheduleModel);
                lstTime.ForEach(n =>
                {
                    if (merchantWorkShiftDetialModels.FirstOrDefault(a => a.WorkShiftGuid == item.WorkShiftGuid && a.StartTime.CompareTo(n.StartTime) <= 0 && a.EndTime.CompareTo(n.StartTime) > 0) == null)
                    {
                        scheduleDetails.Add(new MerchantScheduleDetailModel
                        {
                            ScheduleDetailGuid = Guid.NewGuid().ToString("N"),
                            ScheduleGuid = merchantScheduleModel.ScheduleGuid,
                            StartTime = n.StartTime,
                            EndTime = n.EndTime,
                            ConsumptionGuid = "Lock",
                            CreatedBy = UserID,
                            LastUpdatedBy = UserID,
                            OrgGuid = string.Empty
                        });
                    }
                });
            }
            var result = await scheduleTemplateBiz.ModifyTherapistsWorkShiftInBatchesAsync(scheduleTemplateModel, schedulemodel, scheduleDetails);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "批量全覆盖性修改整个周期的排班出错");
        }

        /// <summary>
        /// 服务人员一日批量修改排班；一次提交一天的修改
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> ModifyTherapistsWorkShiftDailyInBatchesAsync([FromBody]ModifyTherapistsWorkShiftDailyInBatchesRequestDto requestDto)
        {
            var scheduleTemplateModel = await new ScheduleTemplateBiz().GetModelAsync(requestDto.ScheduleTemplateGuid);

            var check = await new MerchantScheduleDetailBiz().CheckConsumerScheduledOnDay(scheduleTemplateModel.MerchantGuid, requestDto.ScheduleDate);
            if (check)
            {
                return Failed(ErrorCode.UserData, "店铺当天已有客户进行过预约，不能修改班次");
            }
            var merchantWorkShiftDetialModels = await new MerchantWorkShiftDetailBiz().GetModelsByTemplateGuidAsync(scheduleTemplateModel.TemplateGuid);
            List<TimeDto> lstTime = await GetMerchantMaxDurationTimeButtonsAsync(UserID, scheduleTemplateModel.TemplateGuid);
            if (!lstTime.Any())
            {
                return Failed(ErrorCode.Empty, "店铺未配置班次！");
            }

            List<MerchantScheduleModel> schedulemodel = new List<MerchantScheduleModel>();
            List<MerchantScheduleDetailModel> scheduleDetails = new List<MerchantScheduleDetailModel>();

            foreach (var item in requestDto.TherapistWorkShifts)
            {
                var merchantScheduleModel = new MerchantScheduleModel
                {
                    ScheduleGuid = Guid.NewGuid().ToString("N"),
                    ScheduleDate = requestDto.ScheduleDate.Date,
                    MerchantGuid = UserID,
                    TargetGuid = item.TherapistGuid,
                    TargetType = ScheduleTargetType.Therapist.ToString(),
                    ScheduleTemplateGuid = scheduleTemplateModel.ScheduleTemplateGuid,
                    WorkShiftGuid = item.WorkShiftGuid,
                    PlatformType = GD.Common.EnumDefine.PlatformType.LifeCosmetology.ToString(),
                    CreatedBy = UserID,
                    LastUpdatedBy = UserID,
                    OrgGuid = string.Empty
                };
                schedulemodel.Add(merchantScheduleModel);
                lstTime.ForEach(n =>
                {
                    if (merchantWorkShiftDetialModels.FirstOrDefault(a => a.WorkShiftGuid == item.WorkShiftGuid && a.StartTime.CompareTo(n.StartTime) <= 0 && a.EndTime.CompareTo(n.StartTime) > 0) == null)
                    {
                        scheduleDetails.Add(new MerchantScheduleDetailModel
                        {
                            ScheduleDetailGuid = Guid.NewGuid().ToString("N"),
                            ScheduleGuid = merchantScheduleModel.ScheduleGuid,
                            StartTime = n.StartTime,
                            EndTime = n.EndTime,
                            ConsumptionGuid = "Lock",
                            CreatedBy = UserID,
                            LastUpdatedBy = UserID,
                            OrgGuid = string.Empty
                        });
                    }
                });
            }
            var result = await new ScheduleTemplateBiz().ModifyTherapistsWorkShiftDailyInBatchesAsync(scheduleTemplateModel.ScheduleTemplateGuid, requestDto.ScheduleDate, schedulemodel, scheduleDetails);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "批量修改排班出错");

        }

        /// <summary>
        /// 分页获取商户排班周期数据
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetScheduleTemplateListResponseDto>))]
        public async Task<IActionResult> GetScheduleTemplateListAsync([FromBody]GetScheduleTemplateListRequestDto requestDto)
        {
            requestDto.MerchantGuid = string.IsNullOrWhiteSpace(requestDto.MerchantGuid) ? (UserID ?? "") : requestDto.MerchantGuid;
            if (string.IsNullOrWhiteSpace(requestDto.MerchantGuid))
            {
                return Failed(ErrorCode.Empty, "商铺guid必填");
            }
            var response = await new ScheduleTemplateBiz().GetScheduleTemplateListAsync(requestDto);
            return Success(response);
        }


        /// <summary>
        /// 获取服务人员连续天数的排班明细数据
        /// </summary>
        /// <param name="therapistId"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetTherapistScheduleDetailForDurationResponseDto>>))]
        public async Task<IActionResult> GetTherapistScheduleDetailForDurationAsync(string therapistId)
        {
            var therapistModel = await new TherapistBiz().GetModelAsync(therapistId);
            if (therapistModel == null)
            {
                return Failed(ErrorCode.Empty, "未查到此服务人员数据");
            }

            int duration = 7;
            DateTime sDate = DateTime.Now;
            DateTime eDate = sDate.AddDays(duration - 1);
            var cheduleDetaiModels = await new MerchantScheduleDetailBiz().GetScheduleDetailDtoByTargetGuid(sDate.Date, eDate.Date, therapistId);
#warning 需要传入班次模板guid
            var maxDuration = await new MerchantWorkShiftDetailBiz().GetMaxDuration(therapistModel.MerchantGuid, "");
            if (maxDuration == null)
            {
                return Failed(ErrorCode.Empty, "店铺未配置班次，请联系店铺处理！");
            }
            string businessHoursStart = Convert.ToDateTime(maxDuration.StartTime).ToString("HH:mm");
            string businessHourEnd = Convert.ToDateTime(maxDuration.EndTime).ToString("HH:mm");
            int timeStep = 15;
            DateTime dtStart = Convert.ToDateTime(businessHoursStart);
            DateTime dtEnd = Convert.ToDateTime(businessHourEnd);
            DateTime nextTime = dtStart;
            List<TimeDto> lstTime = new List<TimeDto>();
            while (nextTime < dtEnd)
            {
                lstTime.Add(new TimeDto { StartTime = nextTime.ToString("HH:mm"), EndTime = nextTime.AddMinutes(timeStep).ToString("HH:mm") });
                nextTime = nextTime.AddMinutes(timeStep);
            }
            var cheduleDetailsGroupByScheduleDate = cheduleDetaiModels.GroupBy(a => a.ScheduleDate);
            var groupTimeButtons = new List<GetTherapistScheduleDetailForDurationResponseDto>();
            foreach (var item in cheduleDetailsGroupByScheduleDate)
            {
                var timeButtons = new List<ScheduleTimeDetailDto>();
                var lstScheduleTimeDetail = cheduleDetaiModels.Where(a => a.ScheduleDate == item.Key).ToList();
                foreach (var time in lstTime)
                {
                    var result = lstScheduleTimeDetail.Where(a => a.StartTime.CompareTo(time.StartTime) <= 0 && a.EndTime.CompareTo(time.StartTime) > 0).FirstOrDefault();
                    timeButtons.Add(
                        new ScheduleTimeDetailDto
                        {
                            ScheduleDetailGuid = result?.ScheduleDetailGuid,
                            StartTime = time.StartTime,
                            EndTime = time.EndTime,
                            ConsumptionGuid = result?.ConsumptionGuid
                        });
                }
                groupTimeButtons.Add(new GetTherapistScheduleDetailForDurationResponseDto
                {
                    ScheduleDate = item.Key,
                    ScheduleTimeDetails = timeButtons
                });
            }
            return Success(groupTimeButtons);
        }
        #endregion

        #region 订单管理
        /// <summary>
        /// 查询商户订单分页列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetMerchantOrderPageListResponseDto>))]
        public async Task<IActionResult> GetMerchantOrderPageListAsync([FromQuery]GetMerchantOrderPageListRequestDto requestDto)
        {
            if (string.IsNullOrWhiteSpace(requestDto.MerchantGuid))
            {
                requestDto.MerchantGuid = UserID;
            }
            var orderBiz = new OrderBiz();
            var orderList = await orderBiz.GetMerchantOrderPageListAsync(requestDto);
            if (orderList.CurrentPage.Any())
            {
                var orderIds = orderList.CurrentPage.Select(a => a.OrderGuid).ToList();
                var products = await orderBiz.GetOrderProductListAsync(orderIds);
                var productIds = products.Select(a => a.ProductGuid).Distinct().ToList();
                var projects = await orderBiz.GetOrderProductProjectsAsync(productIds);
                for (int i = 0; i < orderList.CurrentPage.Count(); i++)
                {
                    var order = orderList.CurrentPage.ElementAt(i);
                    order.Products = products.Where(a => a.OrderGuid == order.OrderGuid).OrderBy(a => a.ProductName).ToList();
                    for (int j = 0; j < order.Products.Count(); j++)
                    {
                        var product = order.Products[j];
                        product.Projects = projects.Where(a => a.ProductGuid == product.ProductGuid).OrderBy(a => a.ProjectName).ToList();
                    }
                }
            }
            return Success(orderList);
        }

        /// <summary>
        /// 门店端获取订单列表筛选条件订单状态列表数据
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetMerchantOrderStatusConditionListResponseDto>>)), AllowAnonymous]
        public IActionResult GetMerchantOrderStatusConditionList()
        {
            var result = new List<GetMerchantOrderStatusConditionListResponseDto>();
            foreach (OrderStatusConditionEnum item in Enum.GetValues(typeof(OrderStatusConditionEnum)))
            {
                result.Add(new GetMerchantOrderStatusConditionListResponseDto
                {
                    OrderStatusCode = item.ToString(),
                    OrderStatusName = item.GetDescription()
                });
            }
            return Success(result);
        }

        /// <summary>
        /// 支付方式枚举值列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetPayTypeListItemResponseDto>>)), AllowAnonymous]
        public IActionResult GetPayTypeListItem()
        {
            var result = new List<GetPayTypeListItemResponseDto>();
            foreach (PayTypeEnum item in Enum.GetValues(typeof(PayTypeEnum)))
            {
                result.Add(new GetPayTypeListItemResponseDto
                {
                    PayTypeCode = item.ToString(),
                    PayTypeName = item.GetDescription()
                });
            }
            return Success(result);
        }

        /// <summary>
        /// 订单确认付款
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> ConfirmOrderPaymentAsync(string orderId)
        {
            var order = await new OrderBiz().GetAsync(orderId);
            var checkStatus = string.Equals(order.OrderStatus, OrderStatusEnum.Obligation.ToString(), StringComparison.OrdinalIgnoreCase) &&
                string.Equals(order.PayType, PayTypeEnum.OffLinePay.ToString(), StringComparison.OrdinalIgnoreCase);
            if (!checkStatus)
            {
                return Failed(ErrorCode.UserData, "当前订单不是[待线下付款]，无法进行确认付款操作");
            }
            var orderModels = new List<OrderModel>();
            order.Enable = false;
            order.OrderStatus = OrderStatusEnum.Completed.ToString();
            order.LastUpdatedBy = "Merchant";
            order.LastUpdatedDate = DateTime.Now;
            order.PaymentDate = DateTime.Now;
            orderModels.Add(order);
            var orders = await new OrderBiz().GetGetModelsByKey(order.OrderKey);
            var secondaryOrder = orders.Where(a => a.OrderMark == OrderMarkEnum.Secondary.ToString()).ToList();
            secondaryOrder.ForEach(a =>
            {
                a.OrderStatus = OrderStatusEnum.Completed.ToString();
                a.LastUpdatedBy = "Merchant";
                a.LastUpdatedDate = DateTime.Now;
                a.PaymentDate = DateTime.Now;
                a.Enable = true;
            });
            orderModels.AddRange(secondaryOrder);

            var result = await new OrderBiz().UpdateModelsAsync(orderModels);
            CreateOrderProductComment(new List<string> { orderId });
            return result ? Success() : Failed(ErrorCode.DataBaseError, "确认订单付款失败");
        }
        /// <summary>
        /// 订单完成后生成订单下待评价商品记录
        /// </summary>
        private void CreateOrderProductComment(List<string> orderIds)
        {
            Task.Run(() =>
            {
                try
                {
                    var check = new OrderProductCommentBiz().GetModelsByOrderGuidAsync(orderIds.FirstOrDefault()).Result;
                    if (check.Any())
                    {
                        return;
                    }
                    var list = new List<OrderDetailModel>();
                    foreach (var orderGuid in orderIds)
                    {
                        var orderDetailModels = new OrderDetailBiz().GetOrderDetailModelListByOrderGuidAsync(orderGuid).Result;
                        list.AddRange(orderDetailModels);
                    }
                    if (!list.Any())
                    {
                        return;
                    }
                    var oneOrderModel = new OrderBiz().GetAsync(orderIds.FirstOrDefault()).Result;
                    var orderProductCommentModels = list.Select(a => new OrderProductCommentModel
                    {
                        ProductCommentGuid = Guid.NewGuid().ToString("N"),
                        UserGuid = oneOrderModel.UserGuid,
                        OrderGuid = a.OrderGuid,
                        OrderDetailGuid = a.DetailGuid,
                        ProductGuid = a.ProductGuid,
                        ProductName = a.ProductName,
                        CommentStatus = CommentStatusEnum.NotEvaluate.ToString(),
                        CreatedBy = "ConfirmOrderPaymentAsync",
                        LastUpdatedBy = "ConfirmOrderPaymentAsync",
                        OrgGuid = string.Empty
                    }).ToList();
                    var result = new OrderProductCommentBiz().InsertAsync(orderProductCommentModels).Result;
                    if (result)
                    {
                        Logger.Debug($"订单完成后生成订单下待评价商品记录-OrderGuids:{JsonConvert.SerializeObject(orderIds)}");
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error($"微信支付回调后创建订单商品评价记录失败 at {nameof(MerchantController)}.{nameof(CreateOrderProductComment)}({JsonConvert.SerializeObject(orderIds)}) {Environment.NewLine}错误信息 ：{ex.Message}");
                }
            });
        }

        /// <summary>
        /// 门店开单（仅限服务类订单）
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> PlaceOrderAsync([FromBody]PlaceOrderRequestDto requestDto)
        {
            var userModel = new UserBiz().GetUserByPhone(requestDto.UserPhone);
            if (userModel == null)
            {
                return Failed(ErrorCode.UserData, "用户不存在");
            }
            if (string.IsNullOrWhiteSpace(requestDto.MerchantGuid))
            {
                requestDto.MerchantGuid = UserID;
            }
            if (requestDto.Products.FirstOrDefault(a => a.ProductNum <= 0) != null)
            {
                return Failed(ErrorCode.UserData, "商品数量不可为0");
            }
            var products = await new ProductBiz().GetModelsAsync(requestDto.Products.Select(a => a.ProductGuid).ToList());
            var projects = await new ProjectBiz().GetDtoListByProductGuids(requestDto.Products.Select(a => a.ProductGuid).ToList());
            var resPrice = from p in requestDto.Products
                           join m in products
                           on p.ProductGuid equals m.ProductGuid
                           select new
                           {
                               Price = p.ProductNum * m.Price
                           };
            var order = new OrderModel
            {
                OrderGuid = Guid.NewGuid().ToString("N"),
                OrderMark = OrderMarkEnum.Secondary.ToString(),
                OrderNo = OrderNoCreater.Create(Common.EnumDefine.PlatformType.CloudDoctor),
                OrderKey = Guid.NewGuid().ToString("N"),
                UserGuid = userModel.UserGuid,
                MerchantGuid = requestDto.MerchantGuid,
                OrderType = OrderTypeEnum.Normal.ToString(),
                OrderCategory = OrderCategoryEnum.Service.ToString(),
                ProductCount = requestDto.Products.Sum(a => a.ProductNum),
                OrderPhone = string.Empty,
                OrderAddress = string.Empty,
                OrderReceiver = string.Empty,
                OrderStatus = OrderStatusEnum.Completed.ToString(),
                PaymentDate = DateTime.Now,
                PayType = PayTypeEnum.OffLinePay.ToString(),
                PayablesAmount = resPrice.Sum(a => a.Price),
                PaidAmount = resPrice.Sum(a => a.Price),
                TransactionFlowingGuid = string.Empty,
                CreatedBy = "Merchant",
                LastUpdatedBy = "Merchant",
                OrgGuid = string.Empty,
                Remark = string.Empty
            };
            var orderDetails = from p in requestDto.Products
                               join m in products
                               on p.ProductGuid equals m.ProductGuid
                               select new OrderDetailModel
                               {
                                   DetailGuid = Guid.NewGuid().ToString("N"),
                                   OrderGuid = order.OrderGuid,
                                   ProductGuid = p.ProductGuid,
                                   ProductName = m.ProductName,
                                   ProductPrice = m.Price,
                                   ProductCount = p.ProductNum,
                                   CreatedBy = "Merchant",
                                   LastUpdatedBy = "Merchant",
                                   OrgGuid = string.Empty
                               };
            var goodsModels = new List<GoodsModel>();
            var goodsItemModels = new List<GoodsItemModel>();
            foreach (var orderDetail in orderDetails)
            {
                var product = products.Where(a => a.ProductGuid == orderDetail.ProductGuid).FirstOrDefault();
                for (int i = 0; i < orderDetail.ProductCount; i++)
                {
                    var goodsModel = new GoodsModel
                    {
                        GoodsGuid = Guid.NewGuid().ToString("N"),
                        UserGuid = userModel.UserGuid,
                        OrderGuid = orderDetail.OrderGuid,
                        DetailGuid = orderDetail.DetailGuid,
                        Available = true,
                        EffectiveStartDate = product.EffectiveDays == 0 ? null : (DateTime?)DateTime.Now,
                        EffectiveEndDate = product.EffectiveDays == 0 ? null : (DateTime?)DateTime.Now.AddDays(product.EffectiveDays.Value),
                        ProductGuid = orderDetail.ProductGuid,
                        ProductName = orderDetail.ProductName,
                        MerchanGuid = product.MerchantGuid,
                        Price = product.Price,
                        SelectRule = "0",
                        ProjectThreshold = product.ProjectThreshold,
                        PlatformType = product.PlatformType,
                        CreatedBy = UserID,
                        LastUpdatedBy = UserID,
                        OrgGuid = "GuoDan"
                    };
                    goodsModels.Add(goodsModel);
                    var goodsItems = projects.Where(a => a.ProductGuid == orderDetail.ProductGuid).Select(a => new GoodsItemModel
                    {
                        GoodsItemGuid = Guid.NewGuid().ToString("N"),
                        GoodsGuid = goodsModel.GoodsGuid,
                        ProjectGuid = a.ProjectGuid,
                        Count = a.ProjectTimes,
                        Remain = a.ProjectTimes,
                        Used = 0,
                        Available = true,
                        Price = a.Price,
                        AllowPresent = a.AllowPresent,
                        PlatformType = product.PlatformType,
                        CreatedBy = UserID,
                        LastUpdatedBy = UserID,
                        OrgGuid = "GuoDan"
                    });
                    goodsItemModels.AddRange(goodsItems);
                }
            }

            var result = await new OrderBiz().PlaceOrderAsync(order, orderDetails.ToList(), goodsModels, goodsItemModels);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "开单失败");
        }

        /// <summary>
        /// 获取指定类型下的商品列表
        /// </summary>
        /// <param name="classifyGuid"></param>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetProductItemByServiceClassifyResponseDto>>))]
        public async Task<IActionResult> GetProductItemByServiceClassifyAsync(string classifyGuid, string merchantId)
        {
            if (string.IsNullOrWhiteSpace(merchantId))
            {
                merchantId = UserID;
            }
            var models = await new ProductBiz().GetProductsByClassifyGuidAsync(merchantId, classifyGuid);
            var result = models.Select(a => new GetProductItemByServiceClassifyResponseDto
            {
                ProductGuid = a.ProductGuid,
                ProductName = a.ProductName,
                ProductPrice = a.Price
            });
            return Success(result);
        }

        /// <summary>
        /// 提交发货
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostDeliveryAsync([FromBody]PostDeliveryRequestDto requestDto)
        {
            var model = await new OrderBiz().GetAsync(requestDto.OrderGuid);
            if (model == null)
            {
                return Failed(ErrorCode.UserData, "无此订单数据");
            }
            //if (model.OrderStatus.ToString() != OrderStatusEnum.Received.ToString() || !string.IsNullOrWhiteSpace(model.ExpressNo))
            //{
            //    return Failed(ErrorCode.UserData, "当前不能进行发货操作，请检查数据");
            //}
            model.ExpressCompany = requestDto.ExpressCompany;
            model.ExpressNo = requestDto.ExpressNo;
            model.LastUpdatedBy = UserID;
            model.LastUpdatedDate = DateTime.Now;
            var result = new OrderBiz().UpdateModel(model);

            return result ? Success() : Failed(ErrorCode.DataBaseError, "发货失败");
        }

        #endregion

        /// <summary>
        /// 消费预约自动过期（默认超过预约时间一小时后）,门店范围
        /// </summary>
        /// <returns></returns>
        [HttpPost, Obsolete("已弃用，考虑到门店端和用户端都有此调用，避免并发产生数据问题")]
        public async Task<IActionResult> MissConsumptionAutomaticAsyncOfMerchant()
        {
            //var result = await new ConsumptionBiz().MissConsumptionAutomaticAsyncOfMerchant(UserID);
            //return result ? Success() : Failed(ErrorCode.DataBaseError);
            return Success();
        }
    }
}
