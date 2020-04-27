using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Common
{
    /// <summary>
    /// 请求类
    /// </summary>
    public class SqlPageRequestDto : IBaseOrderBy
    {
        /// <summary>
		/// 当前页码
		/// </summary>
		[Required(ErrorMessage = "{0}必填")]
        [Display(Name = "当前页码")]
        [Range(1, int.MaxValue)]
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 每页记录条数
        /// </summary>
        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "当前页码")]
        [Range(1, int.MaxValue)]
        public int PageSize { get; set; } = 10;
        /// <summary>
        /// sql
        /// </summary>
		[Required(ErrorMessage = "{0}必填")]
        [Display(Name = "sql")]
        public string Sql { get; set; }
        /// <summary>
        /// 字段
        /// </summary>
        public string SortField { get; set; }
        /// <summary>
        /// 方向
        /// </summary>
        public bool IsAscending { get; set; }
    }

    /// <summary>
    /// 请求类
    /// </summary>
    public class SqlPageResponseDto
    {
        /// <summary>
        /// 当前页数据
        /// </summary>
        public IEnumerable<dynamic> CurrentPage { get; set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int Total { get; set; }
        /// <summary>
        /// lie
        /// </summary>
        public string[] Columns { get; set; }
    }
}
