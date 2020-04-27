using GD.Common;
using GD.Consumer;
using GD.Dtos.Merchant.Appointment;
using GD.Mall;
using GD.Merchant;
using GD.Models.CommonEnum;
using GD.Models.Consumer;
using GD.Models.Merchant;
using GD.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GD.API.Controllers.Consumer
{
    /// <summary>
    /// 预约
    /// </summary>
    public class StoreManageAppointmentController : ConsumerBaseController
    {
        /// <summary>
        /// 新增预约
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>AllowAnonymous
        [HttpPost, Produces(typeof(bool))]
        public async Task<IActionResult> AddAppointment([FromBody] AddAppointmentRequestDto requestDto)
        {
            #region 校验请求参数
            DateTime.TryParse(requestDto.StartTime, out var startTime);
            if (startTime == DateTime.MinValue)
            {
                return Failed(ErrorCode.Empty, "预约时间格式不正确，请修改！");
            }

            if (!string.IsNullOrEmpty(requestDto.Remark))
            {
                if (requestDto.Remark.Length > 500)
                {
                    return Failed(ErrorCode.Empty, "备注超过最大长度限制，请修改！");
                }
            }
            #endregion

            #region 校验用户账号是否存在
            var userModel = new UserBiz().GetUserByPhone(requestDto.Phone);
            if (userModel == null)
            {
                return Failed(ErrorCode.Empty, $"用户“{requestDto.Phone}”不存在或已禁用！");
            }

            #endregion

            #region 若服务对象存在则校验
            if (!string.IsNullOrEmpty(requestDto.ServiceMemberGuid))
            {
                var members = await new ServiceMemberBiz().GetServiceMemberListAsync(userModel.UserGuid);

                if (members != null && members.Count > 0)
                {
                    if (!members.Any(d => d.ServiceMemberGuid == requestDto.ServiceMemberGuid))
                    {
                        return Failed(ErrorCode.Empty, "服务对象不存在！");
                    }
                }
            }
            #endregion

            #region 校验用户指定卡下服务项目是否可用
            var goodsItemBiz = new GoodsItemBiz();

            var goodsItemModel = await goodsItemBiz.GetModelAsync(requestDto.FromItemGuid);

            if (goodsItemModel is null)
            {
                return Failed(ErrorCode.Empty, "抱歉，尚未有可用的卡项");
            }
            #endregion

            #region 校验商户下项目是否存在
            var projectBiz = new ProjectBiz();

            var projectModel = await projectBiz.GetMerchantPorjectModelById(UserID, goodsItemModel.ProjectGuid);

            if (projectModel is null)
            {
                return Failed(ErrorCode.Empty, "服务项目不存在！");
            }
            #endregion

            var startDate = startTime.AddMinutes(projectModel.OperationTime);

            requestDto.EndTime = startDate.ToString("HH:mm");

            #region 校验用户指定卡是否可用并获取旧卡
            var goodBiz = new GoodsBiz();

            //按照卡创建时间倒序，优先扣除旧卡使用次数
            var goodsModel = await goodBiz.GetAvailableGoods(userModel.UserGuid);

            if (goodsModel is null || goodsModel.Count <= 0)
            {
                return Failed(ErrorCode.Empty, "抱歉，尚未有可用的卡项");
            }

            //首先获取有过期时间限制的卡，优先扣除使用次数
            var goodModel = goodsModel.FirstOrDefault(d => d.EffectiveStartDate.HasValue
              && d.EffectiveEndDate.HasValue && DateTime.Now < d.EffectiveEndDate.Value);

            if (goodModel is null)
            {
                //若没有过期时间限制则获取第一张卡
                goodModel = goodsModel.FirstOrDefault(d => !d.EffectiveStartDate.HasValue && !d.EffectiveEndDate.HasValue);

                if (goodModel is null)
                {
                    return Failed(ErrorCode.UserData, "抱歉，尚未有可用的卡项");
                }
            }

            #endregion

            #region 查询排班是否存在或已约满

            //预约项目需间隔15分钟，即预约项目成功后随即锁定后续的15分钟时间
            int lockTime = 15;

            var merchantScheduleBiz = new MerchantScheduleBiz();
            var scheduleModel = await merchantScheduleBiz.GetModelAsync(requestDto.ScheduleGuid);
            if (scheduleModel == null)
            {
                return Failed(ErrorCode.Empty, $"服务项目“{projectModel.ProjectName}”无排班信息");
            }

            if (scheduleModel.FullStatus)
            {
                return Failed(ErrorCode.Empty, "排班已约满");
            }   
            #endregion

            #region 查询预约时间段是否已被预约

            var merchantScheduleDetaiBiz = new MerchantScheduleDetailBiz();

            var occupied = await merchantScheduleDetaiBiz.CheckScheduleDetailOccupied(requestDto.ScheduleGuid, requestDto.StartTime, startDate.AddMinutes(lockTime).ToString("HH:mm"));

            if (occupied)
            {
                return Failed(ErrorCode.UserData, $"服务时间“{requestDto.StartTime}”不可预约");
            }
            #endregion

            goodsItemModel.Remain--;
            goodsItemModel.Used++;
            goodsItemModel.Available = goodsItemModel.Remain > 0;

            var consumptionGuid = Guid.NewGuid();

            var consumptionModel = new ConsumptionModel
            {
                Remark = requestDto.Remark,
                ConsumptionGuid = consumptionGuid.ToString("N"),
                ConsumptionNo = BitConverter.ToInt64(consumptionGuid.ToByteArray(), 0).ToString(),
                UserGuid = userModel.UserGuid,
                FromItemGuid = goodsItemModel.GoodsItemGuid,
                ProjectGuid = goodsItemModel.ProjectGuid,
                AppointmentDate = Convert.ToDateTime(scheduleModel.ScheduleDate.ToString("yyyy-MM-dd") + " " + requestDto.StartTime),
                MerchantGuid = scheduleModel.MerchantGuid,
                OperatorGuid = scheduleModel.TargetGuid,
                PlatformType = scheduleModel.PlatformType,
                ConsumptionStatus = ConsumptionStatusEnum.Booked.ToString(),
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
                OrgGuid = "GuoDan"
            };

            if (!string.IsNullOrEmpty(requestDto.ServiceMemberGuid))
            {
                consumptionModel.ServiceMemberGuid = requestDto.ServiceMemberGuid;
            }

            var merchantScheduleDetailModel = new MerchantScheduleDetailModel
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

            var lockScheduleDetailModel = (MerchantScheduleDetailModel)null;

            if (lockTime > 0)
            {
                lockScheduleDetailModel = new MerchantScheduleDetailModel
                {
                    ScheduleDetailGuid = Guid.NewGuid().ToString("N"),
                    ScheduleGuid = requestDto.ScheduleGuid,
                    StartTime = requestDto.EndTime,
                    EndTime = startDate.AddMinutes(lockTime).ToString("HH:mm"),
                    ConsumptionGuid = string.Empty,
                    CreatedBy = UserID,
                    LastUpdatedBy = UserID,
                    OrgGuid = "GuoDan"
                };
            }

            var result = await new ConsumptionBiz().MakeAnAppointmentWithConsumption(consumptionModel, merchantScheduleDetailModel, goodsItemModel, lockScheduleDetailModel);

            return !result ? Failed(ErrorCode.DataBaseError, "预约失败") : Success();
        }

        /// <summary>
        /// 条件查询排班列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(GetScheduleListByConditionPageResponseDto))]
        public async Task<IActionResult> SelectScheduleListByCondition([FromBody] GetScheduleListByConditionRequestDto requestDto)
        {
            if (string.IsNullOrWhiteSpace(requestDto.MerchantGuid))
            {
                requestDto.MerchantGuid = UserID;
            }

            var result = await new ConsumptionBiz().SelectScheduleListByCondition(requestDto);
            return Success(result);
        }

        /// <summary>
        /// 查询今日预约/查询全部预约
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(GetAppointmentListResponseDto))]
        public async Task<IActionResult> GetAppointmentList(GetAppointmentListRequestDto requestDto)
        {
            if (string.IsNullOrWhiteSpace(requestDto.UserGuid))
            {
                requestDto.UserGuid = UserID;
            }

            var result = await new ConsumptionBiz().GetAppointmentList(requestDto);
            return Success(result);
        }

        /// <summary>
        /// 取消预约
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(bool))]
        public async Task<IActionResult> CancelOppointmentAsync([FromBody] UpdateOppointmentStatusRequestDto requestDto)
        {
            var consumptionBiz = new ConsumptionBiz();
            var model = consumptionBiz.GetModel(requestDto.ConsumptionGuid);
            if (model == null || string.IsNullOrWhiteSpace(model.ConsumptionGuid))
            {
                return Failed(ErrorCode.Empty, "找不到该预约记录！");
            }

            if (!model.ConsumptionStatus.Equals(ConsumptionStatusEnum.Booked.ToString()))
            {
                return Failed(ErrorCode.UserData, "该预约记录的状态不支持取消，请检查！");
            }
            var projectModel = await new ProjectBiz().GetAsync(model.ProjectGuid);
            var categoryExtensionModel = await new MerchantCategoryBiz().GetModelByClassifyGuidAsync(projectModel.ClassifyGuid, projectModel.MerchantGuid);
            var limitTime = (categoryExtensionModel?.LimitTime) ?? 30;

            if (requestDto.FromPoint == "Consumer")
            {
                if (DateTime.Now >= model.AppointmentDate)
                {
                    return Failed(ErrorCode.UserData, $"当前已过预约时间，请联系门店处理！");
                }
                else if ((model.AppointmentDate - DateTime.Now).TotalMinutes < limitTime)
                {
                    return Failed(ErrorCode.UserData, $"当前离到店服务时间不足{limitTime}分钟，不可取消，请联系门店处理！");
                }
            }
            model.ConsumptionStatus = ConsumptionStatusEnum.Canceled.ToString();
            model.LastUpdatedBy = UserID;
            model.LastUpdatedDate = DateTime.Now;

            //取消预约 须在个人产品数量上+1
            var goodsItemModel = await new GoodsItemBiz().GetAsync(model.FromItemGuid);
            if (goodsItemModel == null)
            {
                return Failed(ErrorCode.Empty, "找不到该记录的个人商品项！");
            }
            goodsItemModel.Remain++;
            goodsItemModel.Used--;
            goodsItemModel.Available = goodsItemModel.Remain > 0;

            //删除预约所占时间
            var merchantScheduleDetailModel = await new MerchantScheduleDetailBiz().GetModelAsyncByConsumptionGuid(model.ConsumptionGuid);
            if (merchantScheduleDetailModel == null)
            {
                return Failed(ErrorCode.Empty, "找不到该预约所占用时间！");
            }

            var isSuccess = await consumptionBiz.CancelOppointment(model, goodsItemModel, merchantScheduleDetailModel);

            return Success(isSuccess);
        }

        /// <summary>
        /// 获得预约状态枚举
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetOppointmentStatusEnumResponseDto>>))]
        public IActionResult GetOppointmentStatusEnum()
        {
            var responseList = new List<GetOppointmentStatusEnumResponseDto>();
            var t = typeof(ConsumptionStatusEnum);
            var arr = Enum.GetValues(t);
            foreach (var item in arr)
            {
                responseList.Add(new GetOppointmentStatusEnumResponseDto()
                {
                    Code = item.ToString(),
                    Name = GetEnumDescription((Enum)item)
                });
            }
            return Success(responseList);
        }

        /// <summary>
        /// get enum description by enumValue
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        private string GetEnumDescription(Enum enumValue)
        {
            string value = enumValue.ToString();
            FieldInfo field = enumValue.GetType().GetField(value);
            object[] objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);    //获取描述属性
            if (objs == null || objs.Length == 0)    //当描述属性没有时，直接返回名称
            {
                return value;
            }

            DescriptionAttribute descriptionAttribute = (DescriptionAttribute)objs[0];
            return descriptionAttribute.Description;
        }
    }
}
