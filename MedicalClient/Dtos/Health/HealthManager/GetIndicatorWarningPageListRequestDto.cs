using GD.Common.Base;
using GD.Dtos.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Health.HealthManager
{
    /// <summary>
    /// 获取健康指标预警分页记录请求dto,
    /// 目前只支持当前登录用户归属的数据
    /// </summary>
    public class GetIndicatorWarningPageListRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 搜索关键字：姓名和手机号模糊搜索
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 预警状态：传null或不传表示全部
        /// </summary>
        public IndicatorWarningStatusEnum? Status { get; set; }
    }

    /// <summary>
    /// 获取健康指标预警分页记录响应
    /// </summary>
    public class GetIndicatorWarningPageListResponseDto : BasePageResponseDto<GetIndicatorWarningPageListItemDto>
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public class GetIndicatorWarningPageListItemDto : BaseDto
    {
        /// <summary>
        /// 预警记录Id
        /// </summary>
        public string WarningGuid { get; set; }

        /// <summary>
        /// 会员姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// 预警日期
        /// </summary>
        public DateTime WarningDate { get; set; }

        /// <summary>
        /// 会员手机号
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 会员头像
        /// </summary>
        public string Portrait { get; set; }

        /// <summary>
        /// 预警描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 预警状态：1:待处理（Pending），2：已失效(Expired)，3：已关闭(Closed)
        /// </summary>
        public IndicatorWarningStatusEnum Status { get; set; }
    }
}
