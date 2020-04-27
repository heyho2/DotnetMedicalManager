using GD.Common.Base;
using System;

namespace GD.Dtos.Admin.Doctor
{
    /// <summary>
    /// 审核医生列表
    /// </summary>
    public class GetReviewDoctorPageRequestDto : BasePageRequestDto, IBaseOrderBy
    {
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime? BeginDate { get; set; }
        /// <summary>
        /// 注册时间 至
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public string SortField { get; set; }
        /// <summary>
        /// 是否升序
        /// </summary>
        public bool IsAscending { get; set; }
    }
    /// <summary>
    /// 审核医生列表
    /// </summary>
    public class GetReviewDoctorPageResponseDto : BasePageResponseDto<GetReviewDoctorPageItemDto>
    {


    }
    /// <summary>
    /// 审核医生列表
    /// </summary>
    public class GetReviewDoctorPageItemDto : BaseDto
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
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreationDate { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string SignatureUrl { get; set; }
    }
}
