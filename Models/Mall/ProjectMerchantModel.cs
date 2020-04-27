
using GD.Common.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Mall
{
    ///<summary>
    ///商品项目适用门店表
    ///</summary>
    [Table("t_mall_project_merchant")]
    public class ProjectMerchantModel : BaseModel
    {
        ///<summary>
        ///适用门店关系GUID
        ///</summary>
        [Column("project_merchant_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "适用门店关系GUID")]
        public string ProjectMerchantGuid { get; set; }

        ///<summary>
        ///商品项目GUID
        ///</summary>
        [Column("project_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "商品项目GUID")]
        public string ProjectGuid { get; set; }

        ///<summary>
        ///商铺GUID
        ///</summary>
        [Column("merchant_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "商铺GUID")]
        public string MerchantGuid { get; set; }

        ///<summary>
        ///平台类型:CloudDoctor(智慧云医)；LifeCosmetology(生活美容)；MedicalCosmetology(医疗美容)
        ///</summary>
        [Column("platform_type")]
        public string PlatformType { get; set; }
        ///<summary>
        ///排序
        ///</summary>
        [Column("sort"), Display(Name = "排序")]
        public int Sort
        {
            get;
            set;
        }
    }
}
