﻿using GD.Common.Base;

namespace GD.Dtos.Doctor.Doctor
{
    /// <summary>
    /// 获取问医服务的医生列表响应Dto
    /// </summary>
    public class GetAskedDoctorsResponseDto : BasePageResponseDto<GetAskedDoctorsItemDto>
    {

    }
    /// <summary>
    /// 获取问医服务的医生列表 项
    /// </summary>
    public class GetAskedDoctorsItemDto : BaseDto
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
