using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Admin.Hospital
{
    /// <summary>
    /// 添加banner
    /// </summary>
    public class AddHospitalRequestDto : BaseDto
    {

        ///<summary>
        ///医院logo GUID
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "医院logo")]
        public string LogoGuid { get; set; }

        ///<summary>
        ///详情
        ///</summary>
        public string Content { get; set; }

        ///<summary>
        ///名称
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "名称")]
        public string HosName { get; set; }

        ///<summary>
        /// 医院标签
        ///</summary>
        public string HosTag { get; set; }

        ///<summary>
        ///简介
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "简介")]
        public string HosAbstract { get; set; }

        ///<summary>
        ///等级
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "等级")]
        public string HosLevel { get; set; }

        ///<summary>
        ///位置
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "位置")]
        public string Location { get; set; }
        /// <summary>
        /// 是否可查询
        /// </summary>
        public bool Visibility { get; set; } = true;

        /// <summary>
        /// 注册时间
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "注册时间")]
        public DateTime RegisteredDate { get; set; } = DateTime.Now;
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        [MaxLength(12, ErrorMessage = "{0}字段超长"), Display(Name = "联系电话")]
        public string ContactNumber { get; set; }
    }
}
