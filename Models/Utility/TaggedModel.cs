using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GD.Common.Base;

namespace GD.Models.Utility
{
    ///<summary>
    ///标签表模型
    ///</summary>
    [Table("t_utility_tagged")]
    public class TaggedModel : BaseModel
    {
        ///<summary>
        ///标签GUID
        ///</summary>
        [Column("tag_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "标签GUID")]
        public string TagGuid
        {
            get;
            set;
        }

        ///<summary>
        ///标签所属者GUID
        ///</summary>
        [Column("owner_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "标签所属者GUID")]
        public string OwnerGuid
        {
            get;
            set;
        }

        ///<summary>
        ///标签内容，JSON格式[{"LABLE":"","TIMES":1}]
        ///</summary>
        [Column("tags"), Required(ErrorMessage = "{0}必填"), Display(Name = "标签内容，JSON格式[{\"LABLE\":\"\",\"TIMES\":1}]")]
        public object Tags
        {
            get;
            set;
        }
        ///<summary>
        ///排序
        ///</summary>
        [Column("sort"), Display(Name = "排序")]
        public string Sort { get; set; }

        ///<summary>
        ///平台类型--默认智慧云医
        ///</summary>
        [Column("platform_type"), Display(Name = "平台类型")]
        public string PlatformType { get; set; } = GD.Common.EnumDefine.PlatformType.CloudDoctor.ToString();
    }
}