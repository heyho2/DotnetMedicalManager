using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GD.Common.Base;

namespace GD.Models.Manager
{
    ///<summary>
    ///推荐表模型
    ///</summary>
    [Table("t_manager_recommend")]
    public class RecommendModel : BaseModel
    {
        ///<summary>
        ///推荐GUID
        ///</summary>
        [Key, Column("recommend_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "推荐GUID")]
        public string RecommendGuid { get; set; }

        ///<summary>
        ///推荐归属GUID
        ///</summary>
        [Column("owner_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "推荐归属GUID")]
        public string OwnerGuid { get; set; }

        ///<summary>
        ///名称
        ///</summary>
        [Column("name"), Required(ErrorMessage = "{0}必填"), Display(Name = "名称")]
        public string Name { get; set; }

        ///<summary>
        ///点击响应目标
        ///</summary>
        [Column("target"), Display(Name = "点击响应目标")]
        public string Target { get; set; }

        ///<summary>
        ///图片GUID
        ///</summary>
        [Column("picture_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "图片GUID")]
        public string PictureGuid { get; set; }

        ///<summary>
        ///排序
        ///</summary>
        [Column("sort"), Required(ErrorMessage = "{0}必填"), Display(Name = "排序")]
        public int Sort { get; set; }
        ///<summary>
        ///类型[Campaign,Article,Product,Doctor,Office,Hostpital]
        ///</summary>
        [Column("type"), Required(ErrorMessage = "{0}必填"), Display(Name = "类型")]
        public string Type { get; set; }
        ///<summary>
        ///备注
        ///</summary>
        [Column("remark"), Display(Name = "备注")]
        public string Remark { get; set; }
        /// <summary>
        /// 推荐类型
        /// </summary>
        public enum TypeEnum
        {
            /// <summary>
            /// 活动
            /// </summary>
            [Description("活动")]
            Campaign = 1,
            /// <summary>
            /// 文章
            /// </summary>
            [Description("文章")]
            Article = 2,
            /// <summary>
            /// 商品
            /// </summary>
            [Description("商品")]
            Product = 3,
            /// <summary>
            /// 医生
            /// </summary>
            [Description("医生")]
            Doctor = 4,
            /// <summary>
            /// 科室
            /// </summary>
            [Description("科室")]
            Office = 5,
            /// <summary>
            /// 医院
            /// </summary>
            [Description("医院")]
            Hostpital = 6,
            /// <summary>
            /// 课程
            /// </summary>
            [Description("课程")]
            Course = 7
        }
    }
}