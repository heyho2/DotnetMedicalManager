using GD.Common.Base;
using System.Collections.Generic;

namespace GD.Dtos.Mall.Product
{
    /// <summary>
    /// 修改商品下包含的服务项目请求Dto
    /// </summary>
    public class ModifyProductProjectsRelationRequestDto : BaseDto
    {
        /// <summary>
        /// 商品guid
        /// </summary>
        public string ProductGuid { get; set; }

        /// <summary>
        /// 商品项目共可使用次数
        /// </summary>
        public int ProjectThreshold { get; set; } = 0;


        /// <summary>
        /// 商品包含的服务项目集合
        /// </summary>
        public List<ProductProjectRelationDto> Projects { get; set; }
    }
}
