using GD.Common.Base;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Mall.Project
{
    /// <summary>
    /// 创建商户服务项目请求类
    /// </summary>
    public class AddMerchantProjectRequestDto : BaseDto
    {
        ///<summary>
        ///服务项目GUID
        ///</summary>
        public string ProjectGuid { get; set; }

        ///<summary>
        ///服务类型一级类型GUID
        ///</summary>
        public string ClassifyGuid { get; set; }

        ///<summary>
        ///服务类型一级类型名称
        ///</summary>
        [Required(ErrorMessage = "所属大类需选择")]
        public string ClassifyName { get; set; }

        ///<summary>
        ///商户GUID
        ///</summary>
        public string MerchantGuid { get; set; }

        ///<summary>
        ///服务项目名称
        ///</summary>
        [Required(ErrorMessage = "项目名称需填写")]
        [MaxLength(50, ErrorMessage = "超过项目名称最大限制长度")]
        public string ProjectName { get; set; }

        ///<summary>
        ///服务时长(分钟)
        ///</summary>
        public int OperationTime { get; set; }

        ///<summary>
        ///项目价格
        ///</summary>
        public decimal Price { get; set; }

        ///<summary>
        ///平台类型:CLOUDDOCTOR(智慧云医)；LIFECOSMETOLOGY(生活美容)；MEDICALCOSMETOLOGY(医疗美容)
        ///</summary>
        public string PlatformType { get; set; } = "CLOUDDOCTOR";
    }
}
