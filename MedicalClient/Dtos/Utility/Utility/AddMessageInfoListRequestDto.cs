using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using GD.Models.CommonEnum;

namespace GD.Dtos.Utility.Utility
{
    /// <summary>
    /// 批量新增Topic记录 请求 
    /// </summary>
    public class AddMessageInfoListRequestDto : BaseDto
    {
        ///<summary>
        ///发起者GUID
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "发起者GUID")]
        public string SponsorGuid
        {
            get;
            set;
        }

        ///<summary>
        ///接收者GUID
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "接收者GUID")]
        public string ReceiverGuid
        {
            get;
            set;
        }

        ///<summary>
        ///话题关于GUID(如来自商品或直接问的医生）
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "话题关于GUID(如来自商品或直接问的医生）")]
        public string AboutGuid
        {
            get;
            set;
        }

        ///<summary>
        ///aboutGuid对应表的类型
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "aboutGuid对应表的类型")]
        public string EnumTb { get; set; } = TopicAboutTypeEnum.Doctor.ToString();

        ///<summary>
        ///开始时间
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "开始时间")]
        public DateTime BeginDate
        {
            get;
            set;
        }

        ///<summary>
        ///结束时间
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "结束时间")]
        public DateTime EndDate
        {
            get;
            set;
        }
    }
}
