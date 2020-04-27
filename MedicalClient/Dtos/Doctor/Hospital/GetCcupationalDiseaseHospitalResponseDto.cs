using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Doctor.Hospital
{
    /// <summary>
    /// 获取职业病资质医院响应Dto
    /// </summary>
    public class GetCcupationalDiseaseHospitalResponseDto : BaseDto
    {
        ///<summary>
        ///医院GUID
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "医院GUID")]
        public string HospitalGuid
        {
            get;
            set;
        }

        ///<summary>
        ///名称
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "名称")]
        public string HosName
        {
            get;
            set;
        }

        ///<summary>
        ///医院标签
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "医院标签")]
        public string HosTag
        {
            get;
            set;
        }

        ///<summary>
        ///简介
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "简介")]
        public string HosAbstract
        {
            get;
            set;
        }

        ///<summary>
        ///图片
        ///</summary>
        
        public string Picture
        {
            get;
            set;
        }
        /// <summary>
        /// 导诊链接
        /// </summary>
        public string GuidanceUrl { get; set; }
    }
}
