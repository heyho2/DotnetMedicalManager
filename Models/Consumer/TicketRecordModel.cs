
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Consumer
{
    ///<summary>
    ///消费者卡券表Model
    ///</summary>
    [Table("t_consumer_ticket_record")]
    public class TicketRecordModel : BaseModel
    {

        ///<summary>
        ///卡券记录主键GUID
        ///</summary>
        [Column("ticket_record_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "卡券记录主键GUID")]
        public string TicketRecordGuid { get; set; }

        ///<summary>
        ///卡券GUID
        ///</summary>
        [Column("card_ticket_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "卡券GUID")]
        public string CardTicketGuid { get; set; }

        /// <summary>
        /// 商品项guid
        /// </summary>
        [Column("project_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "商品项目guid")]
        public string ProjectGuid { get; set; }

        ///<summary>
        ///所属用户GUID
        ///</summary>
        [Column("user_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "所属用户GUID")]
        public string UserGuid { get; set; }

        ///<summary>
        ///卡券获得来源guid:订单guid
        ///</summary>
        [Column("source_guid")]
        public string SourceGuid { get; set; }

        ///<summary>
        ///有效起始日期
        ///</summary>
        [Column("effective_start_date")]
        public DateTime EffectiveStartDate { get; set; }

        ///<summary>
        ///有效终止日期
        ///</summary>
        [Column("effective_end_date")]
        public DateTime EffectiveEndDate { get; set; }

        ///<summary>
        ///平台类型:CLOUDDOCTOR(智慧云医)；LIFECOSMETOLOGY(生活美容)；MEDICALCOSMETOLOGY(医疗美容)
        ///</summary>
        [Column("platform_type")]
        public string PlatformType { get; set; }

        ///<summary>
        ///是否已转赠
        ///</summary>
        [Column("present_status")]
        public bool PresentStatus { get; set; } = false;

    }
}



