using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Merchant.Merchant
{
    /// <summary>
    /// 排班明细Dto
    /// </summary>
    public class MerchantScheduleDetailDto:BaseDto
    {
        ///<summary>
        ///排班明细GUID
        ///</summary>
        public string ScheduleDetailGuid { get; set; }

        ///<summary>
        ///排班GUID
        ///</summary>
        public string ScheduleGuid { get; set; }

        /// <summary>
        /// 排班日期
        /// </summary>
        public DateTime ScheduleDate { get; set; }

        ///<summary>
        ///开始时间
        ///</summary>
        public string StartTime { get; set; }

        ///<summary>
        ///结束时间
        ///</summary>
        public string EndTime { get; set; }

        ///<summary>
        ///消费GUID
        ///</summary>
        public string ConsumptionGuid { get; set; }

        /// <summary>
		/// 组织ID
		/// </summary>
		/// <remarks>
		/// 多组织平台，每一笔数据都归属于具体的组织
		/// </remarks>
        public string OrgGuid
        {
            get;
            set;
        }

        /// <summary>
        /// 使能标志，默认为 true
        /// </summary>
        /// <remarks>
        /// 可用此字段进行软删除，禁用等相关状态
        /// </remarks>
        public bool Enable
        {
            get;
            set;
        } 


        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy
        {
            get;
            set;
        }

        /// <summary>
        /// 创建时间，默认为系统当前时间
        /// </summary>
        public DateTime CreationDate
        {
            get;
            set;
        } 


        /// <summary>
        /// 最后修改人
        /// </summary>
        public string LastUpdatedBy
        {
            get;
            set;
        }

        /// <summary>
        /// 最后修改日期，默认为系统当前时间
        /// </summary>
        public DateTime LastUpdatedDate
        {
            get;
            set;
        } 
    }
}
