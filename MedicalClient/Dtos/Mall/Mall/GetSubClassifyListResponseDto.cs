using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Mall.Mall
{
    /// <summary>
    /// 根据父级Guid获取二级分类菜单 请求
    /// </summary>
    public class GetSubClassifyListRequestDto : BaseDto
    {
        /// <summary>
        /// 分类父级Guid
        /// </summary>
        public string ParentGuid { get; set; }
    }
}
