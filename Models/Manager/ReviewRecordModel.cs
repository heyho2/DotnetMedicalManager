using GD.Common.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Manager
{
    /// <summary>
    /// 审核记录
    /// </summary>
    [Table("t_manager_review_record")]
    public class ReviewRecordModel : BaseModel
    {
        ///<summary>
        ///GUID
        ///</summary>
        [Column("review_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "GUID")]
        public string ReviewGuid { get; set; }

        ///<summary>
        ///推荐归属GUID
        ///</summary>
        [Column("owner_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "推荐归属GUID")]
        public string OwnerGuid { get; set; }

        ///<summary>
        ///类型
        ///</summary>
        [Column("type"), Required(ErrorMessage = "{0}必填"), Display(Name = "类型")]
        public string Type { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [Column("status")]
        public string Status { get; set; }

        ///<summary>
        ///拒绝原因
        ///</summary>
        [Column("reject_reason")]
        public string RejectReason { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public enum TypeEnum
        {
            /// <summary>
            /// 商家
            /// </summary>
            [Description("商家")]
            Merchant,
            /// <summary>
            /// 医生
            /// </summary>
            [Description("医生")]
            Doctors
        }
    }
}
