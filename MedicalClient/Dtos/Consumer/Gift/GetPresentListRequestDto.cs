using System;
using System.Collections.Generic;
using System.Text;
using GD.Common.Base;

namespace GD.Dtos.Consumer.Gift
{
    /// <inheritdoc />
    /// <summary>
    ///生美-	获取商品赠品 请求Dto
    /// </summary>
    public class GetPresentListRequestDto : BasePageRequestDto
    {

        /// <summary>
        /// 产品Guid
        /// </summary>
        public string ProductGuid { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        public bool Enable { get; set; } = true;
    }
    /// <inheritdoc />
    /// <summary>
    /// 响应Dto
    /// </summary>
    public class GetPresentListResponseDto : BasePageResponseDto<GetPresentListItem>
    {
      
    }
    /// <inheritdoc />
    /// <summary>
    /// 子项
    /// </summary>
    public class GetPresentListItem : BaseDto
    {
        /// <summary>
        /// 赠品guid
        /// </summary>
        public string PresentGuid { get; set; }

        /// <summary>
        /// 产品guid
        /// </summary>
        public string ProductGuid { get; set; }

        /// <summary>
        /// 产品guid
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 目标guid
        /// </summary>
        public string TargetGuid { get; set; }

        /// <summary>
        /// 目标guid
        /// </summary>
        public string UserGuid{ get; set; }

        /// <summary>
        /// 赠品名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 赠品类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public string Count { get; set; }

    }

}