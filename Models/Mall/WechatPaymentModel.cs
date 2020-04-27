using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Mall
{
    /// <summary>
    /// 微信支付回调数据记录表
    /// </summary>
    [Table("t_mall_wechat_payment")]
    public class WechatPaymentModel : BaseModel
    {
        /// <summary>
        /// 微信交易单号
        /// </summary>
        [Column("trade_no"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "微信交易单号")]
        public string TradeNo { get; set; }

        /// <summary>
        /// 是否已处理
        /// </summary>
        [Column("handled")]
        public bool Handled { get; set; } = false;
    }
}



