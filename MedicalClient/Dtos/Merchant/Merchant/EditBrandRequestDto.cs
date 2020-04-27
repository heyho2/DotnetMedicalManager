using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Merchant.Merchant
{
    /// <summary>
    /// 修改品牌数据
    /// </summary>
    public class EditBrandRequestDto:BaseDto
    {
        /// <summary>
        /// 品牌guid
        /// </summary>
        public string BrandGuid { get; set; }

        /// <summary>
        /// 品牌名称
        /// </summary>
        public string BrandName { get; set; }

        /// <summary>
        /// 品牌图片
        /// </summary>
        public string PictureGuid { get; set; }

    }
}
