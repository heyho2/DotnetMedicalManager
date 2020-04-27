using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using GD.Common.Base;

namespace GD.Models.CrossTable
{
    /// <summary>
    /// 我的医生 请求
    /// </summary>
    public class GetMyDoctorsRequestDto : BasePageRequestDto
    {
    }
    /// <summary>
    /// 我的医生 响应
    /// </summary>
    public class GetMyDoctorsResponseDto : BasePageResponseDto<GetMyDoctorsItemDto>
    {
    }
    /// <summary>
    /// 我的医生 项
    /// </summary>
    public class GetMyDoctorsItemDto : BaseDto
    {
        ///<summary>
        ///医生Guid
        ///</summary>
        public string DoctorGuid
        {
            get;
            set;
        }

        ///<summary>
        ///医院Guid
        ///</summary>
        public string HospitalGuid
        {
            get;
            set;
        }

        /// <summary>
        /// 擅长
        /// </summary>
        public string AdeptTags { get; set; }

        ///<summary>
        ///医生头像URL
        ///</summary>
        public string DocPortrait
        {
            get;
            set;
        }

        ///<summary>
        ///医生真实姓名
        ///</summary>
        public string UserName
        {
            get;
            set;
        }

        ///<summary>
        ///医院LogoURL
        ///</summary>
        public string HospitalPic
        {
            get;
            set;
        }

        ///<summary>
        ///医院名称
        ///</summary>
        public string HosName
        {
            get;
            set;
        }

        ///<summary>
        ///科室名称
        ///</summary>
        public string OfficeName
        {
            get;
            set;
        }

        ///<summary>
        ///医生职称
        ///</summary>
        public string ConfigName
        {
            get;
            set;
        }
    }
}
