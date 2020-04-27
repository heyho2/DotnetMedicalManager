using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Hospital
{
    /// <summary>
    /// 添加科室
    /// </summary>
    public class AddOfficeRequestDto : BaseDto
    {
        ///<summary>
        ///科室名称
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "科室名称")]
        public string OfficeName { get; set; }
        ///<summary>
        ///上级科室
        ///</summary>
        public string ParentName { get; set; }

        ///<summary>
        ///科室图片
        ///</summary>
        public string PictureGuid { get; set; }

        ///<summary>
        ///排序
        ///</summary>
        public int Sort { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }
    }
}
