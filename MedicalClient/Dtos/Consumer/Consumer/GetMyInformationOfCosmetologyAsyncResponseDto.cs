using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Consumer.Consumer
{
    /// <summary>
    /// 获取双美用户我的信息响应Dto
    /// </summary>
    public class GetMyInformationOfCosmetologyAsyncResponseDto:BaseDto
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserGuid { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime Birthday { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 免密支付
        /// </summary>
        public bool? PayWithoutPwd { get; set; }

        /// <summary>
        /// 持卡人姓名
        /// </summary>
        public string Cardholder { get; set; }

        /// <summary>
        /// 银行卡号
        /// </summary>
        public string CreditCard { get; set; }
        /// <summary>
        /// 头像
        /// </summary>

        public string Portrait { get; set; }
    }
}
