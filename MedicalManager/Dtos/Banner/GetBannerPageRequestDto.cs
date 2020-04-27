using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Banner
{
    /// <summary>
    /// 获取banner 请求
    /// </summary>
    public class GetBannerPageRequestDto : BasePageRequestDto, IBaseOrderBy
    {
        /// <summary>
        /// 类型（位置）
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
    /// 获取banner 响应
    /// </summary>
    public class GetBannerPageResponseDto : BasePageResponseDto<GetBannerPageItemDto>
    {

    }
    /// <summary>
    /// 获取banner 项
    /// </summary>
    public class GetBannerPageItemDto : BaseDto
    {
        /// <summary>
        /// guid
        /// </summary>
        public string BannerGuid { get; set; }
        /// <summary>
        /// Banner名称
        /// </summary>
        public string BannerName { get; set; }

        /// <summary>
        /// 图片Url
        /// </summary>
        public string Picture { get; set; }

        /// <summary>
        /// 链接Url
        /// </summary>
        public string TargetUrl { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 启用？
        /// </summary>
        public bool Enable { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationDate { get; set; }
        ///<summary>
        ///BANNER图片GUID
        ///</summary>
        public string PictureGuid { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string OwnerGuid { get; set; }


    }
}
