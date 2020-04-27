using GD.Dtos.Common;

namespace GD.Dtos.Hospital
{
    /// <summary>
    /// 获取科室列表
    /// </summary>
    public class GetOfficeTreeItemDto : BaseTreeDto<GetOfficeTreeItemDto>
    {
        ///<summary>
        ///科室名称
        ///</summary>
        public string OfficeName { get; set; }
        ///<summary>
        ///上级科室
        ///</summary>
        public string ParentName { get; set; }
        /// <summary>
        /// 顺序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }
        /// <summary>
        /// 科室图片guid
        /// </summary>
        public string PictureGuid { get; set; }
    }
}
