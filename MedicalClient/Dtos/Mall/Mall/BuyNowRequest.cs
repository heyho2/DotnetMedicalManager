using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Mall.Mall
{
    /// <summary>
    ///立即购买
    /// </summary>
    public class BuyNowRequest
    {
        /// <summary>
        /// 产品Guid
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "产品Guid")]
        public string ProductGuid { get; set; }

        /// <summary>
        /// 产品数量
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "产品数量")]
        public int ProductNum { get; set; }

    }
    /// <summary>
    ///response
    /// </summary>
    public class BuyNowResponse:BaseDto
    {
        ///<summary>
        ///商户guid
        ///</summary>
        public string MerchantGuid { get; set; }




        ///<summary>
        ///商户名称
        ///</summary>
        public string MerchantName { get; set; }
        ///<summary>
        ///商户名称
        ///</summary>
        public decimal Freight { get; set; }
        ///<summary>
        ///商品图片URL
        ///</summary>
        public string ProductPicURL{ get; set; }
        ///<summary>
        ///商品类型
        ///</summary>
        public string ProductForm { get; set; }
        /////<summary>
        /////是否预付款
        /////</summary>
        //public bool AllowAdvancePayment { get; set; }



        ///<summary>
        ///GUID(根据USER_GUID区分购车）
        ///</summary>
        public string ItemGuid { get; set; }
        ///<summary>
        ///用户GUID
        ///</summary>
        public string UserGuid { get; set; }
        ///<summary>
        ///产品GUID
        ///</summary>
        public string ProductGuid { get; set; }
        ///<summary>
        ///产品名称
        ///</summary>
        public string ProductName { get; set; }
        ///<summary>
        ///产品数量
        ///</summary>
        public int Count { get; set; }
        ///<summary>
        ///是否有效（当库存不够，则显示无效）
        ///</summary>
        public bool IsValid { get; set; } = true;
        ///<summary>
        ///平台类型
        ///</summary>
        public string PlatformType { get; set; } = GD.Common.EnumDefine.PlatformType.CloudDoctor.ToString();
        /// <summary>
        /// 是否预付款购买
        /// </summary>
        public bool AdvancePayment { get; set; } = false;
    }



}
