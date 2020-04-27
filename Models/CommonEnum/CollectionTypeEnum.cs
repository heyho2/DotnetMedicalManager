using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GD.Models.CommonEnum
{
    /// <summary>
    /// 收藏目标类型
    /// </summary>
    public enum CollectionTypeEnum
    {
        /// <summary>
        /// 商品
        /// </summary>
        [Description("商品")]
        Product,

        /// <summary>
        /// 医院
        /// </summary>
        [Description("医院")]
        Hospital,

        /// <summary>
        /// 科室
        /// </summary>
        [Description("科室")]
        Office,

        /// <summary>
        /// 医生
        /// </summary>
        [Description("医生")]
        Doctor,

        /// <summary>
        /// 文章
        /// </summary>
        [Description("文章")]
        Article,

        /// <summary>
        /// 平台文章
        /// </summary>
        [Description("平台文章")]
        Course
    }
}
