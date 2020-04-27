using System;
using System.Collections.Generic;
using System.Text;
using GD.Common.Base;

namespace GD.Dtos.Mall.Mall
{
   /// <inheritdoc />
   /// <summary>
   /// 云购分类- 子分类
   /// </summary>
    public class SubClassifyResponseDto : BaseDto
    {
        /// <summary>
        /// 一级分类ID
        /// </summary>
        public string FirstClassifyGuid { get; set; }
        /// <summary>
        /// 子分类ID
        /// </summary>
        public string SubClassifyGuid { get; set; }
        /// <summary>
        /// 子分类名称
        /// </summary>
        public string SubClassifyName { get; set; }
        /// <summary>
        /// 子分类logo
        /// </summary>
        public string SubClassifyPic { get; set; }

        ///<summary>
        ///排序
        ///</summary>
        public int Sort { get; set; }
    }
}
