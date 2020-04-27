using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Consumer.Consumer
{
    /// <summary>
    /// 检测个人商品使用项目次数是否达到阈值
    /// </summary>
    public class CheckGoodsThresholdIsExceededDto : BaseDto
    {
        /// <summary>
        /// 个人商品guid
        /// </summary>
        public string GoodsGuid { get; set; }

        /// <summary>
        /// 个人商品使用项目次数阈值
        /// </summary>
        public string ProjectThreshold { get; set; }

        /// <summary>
        /// 个人商品使用项目次数阈值
        /// </summary>
        public int? ProjectUsedSum { get; set; }
    }
}
