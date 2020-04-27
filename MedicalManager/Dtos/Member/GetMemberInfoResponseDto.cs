using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Member
{
    /// <summary>
    /// 获取会员信息
    /// </summary>
    public class GetMemberInfoResponseDto : BaseDto
    {
        ///<summary>
        ///GUID
        ///</summary>
        public string UserGuid { get; set; }

        ///<summary>
        ///用户昵称
        ///</summary>
        public string NickName { get; set; }

        ///<summary>
        ///真实姓名
        ///</summary>
        public string UserName { get; set; }


        ///<summary>
        ///手机号码
        ///</summary>
        public string Phone { get; set; }

        ///<summary>
        ///性别（M/F），默认为M
        ///</summary>
        public string Gender { get; set; }

        ///<summary>
        ///生日
        ///</summary>
        public DateTime Birthday { get; set; }

        /// <summary>
        /// 订单数量
        /// </summary>
        public int OrderQty { get; set; }
        /// <summary>
        /// 订单总金额
        /// </summary>
        public decimal OrderTotalAmount { get; set; }
        /// <summary>
        /// 平均订单金额
        /// </summary>
        public decimal OrderAverage { get; set; }

        /// <summary>
        /// 创建时间，默认为系统当前时间   
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// 使能标志，默认为 true
        /// </summary>
        public bool Enable { get; set; }
        /// <summary>
        /// 最后一次消费时间
        /// </summary>
        public DateTime? LastBuyDate { get; set; }

        /// <summary>
        /// 推荐人
        /// </summary>
        public string Recommended { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

    }
}
