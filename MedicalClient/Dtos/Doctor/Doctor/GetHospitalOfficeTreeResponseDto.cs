using GD.Common.Base;
using System.Collections.Generic;

namespace GD.Dtos.Doctor.Doctor
{
    /// <summary>
    /// 获取医院科室树结构数据 项
    /// </summary>
    public class GetHospitalOfficeTreeOfficeItemDto : BaseDto
    {
        ///<summary>
        ///科室GUID
        ///</summary>
        public string OfficeGuid { get; set; }

        ///<summary>
        ///科室名称
        ///</summary>
        public string OfficeName { get; set; }

        ///<summary>
        ///上级科室
        ///</summary>
        public string ParentOfficeGuid { get; set; }

        ///<summary>
        ///所属医院GUID
        ///</summary>
        public string HospitalGuid { get; set; }

        ///<summary>
        ///科室图片
        ///关联附件表
        ///</summary>
        public string Picture { get; set; }

        ///<summary>
        ///是否推荐
        ///</summary>
        public bool Recommend { get; set; }
        ///<summary>
        ///排序
        ///</summary>
        public int Sort { get; set; }

        /// <summary>
        /// 下属科室
        /// </summary>
        public List<GetHospitalOfficeTreeOfficeItemDto> SubordinateOffeces { get; set; }
    }
}
