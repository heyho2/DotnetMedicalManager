using GD.Common.Base;
using GD.Dtos.Manager.Banner;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Mall.Product
{
    /// <summary>
    /// 获取商品信息响应Dto
    /// </summary>
    public class GetProductInfoResponseDto : BaseDto
    {
        ///<summary>
        ///产品GUID
        ///</summary>
        public string ProductGuid
        {
            get;
            set;
        }

        ///<summary>
        ///产品code
        ///</summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品标题
        /// </summary>
        public string ProductTitle { get; set; }


        ///<summary>
        ///所属分类GUID(二级)
        ///</summary>
        public string CategoryGuid
        {
            get;
            set;
        }

        ///<summary>
        ///所属分类名称
        ///</summary>
        public string CategoryName
        {
            get;
            set;
        }

        /// <summary>
        /// 商品一级分类guid
        /// </summary>
        public string OneLevelCategoryGuid { get; set; }

        ///<summary>
        ///图片GUID
        ///</summary>
        public string PictureGuid
        {
            get;
            set;
        }

        ///<summary>
        ///图片url
        ///</summary>
        public string PictureUrl
        {
            get;
            set;
        }

        ///<summary>
        ///产品名称
        ///</summary>
        public string ProductName
        {
            get;
            set;
        }

        ///<summary>
        ///产品标签
        ///</summary>
        public string ProductLabel
        {
            get;
            set;
        }
        ///<summary>
        ///品牌
        ///</summary>
        public string Brand
        {
            get;
            set;
        }

        ///<summary>
        ///规格
        ///</summary>
        public string Standerd
        {
            get;
            set;
        }

        ///<summary>
        ///保质期
        ///</summary>
        public string RetentionPeriod
        {
            get;
            set;
        }

        ///<summary>
        ///生产厂家
        ///</summary>
        public string Manufacture
        {
            get;
            set;
        }

        ///<summary>
        ///批准文号
        ///</summary>
        public string ApprovalNumber
        {
            get;
            set;
        }
        ///<summary>
        ///价格
        ///</summary>
        public decimal Price
        {
            get;
            set;
        } = 0M;

        ///<summary>
        ///成本价
        ///</summary>
        public decimal? CostPrice
        {
            get;
            set;
        } = 0M;

        ///<summary>
        ///市场价
        ///</summary>
        public decimal? MarketPrice
        {
            get;
            set;
        } = 0M;

        ///<summary>
        ///运费
        ///</summary>
        public decimal? Freight
        {
            get;
            set;
        } = 0M;
        ///<summary>
        ///详情（富文本Guid）
        ///</summary>
        public string IntroduceGuid
        {
            get;
            set;
        }

        ///<summary>
        ///说明书（富文本Guid）
        ///</summary>
        public string ProDetailGuid
        {
            get;
            set;
        }

        /// <summary>
        /// 阈值
        /// </summary>
        public int? ProjectThreshold { get; set; }

        /// <summary>
        /// 库存
        /// </summary>
        public int Inventory
        {
            get;
            set;
        } = 0;

        /// <summary>
        /// 警戒库存
        /// </summary>
        public int WarningInventory
        {
            get;
            set;
        } = 0;

        ///<summary>
        ///位置
        ///</summary>
        public string Location
        {
            get;
            set;
        }

        ///<summary>
        ///是否热门
        ///</summary>
        public bool Recommend
        {
            get;
            set;
        } = false;

        ///<summary>
        ///排序
        ///</summary>
        public int Sort
        {
            get;
            set;
        } = 0;

        ///<summary>
        ///平台类型
        ///</summary>
        public string PlatformType
        {
            get;
            set;
        } = GD.Common.EnumDefine.PlatformType.CloudDoctor.ToString();


        ///<summary>
        ///允许预付款
        ///</summary>
        public bool AllowAdvancePayment { get; set; } = false;

        /// <summary>
        /// 预付款比例
        /// </summary>
        public decimal? AdvancePaymentRate { get; set; }

        /// <summary>
        /// 自购买日期多少天有效（0表示永久有效）
        /// </summary>
        public int? EffectiveDays { get; set; } = 0;

        /// <summary>
        /// 是否是实体商品（是：实体商品；否：虚拟商品）
        /// </summary>
        public bool IsPhysical { get; set; } = true;

        /// <summary>
        /// 是否上架在售
        /// </summary>
        public bool OnSale { get; set; } = true;


        /// <summary>
        /// 体验报告Guid（富文本）
        /// </summary>
        public string ReportGuid { get; set; }

        /// <summary>
        /// 时长
        /// </summary>
        public int? OperationTime { get; set; }

        /// <summary>
        /// 商品banners
        /// </summary>
        public List<BannerBaseDto> Banners { get; set; }
    }
}
