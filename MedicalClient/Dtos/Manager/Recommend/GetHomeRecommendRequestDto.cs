using GD.Common.Base;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Manager.Recommend
{
    /// <summary>
    /// 获取首页推荐 请求
    /// </summary>
    public class GetHomeRecommendRequestDto
    {
        /// <summary>
        /// 取多少条记录 默认5
        /// </summary>
        [Range(1, 100)]
        public int Take { get; set; } = 5;
    }
    /// <summary>
    /// 获取首页推荐 响应
    /// </summary>
    public class GetHomeRecommendItemDto : BaseDto
    {
        ///<summary>
        ///推荐GUID
        ///</summary>
        public string RecommendGuid { get; set; }

        ///<summary>
        ///名称
        ///</summary>
        public string Name { get; set; }

        ///<summary>
        ///点击响应目标
        ///</summary>
        public string Target { get; set; }

        ///<summary>
        ///图片
        ///</summary>
        public string PictureUrl { get; set; }

        ///<summary>
        ///排序
        ///</summary>
        public int Sort { get; set; }

        ///<summary>
        ///备注
        ///</summary>
        public string Remark { get; set; }
        ///<summary>
        ///类型 Campaign 活动，Article 文章，Product 商品，Doctor 医生，Office，科室，Hostpital 医院
        ///</summary>
        public string Type { get; set; }
    }
}
