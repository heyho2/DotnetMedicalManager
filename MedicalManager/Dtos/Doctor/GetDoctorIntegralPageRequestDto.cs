using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Doctor
{
    /// <summary>
    /// 医生积分列表
    /// </summary>
    public class GetDoctorIntegralPageRequestDto : BasePageRequestDto, IBaseOrderBy
    {
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }

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
    /// 医生积分列表
    /// </summary>
    public class GetDoctorIntegralPageResponseDto : BasePageResponseDto<GetDoctorIntegralPageItemDto>
    {
    }
    /// <summary>
    /// 医生积分列表
    /// </summary>
    public class GetDoctorIntegralPageItemDto : BaseDto
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
        public string Phone { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationDate { get; set; }
        /// <summary>
        /// enable
        /// </summary>
        public bool Enable { get; set; }
        /// <summary>
        /// 总积分
        /// </summary>
        public decimal TotalPoints { get; set; }
        /// <summary>
        /// 赚取的积分
        /// </summary>
        public decimal EarnPoints { get; set; }
        /// <summary>
        /// 使用的积分
        /// </summary>
        public decimal UsePoints { get; set; }
    }
}
