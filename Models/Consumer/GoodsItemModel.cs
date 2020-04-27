using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Consumer
{
    ///<summary>
    ///个人购买商品项信息管理表
    ///</summary>
    [Table("t_consumer_goods_item")]
    public class GoodsItemModel : BaseModel
    {
        ///<summary>
        ///个人商品项GUID
        ///</summary>
        [Column("goods_item_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "个人商品项GUID")]
        public string GoodsItemGuid { get; set; }

        ///<summary>
        ///个人商品GUID
        ///</summary>
        [Column("goods_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "个人商品GUID")]
        public string GoodsGuid { get; set; }

        ///<summary>
        ///商品项GUID
        ///</summary>
        [Column("project_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "商品项GUID")]
        public string ProjectGuid { get; set; }

        ///<summary>
        ///项目次数
        ///</summary>
        [Column("count"), Required(ErrorMessage = "{0}必填"), Display(Name = "项目次数")]
        public int Count { get; set; }

        ///<summary>
        ///可用次数
        ///</summary>
        [Column("remain"), Required(ErrorMessage = "{0}必填"), Display(Name = "可用次数")]
        public int Remain { get; set; }

        ///<summary>
        ///已用次数
        ///</summary>
        [Column("used"), Required(ErrorMessage = "{0}必填"), Display(Name = "已用次数")]
        public int Used { get; set; }

        ///<summary>
        ///是否可用
        ///</summary>
        [Column("available"), Required(ErrorMessage = "{0}必填"), Display(Name = "是否可用")]
        public bool Available { get; set; }

        ///<summary>
        ///项单价
        ///</summary>
        [Column("price"), Required(ErrorMessage = "{0}必填"), Display(Name = "项单价")]
        public decimal Price { get; set; }


        /// <summary>
        /// 是否允许转赠
        /// </summary>
        [Column("allow_present"), Required(ErrorMessage = "{0}必填"), Display(Name = "是否允许转赠")]
        public bool AllowPresent { get; set; }

        ///<summary>
        ///平台类型:CloudDoctor(智慧云医)；LifeCosmetology(生活美容)；MedicalCosmetology(医疗美容)
        ///</summary>
        [Column("platform_type")]
        public string PlatformType { get; set; }
    }
}



