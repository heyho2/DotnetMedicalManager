using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Consumer.Consumer
{
    /// <summary>
    /// 获取我的礼物列表--双美 响应Dto
    /// </summary>
    public class GetMyGiftsOfCosmetologyResponseDto : BaseDto
    {
        /// <summary>
        /// 礼物guid
        /// </summary>
        public string GiftGuid { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 项目耗时
        /// </summary>
        public string OperationTime { get; set; }

        /// <summary>
        /// 赠送人昵称
        /// </summary>
        public string FromNickName { get; set; }

        /// <summary>
        /// 有效期开始日期
        /// </summary>
        public string EffectiveStartDate { get; set; }

        /// <summary>
        /// 有效期结束日期
        /// </summary>
        public string EffectiveEndDate { get; set; }

        /// <summary>
        /// 项目图片
        /// </summary>
        public string ProjectPicture { get; set; }

        /// <summary>
        /// 使用门店名称（以逗号拼接）
        /// </summary>
        public string MerchantNames { get; set; }
    }
}
