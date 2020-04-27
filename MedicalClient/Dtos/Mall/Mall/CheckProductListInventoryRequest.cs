using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Mall.Mall
{
    /// <summary>
    /// 批量检测库存
    /// </summary>
    public class CheckProductListInventoryRequest
    {

        /// <summary>
        /// 产品信息列表
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "产品信息列表")]
        public List<CheckProductinventory> CheckProductinventoryList { get; set; }

        /// <summary>
        /// 产品数量类
        /// </summary>
        public class CheckProductinventory
        {
            /// <summary>
            /// 产品Guid
            /// </summary>
            [Required(ErrorMessage = "{0}必填"), Display(Name = "产品Guid")]
            public string ProductGuid { get; set; }

            

            /// <summary>
            /// 数量
            /// </summary>
            [Required(ErrorMessage = "{0}必填"), Display(Name = "数量")]
            public int Num { get; set; }
        }
    }

    /// <summary>
    /// response
    /// </summary>
    public class CheckProductListInventoryResponse
    {
        /// <summary>
        /// 产品Guid
        /// </summary>
        public string ProductGuid { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// 状态是否可提交
        /// </summary>
        public bool IsRightStatus { get; set; }

        /// <summary>
        /// 检测信息
        /// </summary>
        public string Message { get; set; }
    }


}
