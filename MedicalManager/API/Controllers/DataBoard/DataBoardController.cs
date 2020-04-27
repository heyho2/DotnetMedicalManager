using GD.Common;
using GD.Dtos.DataBoard;
using GD.Manager.DataBoard;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.API.Controllers.DataBoard
{
    /// <summary>
    /// 数据营收看板
    /// </summary>
    public class DataBoardController : DataBoardBaseController
    {
        /// <summary>
        /// 获取当天营收总金额
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(ResponseDto<GetTodayTotalResponseDto>))]
        public async Task<IActionResult> GetTodayTotalAsync()
        {
            var result = await new DataBoardBiz().GetTodayTotalAsync(DateTime.Now.Date, DateTime.Now.Date.AddDays(1).AddSeconds(-1));
            return Success(result);
        }
        /// <summary>
        /// 微信扫码收款情况
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(ResponseDto<GetWeChatDetailResponseDto>))]
        public async Task<IActionResult> GetWeChatDetailAsync()
        {
            var result = await new DataBoardBiz().GetWeChatDetailAsync();
            return Success(result);
        }
        /// <summary>
        /// 今日微信扫码收款情况
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(ResponseDto<GetDayWeChatDetailResponseDto>))]
        public async Task<IActionResult> GetDayWeChatDetailAsync()
        {
            GetDayWeChatDetailAndScaleResponseDto result = new GetDayWeChatDetailAndScaleResponseDto();
            var resultDetail = await new DataBoardBiz().GetDayWeChatDetailAsync();
            if (resultDetail != null)
            {
                result.HospitalList = resultDetail.Select(s => new KeyValuePair<string, string>(s.HospitalGuid, s.HospitalName)).ToList();
                result.DayWeChatDetailList = resultDetail;
            }
            return Success(result);
        }
        /// <summary>
        /// 单日医院业绩占比
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(ResponseDto<GetDayWeChatDetailAndtAchievementResponseDto>))]
        public async Task<IActionResult> GetDayWeChatScaleAsync()
        {
            GetDayWeChatDetailAndtAchievementResponseDto result = new GetDayWeChatDetailAndtAchievementResponseDto();
            (List<GetDayWeChatDetailResponseDto> resultDetail, string date) = await new DataBoardBiz().GetDayWeChatAchievementAsync();
            if (resultDetail != null)
            {
                result.HospitalList = resultDetail.Select(s => new KeyValuePair<string, string>(s.HospitalGuid, s.HospitalName)).Distinct().ToList();
                result.DayWeChatDetailList = resultDetail;
                if (!string.IsNullOrEmpty(date))
                {
                    result.UpdateDate = Convert.ToDateTime(date);
                }
            }
            return Success(result);
        }
        /// <summary>
        /// 医院业绩日报(元)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(ResponseDto<GetDayDayAchievementResponseDto>))]
        public async Task<IActionResult> GetDayAchievementAsync()
        {
            GetDayDayAchievementResponseDto result = new GetDayDayAchievementResponseDto();
            (List<GetDayAchievementDetailResponseDto> resultDetail, string date) = await new DataBoardBiz().GetDayAchievementAsync();
            if (resultDetail != null)
            {
                result.HospitalList = resultDetail.Select(s => new KeyValuePair<string, string>(s.HospitalGuid, s.HospitalName)).Distinct().ToList();
                result.DayAchievementDetailList = resultDetail;
                if (!string.IsNullOrEmpty(date))
                {
                    result.UpdateDate = Convert.ToDateTime(date);
                }
            }
            return Success(result);
        }
        /// <summary>
        /// 昨日医院支付笔数占比
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(ResponseDto<GetLastDayAchievementResponseDto>))]
        public async Task<IActionResult> GetLastDayWeChatPayCountAsync()
        {
            GetLastDayAchievementResponseDto result = new GetLastDayAchievementResponseDto();
            var resultDetail = await new DataBoardBiz().GetLastDayWeChatPayDetailAsync();
            if (resultDetail != null)
            {
                result.HospitalList = resultDetail.Select(s => new KeyValuePair<string, string>(s.HospitalGuid, s.HospitalName)).ToList();
                result.LastDayPayDetailList = resultDetail;
                result.UpdateDate = DateTime.Now.Date.AddDays(-1);
            }
            return Success(result);
        }
        /// <summary>
        /// 医院本月累计已完成
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(ResponseDto<List<GetMonthDetailResponseDto>>))]
        public async Task<IActionResult> GetMonthDetailAsync()
        {
            var result = await new DataBoardBiz().GetMonthDetailAsync();
            if (result != null)
            {
                foreach (var item in result)
                {
                    item.Month = DateTime.Now.Month.ToString();
                }
            }
            return Success(result);
        }
        /// <summary>
        /// 医院业绩趋势统计
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(ResponseDto<List<GetTrendResponseDto>>))]
        public async Task<IActionResult> GetTrendAsync()
        {
            var result = await new DataBoardBiz().GetMonthTrendAsync();
            return Success(result);
        }
    }
}
