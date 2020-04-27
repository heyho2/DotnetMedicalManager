using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.User
{
    /// <summary>
    /// 用户积分
    /// </summary>
    public class UserPointsItemDto
    {
        /// <summary>
        /// UserGuid
        /// </summary>
        public string UserGuid { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public decimal Variation { get; set; }
    }
}
