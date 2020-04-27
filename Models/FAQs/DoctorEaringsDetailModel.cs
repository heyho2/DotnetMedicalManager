using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GD.Models.FAQs
{
    /// <summary>
    /// 收益明细表
    /// </summary>
    [Table("t_doctor_earings_detail")]
    public class DoctorEaringsDetailModel:BaseModel
    {
        /// <summary>
        /// 记录主键
        /// </summary>
        [Column("detail_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "记录主键")]
        public string DetailGuid { get; set; }

        /// <summary>
        /// 医生Guid
        /// </summary>
        [Column("doctor_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "医生Guid")]
        public string DoctorGuid { get; set; }

        /// <summary>
        /// 回答Guid
        /// </summary>
        [Column("answer_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "回答Guid")]
        public string AnswerGuid { get; set; }

        /// <summary>
        /// 所获来源枚举（answer:回答问题）
        /// </summary>
        [Column("fee_from"), Required(ErrorMessage = "{0}必填"), Display(Name = " 所获来源")]
        public string FeeFrom { get; set; } = FeeFromTypeEnum.Answer.ToString();

        /// <summary>
        ///所获金额(分)
        /// </summary>
        [Column("received_fee"), Required(ErrorMessage = "{0}必填"), Display(Name = "所获金额(分)")]
        public int ReceivedFee { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column("remark"), Required(ErrorMessage = "{0}必填"), Display(Name = "备注")]
        public string Remark { get; set; }


        /// <summary>
        /// 来源枚举
        /// </summary>
        public enum FeeFromTypeEnum
        {
            /// <summary>
            /// 回答问题
            /// </summary>
            [Description("回答问题")]
            Answer = 1,
       
        }
    }
}
