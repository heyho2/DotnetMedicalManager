using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static GD.Dtos.Decoration.DecorationDto;

namespace GD.Dtos.Decoration
{
    /// <summary>
    /// 创建/修改装修记录请求Dto
    /// </summary>
    public class CreateDecorationRecordRequestDto : BaseDto
    {
        /// <summary>
        /// 若为创建，则不填；若为更新，则必填
        /// </summary>
        public string DecorationGuid { get; set; }

        /// <summary>
        /// 装修记录名称
        /// </summary>
        [Required(ErrorMessage = "名称必填")]
        public string DecorationName { get; set; }

        /// <summary>
        /// 类型guid：创建时必填。专题类型guid=87783a6fb4cc11e986ac00163e0c4296
        /// </summary>
        public string ClassificationGuid { get; set; }

        /// <summary>
        /// 拼图行集合
        /// </summary>
        public List<DecorationRow> Rows { get; set; }
    }
}
