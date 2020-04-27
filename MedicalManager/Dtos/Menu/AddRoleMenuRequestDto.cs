using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Menu
{
    /// <summary>
    /// 保存权限 请求
    /// </summary>
    public class AddRoleMenuRequestDto
    {
        /// <summary>
        /// 权限id 结果集（Menuid,或者buttonid）
        /// </summary>
        [Required]
        public string[] RightGuids { get; set; }
        /// <summary>
        /// 角色id
        /// </summary>
        [Required]
        public string RoleGuid { get; set; }
    }
    
}
