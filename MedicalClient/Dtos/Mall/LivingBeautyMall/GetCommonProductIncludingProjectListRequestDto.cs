using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Mall.LivingBeautyMall
{
    /// <summary>
    /// 生美-商品包含产品项 -请求Dto
    /// </summary>
    public class GetCommonProductIncludingProjectListRequestDto
    {
        /// <summary>
        /// 产品Guid
        /// </summary>
        public string ProductGuid { get; set; }
        /// <summary>
        /// 平台类型
        /// </summary>
        public string PlatformType { get; set; } = GD.Common.EnumDefine.PlatformType.LifeCosmetology.ToString();
        /// <summary>
        /// 是否可用
        /// </summary>
        public bool Enable { get; set; } = true;
    }

    /// <summary>
    /// 响应Dto
    /// </summary>
    public class GetCommonProductIncludingProjectListResponseDto
    {
        /// <summary>
        /// 产品Guid
        /// </summary>
        public string ProductGuid { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName{ get; set; }

        /// <summary>
        /// 子项列表
        /// </summary>
        public List<GetProjectListItemDto> ProjectList { get; set; }

        /// <summary>
        /// 子项项目
        /// </summary>
        public class GetProjectListItemDto
        {
            /// <summary>
            /// 项目Guid
            /// </summary>
            public string ProjectGuid { get; set; }

            /// <summary>
            /// 项目编号
            /// </summary>
            public string ProjectNo { get; set; }

            /// <summary>
            /// 项目名称
            /// </summary>
            public string ProjectName { get; set; }
            /// <summary>
            /// 子项次数
            /// </summary>
            public int ProjectTimes { get; set; }

            /// <summary>
            /// 是否可转赠
            /// </summary>
            public bool AllowPresent { get; set; }
            /// <summary>
            /// 项目价格
            /// </summary>
            public decimal Price { get; set; }
        }
    }

    
}
