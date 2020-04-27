using GD.Common.Base;
using GD.Dtos.Enum;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Health
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateHealthManagerRequestDto : BaseDto
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [Required(ErrorMessage = "姓名必填"), MaxLength(50, ErrorMessage = "姓名超过最大长度限制")]
        public string UserName { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        [Required(ErrorMessage = "工号必填"), MaxLength(50, ErrorMessage = "工号超过最大长度限制")]
        public string JobNumber { get; set; }

        /// <summary>
        /// 性别（M/F），默认为M
        /// </summary>
        [Required(ErrorMessage = "性别必填")]
        public string Gender { get; set; } = "M";

        /// <summary>
        /// 手机号码
        /// </summary>
        [Required(ErrorMessage = "手机号码必填"), Phone(ErrorMessage = "请输入正确的电话号码")]
        public string Phone { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        [Required(ErrorMessage = "身份证号必填"), MaxLength(18, ErrorMessage = "身份证号超过最大长度限制")]
        public string IdentityNumber { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 工作城市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 区
        /// </summary>
        public string District { get; set; }

        /// <summary>
        /// 1:一级健康管理师（FirstLevel），2：二级健康管理师(SecondLevel)，3：三级健康管理师(ThirdLevel)
        /// </summary>
        public OccupationGradeEnum OccupationGrade { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [Required(ErrorMessage = "头像未上传"), MaxLength(32, ErrorMessage = "头像参数错误")]
        public string PortraitGuid { get; set; }

        /// <summary>
        /// 职业资格证书
        /// </summary>
        public List<string> QualificationCertificateGuids { get; set; } = new List<string>();

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    public class UpdateHealthManagerRequestDto : CreateHealthManagerRequestDto
    {
        /// <summary>
        /// 健康管理师标识
        /// </summary>
        [Required(ErrorMessage = "参数错误"), MaxLength(32, ErrorMessage = "参数错误")]
        public string ManagerGuid { get; set; }
    }
}
