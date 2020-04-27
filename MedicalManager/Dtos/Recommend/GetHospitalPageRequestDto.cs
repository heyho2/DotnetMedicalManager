using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Recommend
{
    /// <summary>
    /// 推荐列表 请求
    /// </summary>
    public class GetRecommendPageRequestDto : BasePageRequestDto,IBaseOrderBy
    {
        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public string SortField { get; set; }
        /// <summary>
        /// 是否升序
        /// </summary>
        public bool IsAscending { get; set; }
    }
    /// <summary>
    /// 推荐列表 响应
    /// </summary>
    public class GetRecommendPageResponseDto : BasePageResponseDto<GetRecommendPageItemDto>
    {
    }
    /// <summary>
    /// 推荐列表 项
    /// </summary>
    public class GetRecommendPageItemDto : BaseDto
    {
        ///<summary>
        ///推荐GUID
        ///</summary>
        public string RecommendGuid { get; set; }

        ///<summary>
        ///推荐归属GUID
        ///</summary>
        public string OwnerGuid { get; set; }

        ///<summary>
        ///名称
        ///</summary>
        public string Name { get; set; }

        ///<summary>
        ///点击响应目标
        ///</summary>
        public string Target { get; set; }

        ///<summary>
        ///图片GUID
        ///</summary>
        public string PictureGuid { get; set; }

        ///<summary>
        ///排序
        ///</summary>
        public int Sort { get; set; }
        ///<summary>
        ///类型[Campaign,Article,Product,Doctor,Office,Hostpital]
        ///</summary>
        public string Type { get; set; }
        /// <summary>
        /// 图片url
        /// </summary>
        public string PictureUrl { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationDate { get; set; }
    }
}
