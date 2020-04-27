using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Health.HealthManager
{
    public class CreateConsumerRequestDto
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        [Required(ErrorMessage = "手机号必填"), Phone(ErrorMessage = "请输入正确的手机号码")]
        public string Phone { get; set; }
        /// <summary>
        /// 是否绑定管理师 true：绑定
        /// </summary>
        public bool IsBind { get; set; } = true;
        ///<summary>
        ///真实姓名
        ///</summary>
        public string UserName
        {
            get;
            set;
        }
        ///<summary>
        ///性别（M/F），默认为M
        ///</summary>
        public string Gender
        {
            get;
            set;
        } = "M";
        ///<summary>
        ///生日
        ///</summary>
        public DateTime? Birthday
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
        /// <summary>
        /// 信息集合
        /// </summary>
        public List<Information> Informations { get; set; } = new List<Information>();
    }

    /// <summary>
    /// 
    /// </summary>
    public class Information
    {
        /// <summary>
        /// 信息guid
        /// </summary>
        public string InformationGuid { get; set; }
        /// <summary>
        /// 信息类型
        /// </summary>
        public HealthInformationTypeEnum? InformationType { get; set; }
        /// <summary>
        /// 答案选项 存JSON数组，如：["a","b"]
        /// </summary>
        public string OptionGuids { get; set; }
        /// <summary>
        /// 填空值
        /// </summary>
        public string ResultValue { get; set; }
    }
    public enum HealthInformationTypeEnum
    {
        /// <summary>
        /// 单选题
        /// </summary>
        [Description("单选题")]
        Enum = 1,

        /// <summary>
        /// 判断题
        /// </summary>
        [Description("判断题")]
        Bool,

        /// <summary>
        /// 数值填空
        /// </summary>
        [Description("数值填空")]
        Decimal,

        /// <summary>
        /// 问答
        /// </summary>
        [Description("问答")]
        String,

        /// <summary>
        /// 多选题
        /// </summary>
        [Description("多选题")]
        Array
    }
}
