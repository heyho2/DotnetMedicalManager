using GD.Common.Base;

namespace GD.Dtos.Hospital
{
    /// <summary>
    /// 获取全部医院（下拉框）
    /// </summary>
    public class GetHospitalAllSelectItemDto : BaseDto
    {
        /// <summary>
        /// Guid
        /// </summary>
        public string Guid { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

    }
}
