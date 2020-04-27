using GD.Common;
using GD.Common.EnumDefine;
using GD.Consumer;
using GD.Dtos.FAQs.FAQsIntergral;
using GD.Dtos.Utility.Utility;
using GD.Manager;
using GD.Models.Consumer;
using GD.Models.Manager;
using GD.Models.Utility;
using GD.Module;
using GD.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GD.Common.Helper;

namespace GD.API.Controllers.FAQs
{
    /// <summary>
    /// 问答系统-积分相关
    /// </summary>
    public class FAQsIntergralController : FAQsBaseController
    {
        /// <summary>
        /// 注册送积分（注册成功后，调用该接口）
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto))]
        public IActionResult RegisterSendIntergral(string Phone, UserType userType = UserType.Consumer)
        {
            //注册送积分 判断注册日期是否大于一天
            var userModel = new ConsumerBiz().GetModelByPhone(Phone).Result;
            if (userModel == null)
            {
                return Success();
                //return Failed(ErrorCode.DataBaseError, "找不到该用户信息，请检查！");
            }
            var scoreModel = new ScoreExBiz().GetIntergralRecordByCondition(UserID, SendIntergralEnum.注冊送积分.ToString()).Result;
            if (scoreModel != null)
            {
                return Success();
                //return Failed(ErrorCode.DataBaseError, "注册已送积分，该次无法赠送积分！");
            }
            TimeSpan interval = DateTime.Now - userModel.CreationDate;
            var isRegisteRightNow = interval.TotalHours < 24;
            if (!isRegisteRightNow)
            {
                return Success();
                //return Failed(ErrorCode.UserData, "用户注册时间过久，注册送积分失败，请联系管理员！");
            }
            if (!InsertIntergral(150, SendIntergralEnum.注冊送积分, userType))
            {
                Common.Helper.Logger.Error($"注册送积分失败-RegisterSendIntergral({Phone})");
            }
            return Success();
            //var isSucc = InsertIntergral(150, SendIntergralEnum.注冊送积分, userType);
            //return isSucc ? Success() : Failed(ErrorCode.DataBaseError, "注册送积分失败！");
        }
        /// <summary>
        /// 判断是否签到
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<bool>))]
        public IActionResult CheckSignIn(UserType userType = UserType.Consumer)
        {
            var scoreBiz = new ScoreExBiz();
            var result = scoreBiz.CheckInSendIntergral(UserID, userType.ToString());
            return Success(result);
        }

        #region 另一种需求实现
        ///// <summary>
        ///// 获取该用户该月签到情况
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet, Produces(typeof(ResponseDto<GetUserSignInDataCurrentMonthResponse>)), AllowAnonymous]
        //public async Task<IActionResult> GetUserSignInDataCurrentMonthAsync(GetUserSignInDataCurrentMonthRequest request)
        //{
        //    // 显示已连续签到多少天  同一个接口  
        //    // 显示明日签到可获取积分 同一个接口 
        //    if (string.IsNullOrWhiteSpace(request.UserID))
        //    {
        //        request.UserID = UserID;
        //    }
        //    var scoreBiz = new ScoreExBiz();
        //    //var scoreModelList = await scoreBiz.GetUserSignInDataCurrentMonth(request);
        //    var firstDateThisMonth = DateTime.Now.AddDays(1 - DateTime.Now.Day).Date;
        //    var lastDateThisMonth = DateTime.Now.AddDays(1 - DateTime.Now.Day).Date.AddMonths(1).AddSeconds(-1);
        //    var scoreModelList = await scoreBiz.GetUserSignInDataCurrentMonthAllDays(request, CreateDateSql(firstDateThisMonth, lastDateThisMonth));
        //    GetUserSignInDataCurrentMonthResponse response = new GetUserSignInDataCurrentMonthResponse();
        //    var checkInInfoList = new List<GetUserSignInDataCurrentMonthResponse.CheckInInfo>();
        //    foreach (var item in scoreModelList)
        //    {
        //        var model = new GetUserSignInDataCurrentMonthResponse.CheckInInfo
        //        {
        //            CheckInDate = item.CheckInDate,
        //            IsCheckIn = true,
        //            Intergral = item.Intergral
        //        };
        //        checkInInfoList.Add(model);
        //    }
        //    response.CheckInInfoList = checkInInfoList;
        //    //最后签到时间
        //    var recordModel = await scoreBiz.GetCheckInSendIntergralRecord(request.UserID, SendIntergralEnum.连续签到送积分);
        //    TimeSpan interval = DateTime.Now.Date - recordModel.CreationDate.Date;

        //    response.TomorrowCheckInIntergral = interval.Days == 0 ? recordModel.Variation < 7 ? recordModel.Variation + 1 : 7 : 1;
        //    response.ContinuousCheckInDays = interval.Days == 0 ? recordModel.Variation < 7 ? recordModel.Variation : 7 : 1;
        //    if (response.ContinuousCheckInDays.Equals(7))
        //    {
        //        var orderByList = scoreModelList.OrderByDescending(a => a.CheckInDate).Select(a => a.Intergral = 7);
        //        response.ContinuousCheckInDays = orderByList.ToList().Count + 6;
        //    }
        //    return Success(response);
        //}
        #endregion

        /// <summary>
        /// 获取该用户该月签到情况
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetUserSignInDataCurrentMonthResponse>))]//,AllowAnonymous
        public async Task<IActionResult> GetUserSignInDataCurrentMonthAsync(GetUserSignInDataCurrentMonthRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.UserID))
            {
                request.UserID = UserID;
            }
            var scoreBiz = new ScoreExBiz();
            GetUserSignInDataCurrentMonthResponse response = new GetUserSignInDataCurrentMonthResponse();
            var recordModel = await scoreBiz.GetCheckInSendIntergralRecord(request.UserID, SendIntergralEnum.连续签到送积分.ToString(), request.UserType);
            if (recordModel != null)
            {
                TimeSpan interval = DateTime.Now.Date - recordModel.CreationDate.Date;
                if (interval.Days.Equals(1) || interval.Days.Equals(0))
                {
                    var checkInInfoList = new List<GetUserSignInDataCurrentMonthResponse.CheckInInfo>();
                    if (recordModel.Variation < 7)
                    {
                        var modelListLessThan7 = await scoreBiz.GetModelListByCondition(request.UserID, request.UserType.ToString(), recordModel.Variation);
                        return Success(GetResponseData(modelListLessThan7));
                    }
                    var modelList = await scoreBiz.GetModelListByCondition(request.UserID, request.UserType.ToString(), 30);
                    var conditionModel = modelList.Take(7).Where(a => a.Variation.Equals(6)).ToList().Count == 0 ? modelList.Take(14).Where(a => a.Variation.Equals(6)).ToList().Count == 0 ? modelList.Take(21).Where(a => a.Variation.Equals(6)).ToList().Count == 0 ? modelList.Take(28).Where(a => a.Variation.Equals(6)).ToList().Count == 0 ? modelList.Where(a => a.Variation.Equals(6)).ToList() : modelList.Take(28).Where(a => a.Variation.Equals(6)).ToList() : modelList.Take(21).Where(a => a.Variation.Equals(6)).ToList() : modelList.Take(14).Where(a => a.Variation.Equals(6)).ToList() : modelList.Take(7).Where(a => a.Variation.Equals(6)).ToList();
                    if (conditionModel.Count != 1) return Failed(ErrorCode.DataBaseError, "数据有误，请联系管理员！");
                    var dataIndex = modelList.FindIndex(a => a.ScoreGuid.Equals(conditionModel[0].ScoreGuid));
                    var realModelList = modelList.Take(dataIndex + 6).ToList();
                    return Success(GetResponseData(realModelList));
                }
            }
            //重新签到 
            var newModelList = new List<ScoreModel>();
            return Success(GetResponseData(newModelList));
        }
        /// <summary>
        /// 组装
        /// </summary>
        /// <param name="modelList">有效的签到</param>
        /// <returns></returns>
        private GetUserSignInDataCurrentMonthResponse GetResponseData(List<ScoreModel> modelList)
        {
            GetUserSignInDataCurrentMonthResponse response = new GetUserSignInDataCurrentMonthResponse();
            var checkInInfoList = new List<GetUserSignInDataCurrentMonthResponse.CheckInInfo>();
            var orderByModelList = modelList.OrderBy(a => a.CreationDate);
            foreach (var item in orderByModelList)
            {
                var model = new GetUserSignInDataCurrentMonthResponse.CheckInInfo
                {
                    CheckInDate = item.CreationDate,
                    IsCheckIn = true,
                    Intergral = item.Variation
                };
                checkInInfoList.Add(model);
            }
            for (int i = 0; i < 30 - modelList.Count; i++)
            {
                var model = new GetUserSignInDataCurrentMonthResponse.CheckInInfo();
                if (modelList.Count > 7)
                {
                    model.CheckInDate = modelList[0].CreationDate.AddDays(i + 1);
                    model.IsCheckIn = false;
                    model.Intergral = 7;
                }
                else
                {
                    model.CheckInDate = modelList.Count == 0 ? DateTime.Now.AddDays(i) : modelList[0].CreationDate.AddDays(i + 1);
                    model.IsCheckIn = false;
                    model.Intergral = modelList.Count + 1 + i > 6 ? 7 : modelList.Count + 1 + i;
                }
                checkInInfoList.Add(model);
            }
            response.CheckInInfoList = checkInInfoList;
            response.ContinuousCheckInDays = modelList.Count;
            response.NextTimeCheckInIntergral = modelList.Count > 7 ? modelList.Count == 30 ? 1 : 7 : modelList.Count + 1;
            return response;
        }

        /// <summary>
        /// 生成日期
        /// </summary>
        /// <returns></returns>
        private string CreateDateSql(DateTime startTime, DateTime endTime)
        {
            List<string> dateSql = new List<string>();
            DateTime date = startTime;
            while (date < endTime)
            {
                dateSql.Add($" select '{date.ToString("yyyy-MM-dd")}' AS CheckInDate ");
                date = date.AddDays(1);
            }
            return string.Join(" UNION ALL ", dateSql);
        }

        /// <summary>
        /// 签到积分 
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto))]//, AllowAnonymous
        public async Task<IActionResult> CheckInSendIntergral(string userID, UserType userType = UserType.Consumer)
        {
            if (string.IsNullOrWhiteSpace(userID))
            {
                userID = UserID;
            }
            var scoreBiz = new ScoreExBiz();
            if (scoreBiz.CheckInSendIntergral(UserID, userType.ToString()))
            {
                return Failed(ErrorCode.SystemException, "今日已签到");
            }
            var theRightIntergral = 0;
            //连续签到 判断 一个月为期，首日签到1积分，连续签到递增1积分，
            //连续签到上限为7积分，连续签到8,9,10天均为7个积分，若中断则重新计算,一个月最多得189分
            var recordModel = await scoreBiz.GetCheckInSendIntergralRecord(UserID, SendIntergralEnum.连续签到送积分.ToString(), userType);
            if (recordModel != null && (DateTime.Now.Date - recordModel.CreationDate.Date).Days.Equals(1))
            {
                //满勤的情况
                var modelList = await scoreBiz.GetModelListByCondition(UserID, userType.ToString(), 30);
                theRightIntergral = recordModel.Variation < 7 ? recordModel.Variation + 1 : modelList.Where(a => a.Variation.Equals(7)).ToList().Count > 23 && modelList.Where(a => a.Variation.Equals(1)).ToList().Count == 1 && modelList[modelList.Count - 1].Variation == 1 ? 1 : 7;
            }
            else
            {
                theRightIntergral = 1;
            }
            var isSucc = InsertIntergral(theRightIntergral, SendIntergralEnum.连续签到送积分, userType);
            return isSucc ? Success() : Failed(ErrorCode.DataBaseError, "签到积分插入错误");
        }

        /// <summary>
        /// 浏览文章送积分
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> ViewArticleSendIntergral(ViewArticleSendIntergralRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.UserID))
            {
                request.UserID = UserID;
            }
            //每天浏览5篇文章即可得15积分，每篇3分，15分为上限
            var recordModelList = await new ScoreExBiz().GetToDaysIntergralRecordByCondition(UserID, SendIntergralEnum.浏览文章送积分.ToString());
            if (recordModelList == null || recordModelList.Count < 5)
            {
                if (!InsertIntergral(3, SendIntergralEnum.浏览文章送积分, request.UserType))
                {
                    Logger.Error($"浏览文章送积分失败-{JsonConvert.SerializeObject(request)}");
                }
            }
            //else
            //{
            //    //var totalIntergral = recordModelList.Select(a => a.Variation).Sum()>15;
            //    return Failed(ErrorCode.DataBaseError, "今日浏览文章送积分次数已满！");
            //}
            return Success();
        }

        /// <summary>
        /// 分享送积分
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> ShareToSendIntergral(UserType userType = UserType.Consumer)
        {
            var scoreBiz = new ScoreExBiz();
            //每天分享内容1次即可获5积分，每天30分为上限
            var recordModelList = await scoreBiz.GetToDaysIntergralRecordByCondition(UserID, SendIntergralEnum.分享送积分.ToString());
            if (recordModelList == null || recordModelList.Count < 6)
            {
                if (!InsertIntergral(5, SendIntergralEnum.分享送积分, userType))
                {
                    Logger.Error($"分享送积分失败-{JsonConvert.SerializeObject(new { UserId = UserID })}");
                }
            }
            //else
            //{
            //    //var totalIntergral = recordModelList.Select(a => a.Variation).Sum()>30;
            //    return Failed(ErrorCode.DataBaseError, "今日分享送积分次数已满！");
            //}
            return Success();

        }

        /// <summary>
        /// 浏览问题送积分
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> ViewQuestionToSendIntergral(ViewQuestionToSendIntergralRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.UserID))
            {
                request.UserID = UserID;
            }
            var isExist = await new ArticleViewBiz().IsExistThisTargetIDRecord(request.QuestionGuid, request.UserID);
            if (isExist)
            {
                //Common.Helper.Logger.Debug($"该文章或问题今日已浏览过，送积分失败！{JsonConvert.SerializeObject(request)}");
                return Success(); //Failed(ErrorCode.DataBaseError, "该文章或问题今日已浏览过，送积分失败！");
            }

            //浏览每个问题可得3积分，每天9分上限
            var recordModelList = await new ScoreExBiz().GetToDaysIntergralRecordByCondition(UserID, SendIntergralEnum.浏览问题送积分.ToString());
            if (recordModelList == null || recordModelList.Count < 3)
            {
                if (!InsertIntergral(3, SendIntergralEnum.浏览问题送积分, request.UserType))
                    Logger.Error($"浏览问题送积分失败！{JsonConvert.SerializeObject(request)}");
            }
            //else
            //{
            //    Common.Helper.Logger.Debug($"今日浏览问题送积分次数已满！{JsonConvert.SerializeObject(request)}");

            //}
            return Success();
        }

        /// <summary>
        /// 收藏文章或问题
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> CollectArticleOrQuestionSendIntergral(CollectArticleOrQuestionSendIntergralRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.UserID))
            {
                request.UserID = UserID;
            }
            var isExist = new CollectionBiz().IsCollectTheTarget(request.UserID, request.TargetGuid);
            if (!isExist)
            {
                return Failed(ErrorCode.DataBaseError, "该文章或问题还未收藏！");
            }
            var scoreBiz = new ScoreExBiz();
            // 收藏1个问题可得3积分，每天9积分为上限
            var recordModelList = await scoreBiz.GetToDaysIntergralRecordByCondition(request.UserID, SendIntergralEnum.收藏文章或问题送积分.ToString());
            if (recordModelList == null || recordModelList.Count < 3)
            {
                if (!InsertIntergral(3, SendIntergralEnum.收藏文章或问题送积分, request.UserType))
                    Logger.Error($"收藏文章或问题送积分失败！{JsonConvert.SerializeObject(request)}");
            }
            //else
            //{
            //    //var totalIntergral = recordModelList.Select(a => a.Variation).Sum()>9;
            //    return Failed(ErrorCode.DataBaseError, "今日收藏文章或问题送积分次数已满！");
            //}
            return Success();
        }

        /// <summary>
        /// 提问问题
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> AskQuestionSendIntergral(UserType userType = UserType.Consumer)
        {
            // 每提问1个问题，获得20积分,每天前三个问题赠送积分
            var recordModelList = await new ScoreExBiz().GetToDaysIntergralRecordByCondition(UserID, SendIntergralEnum.提问问题送积分.ToString());
            if (recordModelList == null || recordModelList.Count < 3)
            {
                if (!InsertIntergral(20, SendIntergralEnum.提问问题送积分, userType))
                    Logger.Error($"提问问题送积分失败！{JsonConvert.SerializeObject(new { UserId = UserID })}");
            }
            //else
            //{
            //    //var totalIntergral = recordModelList.Select(a => a.Variation).Sum()>60;
            //    return Failed(ErrorCode.DataBaseError, "今日提问问题送积分次数已满！");
            //}
            return Success();
        }

        /// <summary>
        /// 推荐好友关注
        /// </summary>
        /// <returns></returns>
        //[HttpGet, Produces(typeof(ResponseDto))] //该接口已有，
        //public async Task<IActionResult> IntroduceFriendToAttentionSendIntergral(UserType userType = UserType.Consumer)
        //{
        //    var scoreBiz = new ScoreExBiz();
        //    // 推荐一位好友关注获得50积分
        //    var isSucc = false;
        //    var recordModelList = await scoreBiz.GetToDaysIntergralRecordByCondition(UserID, SendIntergralEnum.推荐好友关注.ToString());
        //    if (recordModelList == null || recordModelList.Count < 3)
        //    {
        //        isSucc = InsertIntergral(50, SendIntergralEnum.IntroduceFriendToAttentionSendIntergral, userType);
        //    }
        //    return isSucc ? Success() : Failed(ErrorCode.DataBaseError, "推荐好友关注送积分失败！");
        //}

        /// <summary>
        /// 问医评价送积分
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto))]
        public IActionResult EvaluateDoctorToSendIntergral(EvaluateDoctorToSendIntergralRequest request)
        {
            var commentModel = new CommentBiz().GetModel(request.CommentGuid);
            if (commentModel == null)
            {
                return Success();//Failed(ErrorCode.DataBaseError, "评论Guid查不到信息！");
            }
            //一小时内
            TimeSpan interval = DateTime.Now - commentModel.CreationDate;
            var isRegisteRightNow = interval.TotalHours < 1;
            if (!isRegisteRightNow)
            {
                return Success();//return Failed(ErrorCode.UserData, "评论时间过久，送积分失败，请联系管理员！");
            }
            //每次评价获10积分
            if (!(commentModel.Enable ? InsertIntergral(10, SendIntergralEnum.问医评价送积分, request.UserType) : false))
            {
                Logger.Error($"问医评价送积分失败！{JsonConvert.SerializeObject(request)}");
            }

            return Success();
            // return isSucc ? Success() : Failed(ErrorCode.DataBaseError, "问医评价送积分失败！");
        }

        /// <summary>
        /// 关注医生送积分
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto))]
        public IActionResult AttentionToDoctorSendIntergral(AttentionToDoctorSendIntergralRequest request)
        {
            var collectionModel = new CollectionBiz().GetTheModelByUserId(UserID, request.DoctorGuid);
            if (collectionModel == null)
            {
                return Success();//Failed(ErrorCode.DataBaseError, "该医生还未被关注，guid查不到信息！");
            }
            //12小时内
            //TimeSpan interval = DateTime.Now - collectionModel.CreationDate;
            //var isRegisteRightNow = interval.TotalHours < 12;
            //if (!isRegisteRightNow)
            //{
            //    return Failed(ErrorCode.UserData, "收藏时间过久，送积分失败，请联系管理员！");
            //}
            //关注1位医生，获10积分
            if (!InsertIntergral(10, SendIntergralEnum.关注医生送积分, request.UserType))
            {
                Logger.Error($"关注医生送积分失败！{JsonConvert.SerializeObject(request)}");
            }
            return Success();
        }

        /// <summary>
        /// 取消关注医生减扣积分
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto))]
        public IActionResult CancelAttentionToDoctorReduceIntergral(CancelAttentionToDoctorReduceIntergralRequest request)
        {
            var collectionBiz = new CollectionBiz();
            var collectionModel = collectionBiz.GetTheModelByUserId(UserID, request.DoctorGuid);
            if (collectionModel != null)
            {
                return Success();//return Failed(ErrorCode.DataBaseError, "该收藏没有取消，取消失败！");
            }
            if (new ScoreExBiz().GetTotalScore(UserID, request.UserType).Result > 10)
            {
                if (!InsertIntergral(-10, SendIntergralEnum.取消关注医生减扣积分, request.UserType))
                {
                    Logger.Error($"取消关注医生减扣积分失败！{JsonConvert.SerializeObject(request)}");
                }
            }

            //12小时内不能取消
            //TimeSpan interval = DateTime.Now - collectionModel.CreationDate;
            //var isRegisteRightNow = interval.TotalHours < 12;
            //if (isRegisteRightNow)
            //{
            //    return Failed(ErrorCode.UserData, "收藏12小时内不能取消，请联系管理员！");
            //}
            //collectionModel.Enable = false;
            //var isSuccess = collectionBiz.UpdateModel(collectionModel);
            //关注1位医生，获10积分
            //取消关注，减扣10积分

            return Success();// : Failed(ErrorCode.DataBaseError, "取消关注医生减扣积分失败！");
        }


        /// <summary>
        /// 完善个人信息送积分
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto))]
        public IActionResult ImprovePersonalInformationToSendIntergral(UserType userType = UserType.Consumer)
        {
            var scoreModel = new ScoreExBiz().GetIntergralRecordByCondition(UserID, SendIntergralEnum.完善个人信息送积分.ToString()).Result;
            if (scoreModel != null)
            {
                return Success();//Failed(ErrorCode.Empty, "完善个人信息积分已送，该次无法赠送积分！");
            }

            var uModel = new UserBiz().GetUser(UserID);
            if (uModel == null || !uModel.Enable) return Success();//Failed(ErrorCode.DataBaseError, "用户状态不可用。");
            var accModel = new AccessoryBiz().GetAccessoryModelByGuid(uModel.PortraitGuid);
            var outDto = new GetUserInfoResponseDto
            {
                Portrait = $"{accModel?.BasePath}{accModel?.RelativePath}", // +"/" 格式确认,
                NickName = uModel.NickName,
                Gender = uModel.Gender,
                Birthday = uModel.Birthday,
                UserName = uModel.UserName,
                IdentityNumber = uModel.IdentityNumber,
                Phone = uModel.Phone
            };
            var result = false;
            PropertyInfo[] properties = outDto.GetType().GetProperties();
            foreach (var item in properties)
            {
                if (string.IsNullOrWhiteSpace(item.GetValue(outDto)?.ToString()))
                {
                    result = true;
                }
            }
            if (result)
            {
                return Success();//Failed(ErrorCode.DataBaseError, "信息未完善，送积分失败！");
            }
            //完善个人信息，获50积分
            if (!InsertIntergral(50, SendIntergralEnum.完善个人信息送积分, userType))
            {
                Logger.Error($"完善个人信息送积分失败！{JsonConvert.SerializeObject(new { UserId = UserID })}");
            }
            return Success();//isSucc ? Success() : Failed(ErrorCode.DataBaseError, "完善个人信息送积分失败！");
        }

        /// <summary>
        /// 积分操作(只是Consumer)
        /// </summary>
        /// <param name="variation"></param>
        /// <param name="reasonEnum"></param>
        /// <param name="userType"></param>
        /// <returns></returns>
        private bool InsertIntergral(int variation, SendIntergralEnum reasonEnum, UserType userType = UserType.Consumer)
        {
            var scoreBiz = new ScoreExBiz();
            var model = new ScoreModel
            {
                ScoreGuid = Guid.NewGuid().ToString("N"),
                Variation = variation,
                Reason = reasonEnum.ToString(),
                UserGuid = UserID,
                UserTypeGuid = string.IsNullOrWhiteSpace(userType.ToString()) ? "Consumer" : userType.ToString(),
                OrgGuid = "",
                PlatformType = "CloudDoctor",
                CreatedBy = UserID,
                CreationDate = DateTime.Now,
                LastUpdatedBy = UserID,
            };
            return scoreBiz.InsertScore(model);
        }


        /// <summary>
        /// 送积分类型枚举
        /// </summary>
        private enum SendIntergralEnum
        {
            /// <summary>
            /// 注冊送积分
            /// </summary>
            [Description("注冊送积分")]
            注冊送积分 = 0,

            /// <summary>
            /// 连续签到送积分
            /// </summary>
            [Description("连续签到送积分")]
            连续签到送积分 = 1,


            /// <summary>
            /// 浏览文章送积分
            /// </summary>
            [Description("浏览文章送积分")]
            浏览文章送积分 = 2,

            /// <summary>
            /// 分享送积分
            /// </summary>
            [Description("分享送积分")]
            分享送积分 = 3,

            /// <summary>
            /// 浏览问题送积分
            /// </summary>
            [Description("浏览问题送积分")]
            浏览问题送积分 = 4,

            /// <summary>
            /// 收藏文章或问题送积分
            /// </summary>
            [Description("收藏文章或问题送积分")]
            收藏文章或问题送积分 = 5,

            /// <summary>
            /// 提问问题送积分
            /// </summary>
            [Description("提问问题送积分")]
            提问问题送积分 = 6,

            /// <summary>
            /// 推荐好友关注
            /// </summary>
            [Description("推荐好友关注")]
            推荐好友关注 = 7,

            /// <summary>
            /// 问医评价送积分
            /// </summary>
            [Description("问医评价送积分")]
            问医评价送积分 = 8,

            /// <summary>
            /// 关注医生送积分
            /// </summary>
            [Description("关注医生送积分")]
            关注医生送积分 = 9,

            /// <summary>
            /// 完善个人信息送积分
            /// </summary>
            [Description("完善个人信息送积分")]
            完善个人信息送积分 = 10,
            /// <summary>
            /// 取消关注医生，减扣积分
            /// </summary>
            [Description("取消关注医生减扣积分")]
            取消关注医生减扣积分 = 11,


        }

    }
}
