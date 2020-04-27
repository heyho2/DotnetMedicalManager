using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GD.Common.Base;

namespace GD.Models.Utility
{
    ///<summary>
    ///富文本表模型
    ///</summary>
    [Table("t_utility_richtext")]
    public class RichtextModel : BaseModel
    {
        ///<summary>
        ///富文本GUID
        ///</summary>
        [Column("text_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "富文本GUID")]
        public string TextGuid
        {
            get;
            set;
        }

        ///<summary>
        ///属于谁的富文本
        ///</summary>
        [Column("owner_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "属于谁的富文本")]
        public string OwnerGuid
        {
            get;
            set;
        }

        ///<summary>
        ///文章内容
        ///</summary>
        [Column("content"), Required(ErrorMessage = "{0}必填"), Display(Name = "文章内容")]
        public string Content
        {
            get;
            set;
        }
    }
}