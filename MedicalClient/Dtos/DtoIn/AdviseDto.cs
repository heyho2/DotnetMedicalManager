using GD.Common.Base;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.DtoIn
{
    /// <summary>
    /// 建议
    /// </summary>
    public class AdviseDto : BaseDto
    {
        /// <summary>
        /// 建议人姓名
        /// </summary>
        public string Adviser { get; set; }

        /// <summary>
        /// 建议人手机号
        /// </summary>
        public string AdviserPhone { get; set; }

        /// <summary>
        /// 建议人Emall
        /// </summary>
        public string AdviserEmail { get; set; }

        /// <summary>
        /// 建议内容
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "建议内容")]
        public string AdviseContent { get; set; }
    }
}
