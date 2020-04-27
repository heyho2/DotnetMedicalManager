
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
    /// <summary>
    /// 会员健康档案日常指标预警记录
    /// </summary>
    [Table("t_consumer_indicator_warning")]
    public class IndicatorWarningModel:BaseModel
    {
        
         /// <summary>
         /// 预警记录主键
         /// </summary>
         [Column("warning_guid"),Key,Required(ErrorMessage = "{0}必填"), Display(Name = "预警记录主键")]
         public string WarningGuid{ get;set; }
        
         /// <summary>
         /// 日常指标项guid
         /// </summary>
         [Column("indicator_option_guid"),Required(ErrorMessage = "{0}必填"), Display(Name = "日常指标项guid")]
         public string IndicatorOptionGuid{ get;set; }
        
         /// <summary>
         /// 会员guid
         /// </summary>
         [Column("consumer_guid"),Required(ErrorMessage = "{0}必填"), Display(Name = "会员guid")]
         public string ConsumerGuid{ get;set; }
        
         /// <summary>
         /// 健康管理师guid
         /// </summary>
         [Column("health_manager_guid"),Required(ErrorMessage = "{0}必填"), Display(Name = "健康管理师guid")]
         public string HealthManagerGuid{ get;set; }
        
         /// <summary>
         /// 会员姓名
         /// </summary>
         [Column("name"),Required(ErrorMessage = "{0}必填"), Display(Name = "会员姓名")]
         public string Name{ get;set; }
        
         /// <summary>
         /// 会员年龄
         /// </summary>
         [Column("age"),Required(ErrorMessage = "{0}必填"), Display(Name = "会员年龄")]
         public int Age{ get;set; }
        
         /// <summary>
         /// 会员手机号
         /// </summary>
         [Column("phone"),Required(ErrorMessage = "{0}必填"), Display(Name = "会员手机号")]
         public string Phone{ get;set; }
        
         /// <summary>
         /// 预警状态：1:待处理（Pending），2：已失效(Expired)，3：已关闭(Closed)，默认为待处理
         /// </summary>
         [Column("status"),Required(ErrorMessage = "{0}必填"), Display(Name = "预警状态：1:待处理（Pending），2：已失效(Expired)，3：已关闭(Closed)，默认为待处理")]
         public string Status{ get;set; }
        
         /// <summary>
         /// 预警描述
         /// </summary>
         [Column("description"),Required(ErrorMessage = "{0}必填"), Display(Name = "预警描述")]
         public string Description{ get;set; }
    }
}



