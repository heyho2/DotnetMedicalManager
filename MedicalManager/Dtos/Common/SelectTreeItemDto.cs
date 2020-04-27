namespace GD.Dtos.Common
{
    /// <summary>
    /// 下拉树
    /// </summary>
    public class SelectTreeItemDto : BaseTreeDto<SelectTreeItemDto>
    {
        /// <summary>   
        /// guid
        /// </summary>
        public string Guid { get; set; }
        /// <summary>
        /// name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// code
        /// </summary>
        public string Code { get; set; }

    }
}
