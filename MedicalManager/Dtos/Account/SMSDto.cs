using GD.Common.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Account
{
    /// <summary>
    /// 短信DTO
    /// </summary>
    public class SMSDto : BaseDto
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "电话号码"), Phone(ErrorMessage = "请输入正确的电话号码")]
        public string Phone
        {
            get;
            set;
        }

        /// <summary>
        /// 填充短信模板的参数
        /// </summary>
        public IList<string> Parameters
        {
            get;
            set;
        }

        /// <summary>
        /// 模板ID
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "模板ID"), Range(1, int.MaxValue, ErrorMessage = "{0}无效")]
        public int TemplateId
        {
            get;
            set;
        }

        /// <summary>
        /// 签名
        /// </summary>
        public string Sign
        {
            get;
            set;
        }
    }
}
