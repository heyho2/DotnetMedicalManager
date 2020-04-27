using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace GD.Models.Consumer
{
    ///<summary>
    /// 消费者礼物表
    ///</summary>
    [Table("t_consumer_gift")]
    public class GiftModel : BaseModel
    {
        ///<summary>
        ///礼物GUID
        ///</summary>
        [Column("gift_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "礼物GUID")]
        public string GiftGuid { get; set; }

        ///<summary>
        ///用户GUID
        ///</summary>
        [Column("user_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "用户GUID")]
        public string UserGuid { get; set; }

        ///<summary>
        ///来源guid：卡券guid/个人商品项guid
        ///</summary>
        [Column("from_item_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "来源guid：卡券guid/个人商品项guid")]
        public string FromItemGuid { get; set; }

        /// <summary>
        /// 来源类型：卡券赠送、项目赠送
        /// </summary>
        [Column("from_item_type"), Required(ErrorMessage = "{0}必填"), Display(Name = "来源类型：卡券赠送、项目赠送")]
        public string FromItemType { get; set; }

        ///<summary>
        ///项目GUID
        ///</summary>
        [Column("project_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "项目GUID")]
        public string ProjectGuid { get; set; }

        ///<summary>
        ///有效期开始日期(继承自来源个人商品)
        ///</summary>
        [Column("effective_start_date")]
        public DateTime? EffectiveStartDate { get; set; }

        ///<summary>
        ///有效期结束日期(继承自来源个人商品)
        ///</summary>
        [Column("effective_end_date")]
        public DateTime? EffectiveEndDate { get; set; }

        ///<summary>
        ///赠送人GUID
        ///</summary>
        [Column("from_user_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "赠送人GUID")]
        public string FromUserGuid { get; set; }

        ///<summary>
        ///未使用 UNUSED ；已预约 BOOKED；已使用 USED
        ///</summary>
        [Column("gift_status"), Required(ErrorMessage = "{0}必填"), Display(Name = "未使用 UNUSED ；已预约 BOOKED；已使用 USED")]
        public string GiftStatus { get; set; } = GiftStatusEnum.Unused.ToString();

        ///<summary>
        ///礼物收取时间
        ///</summary>
        [Column("receive_date"), Required(ErrorMessage = "{0}必填"), Display(Name = "礼物收取时间")]
        public DateTime? ReceiveDate { get; set; }


        ///<summary>
        ///平台类型:CLOUDDOCTOR(智慧云医)；LIFECOSMETOLOGY(生活美容)；MEDICALCOSMETOLOGY(医疗美容)
        ///</summary>
        [Column("platform_type")]
        public string PlatformType { get; set; }

        /// <summary>
        /// 并发版本号
        /// </summary>
        [Column("Version")]
        public int Version { get; set; } = 0;
        /// <summary>
        /// 排序
        /// </summary>
        [Column("sort")]
        public int Sort { get; set; } = 1;
    }

    /// <summary>
    /// 礼物使用状态
    /// </summary>
    public enum GiftStatusEnum
    {
        /// <summary>
        /// 未使用
        /// </summary>
        [Description("未使用")]
        Unused = 1,

        /// <summary>
        /// 已预约
        /// </summary>
        [Description("已预约")]
        Booked,

        /// <summary>
        /// 已使用
        /// </summary>
        [Description("已使用")]
        Used
    }

    /// <summary>
    /// 礼物来源类型
    /// </summary>
    public enum GiftFromTypeEnum
    {
        /// <summary>
        /// 来自卡券转赠
        /// </summary>
        CardTicket = 1,

        /// <summary>
        /// 来自个人商品项转赠
        /// </summary>
        GoodsItem
    }
}



