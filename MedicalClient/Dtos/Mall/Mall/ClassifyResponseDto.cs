using GD.Common.Base;

namespace GD.Dtos.Mall.Mall
{
    /// <inheritdoc />
    /// <summary>
    /// 云购分类-一级
    /// </summary>
    public class ClassifyResponseDto : BaseDto
    {
        /// <summary>
        /// 一级分类ID
        /// </summary>
        public string ClassifyGuid { get; set; }
        /// <summary>
        /// 一级分类ID
        /// </summary>
        public string ClassifyName { get; set; }
        /// <summary>
        /// 一级分类ID
        /// </summary>
        public string ClassifyPic { get; set; }

        ///<summary>
        ///排序
        ///</summary>
        public int Sort { get; set; }
    }
}
