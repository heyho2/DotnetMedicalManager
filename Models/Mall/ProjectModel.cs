
using GD.Common.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Mall
{
    ///<summary>
    ///
    ///</summary>
    [Table("t_mall_project")]
    public class ProjectModel:BaseModel
    {
        
         ///<summary>
         ///服务项目GUID
         ///</summary>
         [Column("project_guid"),Key,Required(ErrorMessage = "{0}必填"), Display(Name = "服务项目GUID")]
         public string ProjectGuid{ get;set; }
        
         ///<summary>
         ///服务类型一级类型GUID
         ///</summary>
         [Column("classify_guid"),Required(ErrorMessage = "{0}必填"), Display(Name = "服务类型一级类型GUID")]
         public string ClassifyGuid{ get;set; }
        
         ///<summary>
         ///服务类型一级类型名称
         ///</summary>
         [Column("classify_name"),Required(ErrorMessage = "{0}必填"), Display(Name = "服务类型一级类型名称")]
         public string ClassifyName{ get;set; }
        
         ///<summary>
         ///商户GUID
         ///</summary>
         [Column("merchant_guid"),Required(ErrorMessage = "{0}必填"), Display(Name = "商户GUID")]
         public string MerchantGuid{ get;set; }
        
         ///<summary>
         ///服务项目名称
         ///</summary>
         [Column("project_name"),Required(ErrorMessage = "{0}必填"), Display(Name = "服务项目名称")]
         public string ProjectName{ get;set; }
        
         ///<summary>
         ///服务时长(分钟)
         ///</summary>
         [Column("operation_time")]
         public int OperationTime{ get;set; }
        
         ///<summary>
         ///项目价格
         ///</summary>
         [Column("price")]
         public decimal Price{ get;set; }

        /// <summary>
        /// 项目图片附件guid
        /// </summary>
        [Column("picture_guid")]
        public string PictureGuid { get; set; }

        ///<summary>
        ///平台类型:CLOUDDOCTOR(智慧云医)；LIFECOSMETOLOGY(生活美容)；MEDICALCOSMETOLOGY(医疗美容)
        ///</summary>
        [Column("platform_type")]
         public string PlatformType{ get;set; }
    }
}



