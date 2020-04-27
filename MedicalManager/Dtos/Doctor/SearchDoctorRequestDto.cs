using GD.Common.Base;

namespace GD.Dtos.Doctor.Doctor
{
    /// <summary>
    /// 搜索医生 请求
    /// </summary>
    public class SearchDoctorRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string Keyword { get; set; }
    }
    /// <summary>
    /// 搜索医生 响应
    /// </summary>
    public class SearchDoctorResponseDto : BasePageResponseDto<SearchDoctorItemDto>
    {

    }
    /// <summary>
    /// 搜索医生 项
    /// </summary>
    public class SearchDoctorItemDto : BaseDto
    {
        ///<summary>
        ///医生GUID
        ///</summary>
        public string DoctorGuid { get; set; }
        ///<summary>
        ///所属医院GUID
        ///</summary>
        public string HospitalGuid { get; set; }
        ///<summary>
        ///所属医院名称
        ///</summary>
        public string HospitalName { get; set; }
        /// <summary>
        /// 所属医院logo
        /// </summary>
        public string HospitalPic { get; set; }
        ///<summary>
        ///所属科室GUID
        ///</summary>
        public string OfficeGuid { get; set; }
        ///<summary>
        ///所属科室GUID
        ///</summary>
        public string OfficeName { get; set; }
        ///<summary>
        ///一寸照Guid
        ///</summary>
        public string PortraitUrl { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string UserName { get; set; }
        ///<summary>
        ///工作城市
        ///</summary>
        public string WorkCity { get; set; }

        ///<summary>
        ///背景
        ///</summary>
        public string Background { get; set; }

        ///<summary>
        ///职称GUID
        ///</summary>
        public string Title { get; set; }

        ///<summary>
        ///擅长的标签
        ///</summary>
        public string AdeptTags { get; set; }

        ///<summary>
        ///工龄
        ///</summary>
        public int WorkAge { get; set; }
        /// <summary>
        /// 工号
        /// </summary>
        public string JobNumber { get; set; }

        /// <summary>
        /// 所获所获荣誉
        /// </summary>
        public string Honor { get; set; }

        /// <summary>
        /// 执业医院
        /// </summary>
        public string PractisingHospital { get; set; }

        /// <summary>
        /// 账号申请状态
        /// 'reject','approved','submit','draft'
        /// </summary>
        public string Status { get; set; }
    }
}
