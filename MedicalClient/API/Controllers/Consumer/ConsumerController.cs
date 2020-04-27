using GD.API.Code;
using GD.AppSettings;
using GD.Common;
using GD.Common.Helper;
using GD.Consumer;
using GD.Dtos.Consumer.Consumer;
using GD.Dtos.DtoIn;
using GD.Dtos.Merchant.Merchant;
using GD.Dtos.WeChat;
using GD.Mall;
using GD.Merchant;
using GD.Models.CommonEnum;
using GD.Models.Consumer;
using GD.Models.CrossTable;
using GD.Models.Merchant;
using GD.Module;
using GD.Module.WeChat;
using GD.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static GD.Models.Consumer.OrderProductCommentModel;

namespace GD.API.Controllers.Consumer
{
    /// <inheritdoc />
    /// <summary>
    /// 消费者控制器
    /// </summary>
    public class ConsumerController : ConsumerBaseController
    {
        /// <summary>
        /// 投递意见反馈
        /// </summary>
        /// <param name="adviseDto">意见反馈Dto</param>
        /// <returns></returns>
        [HttpPost, AllowAnonymous, Produces(typeof(ResponseDto))]
        public IActionResult SendFeedback([FromBody]AdviseDto adviseDto)
        {
            var adviseModel = new AdviseModel
            {
                AdviseGuid = Guid.NewGuid().ToString("N"),
                UserGuid = UserID,
                Adviser = adviseDto.Adviser,
                AdviserPhone = adviseDto.AdviserPhone,
                AdviserEmail = adviseDto.AdviserEmail,
                AdviseContent = adviseDto.AdviseContent,
                CreatedBy = UserID,
                Enable = true
            };
            var adviseBiz = new AdviseBiz();
            return adviseBiz.InsertAdvise(adviseModel) ? Success() : Failed(ErrorCode.DataBaseError, "智慧云医意见反馈信息插入数据库失败！");
        }

        #region 地址

        /// <summary>
        /// 消费者选择地址
        /// </summary>
        /// <param name="addressGuid">消费者地址Guid</param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<SelectAddressResponseDto>))]
        public IActionResult SelectAddress(string addressGuid)
        {
            AddressBiz addressBiz = new AddressBiz();
            var addressModel = addressBiz.GetAddress(addressGuid);
            if (addressModel == null)
            {
                return Failed(ErrorCode.Empty);
            }
            var dto = addressModel.ToDto<SelectAddressResponseDto>();
            return Success(dto);
        }

        /// <summary>
        /// 获取用户默认地址
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetUserDefaultAddressResponseDto>))]
        public IActionResult GetUserDefaultAddress()
        {
            AddressBiz addressBiz = new AddressBiz();
            var model = addressBiz.GetUserDefaultAddress(UserID);
            if (model == null)
            {
                return Success();
            }
            var response = model.ToDto<GetUserDefaultAddressResponseDto>();
            return Success(response);
        }

        /// <summary>
        /// 新增消费者地址数据
        /// </summary>
        /// <param name="addressDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public IActionResult AddAddress([FromBody]AddAddressRequestDto addressDto)
        {
            var addressModel = new AddressModel
            {
                AddressGuid = Guid.NewGuid().ToString("N"),
                UserGuid = UserID,
                Receiver = addressDto.Receiver,
                Phone = addressDto.Phone,
                Province = addressDto.Province,
                City = addressDto.City,
                Area = addressDto.Area,
                ProvinceId = addressDto.ProvinceId,
                CityId = addressDto.CityId,
                AreaId = addressDto.AreaId,
                DetailAddress = addressDto.DetailAddress,
                IsDefault = addressDto.IsDefault,
                CreatedBy = UserID,
                Enable = true
            };
            var addressBiz = new AddressBiz();
            bool result = true;
            if (addressDto.IsDefault)
            {
                var lstAddress = addressBiz.GetAddresses(UserID);
                if (lstAddress.Any())
                {
                    lstAddress.ForEach(item => item.IsDefault = false);
                    result = addressBiz.UpdateAddress(lstAddress);
                }
            }

            if (!result)
            {
                return Failed(ErrorCode.DataBaseError, "消费地址默认情况更新失败");
            }

            return addressBiz.InsertAddress(addressModel) ? Success() : Failed(ErrorCode.DataBaseError, "地址数据插入不成功");
        }

        /// <summary>
        /// 更新消费者地址数据
        /// </summary>
        /// <param name="addressDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public IActionResult EditAddress([FromBody]EditAddressRequestDto addressDto)
        {
            var addressBiz = new AddressBiz();
            var model = addressBiz.GetAddress(addressDto.AddressGuid);
            if (model == null)
                return Failed(ErrorCode.Empty, "未查询到该地址数据");
            var oldIsDefault = model.IsDefault;
            var newIsDefault = addressDto.IsDefault;
            model.Receiver = addressDto.Receiver;
            model.Phone = addressDto.Phone;
            model.Province = addressDto.Province;
            model.City = addressDto.City;
            model.Area = addressDto.Area;
            model.ProvinceId = addressDto.ProvinceId;
            model.CityId = addressDto.CityId;
            model.AreaId = addressDto.AreaId;
            model.DetailAddress = addressDto.DetailAddress;
            model.IsDefault = addressDto.IsDefault;
            model.LastUpdatedBy = UserID;
            bool result;
            //设为默认地址
            if (addressDto.IsDefault && oldIsDefault != newIsDefault)
            {
                var lstAddress = addressBiz.GetAddresses(UserID);
                lstAddress.ForEach(item => item.IsDefault = item.AddressGuid == addressDto.AddressGuid);
                result = addressBiz.UpdateAddress(lstAddress);
            }
            else
            {
                result = addressBiz.UpdateAddress(model);
            }
            return result ? Success() : Failed(ErrorCode.DataBaseError, "数据库更新地址信息失败");
        }

        /// <summary>
        /// 删除消费者地址
        /// </summary>
        /// <param name="addressGuid">消费者地址Guid</param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto))]
        public IActionResult DeleteAddress(string addressGuid)
        {
            var addressBiz = new AddressBiz();
            return addressBiz.DeleteAddress(addressGuid) ? Success() : Failed(ErrorCode.DataBaseError, "数据库删除地址信息失败");
        }

        /// <summary>
        /// 获取用户地址列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetConsumerAddressResponseDto>>))]
        public IActionResult GetConsumerAddress()
        {
            var addressBiz = new AddressBiz();
            var lstAddress = addressBiz.GetAddresses(UserID);
            var dto = lstAddress.Select(a => a.ToDto<GetConsumerAddressResponseDto>());
            return Success(dto);
        }

        #endregion 地址

        #region 我的关注

        /// <summary>
        /// 分页查询我关注的文章
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetMyArticlesResponseDto>))]
        public async Task<IActionResult> GetMyArticles([FromBody]PageRequestDto pageDto)
        {
            var response = await new CollectionBiz().GetMyArticleListAsync(new GetMyArticlesRequestDto
            {
                PageIndex = pageDto.PageIndex,
                PageSize = pageDto.PageSize,
                UserId = UserID
            });

            return Success(response);
        }

        /// <summary>
        /// 分页查询我关注的医生
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<List<GetMyDoctorsResponseDto>>))]
        public IActionResult GetMyDoctors([FromBody]GetMyDoctorsRequestDto pageDto)
        {
            var colBiz = new CollectionBiz();
            var colList = colBiz.GetMyDoctorList(UserID, pageDto);
            return Success(colList);
        }

        /// <summary>
        /// 获取我关注的产品列表
        /// </summary>
        /// <param name="pageDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetMyProductsResponseDto>))]
        public IActionResult GetMyProducts([FromBody]PageRequestDto pageDto)
        {
            var colBiz = new CollectionBiz();
            var response = colBiz.GetMyProductsList(UserID, pageDto);
            if (response == null)
            {
                return Failed(ErrorCode.Empty, "未查询到我关注的商品列表");
            }
            return Success(response);
        }

        #endregion 我的关注

        /// <summary>
        ///添加用户浏览记录
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public IActionResult AddConsumerBrowseInfo([FromBody]AddConsumerBrowseInfoRequestDto requestDto)
        {
            var behaviorBiz = new BehaviorBiz();
            var model = requestDto.ToModel<BehaviorModel>();
            model.BehaviorGuid = Guid.NewGuid().ToString();
            model.UserGuid = UserID;
            model.CreatedBy = UserID;
            model.CreationDate = DateTime.Now;
            model.LastUpdatedBy = UserID;
            model.LastUpdatedDate = DateTime.Now;
            var isSuccess = behaviorBiz.Add(model);
            if (string.IsNullOrWhiteSpace(isSuccess))
            {
                return Failed(ErrorCode.DataBaseError, "客户动作记录添加失败！");
            }
            return Success();
        }

        /// <summary>
        ///添加用户文章浏览记录
        ///（一天一人一文章）
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public IActionResult AddConsumerArticleView([FromBody]AddConsumerArticleViewRequestDto requestDto)
        {
            //更新hot表浏览量
            var upRes = new HotExBiz().UpdateVisitTotalAsync(requestDto.TargetGuid);
            //今天该用户是否已存在该目标的浏览记录了
            #region 需求修改：不需要判断当天是否浏览此文章，用户没进入一次文章，算一次浏览量，可重复浏览
            //var articleViewBiz = new ArticleViewBiz();
            //var model = articleViewBiz.GetModelAsyncBySqlWhere(requestDto.TargetGuid, UserID, true).Result;
            //if (model != null)
            //{
            //    return Failed(ErrorCode.UserData, "已存在该记录！");
            //} 
            #endregion

            ArticleViewModel newModel = new ArticleViewModel
            {
                ViewGuid = Guid.NewGuid().ToString(),
                TargetGuid = requestDto.TargetGuid,
                CreatedBy = UserID,
                LastUpdatedBy = UserID
            };

            var isSuccess = new ArticleViewBiz().AddAsync(newModel);
            if (!isSuccess.Result)
            {
                return Failed(ErrorCode.DataBaseError, "记录添加失败！");
            }

            return Success();
        }

        /// <summary>
        /// 获取智慧云医搜索引擎热词列表Top10
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<string>>)), AllowAnonymous]
        public async Task<IActionResult> GetSearchHotWordListAsync()
        {
            var lst = await new SearchDicBiz().GetSearchHotWordListAsync("CloudDoctor:SearchHotWordList");
            return Success(lst);
        }

        #region 微信公众号开发

        /// <summary>
        /// 获取云医客户端公众号 JS-SDK权限验证的签名Signature
        /// </summary>
        /// <param name="url">当前网页的URL，不包含#及其后面部分</param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<WeChatJsSDKSignatureResponse>)), AllowAnonymous]
        public async Task<IActionResult> GetClientWechatJSSDKSignatureAsync(string url)
        {
            var result = await WechatJSSDKHelper.GetSignatureAsync(PlatformSettings.CDClientAppId, PlatformSettings.CDClientAppSecret, url);
            return Success(result);
        }

        /// <summary>
        /// 绑定微信公众号服务器交互回调api
        /// </summary>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        public IActionResult WeChatXmlMsgCallBack(string strechostr)
        {
            var wechatServerToken = PlatformSettings.CDClientAppToken;//"guodan2019";
            var queryStrings = Request.Query;
            var echostr = queryStrings["echostr"];
            if (string.IsNullOrWhiteSpace(echostr))
            {
                return Content("");
            }
            var result = WeChatUtils.CheckSignature(wechatServerToken, Request.Query.ToDictionary(a => a.Key, a => a.Value.ToString()));
            if (result)
            {
                return Content(echostr);
            }
            return Content("");
        }

        /// <summary>
        /// 绑定微信公众号服务器交互回调
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto)), AllowAnonymous]
        public IActionResult WeChatXmlMsgCallBack()
        {
            var wechatServerToken = PlatformSettings.CDClientAppToken;//"guodan2019";
            var result = WeChatUtils.CheckSignature(wechatServerToken, Request.Query.ToDictionary(a => a.Key, a => a.Value.ToString()));
            if (!result)
            {
                return Content("");
            }
            var strem = Request.Body;
            BaseWeChatXmlMsg msg = null;
            using (StreamReader sr = new StreamReader(strem))
            {
                var xmlStr = sr.ReadToEnd();
                msg = WeChatXmlMessageFactory.CreateMessage(xmlStr);
                if (msg == null)
                {
                    return Content("");
                }
                Task.Run(() =>
                {
                    new WeChatXmlMsgCallBackBiz().MessageHandling(msg);
                });
            }
            return Content("");
        }

        /// <summary>
        /// 获取消费者端用户微信openid
        /// </summary>
        /// <param name="code">微信code</param>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        public async Task<IActionResult> GetConsumerOpenId(string code)
        {
            var result = await WeChartApi.Oauth2AccessTokenAsync(PlatformSettings.CDClientAppId, PlatformSettings.CDClientAppSecret, code);
            if (result.Errcode != 0)
            {
                return Failed(ErrorCode.SystemException, "获取用户openId失败");
            }
            if (!string.IsNullOrWhiteSpace(UserID) && !string.IsNullOrWhiteSpace(result.OpenId))
            {
                var userBiz = new UserBiz();
                var userModel = await userBiz.GetModelAsync(UserID);
                if (userModel?.WechatOpenid != result.OpenId)
                {
                    userModel.WechatOpenid = result.OpenId;
                    userModel.LastUpdatedBy = UserID;
                    userModel.LastUpdatedDate = DateTime.Now;
                    await userBiz.UpdateAsync(userModel);
                }
            }

            return Success(result.OpenId, null);
        }

        /// <summary>
        /// 检测用户是否已关注智慧云医公众号
        /// </summary>
        /// <param name="openId">用户openid</param>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        [Produces(typeof(ResponseDto<IsConsumerSubscribeResponseDto>))]
        public async Task<IActionResult> IsConsumerSubscribeAsync(string openId)
        {
            var resToken = await WeChartApi.GetAccessToken(PlatformSettings.CDClientAppId, PlatformSettings.CDClientAppSecret);
            Logger.Debug($"NotifyUser获取token-{JsonConvert.SerializeObject(resToken)}");
            if (string.IsNullOrWhiteSpace(resToken.AccessToken))
            {
                Common.Helper.Logger.Error($"GD.API.Controllers.Consumer.{nameof(ConsumerController)}.{nameof(IsConsumerSubscribeAsync)}  openId:[{openId}] {Environment.NewLine} error：检测用户是否关注用户端公众号-获取token失败。{resToken.Errmsg}");
                return Failed(ErrorCode.SystemException, "检测用户是否关注用户端公众号失败");
            }

            var result = await WeChartApi.GetUserInfoAsync(openId, resToken.AccessToken);
            if (result.Errcode != 0)
            {
                Common.Helper.Logger.Error($"GD.API.Controllers.Consumer.{nameof(ConsumerController)}.{nameof(IsConsumerSubscribeAsync)}  openId:[{openId}] {Environment.NewLine} error：检测用户是否关注用户端公众号-获取用户信息失败。{resToken.Errmsg}");
                return Failed(ErrorCode.SystemException, "检测用户是否关注用户端公众号失败");
            }
            var response = new IsConsumerSubscribeResponseDto
            {
                Subscribe = result.Subscribe==1,
                BizId = PlatformSettings.CDClientAppBizId
            };
            return Success(response);
        }

        /// <summary>
        /// 获取云医用户端AppId
        /// </summary>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        public IActionResult GetClientAppId()
        {
            Settings settings = Factory.GetSettings("host.json");
            return Success<string>(settings["WeChat:Client:AppId"]);
        }

        #endregion 微信公众号开发

        #region 消费者预约相关

        /// <summary>
        /// 预约美疗师(消费者入口)
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<MakeAnAppointmentWithTherapistResponseDto>))]
        public async Task<IActionResult> MakeAnAppointmentWithTherapist([FromBody]MakeAnAppointmentWithTherapistRequestDto requestDto)
        {
            var goodsBiz = new GoodsBiz();
            int lockTime = 15;//预约项目需间隔15分钟，即预约项目成功后随即锁定后续的15分钟时间
            var merchantScheduleBiz = new MerchantScheduleBiz();
            var scheduleModel = await merchantScheduleBiz.GetAsync(requestDto.ScheduleGuid);
            if (scheduleModel == null)
            {
                return Failed(ErrorCode.Empty, "操作非法，无此排班信息");
            }
            if (scheduleModel.FullStatus)
            {
                return Failed(ErrorCode.Empty, "此美疗师已经约满");
            }
            string projectGuid = "";
            GoodsItemModel goodsItemModel = await new GoodsItemBiz().GetAsync(requestDto.FromItemGuid);

            if (goodsItemModel == null)
                return Failed(ErrorCode.Empty, "未查到卡项数据");

            var goodsModel = await goodsBiz.GetAsync(goodsItemModel.GoodsGuid);
            if (goodsModel == null)
                return Failed(ErrorCode.Empty, "数据异常,未查到卡项的个人商品数据");
            if (goodsModel.UserGuid != UserID)
                return Failed(ErrorCode.UserData, "操作非法，操作人预约的不是属于自己的项目");
            if (goodsModel.EffectiveStartDate != null && goodsModel.EffectiveEndDate != null && DateTime.Now >= goodsModel.EffectiveEndDate.Value)
                return Failed(ErrorCode.UserData, "该商品已过期，不可用");
            if (!goodsModel.Available)
                return Failed(ErrorCode.UserData, "当前选择的商品已用完，不可操作");
            if (goodsModel.ProjectThreshold.HasValue && goodsModel.ProjectThreshold.Value > 0)
            {
                var checkThreshold = await goodsBiz.CheckGoodsThresholdIsExceededAsync(goodsItemModel.GoodsGuid);
                if (checkThreshold != null)
                {
                    return Failed(ErrorCode.UserData, $"当前商品下项目使用总次数不可超过{checkThreshold.ProjectThreshold}次,目前已使用{checkThreshold.ProjectUsedSum}次");
                }
            }
            if (goodsItemModel.Remain == 0 || !goodsItemModel.Available)
                return Failed(ErrorCode.UserData, "此卡项已使用完或不可用，故无法预约");

            projectGuid = goodsItemModel.ProjectGuid;
            goodsItemModel.Remain--;
            goodsItemModel.Used++;
            goodsItemModel.Available = goodsItemModel.Remain > 0;

            var projectModel = await new ProjectBiz().GetAsync(projectGuid);
            int projectOperateTime = projectModel.OperationTime;
            requestDto.StartTime = Convert.ToDateTime(requestDto.StartTime).ToString("HH:mm");
            requestDto.EndTime = Convert.ToDateTime(requestDto.StartTime).AddMinutes(projectOperateTime).ToString("HH:mm");

            var merchantScheduleDetaiBiz = new MerchantScheduleDetailBiz();
            var occupied = await merchantScheduleDetaiBiz.CheckScheduleDetailOccupied(requestDto.ScheduleGuid, requestDto.StartTime, Convert.ToDateTime(requestDto.EndTime).AddMinutes(lockTime).ToString("HH:mm"));
            if (occupied)
            {
                return Failed(ErrorCode.UserData, "该时间可能已被预约");
            }

            var consumptionGuid = Guid.NewGuid();
            ConsumptionModel consumptionModel = new ConsumptionModel
            {
                ConsumptionGuid = consumptionGuid.ToString("N"),
                ConsumptionNo = BitConverter.ToInt64(consumptionGuid.ToByteArray(), 0).ToString(),
                UserGuid = UserID,
                FromItemGuid = requestDto.FromItemGuid,
                ServiceMemberGuid = requestDto.ServiceMemberGuid,//新增服务对象guid
                ProjectGuid = projectGuid,
                AppointmentDate = Convert.ToDateTime(scheduleModel.ScheduleDate.ToString("yyyy-MM-dd") + " " + requestDto.StartTime),
                MerchantGuid = scheduleModel.MerchantGuid,
                OperatorGuid = scheduleModel.TargetGuid,
                PlatformType = scheduleModel.PlatformType,
                ConsumptionStatus = ConsumptionStatusEnum.Booked.ToString(),
                Remark = requestDto.Remark,
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
                OrgGuid = "GuoDan"
            };
            MerchantScheduleDetailModel merchantScheduleDetailModel = new MerchantScheduleDetailModel
            {
                ScheduleDetailGuid = Guid.NewGuid().ToString("N"),
                ScheduleGuid = requestDto.ScheduleGuid,
                StartTime = requestDto.StartTime,
                EndTime = requestDto.EndTime,
                ConsumptionGuid = string.Empty,
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
                OrgGuid = "GuoDan"
            };
            MerchantScheduleDetailModel lockScheduleDetailModel = null;
            if (lockTime != 0)
            {
                lockScheduleDetailModel = new MerchantScheduleDetailModel
                {
                    ScheduleDetailGuid = Guid.NewGuid().ToString("N"),
                    ScheduleGuid = requestDto.ScheduleGuid,
                    StartTime = requestDto.EndTime,
                    EndTime = Convert.ToDateTime(requestDto.EndTime).AddMinutes(lockTime).ToString("HH:mm"),
                    ConsumptionGuid = string.Empty,
                    CreatedBy = UserID,
                    LastUpdatedBy = UserID,
                    OrgGuid = "GuoDan"
                };
            }

            bool result = await new ConsumptionBiz().MakeAnAppointmentWithConsumption(consumptionModel, merchantScheduleDetailModel, goodsItemModel, lockScheduleDetailModel);
            //预约完成后检测个人商品是否已经用完，若用完，更新个人商品（GoodsModel）的是否可使用标记（Available）为false
            if (result)
            {
                //检测个人商品项目阈值规则
                if (goodsModel?.ProjectThreshold != null && goodsModel?.ProjectThreshold.Value > 0)
                {
                    var checkThreshold = await new GoodsBiz().CheckGoodsThresholdIsExceededAsync(goodsItemModel.GoodsGuid);
                    if (checkThreshold != null)
                    {
                        goodsModel.Available = false;
                        goodsModel.LastUpdatedBy = UserID;
                        goodsModel.LastUpdatedDate = DateTime.Now;
                        await goodsBiz.UpdateAsync(goodsModel);
                    }
                }
                else
                {
                    var checkGoodsAvailableResult = await goodsBiz.CheckGoodsHasRunOutAsync(goodsItemModel.GoodsGuid);
                    if (checkGoodsAvailableResult)
                    {
                        if (goodsModel != null)
                        {
                            goodsModel.Available = false;
                            goodsModel.LastUpdatedBy = UserID;
                            goodsModel.LastUpdatedDate = DateTime.Now;
                            await goodsBiz.UpdateAsync(goodsModel);
                        }
                    }
                }
            }
            if (!result)
            {
                return Failed(ErrorCode.DataBaseError, "预约失败");
            }
            var merchantModel = await new MerchantBiz().GetAsync(scheduleModel.MerchantGuid);
            var therapistModel = await new TherapistBiz().GetAsync(scheduleModel.TargetGuid);
            var accessoryModel = await new AccessoryBiz().GetAsync(projectModel.PictureGuid);
            var categoryExModel = await new MerchantCategoryBiz().GetModelByClassifyGuidAsync(projectModel.ClassifyGuid, projectModel.MerchantGuid);
            var serviceMemberModel = await new ServiceMemberBiz().GetAsync(requestDto.ServiceMemberGuid);
            var memberSex = serviceMemberModel.Sex == "M" ? "男" : "女";
            var memberAge = (serviceMemberModel.AgeYear > 0 ? $"{serviceMemberModel.AgeYear}岁" : "") + (serviceMemberModel.AgeMonth > 0 ? $"{serviceMemberModel.AgeMonth}月" : "");
            var response = new MakeAnAppointmentWithTherapistResponseDto
            {
                MerchantName = merchantModel?.MerchantName,
                MerchantAddress = merchantModel?.MerchantAddress,
                TherapistName = therapistModel?.TherapistName,
                ProjectName = projectModel?.ProjectName,
                ConsumptionNo = consumptionModel?.ConsumptionNo,
                AppointmentDate = consumptionModel.AppointmentDate,
                ProjectPictureUrl = $"{accessoryModel?.BasePath}{accessoryModel?.RelativePath}",
                ServiceMember = $"{serviceMemberModel.Name},{memberSex},{memberAge}",
                CategoryExtension = categoryExModel?.CategoryName
            };

            //预约通知用户
            NotifyUser(consumptionModel.ConsumptionGuid);

            //预约通知服务人员
            NotifyOperator(consumptionModel.ConsumptionGuid);

            return Success(response);
        }

        /// <summary>
        /// 预约通知用户
        /// </summary>
        /// <param name="consumptionGuid"></param>
        private void NotifyUser(string consumptionGuid)
        {
            Task.Run(() =>
            {
                Logger.Debug(PlatformSettings.UserAppointmentNotificationTemplate);
                if (string.IsNullOrWhiteSpace(PlatformSettings.UserAppointmentNotificationTemplate))
                {
                    return;
                }

                var userOpenId = "";
                string controllerName = ControllerContext.ActionDescriptor.ControllerName;
                string actionName = ControllerContext.ActionDescriptor.ActionName;
                try
                {
                    var model = new ConsumptionBiz().GetModel(consumptionGuid);
                    var userModel = new UserBiz().GetModelAsync(model.UserGuid).Result;
                    Logger.Debug($"NotifyUser-用户openid-{userModel?.WechatOpenid}");
                    if (string.IsNullOrWhiteSpace(userModel?.WechatOpenid ?? ""))
                    {
                        return;
                    }
                    var merchantModel = new MerchantBiz().GetAsync(model.MerchantGuid).Result;
                    var projectModel = new ProjectBiz().GetAsync(model.ProjectGuid).Result;
                    var goodsItemModel = new GoodsItemBiz().GetAsync(model.FromItemGuid).Result;
                    var merchantCagetoryExModel = new MerchantCategoryBiz().GetModelByClassifyGuidAsync(projectModel.ClassifyGuid, model.MerchantGuid).Result;
                    var therapistModel = new TherapistBiz().GetAsync(model.OperatorGuid).Result;
                    var resToken = WeChartApi.GetAccessToken(PlatformSettings.CDClientAppId, PlatformSettings.CDClientAppSecret).Result;
                    Logger.Debug($"NotifyUser获取token-{JsonConvert.SerializeObject(resToken)}");
                    if (string.IsNullOrWhiteSpace(resToken.AccessToken))
                    {
                        Common.Helper.Logger.Error($"GD.API.Controllers.Consumer.{controllerName}.{actionName}  openId:[{userOpenId}] {Environment.NewLine} error:用户预约后发送模板消息通知云医执行端公众号-获取token失败。{resToken.Errmsg}");
                        return;
                    }
                    userOpenId = userModel.WechatOpenid;
                    var clientMsg = new WeChatTemplateMsg
                    {
                        Touser = userModel.WechatOpenid,
                        Template_Id = PlatformSettings.UserAppointmentNotificationTemplate,
                        Data = new
                        {
                            First = new { Value = "【预约成功】" },
                            //预约门店名称+类别
                            Keyword1 = new { Value = $"{merchantModel?.MerchantName}-{merchantCagetoryExModel?.CategoryName}" },
                            //预约项目
                            Keyword2 = new { Value = projectModel?.ProjectName },
                            //剩余次数
                            Keyword3 = new { Value = goodsItemModel.Remain.ToString() },
                            //预约时间
                            Keyword4 = new { Value = model.AppointmentDate.ToString("MM月dd日 HH:mm") },
                            //美疗师名称
                            Keyword5 = new { Value = therapistModel?.TherapistName },
                            Remark = new { Value = "如有特殊情况，请及时与门店进行联系。" },
                        }
                    };
                    var clientTempMsgRes = WeChartApi.SendTemplateMsg(clientMsg, resToken.AccessToken);
                    Logger.Debug($"NotifyUser发送模板消息-{JsonConvert.SerializeObject(clientTempMsgRes)}");
                }
                catch (Exception ex)
                {
                    Common.Helper.Logger.Error($"GD.API.Controllers.Consumer.{controllerName}.{actionName}  openId:[{userOpenId}] {Environment.NewLine} error:用户预约后发送模板消息通知云医用户端公众号端失败。{ex.Message}");
                }
            });
        }

        /// <summary>
        /// 预约通知服务人员(需要修改模板Id)
        /// </summary>
        private void NotifyOperator(string consumptionGuid)
        {
            Task.Run(() =>
            {
                if (string.IsNullOrWhiteSpace(PlatformSettings.OperatorAppointmentNotificationTemplate))
                {
                    return;
                }

                var userOpenId = "";
                string controllerName = ControllerContext.ActionDescriptor.ControllerName;
                string actionName = ControllerContext.ActionDescriptor.ActionName;
                try
                {
                    var model = new ConsumptionBiz().GetModel(consumptionGuid);
                    var therapistModel = new TherapistBiz().GetAsync(model.OperatorGuid).Result;
                    if (string.IsNullOrWhiteSpace(therapistModel?.WeChatOpenId ?? ""))
                    {
                        return;
                    }
                    var userModel = new UserBiz().GetModelAsync(model.UserGuid).Result;
                    var merchantModel = new MerchantBiz().GetAsync(model.MerchantGuid).Result;
                    var projectModel = new ProjectBiz().GetAsync(model.ProjectGuid).Result;
                    var goodsItemModel = new GoodsItemBiz().GetAsync(model.FromItemGuid).Result;
                    var merchantCagetoryExModel = new MerchantCategoryBiz().GetModelByClassifyGuidAsync(projectModel.ClassifyGuid, model.MerchantGuid).Result;

                    var resToken = WeChartApi.GetAccessToken(PlatformSettings.DoctorClientAppId, PlatformSettings.DoctorClientAppSecret).Result;
                    if (string.IsNullOrWhiteSpace(resToken.AccessToken))
                    {
                        Common.Helper.Logger.Error($"GD.API.Controllers.Consumer.{controllerName}.{actionName}  openId:[{userOpenId}] {Environment.NewLine} error:用户预约后发送模板消息通知云医执行端公众号-获取token失败。{resToken.Errmsg}");
                        return;
                    }
                    userOpenId = therapistModel.WeChatOpenId;
                    var userPhone = userModel?.Phone;
                    var clientMsg = new WeChatTemplateMsg
                    {
                        Touser = userModel.WechatOpenid,
                        Template_Id = PlatformSettings.OperatorAppointmentNotificationTemplate,
                        Data = new
                        {
                            First = new { Value = "【预约提醒】" },
                            //用户手机号
                            Keyword1 = new
                            {
                                Value = $"{userPhone.Substring(0, 3)}****{userModel?.Phone.Substring(userPhone.Length - 4)}"
                            },
                            //预约项目
                            Keyword2 = new { Value = projectModel?.ProjectName },
                            //预约时间
                            Keyword3 = new { Value = model.AppointmentDate.ToString("MM月dd日 HH:mm") },
                            //服务人员姓名
                            Keyword4 = new { Value = therapistModel?.TherapistName },
                            Remark = new { Value = "如用户未按时到达，请及时与用户进行联系确认。" },
                        }
                    };
                    var clientTempMsgRes = WeChartApi.SendTemplateMsg(clientMsg, resToken.AccessToken);
                }
                catch (Exception ex)
                {
                    Common.Helper.Logger.Error($"GD.API.Controllers.Consumer.{controllerName}.{actionName}  openId:[{userOpenId}] {Environment.NewLine} error:用户预约后发送模板消息通知云医执行端公众号失败。{ex.Message}");
                }
            });
        }

        #region --预约通知(旧逻辑)--

        /*
        /// <summary>
        /// 预约消息通知
        /// 1.微信公众号客服消息通知美疗师执行端
        /// 2.rabbitMQ消息发布通知双美门店端
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="rabbitMQ"></param>
        /// <param name="wechat"></param>
        /// <param name="openId"></param>
        /// <param name="consumerOpenId"></param>
        /// <returns></returns>
        private bool AppointmentNotification(AppointmentNotificationDto dto, bool rabbitMQ = true, bool wechat = true, string openId = "", string consumerOpenId = "")
        {
            Task.Run(() =>
            {
                string controllerName = ControllerContext.ActionDescriptor.ControllerName;
                string actionName = ControllerContext.ActionDescriptor.ActionName;
                if (rabbitMQ)
                {
                    try
                    {
                        var queueName = dto.MerchantGuid;
                        var bus = Communication.MQ.Client.CreateConnection();
                        var advancedBus = bus.Advanced;
                        if (advancedBus.IsConnected)
                        {
                            var queue = advancedBus.QueueDeclare(queueName);
                            advancedBus.Publish(EasyNetQ.Topology.Exchange.GetDefault(), queue.Name, false, new EasyNetQ.Message<AppointmentNotificationDto>(dto));
                        }
                        bus.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Common.Helper.Logger.Error($"rabbitMQ 发生错误：{ex.Message} {Environment.NewLine} at GD.API.Controllers.Consumer.{controllerName}.{actionName}:");
                    }
                }

                if (wechat)
                {
                    #region 通知美疗师执行端

                    try
                    {
                        if (!string.IsNullOrWhiteSpace(openId))
                        {
                            var token = WeChartApi.GetAccessToken("", "").Result;
                            if (string.IsNullOrEmpty(token.AccessToken))
                            {
                                var encryptPhone = $"{dto.ConsumerPhone.Substring(0, 3)}****{dto.ConsumerPhone.Substring(dto.ConsumerPhone.Length - 4)}";

                                var tmplateMsg = new WeChatTemplateMsg
                                {
                                    Touser = openId,
                                    Template_Id = "I419ax64YFM-ef_M-qPeTyFsEC103CayjWcWHfg-fXI",
                                    Data = new
                                    {
                                        First = new { Value = "【预约提醒】" },
                                        //消费者手机号
                                        Keyword1 = new { Value = encryptPhone },
                                        //预约项目
                                        Keyword2 = new { Value = dto.ProjectName },
                                        //预约时间
                                        Keyword3 = new { Value = dto.AppointmentDate.ToString("MM月dd日 HH:mm") },
                                        //美疗师名称
                                        Keyword4 = new { Value = dto.ThrapistName },
                                        Remark = new { Value = "如用户未按时到达，请及时与用户进行联系。" },
                                    }
                                };
                                var response = WeChartApi.SendTemplateMsg(tmplateMsg, token.AccessToken);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Common.Helper.Logger.Error($"GD.API.Controllers.Consumer.{controllerName}.{actionName}  openId:[{openId}] {Environment.NewLine} error:{ex.Message}");
                    }

                    #endregion 通知美疗师执行端

                    #region 通知用户端端

                    try
                    {
                        if (!string.IsNullOrWhiteSpace(consumerOpenId))
                        {
                            var resToken = WeChartApi.GetAccessToken(PlatformSettings.CDClientAppId, PlatformSettings.CDClientAppSecret).Result;
                            var clientMsg = new WeChatTemplateMsg
                            {
                                Touser = consumerOpenId,
                                Template_Id = "FvKs2fYirGajOl9Tn9ieBQELf5-P7yZssyeFuRHeHtg",
                                Data = new
                                {
                                    First = new { Value = "【预约成功】" },
                                    //预约门店名称+类别
                                    Keyword1 = new { Value = dto.MerchantName },
                                    //预约项目
                                    Keyword2 = new { Value = dto.ProjectName },
                                    //剩余次数
                                    Keyword3 = new { Value = "2" },
                                    //预约时间
                                    Keyword4 = new { Value = dto.AppointmentDate.ToString("MM月dd日 HH:mm") },
                                    //美疗师名称
                                    Keyword5 = new { Value = dto.ThrapistName },
                                    Remark = new { Value = "如有特殊情况，请及时与门店进行联系。" },
                                }
                            };
                            var clientTempMsgRes = WeChartApi.SendTemplateMsg(clientMsg, resToken.AccessToken);
                        }
                    }
                    catch (Exception ex)
                    {
                        Common.Helper.Logger.Error($"GD.API.Controllers.Consumer.{controllerName}.{actionName}  openId:[{consumerOpenId}] {Environment.NewLine} error:通知云医客户端失败。{ex.Message}");
                    }

                    #endregion 通知用户端端
                }
            });
            return true;
        }
        */

        #endregion --预约通知(旧逻辑)--

        /// <summary>
        /// 我的预约列表
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<List<GetMyBookedItemListOfCosmetologyResponseDto>>))]
        public async Task<IActionResult> GetMyBookedItemListOfCosmetologyAsync([FromBody]GetMyBookedItemListOfCosmetologyRequestDto requestDto)
        {
            if (string.IsNullOrWhiteSpace(requestDto.UserGuid))
            {
                requestDto.UserGuid = UserID;
            }
            var response = await new ConsumptionBiz().GetMyBookedItemListOfCosmetologyAsync(requestDto);

            return Success(response);
        }

        /// <summary>
        /// 修改预约时间
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AmendAppointmentAsync([FromBody]AmendAppointmentRequestDto requestDto)
        {
            var consumptionBiz = new ConsumptionBiz();
            var consumptionModel = await consumptionBiz.GetAsync(requestDto.ConsumptionGuid);
            if (consumptionModel == null)
            {
                return Failed(ErrorCode.UserData, "非法数据，无此预约记录");
            }
            if (!string.Equals(consumptionModel.ConsumptionStatus, ConsumptionStatusEnum.Booked.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return Failed(ErrorCode.UserData, $"当前修改的预约记录状态不是[{ConsumptionStatusEnum.Booked.GetDescription()}],不可修改");
            }
            var projectModel = await new ProjectBiz().GetAsync(consumptionModel.ProjectGuid);
            var categoryExtensionModel = await new MerchantCategoryBiz().GetModelByClassifyGuidAsync(projectModel.ClassifyGuid, projectModel.MerchantGuid);
            var limitTime = (categoryExtensionModel?.LimitTime) ?? 30;

            if (DateTime.Now >= consumptionModel.AppointmentDate)
            {
                return Failed(ErrorCode.UserData, $"当前已过预约时间，请联系门店处理！");
            }
            else if ((consumptionModel.AppointmentDate - DateTime.Now).TotalMinutes < limitTime)
            {
                return Failed(ErrorCode.UserData, $"服务预约距到店服务时间不足{limitTime}分钟，不可修改，请联系门店处理！");
            }

            //if (DateTime.Now > consumptionModel.AppointmentDate.AddMinutes(-1 * limitTime))
            //{
            //    return Failed(ErrorCode.UserData, $"服务预约距到店服务时间不足{limitTime}分钟，不可修改时间");
            //}
            var goodsBiz = new GoodsBiz();
            int lockTime = 15;//预约项目需间隔15分钟，即预约项目成功后随即锁定后续的15分钟时间
            var merchantScheduleBiz = new MerchantScheduleBiz();
            var scheduleModel = await merchantScheduleBiz.GetAsync(requestDto.ScheduleGuid);
            if (scheduleModel == null)
            {
                return Failed(ErrorCode.Empty, "操作非法，无此排班信息");
            }
            if (scheduleModel.FullStatus)
            {
                return Failed(ErrorCode.Empty, "此美疗师已经约满");
            }
            var merchantScheduleDetaiBiz = new MerchantScheduleDetailBiz();
            var occupied = await merchantScheduleDetaiBiz.CheckScheduleDetailOccupied(requestDto.ScheduleGuid, requestDto.StartTime, Convert.ToDateTime(requestDto.EndTime).AddMinutes(lockTime).ToString("HH:mm"));
            if (occupied)
            {
                return Failed(ErrorCode.UserData, "该时间可能已被预约");
            }
            consumptionModel.AppointmentDate = Convert.ToDateTime(scheduleModel.ScheduleDate.ToString("yyyy-MM-dd") + " " + requestDto.StartTime);
            consumptionModel.LastUpdatedDate = DateTime.Now;
            consumptionModel.LastUpdatedBy = UserID;
            MerchantScheduleDetailModel merchantScheduleDetailModel = new MerchantScheduleDetailModel
            {
                ScheduleDetailGuid = Guid.NewGuid().ToString("N"),
                ScheduleGuid = requestDto.ScheduleGuid,
                StartTime = requestDto.StartTime,
                EndTime = requestDto.EndTime,
                ConsumptionGuid = string.Empty,
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
                OrgGuid = "GuoDan"
            };
            MerchantScheduleDetailModel lockScheduleDetailModel = null;
            if (lockTime != 0)
            {
                lockScheduleDetailModel = new MerchantScheduleDetailModel
                {
                    ScheduleDetailGuid = Guid.NewGuid().ToString("N"),
                    ScheduleGuid = requestDto.ScheduleGuid,
                    StartTime = requestDto.EndTime,
                    EndTime = Convert.ToDateTime(requestDto.EndTime).AddMinutes(lockTime).ToString("HH:mm"),
                    ConsumptionGuid = string.Empty,
                    CreatedBy = UserID,
                    LastUpdatedBy = UserID,
                    OrgGuid = "GuoDan"
                };
            }

            #region 修改预约时间后，应删除旧的预约时间

            var oldLockTimeModel = await merchantScheduleDetaiBiz.GetModelAsyncByConsumptionGuid(consumptionModel.ConsumptionGuid);
            var deleteTimes = new List<string>();
            deleteTimes.Add(oldLockTimeModel.ScheduleDetailGuid);
            deleteTimes.Add(oldLockTimeModel.LockDetailGuid);

            #endregion 修改预约时间后，应删除旧的预约时间

            var result = await new ConsumptionBiz().AmendAppointmentAsync(consumptionModel, deleteTimes, merchantScheduleDetailModel, lockScheduleDetailModel);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "变更预约时间失败");
        }

        /// <summary>
        /// 通过预约的卡项获取门店类别扩展信息
        /// </summary>
        /// <param name="goodsItemId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetMerchantCategoryFromProjectAsync(string goodsItemId)
        {
            var goodsItemModel = await new GoodsItemBiz().GetAsync(goodsItemId);
            var goodsModel = await new GoodsBiz().GetAsync(goodsItemModel?.GoodsGuid);
            var productModel = await new ProductBiz().GetModelByGuidAsync(goodsModel?.ProductGuid);
            var category = await new MerchantCategoryBiz().GetModelByClassifyGuidAsync(productModel?.CategoryGuid, productModel?.MerchantGuid);
            return Success(category?.CategoryName, null);
        }

        /// <summary>
        /// 消费预约自动过期（默认超过预约时间一小时后）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> MissConsumptionAutomaticAsync()
        {
            var biz = new ConsumptionBiz();
            var models = await biz.GetMissedConsumptionAsync(UserID);
            foreach (var item in models)
            {
                item.LastUpdatedBy = UserID;
                item.LastUpdatedDate = DateTime.Now;
                item.ConsumptionStatus = ConsumptionStatusEnum.Miss.ToString();
                var exceed = await biz.IsMissedConsumptionExceedTheLimitAsync(UserID, item.MerchantGuid, item.AppointmentDate);
                if (exceed)//超过限制，直接过期
                {
                    await biz.UpdateAsync(item);
                    Logger.Debug($"用户[{UserID}]在门店[{item.MerchantGuid}]爽约预约[{item.ConsumptionGuid}]，超过爽约限制，扣减项目次数");
                }
                else//没有超过限制，返还项目次数
                {
                    var goodsItemModel = await new GoodsItemBiz().GetAsync(item.FromItemGuid);
                    goodsItemModel.Remain++;
                    goodsItemModel.Used--;
                    goodsItemModel.Available = goodsItemModel.Remain > 0;
                    await biz.MissConsumptionWithoutAbatementAsync(item, goodsItemModel);
                    Logger.Debug($"用户[{UserID}]在门店[{item.MerchantGuid}]爽约预约[{item.ConsumptionGuid}]，未超过爽约限制，返还项目次数");
                }
            }
            return Success();
        }

        #endregion 消费者预约相关

        /// <summary>
        /// 获取用户预约记录状态为已预约的记录数量
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetMyBookedItemCountAsync()
        {
            var result = await new ConsumptionBiz().GetMyBookedItemCountAsync(UserID);
            return Success(result, null);
        }

        #region 订单商品评价相关
        /// <summary>
        /// 获取用户订单商品评价分页列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetUserOrderProductCommentPageListResponseDto>))]
        public async Task<IActionResult> GetUserOrderProductCommentPageListAsync(GetUserOrderProductCommentPageListRequestDto requestDto)
        {
            if (string.IsNullOrWhiteSpace(requestDto.UserGuid))
            {
                requestDto.UserGuid = UserID;
            }
            var userModel = await new UserBiz().GetModelAsync(requestDto.UserGuid);
            var userPic = await new AccessoryBiz().GetAsync(userModel.PortraitGuid);
            var result = await new OrderProductCommentBiz().GetUserOrderProductCommentPageListAsync(requestDto);
            if (userPic != null)
            {
                foreach (var item in result.CurrentPage)
                {
                    item.UserPortrait = $"{userPic.BasePath}{userPic.RelativePath}";
                }
            }
            return Success(result);
        }

        /// <summary>
        /// 评价订单商品
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CommentOrderProductAsync([FromBody]CommentOrderProductRequestDto requestDto)
        {
            var model = await new OrderProductCommentBiz().GetAsync(requestDto.ProductCommentGuid);
            if (model == null)
            {
                return Failed(ErrorCode.UserData, "无此数据");
            }
            if (model.CommentStatus != CommentStatusEnum.NotEvaluate.ToString())
            {
                return Failed(ErrorCode.UserData, "此订单商品已评价，无需再次评价");
            }
            var commentModel = new CommentModel
            {
                CommentGuid = Guid.NewGuid().ToString("N"),
                TargetGuid = model.ProductGuid,
                Content = requestDto.Content,
                Score = requestDto.Score,
                Anonymous = requestDto.Anonymous,
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
                OrgGuid = string.Empty
            };

            model.CommentStatus = CommentStatusEnum.Evaluate.ToString();
            model.CommentGuid = commentModel.CommentGuid;
            model.LastUpdatedBy = UserID;
            model.LastUpdatedDate = DateTime.Now;
            var result = await new OrderProductCommentBiz().CommentOrderProductAsync(model, commentModel);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "评价订单商品失败");
        }
        #endregion

        [HttpGet, AllowAnonymous]
        public async Task<IActionResult> TestExAsync(string UserID)
        {

            var biz = new ConsumptionBiz();
            var models = await biz.GetMissedConsumptionAsync(UserID);
            foreach (var item in models)
            {
                item.LastUpdatedBy = UserID;
                item.LastUpdatedDate = DateTime.Now;
                item.ConsumptionStatus = ConsumptionStatusEnum.Miss.ToString();
                var exceed = await biz.IsMissedConsumptionExceedTheLimitAsync(UserID, item.MerchantGuid, item.AppointmentDate);
                if (exceed)//超过限制，直接过期
                {
                    await biz.UpdateAsync(item);
                    Logger.Debug($"用户[{UserID}]在门店[{item.MerchantGuid}]爽约预约[{item.ConsumptionGuid}]，超过爽约限制，扣减项目次数");
                }
                else//没有超过限制，返还项目次数
                {
                    var goodsItemModel = await new GoodsItemBiz().GetAsync(item.FromItemGuid);
                    goodsItemModel.Remain++;
                    goodsItemModel.Used--;
                    goodsItemModel.Available = goodsItemModel.Remain > 0;
                    await biz.MissConsumptionWithoutAbatementAsync(item, goodsItemModel);
                    Logger.Debug($"用户[{UserID}]在门店[{item.MerchantGuid}]爽约预约[{item.ConsumptionGuid}]，未超过爽约限制，返还项目次数");
                }
            }
            return Success();
        }

        /// <summary>
        /// 获取用户临近过期的卡数据列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetMyGoodsListNearExpirationResponseDto>>))]
        public async Task<IActionResult> GetMyGoodsListNearExpirationAsync()
        {
            var result = await new GoodsBiz().GetMyGoodsListNearExpirationAsync(UserID);
            return Success(result);
        }

        /// <summary>
        /// 获取消费者从未使用过的项目列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetMyUnusedGoodsItemListResponseDto>>))]
        public async Task<IActionResult> GetMyUnusedGoodsItemListAsync()
        {
            var result = await new GoodsBiz().GetMyUnusedGoodsItemListAsync(UserID);
            return Success(result);
        }

    }
}