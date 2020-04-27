using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GD.Common.Base;

namespace GD.Models.Mall
{
    ///<summary>
    ///订单表详情模型
    ///</summary>
    [Table("t_mall_order_detail")]
    public class OrderDetailModel : BaseModel
    {
        ///<summary>
        ///订单详情GUID（对应订单GUID,此仅主键）
        ///</summary>
        [Column("detail_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "订单详情GUID（对应订单GUID,此仅主键）")]
        public string DetailGuid
        {
            get;
            set;
        }

        ///<summary>
        ///订单GUID
        ///</summary>
        [Column("order_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "订单GUID")]
        public string OrderGuid
        {
            get;
            set;
        }

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
        ///价格
        ///</summary>
        [Column("product_price"), Required(ErrorMessage = "{0}必填"), Display(Name = "价格")]
        public decimal ProductPrice
        {
            get;
            set;
        }

        ///<summary>
        ///数量
        ///</summary>
        [Column("product_count"), Required(ErrorMessage = "{0}必填"), Display(Name = "数量")]
        public int ProductCount
        {
            get;
            set;
        }
        ///<summary>
        ///优惠Guid
        ///</summary>
        [Column("campaign_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "优惠Guid")]
        public string CampaignGuid
        {
            get;
            set;
        }

        ///<summary>
        ///商品评价guid
        ///</summary>
        [Column("comment_guid"), Display(Name = "商品评价guid")]
        public string CommentGuid
        {
            get;
            set;
        }

    }
}