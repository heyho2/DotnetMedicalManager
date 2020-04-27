using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Mall
{
    ///<summary>
    ///超值套餐表
    ///</summary>
    [Table("t_mall_extra_package")]
    public class ExtraPackageModel : BaseModel
    {

        ///<summary>
        ///超值套餐GUID
        ///</summary>
        [Column("package_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "超值套餐GUID")]
        public string PackageGuid { get; set; }

        ///<summary>
        ///超值套餐名称
        ///</summary>
        [Column("package_name")]
        public string PackageName { get; set; }

        ///<summary>
        ///活动GUID
        ///</summary>
        [Column("campaign_guid")]
        public string CampaignGuid { get; set; }

        ///<summary>
        ///产品组合和选择规则JSON
        ///</summary>
        [Column("details")]
        public object Details { get; set; }


        ///<summary>
        ///平台类型:CloudDoctor(智慧云医)；LifeCosmetology(生活美容)；MedicalCosmetology(医疗美容)
        ///</summary>
        [Column("platform_type")]
        public string PlatformType { get; set; }
    }
}



