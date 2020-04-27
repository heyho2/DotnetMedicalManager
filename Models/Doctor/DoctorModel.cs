using GD.Common.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Doctor
{
    ///<summary>
    ///医生表模型
    ///</summary>
    [Table("t_doctor")]
    public class DoctorModel : BaseModel
    {
        ///<summary>
        ///医生GUID
        ///</summary>
        [Column("doctor_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "医生GUID")]
        public string DoctorGuid { get; set; }

        ///<summary>
        ///微信ID
        ///</summary>
        [Column("wechat_openid")]
        public string WechatOpenid { get; set; }

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
        ///所属科室GUID
        ///</summary>
        [Column("office_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "所属科室GUID")]
        public string OfficeGuid { get; set; }
        ///<summary>
        ///所属科室GUID
        ///</summary>
        [Column("office_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "所属科室名称")]
        public string OfficeName { get; set; }
        ///<summary>
        ///一寸照Guid
        ///</summary>
        [Column("portrait_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "一寸照Guid")]
        public string PortraitGuid { get; set; }

        ///<summary>
        ///工作城市
        ///</summary>
        [Column("work_city"), Required(ErrorMessage = "{0}必填"), Display(Name = "工作城市")]
        public string WorkCity { get; set; }

        ///<summary>
        ///背景
        ///</summary>
        [Column("background"), Required(ErrorMessage = "{0}必填"), Display(Name = "背景")]
        public string Background { get; set; }

        ///<summary>
        ///职称GUID
        ///</summary>
        [Column("title_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "职称GUID")]
        public string TitleGuid { get; set; }

        ///<summary>
        ///擅长的标签
        ///</summary>
        [Column("adept_tags"), Required(ErrorMessage = "{0}必填"), Display(Name = "擅长的标签")]
        public string AdeptTags { get; set; }

        ///<summary>
        ///工龄
        ///</summary>
        [Column("work_age"), Required(ErrorMessage = "{0}必填"), Display(Name = "工龄")]
        public int WorkAge { get; set; }
        ///<summary>
        ///工号
        ///</summary>
        [Column("job_number"), Display(Name = "工号")]
        public string JobNumber { get; set; }

        /// <summary>
        /// 所获所获荣誉
        /// </summary>
        [Column("honor"), Display(Name = "所获所获荣誉")]
        public string Honor { get; set; }

        /// <summary>
        /// 执业医院
        /// </summary>
        [Column("practising_hospital"), Display(Name = "执业医院")]
        public string PractisingHospital { get; set; }

        /// <summary>
        /// 账号申请状态
        /// 'reject','approved','submit','draft'
        /// </summary>
        [Column("status"), Required(ErrorMessage = "{0}必填"), Display(Name = "账号申请状态")]
        public string Status { get; set; } = StatusEnum.Draft.ToString();


        ///<summary>
        ///平台类型
        ///</summary>
        [Column("platform_type"), Required(ErrorMessage = "{0}必填"), Display(Name = "平台类型")]
        public string PlatformType { get; set; } = GD.Common.EnumDefine.PlatformType.CloudDoctor.ToString();

        ///<summary>
        ///签名附件guid
        ///</summary>
        [Column("signature_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "签名附件guid")]
        public string SignatureGuid { get; set; }
        ///<summary>
        ///是否推荐
        ///</summary>
        [Column("is_recommend"), Required(ErrorMessage = "{0}必填"), Display(Name = "是否推荐")]
        public bool IsRecommend { get; set; }
        ///<summary>
        ///推荐排序
        ///</summary>
        [Column("recommend_sort"), Required(ErrorMessage = "{0}必填"), Display(Name = "推荐排序")]
        public int RecommendSort { get; set; }

        /// <summary>
        /// 医生状态
        /// </summary>
        public enum StatusEnum
        {
            /// <summary>
            /// 驳回
            /// </summary>
            [Description("驳回")]
            Reject,

            /// <summary>
            /// 同意
            /// </summary>
            [Description("同意")]
            Approved,

            /// <summary>
            /// 提交
            /// </summary>
            [Description("提交")]
            Submit,

            /// <summary>
            /// 草稿
            /// </summary>
            [Description("草稿")]
            Draft
        }
    }

}