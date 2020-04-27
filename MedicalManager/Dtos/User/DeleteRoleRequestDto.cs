using GD.Common.Base;

namespace GD.Dtos.User
{
    /// <summary>
    /// 删除角色 请求
    /// </summary>
    public class DeleteRoleRequestDto : BaseDto
    {
        /// <summary>
        /// guid
        /// </summary>
        public string OrgGuid { get; set; }
    }
}
