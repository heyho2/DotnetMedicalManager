using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Mall
{
    /// <summary>
    /// 团购表
    /// </summary>
    [Table("t_mall_groupbuy")]
    public class GroupbuyModel : BaseModel
    {
        ///<summary>
        ///主键
        ///</summary>
        [Column("groupbuy_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "主键")]
        public string GroupBuyGuid { get; set; }

        ///<summary>
        ///产品GUID
        ///</summary>
        [Column("product_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "产品GUID")]
        public string ProductGuid { get; set; }

        ///<summary>
        ///团购标题
        ///</summary>
        [Column("name"), Required(ErrorMessage = "{0}必填"), Display(Name = "团购标题")]
        public string Name { get; set; }
        
        ///<summary>
        ///团购数量
        ///</summary>
        [Column("qty")]
        public int Qty { get; set; }

        ///<summary>
        ///一次最低购买数量最低
        ///</summary>
        [Column("buy_qty")]
        public int BuyQty { get; set; }

        ///<summary>
        ///团购价
        ///</summary>
        [Column("price"), Required(ErrorMessage = "{0}必填"), Display(Name = "团购价")]
        public decimal Price { get; set; }

        ///<summary>
        ///团购开始时间
        ///</summary>
        [Column("begin_date")]
        public DateTime BeginDate { get; set; }

        ///<summary>
        ///团购结束时间
        ///</summary>
        [Column("end_date")]
        public DateTime EndDate { get; set; }

        ///<summary>
        ///活动说明
        ///</summary>
        [Column("description")]
        public string Description { get; set; }

        ///<summary>
        ///分享内容
        ///</summary>
        [Column("share_content")]
        public string ShareContent { get; set; }

        ///<summary>
        ///分享图标
        ///</summary>
        [Column("share_icon")]
        public string ShareIcon { get; set; }

        ///<summary>
        ///分享标题
        ///</summary>
        [Column("share_title")]
        public string ShareTitle { get; set; }

        ///<summary>
        ///是否已成团
        ///</summary>
        [Column("lump"), Required(ErrorMessage = "{0}必填"), Display(Name = "是否已成团")]
        public string Lump { get; set; }

        ///<summary>
        ///是否推荐
        ///</summary>
        [Column("recommend"), Display(Name = "是否推荐")]
        public string Recommend { get; set; }

        ///<summary>
        ///排序
        ///</summary>
        [Column("sort"), Display(Name = "排序")]
        public string Sort { get; set; }
        
        ///<summary>
        ///平台类型--默认智慧云医
        ///</summary>
        [Column("platform_type"), Display(Name = "平台类型")]
        public string PlatformType { get; set; }= GD.Common.EnumDefine.PlatformType.CloudDoctor.ToString();
       
        /// <summary>
        /// 团购自信息列表
        /// </summary>
        public List<GroupBuyDetailModel> DetailModel { get; set; }
    }
}
