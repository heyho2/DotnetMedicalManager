using GD.Common.Base;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Doctor
{
    ///<summary>
    ///医院表模型
    ///</summary>
    [Table("t_doctor_hospital")]
    public class HospitalModel : BaseModel
    {
        ///<summary>
        ///医院GUID
        ///</summary>
        [Column("hospital_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "医院GUID")]
        public string HospitalGuid { get; set; }


        ///<summary>
        ///医院logo GUID
        ///</summary>
        [Column("logo_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "医院Logo GUID")]
        public string LogoGuid { get; set; }


        /// <summary>
        /// 账号
        /// </summary>
        [Column("account"), Required(ErrorMessage = "{0}必填"), Display(Name = "医院账号")]
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Column("password"), Required(ErrorMessage = "{0}必填"), Display(Name = "医院密码")]
        public string Password { get; set; }

        ///<summary>
        ///详情
        ///</summary>
        [Column("hos_detail_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "详情")]
        public string HosDetailGuid { get; set; }

        ///<summary>
        ///名称
        ///</summary>
        [Column("hos_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "名称")]
        public string HosName { get; set; }

        ///<summary>
        /// 医院标签
        ///</summary>
        [Column("hos_tag"), Required(ErrorMessage = "{0}必填"), Display(Name = "医院标签")]
        public string HosTag { get; set; }

        ///<summary>
        ///简介
        ///</summary>
        [Column("hos_abstract"), Required(ErrorMessage = "{0}必填"), Display(Name = "简介")]
        public string HosAbstract { get; set; }

        ///<summary>
        ///等级
        ///</summary>
        [Column("hos_level"), Required(ErrorMessage = "{0}必填"), Display(Name = "等级")]
        public string HosLevel { get; set; } = HosLevelEnum.Unknown.ToString();

        ///<summary>
        ///位置
        ///</summary>
        [Column("location"), Required(ErrorMessage = "{0}必填"), Display(Name = "位置")]
        public string Location { get; set; }
        /// <summary>
        /// 是否可查询
        /// </summary>
        [Column("visibility"), Required(ErrorMessage = "{0}必填"), Display(Name = "是否可查询")]
        public bool Visibility { get; set; } = true;

        /// <summary>
        /// 注册时间
        /// </summary>
        [Column("registered_date"), Required(ErrorMessage = "{0}必填"), Display(Name = "注册时间")]
        public DateTime RegisteredDate { get; set; } = DateTime.Now;

        ///<summary>
        ///平台类型
        ///</summary>
        [Column("platform_type"), Required(ErrorMessage = "{0}必填"), Display(Name = "平台类型")]
        public string PlatformType { get; set; } = GD.Common.EnumDefine.PlatformType.CloudDoctor.ToString();
        /// <summary>
        /// 联系电话
        /// </summary>
        [Column("contact_number"), Required(ErrorMessage = "{0}必填"), Display(Name = "联系电话")]
        public string ContactNumber { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Column("sort"), Required(ErrorMessage = "{0}必填"), Display(Name = "排序")]
        public int Sort { get; set; }

        /// <summary>
        /// 导诊链接
        /// </summary>
        [Column("guidance_url"), Required(ErrorMessage = "{0}必填"), Display(Name = "导诊链接")]
        public string GuidanceUrl { get; set; }

        /// <summary>
        /// 导诊链接
        /// </summary>
        [Column("external_link"), Required(ErrorMessage = "{0}必填"), Display(Name = "外联链接")]
        public string ExternalLink { get; set; }
        /// <summary>
        /// 是否是医院
        /// </summary>
        [Column("is_hospital"), Required(ErrorMessage = "{0}必填"), Display(Name = "是否是医院")]
        public bool IsHospital { get; set; } = true;
        /// <summary>
        /// 纬度
        /// </summary>
        [Column("latitude"), Required(ErrorMessage = "{0}必填"), Display(Name = "纬度")]
        public decimal? Latitude { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        [Column("longitude"), Required(ErrorMessage = "{0}必填"), Display(Name = "经度")]
        public decimal? Longitude { get; set; }

        /// <summary>
        /// 等级枚举
        /// </summary>
        public enum HosLevelEnum
        {
            /// <summary>
            /// 三及甲
            /// </summary>
            [Description("三级甲")]
            ThirdA,
            /// <summary>
            /// 三及乙
            /// </summary>
            [Description("三级乙")]
            ThirdB,
            /// <summary>
            /// 三及丙
            /// </summary>
            [Description("三级丙")]
            ThirdC,
            /// <summary>
            /// 三级
            /// </summary>
            [Description("三级")]
            ThirdZ,
            /// <summary>
            /// 二级甲
            /// </summary>
            [Description("活动")]
            SecondA,
            /// <summary>
            /// 二级乙
            /// </summary>
            [Description("二级乙")]
            SecondB,
            /// <summary>
            /// 二级丙
            /// </summary>
            [Description("二级丙")]
            SecondC,
            /// <summary>
            /// 二级
            /// </summary>
            [Description("二级")]
            SecondZ,
            /// <summary>
            /// 一级甲
            /// </summary>
            [Description("一级甲")]
            FirstA,
            /// <summary>
            /// 一级乙
            /// </summary>
            [Description("一级乙")]
            FirstB,
            /// <summary>
            /// 一级丙
            /// </summary>
            [Description("一级丙")]
            FirstC,
            /// <summary>
            /// 一级
            /// </summary>
            [Description("一级")]
            FirstZ,
            /// <summary>
            /// 未知
            /// </summary>
            [Description("未知")]
            Unknown
        }

    }
}