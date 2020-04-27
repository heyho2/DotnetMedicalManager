using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Mall
{
    ///<summary>
    ///
    ///</summary>
    [Table("t_mall_aftersale_consultation")]
    public class AfterSaleConsultationModel : BaseModel
    {

        ///<summary>
        ///售后协商历史ID(协商记录若存在图片，则附件OWNER_GUID填入此主键值)
        ///</summary>
        [Column("consultation_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "售后协商历史ID(协商记录若存在图片，则附件OWNER_GUID填入此主键值)")]
        public string ConsultationGuid { get; set; }

        ///<summary>
        ///售后详情ID
        ///</summary>
        [Column("detail_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "售后详情ID")]
        public string DetailGuid { get; set; }

        ///<summary>
        ///协商标题
        ///</summary>
        [Column("title"), Required(ErrorMessage = "{0}必填"), Display(Name = "协商标题")]
        public string Title { get; set; }

        ///<summary>
        ///协商内容
        ///</summary>
        [Column("content")]
        public string Content { get; set; }

        ///<summary>
        ///协商记录发出者角色
        ///</summary>
        [Column("role_type")]
        public string RoleType { get; set; }
    }
}



