using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GD.Common.Base;

namespace GD.Models.Consumer
{
    ///<summary>
    ///收藏表模型
    ///</summary>
    [Table("t_consumer_collection")]
    public class CollectionModel : BaseModel
    {
        ///<summary>
        ///收藏GUID
        ///</summary>
        [Column("collection_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "收藏GUID")]
        public string CollectionGuid
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
        ///目标GUID
        ///</summary>
        [Column("target_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "目标GUID")]
        public string TargetGuid
        {
            get;
            set;
        }

        ///<summary>
        ///收藏目标类型：产品，医院，科室，医生，文章
        ///</summary>
        [Column("target_type"), Required(ErrorMessage = "{0}必填"), Display(Name = "收藏目标类型：产品，医院，科室，医生，文章")]
        public string TargetType
        {
            get;
            set;
        }

        ///<summary>
        ///平台类型
        ///</summary>
        [Column("platform_type"), Required(ErrorMessage = "{0}必填"), Display(Name = "平台类型")]
        public string PlatformType
        {
            get;
            set;
        } = GD.Common.EnumDefine.PlatformType.CloudDoctor.ToString();
    }
}