
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
    /// 个人卡券明细
    ///</summary>
    [Table("t_consumer_ticket_record_detail")]
    public class TicketRecordDetailModel : BaseModel
    {

        ///<summary>
        ///个人卡券明细GUID
        ///</summary>
        [Column("ticket_record_detail_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "个人卡券明细GUID")]
        public string TicketRecordDetailGuid { get; set; }

        ///<summary>
        ///个人卡券GUID
        ///</summary>
        [Column("ticket_record_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "个人卡券GUID")]
        public string TicketRecordGuid { get; set; }

        ///<summary>
        ///项目GUID
        ///</summary>
        [Column("project_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "项目GUID")]
        public string ProjectGuid { get; set; }

        ///<summary>
        ///项目次数
        ///</summary>
        [Column("count"), Required(ErrorMessage = "{0}必填"), Display(Name = "项目次数")]
        public int Count { get; set; }

    }
}



