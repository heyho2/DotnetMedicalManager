using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Consumer.Consumer
{
    /// <summary>
    /// 生美--获取已预约列表 响应Dto
    /// </summary>
    public class GetMyBookedItemListOfCosmetologyResponseDto : BasePageResponseDto<GetMyBookedItemListOfCosmetologyItemDto>
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public class GetMyBookedItemListOfCosmetologyItemDto : BaseDto
    {
        /// <summary>
        /// 消费guid
        /// </summary>
        public string ConsumptionGuid { get; set; }

        /// <summary>
        /// 消费码
        /// </summary>
        public string ConsumptionNo { get; set; }

        /// <summary>
        /// 预约时间
        /// </summary>
        public DateTime AppointmentDate { get; set; }

        /// <summary>
        /// 预约状态
        /// </summary>
        public string ConsumptionStatus { get; set; }

        /// <summary>
        /// 项目guid
        /// </summary>
        public string ProjectGuid { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 店铺guid
        /// </summary>
        public string MerchantGuid { get; set; }

        /// <summary>
        /// 店铺名称
        /// </summary>
        public string MerchantName { get; set; }

        /// <summary>
        /// 店铺地址
        /// </summary>
        public string MerchantAddress { get; set; }

        /// <summary>
        /// 美疗师guid
        /// </summary>
        public string TherapistGuid { get; set; }

        /// <summary>
        /// 美疗师姓名
        /// </summary>
        public string TherapistName { get; set; }

        /// <summary>
        /// 美疗师手机号
        /// </summary>
        public string TherapistPhone { get; set; }

        /// <summary>
        /// 项目时长
        /// </summary>
        public int OperationTime { get; set; }

        /// <summary>
        /// 美疗师头像
        /// </summary>
        public string TherapistPortrait { get; set; }

        /// <summary>
        /// 商户备注
        /// </summary>
        public string MerchantRemark { get; set; }


    }
}
