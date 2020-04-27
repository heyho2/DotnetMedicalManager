using GD.Common.Base;
using GD.Models.CommonEnum;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Consumer.Consumer
{

    /// <summary>
    /// 获取已完成的消费记录分页列表请求Dto
    /// </summary>
    public class GetCompletedConsumptionPageListRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 消费者guid(选填，默认为当前登录的用户guid)
        /// </summary>
        public string ConsumerGuid { get; set; }
    }

    /// <summary>
    /// 获取已完成的消费记录分页列表响应Dto
    /// </summary>
    public class GetCompletedConsumptionPageListResponseDto : BasePageResponseDto<GetCompletedConsumptionPageListItemDto>
    {
    }

    /// <summary>
    /// 获取已完成的消费记录分页列表ItemDto
    /// </summary>
    public class GetCompletedConsumptionPageListItemDto : BaseDto
    {
        ///<summary>
        ///消费GUID
        ///</summary>
        public string ConsumptionGuid { get; set; }

        ///<summary>
        ///服务项目guid
        ///</summary>
        public string ProjectGuid { get; set; }

        ///<summary>
        ///服务项目名称
        ///</summary>S
        public string ProjectName { get; set; }

        ///<summary>
        ///服务项目图片
        ///</summary>
        public string ProjectPicture { get; set; }

        /// <summary>
        /// 消费门店
        /// </summary>
        public string MerchantName { get; set; }

        ///<summary>
        ///消费时间
        ///</summary>
        public DateTime ConsumptionDate { get; set; }

        ///<summary>
        ///商户地址
        ///</summary>
        public string MerchantAddress { get; set; }

        ///<summary>
        ///美疗师Guid
        ///</summary>
        public string TherapistGuid { get; set; }

        ///<summary>
        ///美疗师名称
        ///</summary>
        public string TherapistName { get; set; }
       
        /// <summary>
        /// 评价guid
        /// </summary>
        public string CommentGuid { get; set; }

        /// <summary>
        /// 是否已评价
        /// </summary>
        public bool IsComment { get; set; }
    }
}
