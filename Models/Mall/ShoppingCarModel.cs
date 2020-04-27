using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GD.Common.Base;
using GD.Models.CommonEnum;

namespace GD.Models.Mall
{
    ///<summary>
    ///购物车表模型
    ///</summary>
    [Table("t_mall_shopping_car")]
    public class ShoppingCarModel : BaseModel
    {
        ///<summary>
        ///GUID(根据USER_GUID区分购车）
        ///</summary>
        [Column("item_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "GUID(根据USER_GUID区分购车）")]
        public string ItemGuid
        {
            get;
            set;
        }

        ///<summary>
        ///用户GUID
        ///</summary>
        [Column("user_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "用户GUID")]
        public string UserGuid
        {
            get;
            set;
        }
        ///<summary>
        ///商户guid
        ///</summary>
        [Column("merchant_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "商户GUID")]
        public string MerchantGuid { get; set; }

        ///<summary>
        ///产品GUID
        ///</summary>
        [Column("product_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "产品GUID")]
        public string ProductGuid
        {
            get;
            set;
        }
        ///<summary>
        ///产品名称
        ///</summary>
        [Column("product_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "产品名称")]
        public string ProductName
        {
            get;
            set;
        }

        ///<summary>
        ///产品数量
        ///</summary>
        [Column("count"), Required(ErrorMessage = "{0}必填"), Display(Name = "产品数量")]
        public int Count
        {
            get;
            set;
        }

        ///<summary>
        ///是否有效（当库存不够，则显示无效）
        ///</summary>
        [Column("is_valid"), Required(ErrorMessage = "{0}必填"), Display(Name = "是否有效")]
        public bool IsValid { get; set; } = true;

        ///<summary>
        ///平台类型
        ///</summary>
        [Column("platform_type"), Required(ErrorMessage = "{0}必填"), Display(Name = "平台类型")]
        public string PlatformType
        {
            get;
            set;
        } = GD.Common.EnumDefine.PlatformType.CloudDoctor.ToString();

        /// <summary>
        /// 是否预付款购买
        /// </summary>
        [Column("advance_payment"), Required(ErrorMessage = "{0}必填"), Display(Name = "是否预付款购买")]
        public bool AdvancePayment { get; set; } = false;
    }
}