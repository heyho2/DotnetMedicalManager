using GD.Common.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Utility.User
{
    /// <summary>
    /// 修改用户基础信息
    /// </summary>
    public class UpdateUserInfoRequestDto : BaseDto
    {
        ///<summary>
        ///用户昵称
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "用户昵称")]
        public string NickName
        {
            get;
            set;
        }

        ///<summary>
        ///真实姓名
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "真是姓名")]
        public string UserName
        {
            get;
            set;
        }

        ///<summary>
        ///性别（M/F），默认为M
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "性别")]
        public string Gender
        {
            get;
            set;
        } = "M";

        ///<summary>
        ///生日
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "生日")]
        public DateTime Birthday
        {
            get;
            set;
        }

        ///<summary>
        ///身份证号
        ///</summary>
        public string IdentityNumber
        {
            get;
            set;
        }


    }
}
