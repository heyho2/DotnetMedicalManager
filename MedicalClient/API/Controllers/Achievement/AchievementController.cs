using Achievement;
using EasyNetQ.Logging;
using GD.Achievement;
using GD.Common;
using GD.Dtos.Achievement;
using GD.Dtos.Payment.HospitalPayment;
using GD.Models.Achievement;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.API.Controllers.Achievement
{
    /// <summary>
    /// 业绩看板控制器
    /// </summary>
    public class AchievementController : AchievementBaseController
    {
        /// <summary>
        /// 添加月业绩目标计划
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateAchievementMonth([FromBody]CreateMonthAchievementDto requestDto)
        {
            DateTime dtR = new DateTime(requestDto.Date.Year, requestDto.Date.Month, 1);
            DateTime dtN = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime dtS = dtN.AddMonths(-4);
            if (dtR < dtS || dtR > dtN)
            {
                return Failed(Common.ErrorCode.UserData, "月份只可选当月及前4个月");
            }
            if (requestDto.MonthGoal <= 0)
            {
                return Failed(Common.ErrorCode.UserData, "目标值必须大于0");
            }
            //校验月份是否存在
            var achievementModel = new AchievementMonthBiz().GetAchievementMonthGoalModel(requestDto, UserID).Result;
            if (achievementModel != null)
            {
                return Failed(Common.ErrorCode.UserData, "此时间的目标已存在");
            }
            var model = new AchievementMonthGoalModel();
            model.GoalGuid = Guid.NewGuid().ToString("N");
            model.HospitalGuid = UserID;
            model.Year = requestDto.Date.Year;
            model.Month = requestDto.Date.Month;
            model.MonthGoal = requestDto.MonthGoal;
            model.CreationDate = DateTime.Now;
            model.LastUpdatedDate = DateTime.Now;
            model.CreatedBy = UserID;
            model.LastUpdatedBy = UserID;
            model.OrgGuid = string.Empty;
            var result = await new AchievementMonthBiz().InsertAsync(model);
            if (result)
            {
                MQNotice();
            }
            return result ? Success() : Failed(ErrorCode.DataBaseError, "添加月目标失败");
        }
        /// <summary>
        /// 修改月目标数据
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UpdateAchievementMonth([FromBody]UpdateMonthAchievementDto requestDto)
        {
            //校验数据是否存在
            var achievementModel = new AchievementMonthBiz().GetAsync(requestDto.GoalGuid).Result;
            if (achievementModel == null)
            {
                return Failed(Common.ErrorCode.UserData, "修改数据不存在");
            }
            if (requestDto.MonthGoal <= 0)
            {
                return Failed(Common.ErrorCode.UserData, "目标值必须大于0");
            }
            achievementModel.MonthGoal = requestDto.MonthGoal;
            achievementModel.LastUpdatedBy = UserID;
            achievementModel.LastUpdatedDate = DateTime.Now;
            var result = await new AchievementMonthBiz().UpdateAsync(achievementModel);
            if (result)
            {
                MQNotice();
            }
            return result ? Success() : Failed(ErrorCode.DataBaseError, "修改月目标失败");
        }
        /// <summary>
        /// 获取所有的月目标数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(ResponseDto<List<GetMonthAchievementDto>>))]
        public async Task<IActionResult> GetMonthAchievementAsync()
        {
            var result = await new AchievementMonthBiz().GetMonthAchievementDtoAsync(UserID);
            return Success(result);
        }
        /// <summary>
        /// 根据月目标查询每天的数据
        /// </summary>
        /// <param name="goalGuid"></param>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(ResponseDto<GetAchievementDto>))]
        public async Task<IActionResult> GetAchievementAsync(string goalGuid)
        {
            GetAchievementDto response = new GetAchievementDto();
            var mothAchievement = await new AchievementMonthBiz().GetAsync(goalGuid);
            if (mothAchievement == null)
            {
                return Failed(ErrorCode.UserData, "未找到指定月份数据");
            }
            response.DataSource = new DataSource();
            response.DataSource.MonthGoal = mothAchievement.MonthGoal;
            var result = await new AchievementMonthBiz().GetAchievementDtoAsync(goalGuid);
            if (result != null)
            {
                decimal tComplete = result.Select(s => s.TodayComplete).Sum();
                response.DataSource.TotalComplete = tComplete;
                if (response.DataSource.MonthGoal != 0)
                {
                    response.DataSource.MonthScale = Math.Round((tComplete / response.DataSource.MonthGoal) * 100, 2, MidpointRounding.AwayFromZero);
                }
                response.AchievementDayList = result.Select(s => new AchievementDayDto
                {
                    AchievementGuid = s.AchievementGuid,
                    AchievementDate = s.AchievementDate,
                    TodayGoal = s.TodayGoal,
                    TodayComplete = s.TodayComplete,
                    DayScale = s.TodayGoal != 0 ? Math.Round((s.TodayComplete / s.TodayGoal) * 100, 2, MidpointRounding.AwayFromZero) : 0
                }).ToList();
            }
            return Success(response);
        }
        /// <summary>
        /// 添加日业绩
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateAchievementDay([FromBody]CreateDayAchievementDto requestDto)
        {
            if (requestDto.Date.Date > DateTime.Now.Date)
            {
                return Failed(Common.ErrorCode.UserData, "只能上传今日及过去的数据");
            }
            if (requestDto.DayGoal <= 0)
            {
                return Failed(Common.ErrorCode.UserData, "目标值必须大于0");
            }
            CreateMonthAchievementDto request = new CreateMonthAchievementDto { Date = requestDto.Date };
            //校验月份是否存在
            var achievementMonthModel = new AchievementMonthBiz().GetAchievementMonthGoalModel(request, UserID).Result;
            if (achievementMonthModel == null)
            {
                return Failed(Common.ErrorCode.UserData, "此月份的业绩目标不存在");
            }
            //校验当天业绩数据是否上传过
            var achievementModel = await new AchievementBiz().GetAchievementModel(requestDto.Date, achievementMonthModel.GoalGuid);
            if (achievementModel != null)
            {
                return Failed(Common.ErrorCode.UserData, "当天的业绩目标已存在");
            }
            var model = new AchievementModel();
            model.AchievementGuid = Guid.NewGuid().ToString("N");
            model.GoalGuid = achievementMonthModel.GoalGuid;
            model.AchievementDate = requestDto.Date;
            model.TodayGoal = requestDto.DayGoal;
            model.TodayComplete = requestDto.DayCompleteGoal;
            model.CreationDate = DateTime.Now;
            model.LastUpdatedDate = DateTime.Now;
            model.LastUpdatedBy = UserID;
            model.CreatedBy = UserID;
            model.OrgGuid = string.Empty;
            var result = await new AchievementBiz().InsertAsync(model);
            if (result)
            {
                MQNotice();
            }
            return result ? Success() : Failed(ErrorCode.DataBaseError, "添加日业绩目标失败");
        }
        /// <summary>
        /// 修改日业绩目标
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UpdateAchievementDay([FromBody]UpdateDayAchievementDto requestDto)
        {
            //校验数据是否存在
            var achievementModel = await new AchievementBiz().GetAsync(requestDto.AchievementGuid);
            if (achievementModel == null)
            {
                return Failed(Common.ErrorCode.UserData, "修改数据不存在");
            }
            if (requestDto.TodayGoal <= 0)
            {
                return Failed(Common.ErrorCode.UserData, "目标值必须大于0");
            }
            achievementModel.TodayGoal = requestDto.TodayGoal;
            achievementModel.TodayComplete = requestDto.TodayCompleteGoal;
            achievementModel.LastUpdatedBy = UserID;
            achievementModel.LastUpdatedDate = DateTime.Now;
            var result = await new AchievementBiz().UpdateAsync(achievementModel);
            if (result)
            {
                MQNotice();
            }
            return result ? Success() : Failed(ErrorCode.DataBaseError, "修改日业绩目标失败");
        }
        /// <summary>
        /// 通知刷新界面
        /// </summary>
        private void MQNotice()
        {
            try
            {
                var bus = Communication.MQ.Client.CreateConnection();
                var advancedBus = bus.Advanced;
                if (advancedBus.IsConnected)
                {
                    var exchange = advancedBus.ExchangeDeclare("HospitalDataBoardExchange", "fanout");
                    var queue = advancedBus.QueueDeclare("HospitalDataBoardQueue");
                    advancedBus.Bind(exchange, queue, "hospitalDataBoard");
                    advancedBus.Publish(exchange, "", false, new EasyNetQ.Message<HospitalDataBoardNotificationMsg>(
                        new HospitalDataBoardNotificationMsg
                        {
                            NotificationType = 2
                        }));
                }
                bus.Dispose();
            }
            catch (Exception ex)
            {
                GD.Common.Helper.Logger.Error($"业绩上传变更通知失败：{ex.Message}{Environment.NewLine} at AchievementController.MQNotice");
            }
        }
    }
}
