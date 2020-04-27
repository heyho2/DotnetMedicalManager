using GD.Common.Base;

namespace GD.Dtos.Utility.Article
{
    /// <summary>
    /// 获取首页头条 请求
    /// </summary>
    public class GetHomeHeadlineRequestDto : BaseDto
    {
        /// <summary>
        /// 取多少条记录 默认 5
        /// </summary>
        public int Take { get; set; } = 5;
    }
    /// <summary>
    /// 获取首页头条 项
    /// </summary>
    public class GetHomeHeadlineItemDto : BaseDto
    {
        ///<summary>
        ///头条GUID
        ///</summary>
        public string HeadlineGuid { get; set; }

        ///<summary>
        ///头条名称
        ///</summary>
        public string HeadlineName { get; set; }

        ///<summary>
        ///头条简介
        ///</summary>
        public string HeadlineAbstract { get; set; }

        ///<summary>
        ///点击响应目标
        ///</summary>
        public string Target { get; set; }

        ///<summary>
        ///排序
        ///</summary>
        public int Sort { get; set; }

        ///<summary>
        ///平台类型
        ///</summary>
        public string PlatformType { get; set; }
    }
}
