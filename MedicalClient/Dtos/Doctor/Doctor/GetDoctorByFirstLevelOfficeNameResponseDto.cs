using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Doctor.Doctor
{
    /// <summary>
    /// 根据一级科室名称获取医生响应Dto
    /// </summary>
    public class GetDoctorByFirstLevelOfficeNameResponseDto : BasePageResponseDto<GetDoctorByFirstLevelOfficeNameItemDto>
    {
    }

    /// <summary>
    /// 根据一级科室名称获取医生ItemDto
    /// </summary>
    public class GetDoctorByFirstLevelOfficeNameItemDto : BaseDto
    {
        ///<summary>
        ///医生GUID
        ///</summary>
        public string DoctorGuid { get; set; }

        /// <summary>
        /// 医生姓名
        /// </summary>
        public string DoctorName { get; set; }

        ///<summary>
        ///所属医院GUID
        ///</summary>
        public string HospitalGuid { get; set; }

        ///<summary>
        ///所属医院名称
        ///</summary>
        public string HospitalName { get; set; }

        /// <summary>
        /// 医院图片
        /// </summary>
        public string HospitalPicture { get; set; }

        ///<summary>
        ///所属科室GUID
        ///</summary>
        public string OfficeGuid { get; set; }

        ///<summary>
        ///所属科室名称
        ///</summary>
        public string OfficeName { get; set; }

        ///<summary>
        ///职称
        ///关联字典表获取
        ///</summary>
        public string Title { get; set; }

        ///<summary>
        ///擅长的标签
        ///</summary>
        public string AdeptTags { get; set; }

        /// <summary>
        /// 所获所获荣誉
        /// </summary>
        public string Honor { get; set; }

        /// <summary>
        /// 一寸照
        /// </summary>
        public string Picture { get; set; }
    }
}
