using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.CrossTable
{
    /// <summary>
    /// 分页查询产品列表
    /// </summary>
    public class GetMerchantProductInfoModel : BaseModel
    {

        /// <summary>
        /// 商户Guid
        /// </summary>
        public string MerchantGuid { get; set; }

        /// <summary>
        /// 商户名称
        /// </summary>
        public string MerchantName { get; set; }

        /// <summary>
        /// 分类Guid（字典dicGuid）
        /// </summary>
        public string CategoryGuid { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string ConfigName { get; set; }

        /// <summary>
        /// 商品Guid
        /// </summary>
        public string ProductGuid { get; set; }
        /// <summary>
        /// 商品Logo
        /// </summary>
        public string ProductPicUrl { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 商品价格
        /// </summary>
        public string Price { get; set; }
        /// <summary>
        /// 商品形态(服务，实体)'Service','Physical'
        /// </summary>
        public string ProductForm { get; set; }
        /// <summary>
        /// 销售量
        /// </summary>
        public int SaleNum { get; set; }



        ///// <summary>获取模型对应的表名</summary>
        ///// <returns></returns>
        //public string GetTableName()
        //{
        //    return ((IEnumerable<object>)this.GetType().GetCustomAttributes(typeof(TableAttribute), false)).Select(a => new
        //    {
        //        a = a,
        //        t = a as TableAttribute
        //    }).Where(_param1 => _param1.t != null).Select(_param1 => _param1.t.Name).FirstOrDefault<string>();
        //}

        //IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        //{
        //    return this.Verify(validationContext);
        //}
        ///// <summary>数据校验</summary>
        ///// <param name="validationContext"></param>
        ///// <returns></returns>
        ///// <remarks>子类可重载此方法，执行自定义的有效性校验</remarks>
        //protected  IEnumerable<ValidationResult> Verify(ValidationContext validationContext)
        //{
        //    return (IEnumerable<ValidationResult>)new List<ValidationResult>();
        //}
    }
}
