using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GD.Dtos.Article
{
    /// <summary>
    /// 文章来源类型
    /// </summary>
    public enum ArticleSourceTypeEnum
    {
        /// <summary>
        /// 医生文章
        /// </summary>
        [Description("医生文章")]
        Doctor = 1,

        /// <summary>
        /// 平台文章
        /// </summary>
        [Description("平台文章")]
        Manager
    }
}
