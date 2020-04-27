using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Utility
{
    ///<summary>
    ///平台帮助控制器
    ///</summary>
    [Table("t_utility_help")]
    public class HelpModel:BaseModel
    {
         ///<summary>
         ///主键GUID
         ///</summary>
         [Column("help_guid"),Key,Required(ErrorMessage = "{0}必填"), Display(Name = "主键GUID")]
         public string HelpGuid{ get;set; }
        
         ///<summary>
         ///问题
         ///</summary>
         [Column("question"),Required(ErrorMessage = "{0}必填"), Display(Name = "问题")]
         public string Question{ get;set; }
        
         ///<summary>
         ///解答
         ///</summary>
         [Column("answer"),Required(ErrorMessage = "{0}必填"), Display(Name = "解答")]
         public string Answer{ get;set; }
        
         ///<summary>
         ///排序
         ///</summary>
         [Column("sort"),Required(ErrorMessage = "{0}必填"), Display(Name = "排序")]
         public int Sort{ get;set; }
        
         ///<summary>
         ///平台类型:CLOUDDOCTOR(智慧云医)；LIFECOSMETOLOGY(生活美容)；MEDICALCOSMETOLOGY(医疗美容)
         ///</summary>
         [Column("platform_type")]
         public string PlatformType{ get;set; }
    }
}



