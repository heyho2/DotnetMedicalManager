using GD.Common.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Consumer
{
    ///<summary>
    ///个人消费记录信息管理表
    ///</summary>
    [Table("t_consumer_consumption")]
    public class ConsumptionModel : BaseModel
    {
        ///<summary>
        ///消费GUID
        ///</summary>
        [Column("consumption_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "消费GUID")]
        public string ConsumptionGuid { get; set; }

        ///<summary>
        ///服务对象guid
        ///</summary>
        [Column("service_member_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "服务对象guid")]
        public string ServiceMemberGuid { get; set; }

        ///<summary>
        ///用户guid
        ///</summary>
        [Column("consumption_no"), Required(ErrorMessage = "{0}必填"), Display(Name = "消费码")]
        public string ConsumptionNo { get; set; }

        ///<summary>
        ///用户guid
        ///</summary>
        [Column("user_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "用户guid")]
        public string UserGuid { get; set; }

        ///<summary>
        ///项来源guid(个人商品项guid/礼物guid/卡券guid)
        ///</summary>
        [Column("from_item_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "项来源guid")]
        public string FromItemGuid { get; set; }

        ///<summary>
        ///服务项目GUID
        ///</summary>
        [Column("project_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "服务项目GUID")]
        public string ProjectGuid { get; set; }

        /// <summary>
        /// 预约时间
        /// </summary>
        [Column("appointment_date")]
        public DateTime AppointmentDate { get; set; }

        ///<summary>
        ///消费时间
        ///</summary>
        [Column("consumption_date")]
        public DateTime? ConsumptionDate { get; set; }

        ///<summary>
        ///消费完成时间
        ///</summary>
        [Column("consumption_end_date")]
        public DateTime? ConsumptionEndDate { get; set; }

        ///<summary>
        ///消费门店GUID
        ///</summary>
        [Column("merchant_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "消费门店GUID")]
        public string MerchantGuid { get; set; }

        ///<summary>
        ///操作者GUID(医生或美疗师)
        ///</summary>
        [Column("operator_guid")]
        public string OperatorGuid { get; set; }

        ///<summary>
        ///消费状态:已预约/已到店/已完成/已取消/已错过
        ///</summary>
        [Column("consumption_status")]
        public string ConsumptionStatus { get; set; }

        /// <summary>
        /// 评价guid
        /// </summary>
        [Column("comment_guid")]
        public string CommentGuid { get; set; }

        /// <summary>
        /// 评价guid
        /// </summary>
        [Column("is_comment")]
        public bool IsComment { get; set; } = false;

        ///<summary>
        ///平台类型:CloudDoctor(智慧云医)；LifeCosmetology(生活美容)；MedicalCosmetology(医疗美容)
        ///</summary>
        [Column("platform_type")]
        public string PlatformType { get; set; }

        /// <summary>
        /// 用户备注
        /// </summary>
        [Column("remark")]
        public string Remark { get; set; }

        /// <summary>
        /// 商家备注
        /// </summary>
        [Column("merchant_remark")]
        public string MerchantRemark { get; set; }
    }
}