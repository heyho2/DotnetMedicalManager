using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GD.Common.Base;

namespace GD.Models.Utility
{
    ///<summary>
    ///附件表模型
    ///</summary>
    [Table("t_utility_accessory")]
    public class AccessoryModel : BaseModel
    {
        ///<summary>
        ///附件GUID
        ///</summary>
        [Column("accessory_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "附件GUID")]
        public string AccessoryGuid
        {
            get;
            set;
        }

        ///<summary>
        ///来源GUID
        ///</summary>
        [Column("owner_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "来源GUID")]
        public string OwnerGuid
        {
            get;
            set;
        }

        ///<summary>
        ///基础路径
        ///</summary>
        [Column("base_path"), Required(ErrorMessage = "{0}必填"), Display(Name = "基础路径")]
        public string BasePath
        {
            get;
            set;
        }

        ///<summary>
        ///相对路径
        ///</summary>
        [Column("relative_path"), Required(ErrorMessage = "{0}必填"), Display(Name = "相对路径")]
        public string RelativePath
        {
            get;
            set;
        }

        ///<summary>
        ///文件后缀名
        ///</summary>
        [Column("extension"), Required(ErrorMessage = "{0}必填"), Display(Name = "文件后缀名")]
        public string Extension
        {
            get;
            set;
        }
    }
}