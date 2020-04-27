using GD.Common.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Consumer
{
    /// <summary>
    ///
    /// </summary>
    [Table("t_consumer_order_product_comment")]
    public class OrderProductCommentModel : BaseModel
    {
        /// <summary>
        /// 订单商品评价GUID
        /// </summary>
        [Column("product_comment_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "订单商品评价GUID")]
        public string ProductCommentGuid { get; set; }

        /// <summary>
        /// 订单所属用户guid
        /// </summary>
        [Column("user_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "订单所属用户guid")]
        public string UserGuid { get; set; }

        /// <summary>
        /// 订单GUID
        /// </summary>
        [Column("order_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "订单GUID")]
        public string OrderGuid { get; set; }

        /// <summary>
        /// 订单明细GUID
        /// </summary>
        [Column("order_detail_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "订单明细GUID")]
        public string OrderDetailGuid { get; set; }

        /// <summary>
        /// 商品GUID
        /// </summary>
        [Column("product_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "商品GUID")]
        public string ProductGuid { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        [Column("product_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "商品名称")]
        public string ProductName { get; set; }

        /// <summary>
        /// 评价GUID
        /// </summary>
        [Column("comment_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "评价GUID")]
        public string CommentGuid { get; set; }

        /// <summary>
        /// 评价状态
        /// </summary>
        [Column("comment_status"), Required(ErrorMessage = "{0}必填"), Display(Name = "评价状态")]
        public string CommentStatus { get; set; } = CommentStatusEnum.NotEvaluate.ToString();

        /// <summary>
        /// 评价状态
        /// </summary>
        public enum CommentStatusEnum
        {
            /// <summary>
            /// 未评价
            /// </summary>
            [Description("未评价")]
            NotEvaluate = 1,

            /// <summary>
            /// 已评价
            /// </summary>
            [Description("已评价")]
            Evaluate,

            /// <summary>
            /// 已过期
            /// </summary>
            [Description("已过期")]
            Expired
        }
    }
}