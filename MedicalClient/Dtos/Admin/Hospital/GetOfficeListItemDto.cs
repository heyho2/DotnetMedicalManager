using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Admin.Hospital
{
    /// <summary>
    /// 获取科室列表
    /// </summary>
    public class GetOfficeListItemDto : BaseDto
    {

        ///<summary>
        ///科室名称
        ///</summary>
        public string OfficeName { get; set; }
        ///<summary>
        ///上级科室
        ///</summary>
        public string ParentName { get; set; }
        /// <summary>
        /// 顺序
        /// </summary>
        public string Sort { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }
        /// <summary>
        /// 子集科室
        /// </summary>
        public List<GetOfficeListItemDto> Children { get; set; }
    }
}
