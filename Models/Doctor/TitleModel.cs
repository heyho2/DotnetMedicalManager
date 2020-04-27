using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GD.Common.Base;

namespace GD.Models.Doctor
{
    ///<summary>
    ///职称表模型
    ///</summary>
    [Table("t_doctor_title")]
    public class TitleModel : BaseModel
    {
        ///<summary>
        ///职称GUID
        ///</summary>
        [Column("title_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "职称GUID")]
        public string TitleGuid
        {
            get;
            set;
        }

        ///<summary>
        ///职称名称
        ///</summary>
        [Column("title_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "职称名称")]
        public string TitleName
        {
            get;
            set;
        }

        ///<summary>
        ///所属医院GUID
        ///</summary>
        [Column("hospital_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "所属医院GUID")]
        public string HospitalGuid
        {
            get;
            set;
        }

        ///<summary>
        ///职称图片
        ///</summary>
        [Column("picture_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "职称图片")]
        public string PictureGuid
        {
            get;
            set;
        }

        ///<summary>
        ///是否推荐
        ///</summary>
        [Column("recommend"), Required(ErrorMessage = "{0}必填"), Display(Name = "是否推荐")]
        public bool Recommend
        {
            get;
            set;
        }

        ///<summary>
        ///排序
        ///</summary>
        [Column("sort"), Required(ErrorMessage = "{0}必填"), Display(Name = "排序")]
        public int Sort
        {
            get;
            set;
        }
    }
}