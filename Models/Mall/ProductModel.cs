using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GD.Common.Base;
using GD.Models.CommonEnum;

namespace GD.Models.Mall
{
    ///<summary>
    ///产品表模型
    ///</summary>
    [Table("t_mall_product")]
    public class ProductModel : BaseModel
    {
        ///<summary>
        ///产品GUID
        ///</summary>
        [Column("product_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "产品GUID")]
        public string ProductGuid
        {
            get;
            set;
        }

        ///<summary>
        ///产品code
        ///</summary>
        [Column("product_code"), Display(Name = "产品code")]
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品标题
        /// </summary>
        [Column("product_title"), Display(Name = "产品标题")]
        public string ProductTitle { get; set; }

        ///<summary>
        ///商户GUID
        ///</summary>
        [Column("merchant_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "商户GUID")]
        public string MerchantGuid
        {
            get;
            set;
        }

        ///<summary>
        ///所属分类GUID
        ///</summary>
        [Column("category_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "所属分类GUID")]
        public string CategoryGuid
        {
            get;
            set;
        }

        ///<summary>
        ///所属分类名称
        ///</summary>
        [Column("category_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "所属分类名称")]
        public string CategoryName
        {
            get;
            set;
        }

        ///<summary>
        ///图片GUID
        ///</summary>
        [Column("picture_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "图片GUID")]
        public string PictureGuid
        {
            get;
            set;
        }

        ///<summary>
        ///产品名称
        ///</summary>
        [Column("product_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "产品名称")]
        public string ProductName
        {
            get;
            set;
        }

        ///<summary>
        ///产品标签
        ///</summary>
        [Column("product_label"), Display(Name = "产品标签")]
        public string ProductLabel
        {
            get;
            set;
        }
        ///<summary>
        ///品牌
        ///</summary>
        [Column("brand"), Required(ErrorMessage = "{0}必填"), Display(Name = "品牌")]
        public string Brand
        {
            get;
            set;
        }

        ///<summary>
        ///规格
        ///</summary>
        [Column("standerd"), Required(ErrorMessage = "{0}必填"), Display(Name = "规格")]
        public string Standerd
        {
            get;
            set;
        }

        ///<summary>
        ///保质期
        ///</summary>
        [Column("retention_period"), Required(ErrorMessage = "{0}必填"), Display(Name = "保质期")]
        public string RetentionPeriod
        {
            get;
            set;
        }

        ///<summary>
        ///生产厂家
        ///</summary>
        [Column("manufacture"), Display(Name = "生产厂家")]
        public string Manufacture
        {
            get;
            set;
        }

        ///<summary>
        ///批准文号
        ///</summary>
        [Column("approval_number"), Required(ErrorMessage = "{0}必填"), Display(Name = "批准文号")]
        public string ApprovalNumber
        {
            get;
            set;
        }
        ///<summary>
        ///价格
        ///</summary>
        [Column("price"), Required(ErrorMessage = "{0}必填"), Display(Name = "价格")]
        public decimal Price
        {
            get;
            set;
        } = 0M;

        ///<summary>
        ///成本价
        ///</summary>
        [Column("cost_price"), Display(Name = "成本价")]
        public decimal? CostPrice
        {
            get;
            set;
        } = 0M;

        ///<summary>
        ///市场价
        ///</summary>
        [Column("market_price"), Display(Name = "市场价")]
        public decimal? MarketPrice
        {
            get;
            set;
        } = 0M;

        ///<summary>
        ///运费
        ///</summary>
        [Column("freight"), Display(Name = "运费")]
        public decimal? Freight
        {
            get;
            set;
        } = 0M;
        ///<summary>
        ///详情（富文本Guid）
        ///</summary>
        [Column("introduce_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "商品介绍Guid")]
        public string IntroduceGuid
        {
            get;
            set;
        }

        ///<summary>
        ///说明书（富文本Guid）
        ///</summary>
        [Column("pro_detail_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "详情")]
        public string ProDetailGuid
        {
            get;
            set;
        }

        /// <summary>
        /// 库存
        /// </summary>
        [Column("inventory"), Display(Name = "库存")]
        public int Inventory
        {
            get;
            set;
        } = 0;

        /// <summary>
        /// 警戒库存
        /// </summary>
        [Column("warning_inventory"), Display(Name = "警戒库存")]
        public int WarningInventory
        {
            get;
            set;
        } = 0;

        ///<summary>
        ///位置
        ///</summary>
        [Column("location"), Display(Name = "位置")]
        public string Location
        {
            get;
            set;
        }

        ///<summary>
        ///是否热门
        ///</summary>
        [Column("recommend"), Required(ErrorMessage = "{0}必填"), Display(Name = "是否热门")]
        public bool Recommend
        {
            get;
            set;
        } = false;

        ///<summary>
        ///排序
        ///</summary>
        [Column("sort"), Required(ErrorMessage = "{0}必填"), Display(Name = "排序")]
        public int Sort
        {
            get;
            set;
        } = 0;

        ///<summary>
        ///平台类型
        ///</summary>
        [Column("platform_type"), Required(ErrorMessage = "{0}必填"), Display(Name = "平台类型")]
        public string PlatformType
        {
            get;
            set;
        } = GD.Common.EnumDefine.PlatformType.CloudDoctor.ToString();


        ///<summary>
        ///允许预付款
        ///</summary>
        [Column("allow_advance_payment"), Required(ErrorMessage = "{0}必填"), Display(Name = "允许预付款")]
        public bool AllowAdvancePayment { get; set; } = false;

        /// <summary>
        /// 预付款比例
        /// </summary>
        [Column("advance_payment_rate"), Display(Name = "预付款比例")]
        public decimal? AdvancePaymentRate { get; set; }

        /// <summary>
        /// 自购买日期多少天有效（0表示永久有效）
        /// </summary>
        [Column("effective_days"), Display(Name = "自购买日期多少天有效（0表示永久有效）")]
        public int? EffectiveDays { get; set; } = 0;

        /// <summary>
        /// 商品形态（实体商品；虚拟商品）
        /// </summary>
        [Column("product_form"), Required(ErrorMessage = "{0}必填"), Display(Name = "商品形态")]
        public string ProductForm { get; set; }

        /// <summary>
        /// 是否上架在售
        /// </summary>
        [Column("on_sale"), Required(ErrorMessage = "{0}必填"), Display(Name = "是否上架在售")]
        public bool OnSale { get; set; } = true;

        /// <summary>
        /// 是否上架在售(后台管理端)
        /// </summary>
        [Column("platform_on_sale"), Required(ErrorMessage = "{0}必填"), Display(Name = "是否上架在售")]
        public bool PlatformOnSale { get; set; } = true;
        /// <summary>
        /// 体验报告Guid（富文本）
        /// </summary>
        [Column("report_guid"), Display(Name = "体验报告Guid")]
        public string ReportGuid { get; set; }

        /// <summary>
        /// 时长
        /// </summary>
        [Column("operation_time"), Display(Name = "时长")]
        public int? OperationTime { get; set; }

        /// <summary>
        /// 商品关联项目使用总次数阈值,若大于0则商品下项目使用次数最多为设置的阈值，使用次数等于阈值后个人商品改为不可用
        /// 0表示忽略阈值规则限制
        /// </summary>
        [Column("project_threshold"), Display(Name = "项目次数阈值")]
        public int? ProjectThreshold { get; set; } = 0;


        /// <summary>
        /// 
        /// </summary>
        public enum ProductFormEnum
        {
            /// <summary>
            /// 
            /// </summary>
            [Description("服务类")]
            Service = 1,
            /// <summary>
            /// 
            /// </summary>
            [Description("实体类")]
            Physical
        }
    }
}