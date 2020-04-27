using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GD.Common.Base;

namespace GD.Models.Doctor
{
    ///<summary>
    ///科室表模型
    ///</summary>
    [Table("t_doctor_office")]
    public class OfficeModel : BaseModel
    {
        ///<summary>
        ///科室GUID
        ///</summary>
        [Column("office_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "科室GUID")]
        public string OfficeGuid { get; set; }

        ///<summary>
        ///科室名称
        ///</summary>
        [Column("office_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "科室名称")]
        public string OfficeName { get; set; }

        ///<summary>
        ///所属医院GUID
        ///</summary>
        [Column("hospital_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "所属医院GUID")]
        public string HospitalGuid { get; set; }
        ///<summary>
        ///所属医院名称
        ///</summary>
        [Column("hospital_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "所属医院名称")]
        public string HospitalName { get; set; }
        ///<summary>
        ///上级科室
        ///</summary>
        [Column("parent_office_guid")]
        public string ParentOfficeGuid { get; set; }

        ///<summary>
        ///科室图片
        ///</summary>
        [Column("picture_guid")]
        public string PictureGuid { get; set; }


        ///<summary>
        ///是否推荐
        ///</summary>
        [Column("recommend"), Required(ErrorMessage = "{0}必填"), Display(Name = "是否推荐")]
        public bool Recommend { get; set; }

        ///<summary>
        ///排序
        ///</summary>
        [Column("sort"), Required(ErrorMessage = "{0}必填"), Display(Name = "排序")]
        public int Sort { get; set; }
    }
}