using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Doctor
{
    /// <summary>
    /// 获取医生信息
    /// </summary>
    public class GetDoctorInfoRequestDto : BaseDto
    {
        /// <summary>
        /// 医生Guid
        /// </summary>
        public string DoctorGuid { get; set; }
    }
    /// <summary>
    /// 获取医生信息 响应
    /// </summary>
    public class GetDoctorInfoResponseDto : BaseDto
    {

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime Birthday { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdentityNumber { get; set; }

        /// <summary>
        /// 所属医院
        /// </summary>
        public string HospitalGuid { get; set; }

        /// <summary>
        /// 工作城市 省-市-区
        /// </summary>
        public string[] WorkCity { get; set; }

        /// <summary>
        /// 执业医院
        /// </summary>
        public string PractisingHospital { get; set; }

        /// <summary>
        /// 所在科室Guid
        /// </summary>
        public string OfficeGuid { get; set; }

        /// <summary>
        /// 职称Guid
        /// </summary>
        public string TitleGuid { get; set; }

        /// <summary>
        /// 所获荣誉
        /// </summary>
        public string Honor { get; set; }

        /// <summary>
        /// 背景
        /// </summary>
        public string Background { get; set; }

        /// <summary>
        /// 擅长
        /// </summary>
        public string[] AdeptTags { get; set; }
        /// <summary>
        /// 医生id
        /// </summary>
        public string DoctorGuid { get; set; }


        /// <summary>
        /// 签名附件guid
        /// </summary>
        public string SignatureGuid { get; set; }

        /// <summary>
        /// 一寸照附件Guid
        /// </summary>
        public string PortraitGuid { get; set; }
        /// <summary>
        /// 一寸照附件url
        /// </summary>
        public string PortraitUrl { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 推荐
        /// </summary>
        public bool IsRecommend { get; set; }
        /// <summary>
        /// 工作年龄
        /// </summary>
        public int WorkAge { get; set; }
        /// <summary>
        /// 工号
        /// </summary>
        public string JobNumber { get; set; }
        /// <summary>
        /// 是否禁用
        /// </summary>
        public bool Enable { get; set; }
    }
}
