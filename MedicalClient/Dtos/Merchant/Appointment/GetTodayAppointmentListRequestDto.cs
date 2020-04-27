using GD.Common.Base;
using GD.Dtos.Consumer.Consumer;
using System;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Merchant.Appointment
{
    /// <summary>
    /// 获取今日/全部预约列表 - 请求Dto
    /// </summary>
    public class GetAppointmentListRequestDto : PageRequestDto
    {
        /// <summary>
        /// 0：今日预约，1:全部预约
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 登录人GUID，即商户GUID（不用传）
        /// </summary>
        [Display(Name = "登录人GUID，即商户GUID")]
        public string UserGuid { get; set; }

        /// <summary>
        ///分类
        /// </summary>
        public string ClassifyGuid { get; set; }

        /// <summary>
        /// 开始日期
        /// </summary>
        [Display(Name = "开始日期")]
        public DateTime? BeginDate { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        [Display(Name = "结束日期")]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 是否为有效
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "是否为有效")]
        public bool Enable { get; set; } = true;
    }

    /// <summary>
    /// 响应Dto
    /// </summary>
    public class GetAppointmentListResponseDto : BasePageResponseDto<GetAppointmentItemDto>
    {

    }

    /// <summary>
    /// 子项
    /// </summary>
    public class GetAppointmentItemDto : BaseDto
    {
        /// <summary>
        /// 预约记录Guid
        /// </summary>
        public string ConsumptionGuid { get; set; }
        /// <summary>
        /// 所属大类名称
        /// </summary>
        public string ClassifyName { get; set; }
        /// <summary>
        /// 预约时间
        /// </summary>
        public DateTime AppointmentDate { get; set; }
        /// <summary>
        /// 服务人员
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 预约项目
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>                 
        /// 预约状态('Booked'-已预约,'Arrive'-已到店,'Completed'-已完成,'Canceled'-已取消,'Miss'-已错过)
        /// </summary>
        public string ConsumptionStatus { get; set; }
        /// <summary>
        /// 预约创建日期
        /// </summary>
        public DateTime CreationDate { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 服务人员
        /// </summary>
        public string TherapistName { get; set; }
        /// <summary>
        /// 服务对象姓名
        /// </summary>
        public string ServerMemberName { get; set; }
        /// <summary>
        /// 服务对象性别
        /// </summary>
        public string ServerMemberGender { get; set; }
        /// <summary>
        /// 服务对象年龄——岁
        /// </summary>
        public int? ServerMemberAgeYear { get; set; }
        /// <summary>
        /// 服务对象年龄——月
        /// </summary>
        public int? ServerMemberAgeMonth { get; set; }
    }
}
