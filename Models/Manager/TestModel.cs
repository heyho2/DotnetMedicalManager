using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GD.Common.Base;

namespace GD.Models.Manager
{
    /// <summary>
    /// 测试模型
    /// </summary>
    [Table("t_utility_test")]
    public class TestModel : BaseModel
    {
        /// <summary>
        /// 测试ID
        /// </summary>
        [Column("test_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "测试ID")]
        public string TestGuid
        {
            get;
            set;
        }

        /// <summary>
        /// 测试名称
        /// </summary>
        [Column("test_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "测试名称"), MinLength(4, ErrorMessage = "{0}的长度不允许小于{1}"), MaxLength(20, ErrorMessage = "{0}的长度不允许超过{1}")]
        public string TestName
        {
            get;
            set;
        }
    }
}
