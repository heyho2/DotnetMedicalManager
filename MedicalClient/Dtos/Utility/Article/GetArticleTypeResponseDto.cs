using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Utility.Article
{
    /// <summary>
    /// 获取文章类型响应Dto
    /// </summary>
    public class GetArticleTypeResponseDto : BaseDto
    {
        /// <summary>
        /// 配置项Guid
        /// </summary>
        public string DicGuid { get; set; }

        /// <summary>
        /// 配置项Code
        /// </summary>
        public string ConfigCode { get; set; }

        /// <summary>
        /// 配置项名称
        /// </summary>
        public string ConfigName { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
    }
}
