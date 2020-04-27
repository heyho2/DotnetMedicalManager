using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Mall.Mall
{
    /// <summary>
    /// 商品项数据Dto
    /// </summary>
    public class ProjectInfoDto : BaseDto
    {
        /// <summary>
        /// 商品项Guid
        /// </summary>
        public string ProjectGuid { get; set; }

        /// <summary>
        /// 商品项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 商品项目次数
        /// </summary>
        public int ProjectTimes { get; set; }

        /// <summary>
        /// 项目数量是否无限次
        /// </summary>
        public bool Infinite { get; set; } = false;
    }
}
