using GD.Common.Base;
using GD.Dtos.Common;
using System.Collections.Generic;

namespace GD.Dtos.Hospital
{
    /// <summary>
    /// 获取全部医院（下拉框）
    /// </summary>
    public class GetHospitalOfficeTreeItemDto : BaseTreeDto<GetHospitalOfficeTreeItemDto>
    {
        /// <summary>
        /// Guid
        /// </summary>
        public string Guid { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
        ///<summary>
        ///上级科室
        ///</summary>
        public string ParentGuid { get; set; }
    }
}
