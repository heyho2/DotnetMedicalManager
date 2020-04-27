using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GD.Common.Base;

namespace GD.Models.Manager
{
    ///<summary>
    ///首页banner表模型
    ///</summary>
    [Table("t_manager_banner")]
    public class BannerModel : BaseModel
    {
        ///<summary>
        ///BANNER的GUID
        ///</summary>
        [Column("banner_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "BANNER的GUID")]
        public string BannerGuid
        {
            get;
            set;
        }

        ///<summary>
        ///属于谁的GUID
        ///</summary>
        [Column("owner_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "属于谁的GUID")]
        public string OwnerGuid
        {
            get;
            set;
        }

        ///<summary>
        ///BANNER名称
        ///</summary>
        [Column("banner_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "BANNER名称")]
        public string BannerName
        {
            get;
            set;
        }

        ///<summary>
        ///BANNER图片GUID
        ///</summary>
        [Column("picture_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "BANNER图片GUID")]
        public string PictureGuid
        {
            get;
            set;
        }

        ///<summary>
        ///目标地址
        ///</summary>
        [Column("target_url"), Required(ErrorMessage = "{0}必填"), Display(Name = "目标地址")]
        public string TargetUrl
        {
            get;
            set;
        }

        ///<summary>
        ///排序
        ///</summary>
        [Column("sort"), Required(ErrorMessage = "{0}必填"), Display(Name = "排序")]
        public int Sort
        {
            get;
            set;
        }

        /// <summary>
        /// 描述
        /// </summary>
        [Column("Description")]
        public string Description { get; set; }
    }
}