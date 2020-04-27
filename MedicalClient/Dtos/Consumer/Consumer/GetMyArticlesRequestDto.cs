using System;
using System.Collections.Generic;
using System.Text;
using GD.Common.Base;

namespace GD.Dtos.Consumer.Consumer
{
    /// <summary>
    /// 获取我关注的文章分页列表请求
    /// </summary>
    public class GetMyArticlesRequestDto : PageRequestDto
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }
    }

    /// <summary>
    /// 获取我关注的文章分页列表响应
    /// </summary>
    public class GetMyArticlesResponseDto : BasePageResponseDto<GetMyArticlesItemDto>
    {

    }

    /// <summary>
    /// 获取我关注的文章分页列表详情
    /// </summary>
    public class GetMyArticlesItemDto : BaseDto
    {
        ///<summary>
        ///文章GUID
        ///</summary>
        public string ArticleGuid { get; set; }

        ///<summary>
        ///医生姓名
        ///</summary>
        public string UserName { get; set; }

        ///<summary>
        ///基础路径
        ///</summary>
        public string ArticlePic { get; set; }

        /// <summary>
        /// 文章来源
        /// </summary>
        public string SourceType { get; set; }

        ///<summary>
        ///文章标题
        ///</summary>
        public string Title { get; set; }

        ///<summary>
        ///字典ID
        ///</summary>
        public string DicGuid { get; set; }

        ///<summary>
        ///配置名称
        ///</summary>
        public string ConfigName { get; set; }


        ///<summary>
        ///发布日期（创建日期）
        ///</summary>
        public string CreationDate { get; set; }

        ///<summary>
        ///浏览次数
        ///</summary>
        public int ViewNum { get; set; } = 0;

        ///<summary>
        ///点赞次数
        ///</summary>
        public int ThumbUpNum { get; set; } = 0;
    }
}
