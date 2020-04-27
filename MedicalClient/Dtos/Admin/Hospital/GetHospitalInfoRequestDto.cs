﻿using GD.Common.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Admin.Hospital
{
    /// <summary>
    /// 获取医院 详细 请求
    /// </summary>
    public class GetHospitalInfomRequestDto : BaseDto
    {
        ///<summary>
        ///医院GUID
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "医院GUID")]
        public string HospitalGuid { get; set; }
    }
    /// <summary>
    /// 获取医院 详细 响应
    /// </summary>
    public class GetHospitalInfomResponseDto : BaseDto
    {
        ///<summary>
        ///医院GUID
        ///</summary>
        public string HospitalGuid { get; set; }


        ///<summary>
        ///医院logo GUID
        ///</summary>
        public string LogoGuid { get; set; }
        /// <summary>
        ///医院logo url
        /// </summary>
        public string LogoUrl { get; set; }

        ///<summary>
        ///详情
        ///</summary>
        public string Content { get; set; }

        ///<summary>
        ///名称
        ///</summary>
        public string HosName { get; set; }

        ///<summary>
        /// 医院标签
        ///</summary>
        public string HosTag { get; set; }

        ///<summary>
        ///简介
        ///</summary>
        public string HosAbstract { get; set; }

        ///<summary>
        ///等级
        ///</summary>
        public string HosLevel { get; set; }

        ///<summary>
        ///位置
        ///</summary>
        public string Location { get; set; }
        /// <summary>
        /// 是否可查询
        /// </summary>
        public bool Visibility { get; set; } = true;

        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime RegisteredDate { get; set; } = DateTime.Now;
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactNumber { get; set; }
    }
}