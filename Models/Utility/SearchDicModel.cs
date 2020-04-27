using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Mall
{
    ///<summary>
    ///
    ///</summary>
    [Table("t_utility_search_dic")]
    public class SearchDicModel : BaseModel
    {
        ///<summary>
        ///GUID
        ///</summary>
        [Column("dic_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "GUID")]
        public string DicGuid { get; set; }

        ///<summary>
        ///搜索词
        ///</summary>
        [Column("word"), Required(ErrorMessage = "{0}必填"), Display(Name = "搜索词")]
        public string Word { get; set; }

        ///<summary>
        ///搜索次数频率
        ///</summary>
        [Column("frequency")]
        public int Frequency { get; set; }

        ///<summary>
        ///是否已经加载到IK
        ///</summary>
        [Column("loaded")]
        public sbyte Loaded { get; set; }
    }
}



