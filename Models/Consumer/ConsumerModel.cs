using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GD.Common.Base;

namespace GD.Models.Consumer
{
    ///<summary>
    ///消费者表模型
    ///</summary>
    [Table("t_consumer")]
    public class ConsumerModel : BaseModel
    {
        ///<summary>
        ///消费者GUID
        ///</summary>
        [Column("consumer_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "消费者GUID")]
        public string ConsumerGuid
        {
            get;
            set;
        }
        /// <summary>
        /// 支付密码
        /// </summary>
        [Column("payment_code"), Required(ErrorMessage = "{0}必填"), Display(Name = "支付密码")]
        public string PaymentCode { get; set; }

        /// <summary>
        /// 免密支付
        /// </summary>
        [Column("pay_without_pwd"), Required(ErrorMessage = "{0}必填"), Display(Name = "免密支付")]
        public bool PayWithoutPwd { get; set; } = false;

        /// <summary>
        /// 持卡人
        /// </summary>
        [Column("cardholder"), Required(ErrorMessage = "{0}必填"), Display(Name = "持卡人")]
        public string Cardholder { get; set; }

        /// <summary>
        /// 银行卡卡号
        /// </summary>
        [Column("credit_card"), Required(ErrorMessage = "{0}必填"), Display(Name = "银行卡卡号")]
        public string CreditCard { get; set; }

        ///<summary>
        ///推荐人
        ///</summary>
        [Column("recommend_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "推荐人")]
        public string RecommendGuid
        {
            get;
            set;
        }
        /// <summary>
        /// 禁止预约挂号截止时间
        /// </summary>
        [Column("no_appointment_date"), Display(Name = "禁止预约挂号截止时间")]
        public DateTime? NoAppointmentDate { get; set; }
        /// <summary>
        /// 分销关系是否可用
        /// </summary>
        [Column("distribution_enable"), Required(ErrorMessage = "{0}必填"), Display(Name = "分销关系是否可用")]
        public bool DistributionEnable { get; set; } = true;

        /// <summary>
        /// 健康档案是否已填写
        /// </summary>
        [Column("health_fill_status"), Required(ErrorMessage = "{0}必填"), Display(Name = "健康档案是否已填写")]
        public bool HealthFillStatus { get; set; } = false;

        /// <summary>
        /// 绑定的健康管理师guid
        /// </summary>
        [Column("health_manager_guid"), Display(Name = "绑定的健康管理师guid")]
        public string HealthManagerGuid { get; set; }

        /// <summary>
        /// 绑定的健康管理师日期
        /// </summary>
        [Column("manager_bind_date"), Display(Name = "绑定的健康管理师日期")]
        public DateTime? ManagerBindDate { get; set; }
    }
}