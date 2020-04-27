using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GD.Common.Base;

namespace GD.Models.Utility
{
    ///<summary>
    ///消息表模型
    ///</summary>
    [Table("t_utility_message")]
    public class MessageModel : BaseModel
    {
        ///<summary>
        ///消息GUID
        ///</summary>
        [Column("msg_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "消息GUID")]
        public string MsgGuid
        {
            get;
            set;
        }

        ///<summary>
        ///
        ///</summary>
        [Column("topic_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "")]
        public string TopicGuid
        {
            get;
            set;
        }

        ///<summary>
        ///发送者GUID
        ///</summary>
        [Column("from_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "发送者GUID")]
        public string FromGuid
        {
            get;
            set;
        }

        ///<summary>
        ///接收者GUID
        ///</summary>
        [Column("to_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "接收者GUID")]
        public string ToGuid
        {
            get;
            set;
        }

        ///<summary>
        ///消息内容
        ///</summary>
        [Column("context"), Required(ErrorMessage = "{0}必填"), Display(Name = "消息内容")]
        public string Context
        {
            get;
            set;
        }

        ///<summary>
        ///是否是html消息
        ///</summary>
        [Column("is_html"),  Display(Name = "是否是html消息")]
        public bool IsHtml
        {
            get;
            set;
        } = false;
    }
}