using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Health
{
    /// <summary>
    /// 健康基础信息Dto
    /// </summary>
    public class GetHealthInformationRequestDto
    {
        /// <summary>
        /// 更新数据类型 0:健康信息，1:固定五项数据
        /// </summary>
        [Required(ErrorMessage = "更新数据类型必填")]
        public int UpdateType { get; set; }
        /// <summary>
        /// 健康信息Id
        /// </summary>
        [Required(ErrorMessage = "健康基础信息Id必填")]
        public string InformationGuid { get; set; }
        /// <summary>
        /// 健康信息选项Id
        /// </summary>
        public List<string> OptionGuids { get; set; }
        /// <summary>
        /// 用户填空值
        /// </summary>
        public string ResultValue { get; set; }
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
        }
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
    }
    /// <summary>
    /// 用户数据
    /// </summary>
    public class GetUserHealthInformationResponseDto
    {
        /// <summary>
        /// 用户信息数据
        /// </summary>
        public UserResponseDto UserInfo { get; set; }
        /// <summary>
        /// 健康基础信息集合
        /// </summary>
        public List<GetHealthInformationResponseDto> HealthInformationList { get; set; }
    }
    /// <summary>
    /// 用户固定信息
    /// </summary>
    public class UserResponseDto
    {
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
    }
    /// <summary>
    /// 查询基础信息返回Dto
    /// </summary>
    public class GetHealthInformationResponseDto
    {
        /// <summary>
        /// 健康信息Id
        /// </summary>
        public string InformationGuid { get; set; }
        /// <summary>
        /// 指标类型题目类型：单选、判断、数值、文本、多选（'Enum','Bool','Decimal','String','Array'）
        /// </summary>
        public string InformationType { get; set; }
        /// <summary>
        /// 问题名称
        /// </summary>
        public string SubjectName { get; set; }
        /// <summary>
        /// 问题单位
        /// </summary>
        public string SubjectUnit { get; set; }
        /// <summary>
        /// 问题提示语
        /// </summary>
        public string SubjectPromptText { get; set; }
        /// <summary>
        /// 是否单行文本 true 单行， false 多行
        /// </summary>
        public bool IsSingleLine { get; set; }
        /// <summary>
        /// 问题选项集合
        /// </summary>
        public List<HealthInformationOptionResponse> OptionList { get; set; }
        /// <summary>
        /// 用户填写的选项值
        /// </summary>
        public string OptionValue { get; set; }
        /// <summary>
        /// 用户健康信息选项guid数组
        /// </summary>
        public string OptionGuids { get; set; }
        /// <summary>
        /// 用户填空值
        /// </summary>
        public string ResultValue { get; set; }
    }
    /// <summary>
    /// 问题选项Dto
    /// </summary>
    public class HealthInformationOptionResponse
    {
        /// <summary>
        /// 问题选项Id
        /// </summary>
        public string OptionGuid { get; set; }
        /// <summary>
        /// 问题选项名称
        /// </summary>
        public string OptionLabel { get; set; }
        /// <summary>
        /// 是否为默认值
        /// </summary>
        public bool IsDefault { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
    }

}
