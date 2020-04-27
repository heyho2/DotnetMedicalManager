using GD.Common.Base;

namespace GD.Dtos.Admin.Hospital
{
    /// <summary>
    /// 搜索文章课程 请求
    /// </summary>
    public class SearchOfficeRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string Keyword { get; set; }
    }
    /// <summary>
    /// 搜索文章课程 响应
    /// </summary>
    public class SearchOfficeResponseDto : BasePageResponseDto<SearchOfficeItemDto>
    {

    }
    /// <summary>
    /// 搜索文章课程 项
    /// </summary>
    public class SearchOfficeItemDto : BaseDto
    {
        /// <summary>
        /// 科室id
        /// </summary>
        public string OfficeGuid { get; set; }
        ///<summary>
        ///科室名称
        ///</summary>
        public string OfficeName { get; set; }

        ///<summary>
        ///医院名称
        ///</summary>
        public string HospitalName { get; set; }

        ///<summary>
        ///父级科室名称
        ///</summary>
        public string ParentName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreationDate { get; set; }
    }
}
