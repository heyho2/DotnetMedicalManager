using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GD.Common.Base;

namespace GD.Models.Merchant
{
    ///<summary>
    ///产品品牌表模型
    ///</summary>
    [Table("t_merchant_brand")]
    public class BrandModel : BaseModel
    {
        ///<summary>
        ///产品品牌GUID
        ///</summary>
        [Column("brand_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "产品品牌GUID")]
        public string BrandGuid
        {
            get;
            set;
        }

        ///<summary>
        ///所属商户guid
        ///</summary>
        [Column("merchant_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "所属商户guid")]
        public string MerchantGuid
        {
            get;
            set;
        }

        ///<summary>
        ///产品名称
        ///</summary>
        [Column("brand_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "产品名称")]
        public string BrandName
        {
            get;
            set;
        }

        ///<summary>
        ///排序
        ///</summary>
        [Column("sort"), Required(ErrorMessage = "{0}必填"), Display(Name = "排序")]
        public int Sort
        {
            get;
            set;
        }
        

        ///<summary>
        ///图片guid
        ///</summary>
        [Column("picture_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "图片guid")]
        public string PictureGuid
        {
            get;
            set;
        }
    }
}