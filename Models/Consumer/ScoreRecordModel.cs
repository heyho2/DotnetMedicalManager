using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Consumer
{
    ///<summary>
    ///消费者积分来源记录
    ///</summary>
    [Table("t_consumer_score_record")]
    public class ScoreRecordModel : BaseModel
    {
        ///<summary>
        ///主键GUID 
        ///</summary>
        [Column("score_record_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "主键GUID")]
        public string ScoreRecordGuid { get; set; }

        ///<summary>
        ///订单GUID
        ///</summary>
        [Column("order_guid")]
        public string OrderGuid { get; set; }

        ///<summary>
        ///用户GUID
        ///</summary>
        [Column("user_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "用户GUID")]
        public string UserGuid { get; set; }

        ///<summary>
        ///目标用户GUID
        ///</summary>
        [Column("target_user_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "目标用户GUID")]
        public string TargetUserGuid { get; set; }

        ///<summary>
        ///消费金额
        ///</summary>
        [Column("ammount"), Required(ErrorMessage = "{0}必填"), Display(Name = "消费金额")]
        public decimal Ammount { get; set; }

        ///<summary>
        ///积分比例(例如消费积分比例/提成积分比例)
        ///</summary>
        [Column("rate"), Required(ErrorMessage = "{0}必填"), Display(Name = "积分比例(例如消费积分比例/提成积分比例)")]
        public decimal Rate { get; set; }

        ///<summary>
        ///可得积分
        ///</summary>
        [Column("score"), Required(ErrorMessage = "{0}必填"), Display(Name = "可得积分")]
        public int Score { get; set; }

        ///<summary>
        ///积分来源类型：Consumption 消费；Distribution 分销提成
        ///</summary>
        [Column("score_source_type"), Required(ErrorMessage = "{0}必填"), Display(Name = "积分来源类型：Consumption 消费；Distribution 分销提成")]
        public string ScoreSourceType { get; set; }
    }
}



