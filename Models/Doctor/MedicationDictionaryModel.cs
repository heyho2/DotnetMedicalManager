
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Doctor
{
    /// <summary>
    /// 药品使用字典描述
    /// </summary>
    [Table("t_doctor_medication_dictionary")]
    public class MedicationDictionaryModel:BaseModel
    {
         /// <summary>
         /// 用药名称guid
         /// </summary>
         [Column("medication_guid"),Key,Required(ErrorMessage = "{0}必填"), Display(Name = "用药名称guid")]
         public string MedicationGuid{ get;set; }
        
         /// <summary>
         /// 用药名称描述
         /// </summary>
         [Column("name"),Required(ErrorMessage = "{0}必填"), Display(Name = "用药名称描述")]
         public string Name{ get;set; }
        
         /// <summary>
         /// 用药类型:Usage:用法，Dosage:用量，Frequency:频率
         /// </summary>
         [Column("medication_type"),Required(ErrorMessage = "{0}必填"), Display(Name = "用药类型:Usage:用法，Dosage:用量，Frequency:频率")]
         public string MedicationType{ get;set; }
    }
}



