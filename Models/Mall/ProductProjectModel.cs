using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Mall
{
    ///<summary>
    ///
    ///</summary>
    [Table("t_mall_product_project")]
    public class ProductProjectModel : BaseModel
    {

        ///<summary>
        ///商品-商品项关联GUID
        ///</summary>
        [Column("product_project_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "商品-商品项关联GUID")]
        public string ProductProjectGuid { get; set; }

        ///<summary>
        ///商品GUID
        ///</summary>
        [Column("product_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "商品GUID")]
        public string ProductGuid { get; set; }

        ///<summary>
        ///商品项目GUID
        ///</summary>
        [Column("project_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "商品项目GUID")]
        public string ProjectGuid { get; set; }

        /// <summary>
        /// 项目次数
        /// </summary>
        [Column("project_times"), Required(ErrorMessage = "{0}必填"), Display(Name = "项目次数")]
        public int ProjectTimes { get; set; }

        ///<summary>
        ///平台类型:CloudDoctor(智慧云医)；LifeCosmetology(生活美容)；MedicalCosmetology(医疗美容)
        ///</summary>
        [Column("platform_type")]
        public string PlatformType { get; set; }

        /// <summary>
        /// 是否允许转赠
        /// </summary>
        [Column("allow_present"), Required(ErrorMessage = "{0}必填"), Display(Name = "是否允许转赠")]
        public bool AllowPresent { get; set; }
    }
}



