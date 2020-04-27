using GD.Common.Base;
using GD.Dtos.Consumer.Consumer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Merchant.Appointment
{
    /// <summary>
    /// 获取我的预约列表(今日/全部)- 请求Dto
    /// </summary>
    public class GetMyAppointmentListByConditionRequestDto : PageRequestDto
    {
        /// <summary>
        /// 登录人GUID，即商户GUID
        /// </summary>
        public string UserGuid { get; set; }


        /// <summary>
        /// 用户名或手机号
        /// </summary>
        [Display(Name = "用户名或手机号")]
        public string SelectStr { get; set; }

        /// <summary>
        /// 查询今天
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "是否查询今天")]
        public bool IsToday { get; set; } = true;

        /// <summary>
        /// 是否为有效
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "是否为有效")]
        public bool Enable { get; set; } = true;
    }

    /// <summary>
    /// 响应Dto
    /// </summary>
    public class GetMyAppointmentListByConditionResponseDto : BasePageResponseDto<GetMyAppointmentListByConditionItemDto>
    {

    }

    /// <summary>
    /// 子项
    /// </summary>
    public class GetMyAppointmentListByConditionItemDto : BaseDto
    {
        /// <summary>
        /// 预约记录Guid
        /// </summary>
        public string ConsumptionGuid { get; set; }

        /// <summary>
        /// 预约时间
        /// </summary>
        public DateTime AppointmentDate { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 美疗师Guid
        /// </summary>
        public string TherapistGuid { get; set; }
        /// <summary>
        /// 美疗师
        /// </summary>
        public string TherapistName { get; set; }

        /// <summary>
        /// 预约项目Guid
        /// </summary>
        public string ProjectGuid { get; set; }
        /// <summary>
        /// 预约项目
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 用户头像URL
        /// </summary>
        public string PortraitUrl { get; set; }

        /// <summary>
        /// 预约状态
        /// </summary>
        public string ConsumptionStatus { get; set; }

        /// <summary>
        /// 服务对象姓名
        /// </summary>
        public string MemberName { get; set; }

        /// <summary>
        /// 服务对象性别
        /// </summary>
        public string MemberSex { get; set; }

        /// <summary>
        /// 服务对象年龄（年岁）
        /// </summary>
        public int MemberAgeYear { get; set; }

        /// <summary>
        /// 服务对象年龄（月数）
        /// </summary>
        public int MemberAgeMonth { get; set; }
    }
}


