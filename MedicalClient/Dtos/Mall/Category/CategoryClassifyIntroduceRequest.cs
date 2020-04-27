using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Mall.Category
{
    /// <summary>
    /// 类目介绍
    /// </summary>
    public class CategoryClassifyIntroduceRequest
    {
        /// <summary>
        ///类目Guid
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "类目Guid")]
        public string CategoryGuid { get; set; }

    }
    /// <summary>
    /// response
    /// </summary>
    public class CategoryClassifyIntroduceResponse : BaseDto
    {
        /// <summary>
        /// 类目Guid
        /// </summary>
        public string CategoryGuid { get; set; }

        /// <summary>
        ///  二级分类Guid
        /// </summary>
        public string ClassifyGuid { get; set; }
        /// <summary>
        /// 商户guid
        /// </summary>
        public string MerchantGuid { get; set; }
        /// <summary>
        /// 类目名称
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        public string DetailAddress { get; set; }
        /// <summary>
        /// 封面图片URL
        /// </summary>
        public string CoverURL { get; set; }
        /// <summary>
        /// 介绍
        /// </summary>
        public string Introduction { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 类型下项目取消预约需要提前的时间
        /// </summary>
        public int LimitTime { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public decimal Latitude { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public decimal Longitude { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// Banner列表
        /// </summary>
        public List<CategoryBannerInfo> CategoryBannerInfoList { get; set; }

        /// <summary>
        /// Banner
        /// </summary>
        public class CategoryBannerInfo
        {
            /// <summary>
            ///  BannerGuid
            /// </summary>
            public string BannerGuid { get; set; }
            /// <summary>
            ///  Banner名称
            /// </summary>
            public string BannerName { get; set; }
            /// <summary>
            ///  目标地址
            /// </summary>
            public string TargetUrl { get; set; }
            /// <summary>
            ///  描述
            /// </summary>
            public string Description { get; set; }
            /// <summary>
            ///  排序
            /// </summary>
            public int Sort { get; set; }
            /// <summary>
            ///  图片URL 
            /// </summary>
            public string PictureURL { get; set; }

            ///// <summary>
            /////  图片URL
            ///// </summary>
            //public class BannerPicUrl
            //{
            //    /// <summary>
            //    /// 图片URL
            //    /// </summary>
            //    public string PictureURL { get; set; }
            //}


        }

    }
}
