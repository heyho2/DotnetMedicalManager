using GD.Common.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Manager
{
    /// <summary>
    /// 运营账号
    /// </summary>
    [Table("t_manager_account")]

    public class AccountModel : BaseModel
    {
        ///<summary>
        ///GUID
        ///</summary>
        [Column("user_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "GUID")]
        public string UserGuid { get; set; }

        ///<summary>
        ///账号
        ///</summary>
        [Column("account")]
        public string Account { get; set; }

        ///<summary>
        ///密码
        ///</summary>
        [Column("password")]
        public string Password { get; set; }

        ///<summary>
        ///部门ID
        ///</summary>
        [Column("organization_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "部门ID")]
        public string OrganizationGuid { get; set; }

        ///<summary>
        ///超级管理员
        ///</summary>
        [Column("is_super"), Required(ErrorMessage = "{0}必填"), Display(Name = "超级管理员")]
        public bool IsSuper { get; set; }

        ///<summary>
        ///EMAIL
        ///</summary>
        [Column("email")]
        public string Email { get; set; }

        ///<summary>
        ///微信号
        ///</summary>
        [Column("wechat_openid")]
        public string WechatOpenid { get; set; }

        ///<summary>
        ///用户昵称
        ///</summary>
        [Column("nick_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "用户昵称")]
        public string NickName { get; set; }

        ///<summary>
        ///真实姓名
        ///</summary>
        [Column("user_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "真实姓名")]
        public string UserName { get; set; }

        ///<summary>
        ///手机号码
        ///</summary>
        [Column("phone"), Required(ErrorMessage = "{0}必填"), Display(Name = "手机号码")]
        public string Phone { get; set; }

        ///<summary>
        ///性别（M/F），默认为M
        ///</summary>
        [Column("gender"), Required(ErrorMessage = "{0}必填"), Display(Name = "性别（M/F），默认为M")]
        public string Gender { get; set; }

        ///<summary>
        ///生日
        ///</summary>
        [Column("birthday")]
        public DateTime? Birthday { get; set; }

        ///<summary>
        ///头像
        ///</summary>
        [Column("portrait_guid")]
        public string PortraitGuid { get; set; }

    }
}
