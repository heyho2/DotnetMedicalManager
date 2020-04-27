
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
    /// 会员健康指标预警范围
    /// </summary>
    [Table("t_consumer_indicator_warning_limit")]
    public class IndicatorWarningLimitModel:BaseModel
    {
         /// <summary>
         /// 主键
         /// </summary>
         [Column("limit_guid"),Key,Required(ErrorMessage = "{0}必填"), Display(Name = "")]
         public string LimitGuid{ get;set; }
        
         /// <summary>
         /// 用户guid
         /// </summary>
         [Column("user_guid"),Required(ErrorMessage = "{0}必填"), Display(Name = "用户guid")]
         public string UserGuid{ get;set; }
        
         /// <summary>
         /// 指标项guid
         /// </summary>
         [Column("indicator_option_guid"),Required(ErrorMessage = "{0}必填"), Display(Name = "指标项guid")]
         public string IndicatorOptionGuid{ get;set; }

        /// <summary>
        /// 开启预警值
        /// </summary>
        [Column("turn_on_warning"),Required(ErrorMessage = "{0}必填"), Display(Name = "开启预警值")]
         public bool TurnOnWarning{ get;set; }
        
         /// <summary>
         /// 预警最低值
         /// </summary>
         [Column("min_value")]
         public decimal? MinValue{ get;set; }
        
         /// <summary>
         /// 预警最高值
         /// </summary>
         [Column("max_value")]
         public decimal? MaxValue{ get;set; }
    }
}



