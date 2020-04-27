using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Health
{
    /// <summary>
    /// 
    /// </summary>
    public class BatchUpdateConsumerBindMangerRequestDto
    {
        /// <summary>
        /// 前端不需要传
        /// </summary>
        public string ManagerGuid { get; set; }
        /// <summary>
        /// 绑定用户对象
        /// </summary>
        public List<string> ConsumerGuids { get; set; } = new List<string>();
    }
}
