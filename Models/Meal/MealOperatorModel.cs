using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Meal
{
    ///<summary>
    ///食堂操作员
    ///</summary>
    [Table("t_meal_operator")]
    public class MealOperatorModel : BaseModel
    {

        ///<summary>
        ///GUID
        ///</summary>
        [Column("operator_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "GUID")]
        public string OperatorGuid { get; set; }

        ///<summary>
        ///医院GUID
        ///</summary>
        [Column("hospital_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "医院GUID")]
        public string HospitalGuid { get; set; }

        ///<summary>
        ///用户名（手机号码）
        ///</summary>
        [Column("user_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "用户名（手机号码）")]
        public string UserName { get; set; }

        ///<summary>
        ///密码
        ///</summary>
        [Column("password"), Required(ErrorMessage = "{0}必填"), Display(Name = "密码")]
        public string Password { get; set; }
    }
}



