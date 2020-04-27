
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Mall
{
    /// <summary>
    /// t_consumer_questionnaire_hide
    /// </summary>
    [Table("t_consumer_questionnaire_hide")]
    public class QuestionnaireHideModel:BaseModel
    {
        
         /// <summary>
         /// guid
         /// </summary>
         [Column("hide_guid"),Key,Required(ErrorMessage = "{0}必填"), Display(Name = "guid")]
         public string HideGuid{ get;set; }
        
         /// <summary>
         /// 用户guid
         /// </summary>
         [Column("user_guid"),Required(ErrorMessage = "{0}必填"), Display(Name = "用户guid")]
         public string UserGuid{ get;set; }
        
         /// <summary>
         /// 问卷guid
         /// </summary>
         [Column("questionnaire_guid"),Required(ErrorMessage = "{0}必填"), Display(Name = "问卷guid")]
         public string QuestionnaireGuid{ get;set; }
        
        
        
    }
}



