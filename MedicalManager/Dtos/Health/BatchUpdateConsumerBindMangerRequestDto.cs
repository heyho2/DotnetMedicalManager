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
        /// 
        /// </summary>
        [Required(ErrorMessage = "所选健康管理师参数未提供")]
        public string ManagerGuid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> ConsumerGuids { get; set; } = new List<string>();
    }
}
