using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using GD.Common.Base;

namespace GD.Dtos.Mall.Mall
{
    /// <summary>
    /// 提交订单请求信息Dto
    /// </summary>
    public class AddOrderInfoRequestDto
    {
        ///<summary>
        /// 购物车中的商品ItemGuid列表
        /// </summary>
        /// 购物车中的商品ItemGuid列表
        [Required(ErrorMessage = "{0}必填"), Display(Name = "购物车中的商品ItemGuid列表")]
        public string[] ShoppingCarItemIDList { get; set; }
        /// <summary>
        /// 订单地址-即AddressGuid的地址快照
        /// </summary>
        public string AddressStr { get; set; }
        /// <summary>
        /// 收件人
        /// </summary>
        public string Receiver { get; set; }
        /// <summary>
        /// 收件人电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 订单备注
        /// </summary>
        [Display(Name = "订单备注")]
        public string Remark { get; set; }
        /// <summary>
        /// 支付方式（wechat/offlinepay）
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "支付方式")]
        public string PayType { get; set; }

    }
}
