using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GD.Common.Base;

namespace GD.Models.Utility
{
    ///<summary>
    ///用户表模型
    ///</summary>
    [Table("t_utility_user")]
    public class UserModel : BaseModel
    {
        ///<summary>
        ///GUID
        ///</summary>
        [Column("user_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "GUID")]
        public string UserGuid
        {
            get;
            set;
        }

        ///<summary>
        ///微信ID
        ///</summary>
        [Column("wechat_openid")]
        public string WechatOpenid
        {
            get;
            set;
        } = "";

        ///<summary>
        ///推荐人GUID
        ///</summary>
        [Column("recommend_guid")]
        public string RecommendGuid
        {
            get;
            set;
        }

        ///<summary>
        ///用户昵称
        ///</summary>
        [Column("nick_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "用户昵称")]
        public string NickName
        {
            get;
            set;
        }

        ///<summary>
        ///真实姓名
        ///</summary>
        [Column("user_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "真实姓名")]
        public string UserName
        {
            get;
            set;
        }

        ///<summary>
        ///密码
        ///</summary>
        [Column("password"), Required(ErrorMessage = "{0}必填"), Display(Name = "密码")]
        public string Password
        {
            get;
            set;
        }

        ///<summary>
        ///手机号码
        ///</summary>
        [Column("phone"), Required(ErrorMessage = "{0}必填"), Display(Name = "手机号码")]
        public string Phone
        {
            get;
            set;
        }

        ///<summary>
        ///性别（M/F），默认为M
        ///</summary>
        [Column("gender"), Required(ErrorMessage = "{0}必填"), Display(Name = "性别（M/F），默认为M")]
        public string Gender
        {
            get;
            set;
        } = "M";

        ///<summary>
        ///生日
        ///</summary>
        [Column("birthday")]
        public DateTime? Birthday
        {
            get;
            set;
        }

        ///<summary>
        ///身份证号
        ///</summary>
        [Column("identity_number")]
        public string IdentityNumber
        {
            get;
            set;
        }

        ///<summary>
        ///头像
        ///</summary>
        [Column("portrait_guid")]
        public string PortraitGuid
        {
            get;
            set;
        }
    }
}