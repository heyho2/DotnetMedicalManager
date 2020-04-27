using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using GD.Common.Base;

namespace GD.Models.Mall
{
    /// <inheritdoc />
    /// <summary>
    /// 活动表
    /// </summary>
    [Table("t_mall_campaign")]
    public class CampaignModel : BaseModel
    {
        ///<summary>
        ///主键
        ///</summary>
        [Column("campaign_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "主键")]
        public string CampaignGuid { get; set; }

        ///<summary>
        ///活动名称
        ///</summary>
        [Column("campaign_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "活动名称")]
        public string CampaignName { get; set; }

        ///<summary>
        ///产品ID
        ///</summary>
        [Column("product_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "产品ID")]
        public string ProductGuid { get; set; }

        ///<summary>
        ///折扣(0-1)
        ///</summary>
        [Column("discount"), Display(Name = "折扣(0-1)")]
        public string Discount { get; set; }

        ///<summary>
        ///优惠价，非正数表示不适用
        ///</summary>
        [Column("special_prices"), Required(ErrorMessage = "{0}必填"), Display(Name = "优惠价")]
        public string SpecialPrices { get; set; }

        ///<summary>
        ///活动开始日期
        ///</summary>
        [Column("begin_date"), Required(ErrorMessage = "{0}必填"), Display(Name = "活动开始日期")]
        public string BeginDate { get; set; }

        ///<summary>
        ///活动结束日期
        ///</summary>
        [Column("end_date"), Required(ErrorMessage = "{0}必填"), Display(Name = "活动结束日期")]
        public string EndDate { get; set; }

        ///<summary>
        ///排序
        ///</summary>
        [Column("sort"), Required(ErrorMessage = "{0}必填"), Display(Name = "排序")]
        public string Sort { get; set; }
        
    }



}
