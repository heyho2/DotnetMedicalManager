using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GD.Common.Base;

namespace GD.Models.Consumer
{
    ///<summary>
    ///用户画像表模型
    ///</summary>
    [Table("t_consumer_character")]
    public class CharacterModel : BaseModel
    {
        ///<summary>
        ///特征GUID
        ///</summary>
        [Column("character_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "特征GUID")]
        public string CharacterGuid
        {
            get;
            set;
        }

        ///<summary>
        ///用户GUID
        ///</summary>
        [Column("user_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "用户GUID")]
        public string UserGuid
        {
            get;
            set;
        }

        ///<summary>
        ///配置guid(如是否抽烟)
        ///</summary>
        [Column("conf_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "配置GUID(如是否抽烟)")]
        public string ConfGuid
        {
            get;
            set;
        }

        ///<summary>
        ///配置对应的值
        ///</summary>
        [Column("conf_value"), Required(ErrorMessage = "{0}必填"), Display(Name = "配置对应的值")]
        public string ConfValue
        {
            get;
            set;
        }
    }
}