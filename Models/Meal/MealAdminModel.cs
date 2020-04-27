using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Meal
{
    ///<summary>
    ///点餐后台管理员
    ///</summary>
    [Table("t_meal_admin")]
    public class MealAdminModel : BaseModel
    {

        ///<summary>
        ///GUID
        ///</summary>
        [Column("admin_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "GUID")]
        public string AdminGuid { get; set; }

        ///<summary>
        ///账号
        ///</summary>
        [Column("admin_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "账号")]
        public string AdminName { get; set; }

        ///<summary>
        ///密码
        ///</summary>
        [Column("password"), Required(ErrorMessage = "{0}必填"), Display(Name = "密码")]
        public string Password { get; set; }

        ///<summary>
        ///医院名称
        ///</summary>
        [Column("hos_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "医院名称")]
        public string HosName { get; set; }

        ///<summary>
        ///医院GUID
        ///</summary>
        [Column("hospital_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "医院GUID")]
        public string HospitalGuid { get; set; }
    }
}



