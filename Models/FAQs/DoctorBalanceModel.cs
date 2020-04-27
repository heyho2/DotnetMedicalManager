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
    /// 问答账户表
    /// </summary>
    [Table("t_doctor_balance")]
    public class DoctorBalanceModel : BaseModel
    {
        ///<summary>
        ///余额账户主键(即UserGuid)
        ///</summary>
        [Column("balance_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "余额账户主键(即UserGuid)")]
        public string BalanceGuid { get; set; }

        ///<summary>
        ///总收益(分)
        ///</summary>
        [Column("total_earnings"), Required(ErrorMessage = "{0}必填"), Display(Name = "总收益(分)")]
        public int TotalEarnings { get; set; } = 0;

        ///<summary>
        ///收益余额(分)
        ///</summary>
        [Column("acc_balance"), Required(ErrorMessage = "{0}必填"), Display(Name = "收益余额(分)")]
        public int AccBalance { get; set; } = 0;

        ///<summary>
        ///总提现(分)
        ///</summary>
        [Column("total_withdraw"), Required(ErrorMessage = "{0}必填"), Display(Name = "总提现(分)")]
        public int TotalWithdraw { get; set; } = 0;

        ///<summary>
        ///账户状态（frozen冻结，normal启动）
        ///</summary>
        [Column("status"), Required(ErrorMessage = "{0}必填"), Display(Name = "账户状态")]
        public string Status { get; set; } = DoctorBalanceStatusEnum.Normal.ToString();

        /// <summary>
        /// 状态枚举
        /// </summary>
        public enum DoctorBalanceStatusEnum
        {
            /// <summary>
            /// 冻结
            /// </summary>
            [Description("冻结")]
            Frozen = 1,
            /// <summary>
            /// 正常
            /// </summary>
            [Description("正常")]
            Normal
        }

    }
}
