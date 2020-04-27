using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Mall.Project
{
    /// <summary>
    /// 商品包含项目dto
    /// </summary>
    public class ProductProjectDto : BaseDto
    {
        /// <summary>
        /// 商品guid
        /// </summary>
        public string ProductGuid { get; set; }

        /// <summary>
        /// 项目guid
        /// </summary>
        public string ProjectGuid { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 项目时长
        /// </summary>
        public int OperationTime { get; set; }

        /// <summary>
        /// 项目次数
        /// </summary>
        public int ProjectTimes { get; set; }

        /// <summary>
        /// 项目价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 是否允许转让
        /// </summary>
        public bool AllowPresent { get; set; }
    }
}
