using GD.Common.Base;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Doctor.Hospital
{
    public class CreateHospitalEvaluvationRequestDto : BaseDto
    {

        /// <summary>
        /// 获取跳转到提交页面链接参数（预生成评论Id)
        /// </summary>
        [Required(ErrorMessage = "评论参数未提供")]
        public string EvaluationGuid { get; set; }
        /// <summary>
        /// 医院GUID
        /// </summary>
        [Required(ErrorMessage = "医院参数未提供")]
        public string HospitalGuid { get; set; }

        /// <summary>
        /// 科室GUID
        /// </summary>
        [Required(ErrorMessage = "科室必填")]
        public string OfficeGuid { get; set; }

        /// <summary>
        /// 评价标签
        /// </summary>
        [Required(ErrorMessage = "评价标签必填")]
        public string[] Tags { get; set; }

        /// <summary>
        /// 评分
        /// </summary>
        public decimal Score { get; set; }

        /// <summary>
        /// 病情详情
        /// </summary>
        [Required(ErrorMessage = "病情信息必填")]
        [MaxLength(4000, ErrorMessage = "超过最大填写长度")]
        public string ConditionDetail { get; set; }
        /// <summary>
        /// 是否匿名评论
        /// </summary>
        public bool Anonymous { get; set; }
    }
}
