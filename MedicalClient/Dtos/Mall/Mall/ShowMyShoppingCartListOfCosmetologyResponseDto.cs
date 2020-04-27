using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Mall.Mall
{
    /// <summary>
    /// 双美--获取购物车列表数据 响应Dto
    /// </summary>
    public class ShowMyShoppingCartListOfCosmetologyResponseDto : BaseDto
    {
        /// <summary>
        /// 商户guid
        /// </summary>
        public string MerchantGuid { get; set; }

        /// <summary>
        /// 商户名称
        /// </summary>
        public string MerchantName { get; set; }


        /// <summary>
        /// 商户是否可用
        /// </summary>
        public bool MerchantEnable { get; set; }

        /// <summary>
        /// 购物车商品数据
        /// </summary>
        public List<ShoppingCarListProductInfo> Products { get; set; }
    }

    /// <summary>
    /// 购物车商品信息
    /// </summary>
    public class ShoppingCarListProductInfo : BaseDto
    {
        /// <summary>
        /// 购物车主键
        /// </summary>
        public string ItemGuid { get; set; }
        /// <summary>
        /// 购物车商品是否有效
        /// </summary>
        public bool IsValid { get; set; }
        /// <summary>
        /// 商品guid
        /// </summary>
        public string ProductGuid { get; set; }

        /// <summary>
        /// 商品是否上架
        /// </summary>
        public bool OnSale { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 商品价格
        /// </summary>
        public decimal ProductPrice { get; set; }
        /// <summary>
        /// 商品形态，Service/Physical
        /// </summary>
        public string ProductForm { get; set; }
        /// <summary>
        /// 商品图片
        /// </summary>
        public string ProductPicture { get; set; }

        /// <summary>
        /// 商品是否允许预付款
        /// </summary>
        public bool AllowAdvancePayment { get; set; }

        /// <summary>
        /// 商品预付款比例
        /// </summary>
        public decimal? AdvancePaymentRate { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        public int ProductCount { get; set; }
        /// <summary>
        /// 商品运费
        /// </summary>
        public decimal? Freight { get; set; }

        /// <summary>
        /// 商品项数据
        /// </summary>
        public List<ShoppingCarListProductProjectInfo> Projects { get; set; }

    }
    /// <summary>
    /// 购物车商品项信息
    /// </summary>
    public class ShoppingCarListProductProjectInfo : BaseDto
    {
        /// <summary>
        /// 商品guid
        /// </summary>
        public string ProductGuid { get; set; }

        /// <summary>
        /// 商品项guid
        /// </summary>
        public string ProjectGuid { get; set; }

        /// <summary>
        /// 商品项名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 商品项次数
        /// </summary>
        public int ProjectTimes { get; set; }
    }




    /// <summary>
    /// 双美购物车明细
    /// </summary>
    public class ShoppingCartListDetailOfCosmetologyDto : BaseDto
    {
        /// <summary>
        /// 购物车主键
        /// </summary>
        public string ItemGuid { get; set; }

        /// <summary>
        /// 商户guid
        /// </summary>
        public string MerchantGuid { get; set; }

        /// <summary>
        /// 商户名称
        /// </summary>
        public string MerchantName { get; set; }

        /// <summary>
        /// 商户是否可用
        /// </summary>
        public bool MerchantEnable { get; set; }

        /// <summary>
        /// 商品guid
        /// </summary>
        public string ProductGuid { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 是否上架
        /// </summary>
        public bool OnSale { get; set; }

        /// <summary>
        /// 商品价格
        /// </summary>
        public decimal ProductPrice { get; set; }

        /// <summary>
        /// 商品形态，Service/Physical
        /// </summary>
        public string ProductForm { get; set; }
        
        /// <summary>
        /// 商品图片
        /// </summary>
        public string ProductPicture { get; set; }

        /// <summary>
        /// 商品是否允许预付款
        /// </summary>
        public bool AllowAdvancePayment { get; set; }

        /// <summary>
        /// 商品预付款比例
        /// </summary>
        public decimal? AdvancePaymentRate { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        public int ProductCount { get; set; }

        /// <summary>
        /// 商品项guid
        /// </summary>
        public string ProjectGuid { get; set; }

        /// <summary>
        /// 商品项名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 商品项次数
        /// </summary>
        public int ProjectTimes { get; set; }


    }
}
