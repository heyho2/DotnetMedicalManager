using System;
using System.Collections.Generic;
using System.Text;
using GD.Common.Base;

namespace GD.Dtos.Decoration
{
    /// <summary>
    /// 获取装修记录分页列表请求Dto
    /// </summary>
    public class GetDecorationPageListRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 分类guid
        /// </summary>
        public string ClassificationGuid { get; set; }
    }

    /// <summary>
    /// 获取装修记录分页列表响应Dto
    /// </summary>
    public class GetDecorationPageListResponseDto : BasePageResponseDto<GetDecorationPageListItemDto>
    {

    }

    /// <summary>
    /// 获取装修记录分页列表Item Dto
    /// </summary>
    public class GetDecorationPageListItemDto : BaseDto
    {
        /// <summary>
        /// 装修记录guid
        /// </summary>
        public string DecorationGuid { get; set; }

        /// <summary>
        /// 装修记录名称
        /// </summary>
        public string DecorationName { get; set; }

        /// <summary>
        /// 类别名称
        /// </summary>
        public string ClassificationName { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationDate { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }
    }
}
