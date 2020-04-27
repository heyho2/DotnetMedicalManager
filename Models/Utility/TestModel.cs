using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GD.Common.Base;

namespace GD.Models.Utility
{
    ///<summary>
    ///测试表模型
    ///</summary>
    [Table("t_utility_test")]
    public class TestModel : BaseModel
    {
        ///<summary>
        ///GUID
        ///</summary>
        [Column("test_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "GUID")]
        public string TestGuid
        {
            get;
            set;
        }

        ///<summary>
        ///测试名
        ///</summary>
        [Column("test_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "测试名")]
        public string TestName
        {
            get;
            set;
        }
    }
}