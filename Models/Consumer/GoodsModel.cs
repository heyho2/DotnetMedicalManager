using GD.Common.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Consumer
{
    ///<summary>
    ///个人购买商品信息管理表
    ///</summary>
    [Table("t_consumer_goods")]
    public class GoodsModel : BaseModel
    {
        ///<summary>
        ///个人商品GUID
        ///</summary>
        [Column("goods_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "个人商品GUID")]
        public string GoodsGuid { get; set; }

        ///<summary>
        ///用户GUID
        ///</summary>
        [Column("user_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "用户GUID")]
        public string UserGuid { get; set; }

        ///<summary>
        ///订单GUID
        ///</summary>
        [Column("order_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "订单GUID")]
        public string OrderGuid { get; set; }

        ///<summary>
        ///订单明细GUID
        ///</summary>
        [Column("detail_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "订单明细GUID")]
        public string DetailGuid { get; set; }

        /// <summary>
        /// 是否可用(卡项目次数已用完，则变为不可用)
        /// </summary>
        [Column("available"), Required(ErrorMessage = "{0}必填"), Display(Name = "是否可用")]
        public bool Available { get; set; } = true;

        ///<summary>
        ///有效起始日期
        ///</summary>
        [Column("effective_start_date")]
        public DateTime? EffectiveStartDate { get; set; }

        ///<summary>
        ///有效终止日期
        ///</summary>
        [Column("effective_end_date")]
        public DateTime? EffectiveEndDate { get; set; }

        ///<summary>
        ///商品GUID
        ///</summary>
        [Column("product_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "商品GUID")]
        public string ProductGuid { get; set; }

        ///<summary>
        ///商品名称
        ///</summary>
        [Column("product_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "商品名称")]
        public string ProductName { get; set; }

        ///<summary>
        ///商铺GUID
        ///</summary>
        [Column("merchan_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "商铺GUID")]
        public string MerchanGuid { get; set; }

        ///<summary>
        ///商品单价
        ///</summary>
        [Column("price"), Required(ErrorMessage = "{0}必填"), Display(Name = "商品单价")]
        public decimal Price { get; set; }

        ///<summary>
        ///选择方式（例如N选一，N选二，N选三）
        ///</summary>
        [Column("select_rule")]
        public string SelectRule { get; set; }

        ///<summary>
        ///平台类型:CloudDoctor(智慧云医)；LifeCosmetology(生活美容)；MedicalCosmetology(医疗美容)
        ///</summary>
        [Column("platform_type")]
        public string PlatformType { get; set; }

        /// <summary>
        /// 商品关联项目使用总次数阈值,若大于0则商品下项目使用次数最多为设置的阈值，使用次数等于阈值后个人商品改为不可用
        /// 0表示忽略阈值规则限制
        /// </summary>
        [Column("project_threshold"), Display(Name = "项目次数阈值")]
        public int? ProjectThreshold { get; set; } = 0;
    }
}



