using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Utility
{
    ///<summary>
    ///别名表
    ///</summary>
    [Table("t_utility_alias")]
    public class AliasModel : BaseModel
    {
        ///<summary>
        ///别名主键
        ///</summary>
        [Column("alias_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "别名主键")]
        public string AliasGuid { get; set; }

        ///<summary>
        ///用户GUID
        ///</summary>
        [Column("user_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "用户GUID")]
        public string UserGuid { get; set; }

        ///<summary>
        ///别名针对的目标GUID
        ///</summary>
        [Column("target_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "别名针对的目标GUID")]
        public string TargetGuid { get; set; }

        ///<summary>
        ///别名
        ///</summary>
        [Column("alias_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "别名")]
        public string AliasName { get; set; }
    }
}



