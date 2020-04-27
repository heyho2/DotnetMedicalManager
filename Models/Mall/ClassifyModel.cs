using GD.Common.Base;
using GD.Models.CommonEnum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Mall
{
    ///<summary>
    ///产品分类表（或医生）
    ///</summary>
    [Table("t_mall_classify")]
    public class ClassifyModel : BaseModel
    {
        ///<summary>
        ///分类GUID
        ///</summary>
        [Column("classify_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "分类GUID")]
        public string ClassifyGuid { get; set; }

        ///<summary>
        ///分类名称--枚举
        ///</summary>
        [Column("classify_name")]
        public string ClassifyName { get; set; } = ClassifyEnum.StartProduct.ToString();

        ///<summary>
        ///目标GUID
        ///</summary>
        [Column("target_guid")]
        public string TargetGuid { get; set; }

        ///<summary>
        ///关联的医生Guid(用于明星产品关联的医生)
        ///</summary>
        [Column("relation_doctor_guid")]
        public string RelationDoctorGuid { get; set; }
        
        ///<summary>
        ///是否推荐
        ///</summary>
        [Column("recommend")]
        public bool Recommend { get; set; }

        ///<summary>
        ///平台类型:CloudDoctor(智慧云医)；LifeCosmetology(生活美容)；MedicalCosmetology(医疗美容)
        ///</summary>
        [Column("platform_type")]
        public string PlatformType { get; set; }
    }
}
