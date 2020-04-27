using GD.Common.Base;

namespace GD.Dtos.Manager.Banner
{
    /// <summary>
    /// Banner响应基类
    /// </summary>
    public class BannerBaseDto : BaseDto
    {
        /// <summary>
        /// BannerGuid
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
        /// 图片附件guid
        /// </summary>
        public string PictureGuid { get; set; }

        /// <summary>
        /// 链接Url
        /// </summary>
        public string TargetUrl { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int? Sort { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }
}
