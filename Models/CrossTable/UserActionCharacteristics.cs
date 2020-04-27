using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GD.Models.CrossTable
{
    /// <summary>
    /// 用户行为特征实体类
    /// </summary>
    public class UserActionCharacteristics
    {
        /// <summary>
        /// 用户行为GUID
        /// </summary>
        [Column("user_action_guid"), Key]
        public string UserActionGuid { get; set; }

        /// <summary>
        /// 用户类型GUID
        /// </summary>
        [Column("user_type_guid")]
        public string UserTypeGuid { get; set; }

        /// <summary>
        /// 操作GUID
        /// </summary>
        [Column("action_guid")]
        public string ActionGuid { get; set; }

        /// <summary>
        /// 行为特性code
        /// </summary>
        [Column("action_characteristics_code")]
        public string ActioCcharacteristicsCode { get; set; }

        /// <summary>
        /// 行为特性名称
        /// </summary>
        [Column("action_characteristics_name")]
        public string ActionCharacteristicsName { get; set; }

        /// <summary>
        /// 使能标志
        /// </summary>
        [Column("enable")]
        public bool Enable { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [Column("created_by")]
        public string CreatedBy { get; set; }

        /// <summary>
        /// 创建时间，默认为系统当前时间
        /// </summary>
        [Column("creation_date")]
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// 最后修改人
        /// </summary>
        [Column("last_updated_by")]
        public string LastUpdatedBy { get; set; }

        /// <summary>
        /// 最后修改日期，默认为系统当前时间
        /// </summary>
        [Column("last_updated_date")]
        public DateTime LastUpdatedDate { get; set; }
    }
}