using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Course
{
    /// <summary>
    /// 禁用课程
    /// </summary>
    public class DisableEnableCourseRequestDto : BaseDto
    {
        /// <summary>
        /// id
        /// </summary>
        public string Guid { get; set; }
        /// <summary>
        /// Enable
        /// </summary>
        public bool Enable { get; set; }
    }
}
