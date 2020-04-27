using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GD.Dtos.Merchant.Therapist
{
    /// <summary>
    /// 获取服务人员列表响应Dto
    /// </summary>
    public class GetTherapistListResponseDto
    {
        /// <summary>
		/// 当前页数据
		/// </summary>
		public IEnumerable<GetTherapistListItemDto> CurrentPage
        {
            get;
            set;
        }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int Total
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 获取服务人员列表ItemDto
    /// </summary>
    public class GetTherapistListItemDto
    {
        /// <summary>
        /// 服务人员guid
        /// </summary>
        public string TherapistGuid { get; set; }
        /// <summary>
        /// 服务人员姓名
        /// </summary>
        public string TherapistName { get; set; }
        
        /// <summary>
        /// 服务人员手机号
        /// </summary>
        public string TherapistPhone { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<TherapistClassifyItem> Items { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationDate { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class TherapistClassifyItem
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public string TherapistGuid { get; set; }
        /// <summary>
        /// 分类guid
        /// </summary>
        public string ClassifyGuid { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        public string ClassifyName { get; set; }
    }
}
