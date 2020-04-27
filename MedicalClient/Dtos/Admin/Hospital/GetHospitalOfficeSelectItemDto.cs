using GD.Common.Base;
using System.Collections.Generic;

namespace GD.Dtos.Admin.Hospital
{
    /// <summary>
    /// 获取全部医院（下拉框）
    /// </summary>
    public class GetHospitalOfficeSelectItemDto : BaseDto
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
        /// <summary>
        /// 子集科室
        /// </summary>
        public List<GetHospitalOfficeSelectItemDto> Children { get; set; }
    }
}
