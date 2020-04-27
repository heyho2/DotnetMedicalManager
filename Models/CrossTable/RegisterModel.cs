using System.ComponentModel.DataAnnotations;
using GD.Common.EnumDefine;

namespace GD.Models.CrossTable
{
    /// <summary>
    /// 注册子平台数据模型
    /// </summary>
    public class RegisterModel
    {
        /// <summary>
        /// 注册来源平台类型
        /// </summary>
        [Display(Name = "注册来源平台类型")]
        public PlatformType PlatformType
        {
            get;
            set;
        }

        /// <summary>
        /// 扩展参数
        /// 子平台根据自己的需求单独定义规范
        /// </summary>
        [Display(Name = "扩展参数")]
        public object Parameters
        {
            get;
            set;
        }
    }
}
