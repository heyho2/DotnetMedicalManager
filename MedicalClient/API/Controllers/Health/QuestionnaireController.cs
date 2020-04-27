using GD.Common;
using GD.Consumer;
using GD.Dtos.Health;
using GD.Health;
using GD.Models.CommonEnum;
using GD.Models.Mall;
using GD.Models.Questionnaire;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.API.Controllers.Health
{
    /// <summary>
    /// 健康问卷控制器
    /// </summary>
    public class QuestionnaireController : HealthBaseController
    {
        /// <summary>
        /// 健康问卷查询
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetHealthQuestionnairePageListResponseDto>))]
        public async Task<IActionResult> GetHealthQuestionnairePageListAsync([FromQuery]GetHealthQuestionnairePageListRequestDto requestDto)
        {
            string userId = UserID;
            if (!string.IsNullOrWhiteSpace(requestDto.UserId))
            {
                userId = requestDto.UserId;
            }
            var response = await new QuestionnaireResultBiz().GetHealthQuestionnaireListAsync(requestDto, userId);
            return Success(response);
        }
        /// <summary>
        /// 点击问卷进行答题(问卷状态为空时候调用)
        /// </summary>
        /// <param name="questionnaireGuid">问卷Id</param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> UpdateQuestionnaireAsync(string questionnaireGuid)
        {
            //判断问卷是否存在
            var questionnaireModel = await new QuestionnaireBiz().GetAsync(questionnaireGuid);
            if (questionnaireModel == null || !questionnaireModel.Enable)
            {
                return Success(false, "问卷不存在");
            }
            //判断问卷状态
            var questionnaireResultModel = await new QuestionnaireResultBiz().GetQuestionnaireResultModelAsync(UserID, questionnaireGuid);
            if (questionnaireResultModel != null)
            {
                return Success(questionnaireResultModel.ResultGuid);
            }
            questionnaireResultModel = new QuestionnaireResultModel()
            {
                ResultGuid = Guid.NewGuid().ToString("N"),
                QuestionnaireGuid = questionnaireGuid,
                UserGuid = UserID,
                FillStatus = false,
                Commented = false,
                CreationDate = DateTime.Now,
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
                LastUpdatedDate = DateTime.Now
            };
            var result = await new QuestionnaireResultBiz().InsertAsync(questionnaireResultModel);
            return result ? Success(questionnaireResultModel.ResultGuid) : Failed(ErrorCode.DataBaseError, "更新问卷状态失败");
        }
        /// <summary>
        /// 问卷题目查询(做题)
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetQuestionnaireQuestionResponseDto>))]
        public async Task<IActionResult> GetHealthQuestionnaireAsync([FromBody]GetQuestionnaireQuestionRequestDto requestDto)
        {
            //判断问卷是否存在
            var questionnaireModel = await new QuestionnaireBiz().GetAsync(requestDto.QuestionnaireGuid);
            if (questionnaireModel == null || !questionnaireModel.Enable)
            {
                return Failed(ErrorCode.UserData, "问卷不存在");
            }
            //判断用户问卷状态
            var questionnaireResultModel = await new QuestionnaireResultBiz().GetQuestionnaireResultModelAsync(UserID, requestDto.QuestionnaireGuid);
            if (questionnaireResultModel == null)
            {
                return Failed(ErrorCode.UserData, "用户问卷不存在");
            }
            if (questionnaireResultModel.FillStatus)
            {
                return Failed(ErrorCode.UserData, "问卷已提交");
            }
            //无获取题目动作
            if (!requestDto.NextQuestion.HasValue)
            {
                //点击进入问卷答题
                var questionnaireResultDetailModel = await new QuestionnaireResultDetailBiz().QuestionnaireResultDetailModelAsync(questionnaireResultModel.ResultGuid);
                //1.第一次进入
                if (questionnaireResultDetailModel == null)
                {
                    GetQuestionnaireQuestionResponseDto questionnaireQuestionResponse = new GetQuestionnaireQuestionResponseDto();
                    //拿出当前问卷所属第一题返回给前端
                    var qestionnaireQestionModel = await new QestionnaireQestionBiz().QuestionnaireQuestionModelAsync(questionnaireModel.QuestionnaireGuid);
                    if (qestionnaireQestionModel == null)
                    {
                        return Failed(ErrorCode.UserData, "问卷题目为空");
                    }
                    //第一题
                    await Assignment(questionnaireQuestionResponse, 1, qestionnaireQestionModel);
                    //判断是否有下一题
                    var checkResult = await CheckQuestionnaireQuestion(qestionnaireQestionModel.Sort + 1, questionnaireModel.QuestionnaireGuid, questionnaireResultModel.ResultGuid);
                    questionnaireQuestionResponse.Status = checkResult.Status;
                    //查找是否下一题直接依赖当前题目（所选答案是否存在下一题）
                    var dependQuestionAnswerList = await new QestionnaireQestionBiz().GetDependQuestionAnswerAsync(qestionnaireQestionModel.QuestionGuid);
                    if (dependQuestionAnswerList != null)
                    {
                        questionnaireQuestionResponse.DependLastAnswer = dependQuestionAnswerList;
                    }
                    return Success(questionnaireQuestionResponse);
                }
                //继续答题
                var qestionnaireQestionContinueModel = await new QestionnaireQestionBiz().GetAsync(questionnaireResultDetailModel.QuestionGuid);
                //最新问题不存在
                if (qestionnaireQestionContinueModel == null)
                {
                    return Failed(ErrorCode.UserData, "问卷题目错误");
                }
                //查找下一题
                //bool check = true;
                int i = qestionnaireQestionContinueModel.Sort + 1;
                var result = await CheckQuestionnaireQuestion(i, questionnaireModel.QuestionnaireGuid, questionnaireResultModel.ResultGuid);
                if (!string.IsNullOrWhiteSpace(result.Message))
                {
                    return Failed(ErrorCode.UserData, "依赖题目错误");
                }
                //检查是否存在下一题（当前题目存在时候才需要检查）
                if (!result.Status)
                {
                    //如果题目存在判断之前是否做答过题目把历史答案给前端
                    var checkQuestion = await new QuestionnaireResultDetailBiz().CheckAppointQuestionnaireResultDetailModelAsync(questionnaireResultModel.ResultGuid, result.QuestionnaireQuestionDto.QuestionGuid);
                    if (checkQuestion != null)
                    {
                        //赋值用户答案
                        result.QuestionnaireQuestionDto.Result = checkQuestion.Result;
                        if (!string.IsNullOrWhiteSpace(checkQuestion.AnswerGuids))
                        {
                            result.QuestionnaireQuestionDto.AnswerGuids = JsonConvert.DeserializeObject<List<string>>(checkQuestion.AnswerGuids);
                        }
                    }
                    var checkQestionnaireQestionContinueModel = await new QestionnaireQestionBiz().GetAsync(result.QuestionnaireQuestionDto.QuestionGuid);
                    if (checkQestionnaireQestionContinueModel != null)
                    {
                        var checkResult = await CheckQuestionnaireQuestion(checkQestionnaireQestionContinueModel.Sort + 1, questionnaireModel.QuestionnaireGuid, questionnaireResultModel.ResultGuid);
                        result.Status = checkResult.Status;
                        //查找是否下一题直接依赖当前题目（所选答案是否存在下一题）
                        var dependQuestionAnswerList = await new QestionnaireQestionBiz().GetDependQuestionAnswerAsync(result.QuestionnaireQuestionDto.QuestionGuid);
                        if (dependQuestionAnswerList != null)
                        {
                            result.DependLastAnswer = dependQuestionAnswerList;
                        }
                    }
                }
                return Success(result);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(requestDto.QuestionGuid))
                {
                    return Failed(ErrorCode.UserData, "问题Id不能为空");
                }
                var qestionnaireQestionContinueModel = await new QestionnaireQestionBiz().GetAsync(requestDto.QuestionGuid);
                //问题不存在
                if (qestionnaireQestionContinueModel == null)
                {
                    return Failed(ErrorCode.UserData, "问题不存在");
                }
                //下一题
                if (requestDto.NextQuestion.Value)
                {
                    //判断当前提交题目类型
                    if (qestionnaireQestionContinueModel.QuestionType == HealthInformationEnum.Decimal.ToString() || qestionnaireQestionContinueModel.QuestionType == HealthInformationEnum.String.ToString())
                    {
                        if (string.IsNullOrWhiteSpace(requestDto.Result))
                        {
                            return Failed(ErrorCode.UserData, "答案填写不能为空");
                        }
                    }
                    else
                    {
                        if (requestDto.AnswerGuids == null || requestDto.AnswerGuids.Count == 0)
                        {
                            return Failed(ErrorCode.UserData, "答案选项不能为空");
                        }
                    }
                    //保存上一题数据库
                    //1.判断是否之前答题过
                    var checkQuestion = await new QuestionnaireResultDetailBiz().CheckAppointQuestionnaireResultDetailModelAsync(questionnaireResultModel.ResultGuid, requestDto.QuestionGuid);
                    //获取当前最新一条有效的答题结果明细
                    var latestResultDetalModel = await new QuestionnaireResultDetailBiz().QuestionnaireResultDetailModelAsync(questionnaireResultModel.ResultGuid);
                    var nextReusltDetialSort = ((latestResultDetalModel?.Sort) ?? 0) + 1;
                    if (checkQuestion == null)
                    {
                        //第一次答题
                        QuestionnaireResultDetailModel questionnaireResultDetailModel = new QuestionnaireResultDetailModel()
                        {
                            DetailGuid = Guid.NewGuid().ToString("N"),
                            ResultGuid = questionnaireResultModel.ResultGuid,
                            QuestionGuid = requestDto.QuestionGuid,
                            Result = requestDto.Result,
                            CreatedBy = UserID,
                            CreationDate = DateTime.Now,
                            LastUpdatedBy = UserID,
                            LastUpdatedDate = DateTime.Now
                        };
                        if (requestDto.AnswerGuids != null && requestDto.AnswerGuids.Count > 0)
                        {
                            questionnaireResultDetailModel.AnswerGuids = JsonConvert.SerializeObject(requestDto.AnswerGuids);
                        }

                        questionnaireResultDetailModel.Sort = nextReusltDetialSort;

                        var insertResult = await new QuestionnaireResultDetailBiz().InsertAsync(questionnaireResultDetailModel);
                        if (!insertResult)
                        {
                            return Failed(ErrorCode.DataBaseError, "保存数据库做题答案发生异常");
                        }
                    }
                    else
                    {
                        //以前点击过上一题
                        checkQuestion.LastUpdatedBy = UserID;
                        checkQuestion.Result = requestDto.Result;
                        if (requestDto.AnswerGuids != null && requestDto.AnswerGuids.Count > 0)
                        {
                            checkQuestion.AnswerGuids = JsonConvert.SerializeObject(requestDto.AnswerGuids);
                        }
                        checkQuestion.LastUpdatedDate = DateTime.Now;
                        checkQuestion.Sort = checkQuestion.Enable ? checkQuestion.Sort : nextReusltDetialSort;
                        checkQuestion.Enable = true;

                        var updateResult = await new QuestionnaireResultDetailBiz().UpdateAsync(checkQuestion);
                        if (!updateResult)
                        {
                            return Failed(ErrorCode.DataBaseError, "更新数据库做题答案发生异常");
                        }
                    }
                    //查找下一题
                    int i = qestionnaireQestionContinueModel.Sort + 1;
                    var result = await CheckQuestionnaireQuestion(i, questionnaireModel.QuestionnaireGuid, questionnaireResultModel.ResultGuid);
                    if (!string.IsNullOrWhiteSpace(result.Message))
                    {
                        return Failed(ErrorCode.UserData, "依赖题目错误");
                    }
                    //检查是否存在下一题（当前题目存在时候才需要检查）
                    if (!result.Status)
                    {
                        //如果题目存在判断之前是否做答过题目把历史答案给前端
                        var checkQuestionExit = await new QuestionnaireResultDetailBiz().CheckAppointQuestionnaireResultDetailModelAsync(questionnaireResultModel.ResultGuid, result.QuestionnaireQuestionDto.QuestionGuid);
                        if (checkQuestionExit != null)
                        {
                            //赋值用户答案
                            result.QuestionnaireQuestionDto.Result = checkQuestionExit.Result;
                            if (!string.IsNullOrWhiteSpace(checkQuestionExit.AnswerGuids))
                            {
                                result.QuestionnaireQuestionDto.AnswerGuids = JsonConvert.DeserializeObject<List<string>>(checkQuestionExit.AnswerGuids);
                            }
                        }
                        var checkQestionnaireQestionContinueModel = await new QestionnaireQestionBiz().GetAsync(result.QuestionnaireQuestionDto.QuestionGuid);
                        if (checkQestionnaireQestionContinueModel != null)
                        {
                            var checkResult = await CheckQuestionnaireQuestion(checkQestionnaireQestionContinueModel.Sort + 1, questionnaireModel.QuestionnaireGuid, questionnaireResultModel.ResultGuid);
                            result.Status = checkResult.Status;
                            //查找是否下一题直接依赖当前题目（所选答案是否存在下一题）
                            var dependQuestionAnswerList = await new QestionnaireQestionBiz().GetDependQuestionAnswerAsync(result.QuestionnaireQuestionDto.QuestionGuid);
                            if (dependQuestionAnswerList != null)
                            {
                                result.DependLastAnswer = dependQuestionAnswerList;
                            }
                        }
                    }
                    return Success(result);
                }
                else
                {
                    var prevQuestionResponse = new GetQuestionnaireQuestionResponseDto();
                    //获取当前题目的答题结果明细
                    var currentQuestionResultDetial = await new QuestionnaireResultDetailBiz().CheckAppointQuestionnaireResultDetailModelAsync(questionnaireResultModel.ResultGuid, requestDto.QuestionGuid);
                    //当前题目没有提交过
                    if (currentQuestionResultDetial == null || !currentQuestionResultDetial.Enable)
                    {
                        //直接拿取用户答题详情的最新一个问题
                        var questionnaireResultDetailResult = await new QuestionnaireResultDetailBiz().QuestionnaireResultDetailModelAsync(questionnaireResultModel.ResultGuid);
                        if (questionnaireResultDetailResult == null)
                        {
                            return Failed(ErrorCode.DataBaseError, "不存在上一题");
                        }
                        //checkQuestion = questionnaireResultDetailResult;
                        var prevQestionModel = await new QestionnaireQestionBiz().GetAsync(questionnaireResultDetailResult.QuestionGuid);//获取上一题Model
                        await Assignment(prevQuestionResponse, questionnaireResultDetailResult.Sort, prevQestionModel);
                        //赋值用户答案
                        prevQuestionResponse.QuestionnaireQuestionDto.Result = questionnaireResultDetailResult.Result;
                        if (!string.IsNullOrWhiteSpace(questionnaireResultDetailResult.AnswerGuids))
                        {
                            prevQuestionResponse.QuestionnaireQuestionDto.AnswerGuids = JsonConvert.DeserializeObject<List<string>>(questionnaireResultDetailResult.AnswerGuids);
                        }
                        return Success(prevQuestionResponse);
                    }
                    //存在并且下标1
                    if (currentQuestionResultDetial.Sort <= 1)
                    {
                        return Failed(ErrorCode.DataBaseError, "不存在上一题");
                    }

                    //当前题目提交过

                    //获取上一题
                    var checkQuestionnaireResultDetailResult = await new QuestionnaireResultDetailBiz().AppointOrderQuestionnaireResultDetailModelAsync(questionnaireResultModel.ResultGuid, currentQuestionResultDetial.Sort - 1);
                    if (checkQuestionnaireResultDetailResult == null)
                    {
                        return Failed(ErrorCode.DataBaseError, "不存在上一题");
                    }
                    var qestionnaireQestionModel = await new QestionnaireQestionBiz().GetAsync(checkQuestionnaireResultDetailResult.QuestionGuid);
                    if (qestionnaireQestionModel == null)
                    {
                        return Failed(ErrorCode.UserData, "上一题问卷题目为空");
                    }

                    await Assignment(prevQuestionResponse, checkQuestionnaireResultDetailResult.Sort, qestionnaireQestionModel);
                    //赋值用户答案
                    prevQuestionResponse.QuestionnaireQuestionDto.Result = checkQuestionnaireResultDetailResult.Result;
                    if (!string.IsNullOrWhiteSpace(checkQuestionnaireResultDetailResult.AnswerGuids))
                    {
                        prevQuestionResponse.QuestionnaireQuestionDto.AnswerGuids = JsonConvert.DeserializeObject<List<string>>(checkQuestionnaireResultDetailResult.AnswerGuids);
                    }
                    //把上一题作答结果禁用
                    currentQuestionResultDetial.Enable = false;
                    var updateResult = await new QuestionnaireResultDetailBiz().UpdateAsync(currentQuestionResultDetial);
                    if (!updateResult)
                    {
                        return Failed(ErrorCode.DataBaseError, "当前题目结果设置为无效失败");
                    }
                    return Success(prevQuestionResponse);
                }
            }
        }
        /// <summary>
        /// 给返回对象赋值
        /// </summary>
        /// <param name="questionnaireQuestionResponse"></param>
        /// <param name="i"></param>
        /// <param name="qestionnaireQestion"></param>
        /// <returns></returns>
        private static async Task Assignment(GetQuestionnaireQuestionResponseDto questionnaireQuestionResponse, int i, QuestionnaireQuestionModel qestionnaireQestion, QuestionnaireResultModel questionnaireResultModel = null)
        {
            questionnaireQuestionResponse.QuestionnaireQuestionDto = new GetQuestionnaireQuestionDto
            {
                QuestionGuid = qestionnaireQestion.QuestionGuid,
                QuestionNumber = i,
                QuestionnaireGuid = qestionnaireQestion.QuestionnaireGuid,
                QuestionName = qestionnaireQestion.QuestionName,
                QuestionType = qestionnaireQestion.QuestionType,
                Unit = qestionnaireQestion.Unit,
                PromptText = qestionnaireQestion.PromptText
            };
            //查找答案列表
            questionnaireQuestionResponse.QuestionnaireQuestionDto.QuestionnaireAnswerDtoList = await new QuestionnaireAnswerBiz().GetQuestionnaireAnswerModelAsync(questionnaireQuestionResponse.QuestionnaireQuestionDto.QuestionnaireGuid, questionnaireQuestionResponse.QuestionnaireQuestionDto.QuestionGuid);
            ////获取问卷指定问题
            //var nextQestionnaireQestion = await new QestionnaireQestionBiz().AppointQuestionnaireQuestionModelAsync(qestionnaireQestion.QuestionnaireGuid, i + 1);
            ////若下一题不为空
            //if (nextQestionnaireQestion != null)
            //{
            //    //下一题存在依赖
            //    if (nextQestionnaireQestion.IsDepend)
            //    {
            //        //如果下一题依赖的是当前题目，则需要遍历当前题目的选择项
            //        if (nextQestionnaireQestion.DependQuestion == questionnaireQuestionResponse.QuestionnaireQuestionDto.QuestionGuid)
            //        {
            //            questionnaireQuestionResponse.QuestionnaireQuestionDto.HasNext = true;
            //            questionnaireQuestionResponse.QuestionnaireQuestionDto.QuestionnaireAnswerDtoList.ForEach(a => a.HasNext = nextQestionnaireQestion.DependAnswer == a.AnswerGuid);
            //        }
            //        else//如果依赖的不是当前题目，则需要检测之前的答卷结果有没有选择被依赖的选择项
            //        {
            //            var theAnswer = await new QuestionnaireResultDetailBiz().AppointQuestionnaireResultDetailModelAsync(questionnaireResultModel.ResultGuid, nextQestionnaireQestion.QuestionGuid);
            //            if (theAnswer.AnswerGuids.Contains(nextQestionnaireQestion.DependAnswer))
            //            {
            //                questionnaireQuestionResponse.QuestionnaireQuestionDto.HasNext = true;
            //                questionnaireQuestionResponse.QuestionnaireQuestionDto.QuestionnaireAnswerDtoList.ForEach(a => a.HasNext = true);
            //            }
            //        }
            //    }
            //    else
            //    {
            //        questionnaireQuestionResponse.QuestionnaireQuestionDto.HasNext = true;
            //        questionnaireQuestionResponse.QuestionnaireQuestionDto.QuestionnaireAnswerDtoList.ForEach(a => a.HasNext = true);
            //    }

            //}



        }
        /// <summary>
        /// 查找下一题
        /// </summary>
        /// <param name="i">问题排序号</param>
        /// <param name="questionnaireGuid">问卷id</param>
        /// <param name="resultGuid">用户问卷id</param>
        /// <returns></returns>
        public static async Task<GetQuestionnaireQuestionResponseDto> CheckQuestionnaireQuestion(int i, string questionnaireGuid, string resultGuid)
        {
            //返回给前端的题目序号
            //题目序号要为当前答题的总条数+1
            int index = await new QuestionnaireResultDetailBiz().GetQuestionnaireResultDetailCount(resultGuid) + 1;
            GetQuestionnaireQuestionResponseDto questionnaireQuestionResponse = new GetQuestionnaireQuestionResponseDto();
            bool check = true;
            while (check)
            {
                //获取问卷指定问题
                var qestionnaireQestion = await new QestionnaireQestionBiz().AppointQuestionnaireQuestionModelAsync(questionnaireGuid, i);
                if (qestionnaireQestion == null)
                {
                    //查询不到题目结束
                    check = false;
                    questionnaireQuestionResponse.Status = true;
                    return questionnaireQuestionResponse;
                }
                //依赖问题
                if (qestionnaireQestion.IsDepend)
                {
                    //判断该问题是否应该出现在下一题
                    var questionnaireResultDetail = await new QuestionnaireResultDetailBiz().AppointQuestionnaireResultDetailModelAsync(resultGuid, qestionnaireQestion.DependQuestion);
                    if (questionnaireResultDetail != null && !string.IsNullOrWhiteSpace(questionnaireResultDetail.AnswerGuids))
                    {
                        //questionnaireQuestionResponse.Message = "问卷依赖题目错误";
                        //return questionnaireQuestionResponse;
                        List<string> asnwerList = JsonConvert.DeserializeObject<List<string>>(questionnaireResultDetail.AnswerGuids);
                        if (asnwerList.Contains(qestionnaireQestion.DependAnswer))
                        {
                            //存在之前选择的答案
                            await Assignment(questionnaireQuestionResponse, index, qestionnaireQestion);
                            return questionnaireQuestionResponse;
                        }
                    }
                    //绩效查找下一题
                    i += 1;
                }
                else
                {
                    //不是依赖问题直接返回给前端
                    await Assignment(questionnaireQuestionResponse, index, qestionnaireQestion);
                    return questionnaireQuestionResponse;
                }
            }
            return questionnaireQuestionResponse;
        }
        /// <summary>
        /// 获取用户问卷结果
        /// </summary>
        /// <param name="resultGuid">用户问卷结果Id</param>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(ResponseDto<GetConumserQuestionnaireResultResponseDto>))]
        public async Task<IActionResult> GetConumserQuestionnaireResultAsync(string resultGuid)
        {
            var resultModel = await new QuestionnaireResultBiz().GetAsync(resultGuid);
            if (resultModel == null)
            {
                return Failed(ErrorCode.UserData, "无此问卷结果数据");
            }
            if (!resultModel.FillStatus)
            {
                return Failed(ErrorCode.UserData, "问卷未提交状态无法查看");
            }
            var questionnaire = await new QuestionnaireBiz().GetAsync(resultModel.QuestionnaireGuid);
            var questionModels = await new QestionnaireQestionBiz().GetModelsByQuestionnaireGuidAsync(questionnaire.QuestionnaireGuid);
            var answerModels = await new QuestionnaireAnswerBiz().GetModelsByQuestionnaireGuidAsync(questionnaire.QuestionnaireGuid);
            var resultDetailModels = await new QuestionnaireResultDetailBiz().GetModelsByResultGuidAsync(resultGuid);
            var result = new GetConumserQuestionnaireResultResponseDto
            {
                QuestionnaireName = questionnaire.QuestionnaireName,
                Subhead = questionnaire.Subhead,
                Comment = resultModel.Comment,
                CommentDate = resultModel.LastUpdatedDate,
                Questions = resultDetailModels.Join(questionModels, d => d.QuestionGuid, q => q.QuestionGuid, (d, q) => new GetConumserQuestionnaireResultResponseDto.Question
                {
                    QuestionName = q.QuestionName,
                    QuestionType = q.QuestionType,
                    Sort = d.Sort,
                    Unit = q.Unit,
                    PromptText = q.PromptText,
                    Result = d.Result,
                    Answers = answerModels.Where(answer => answer.QuestionGuid == d.QuestionGuid).OrderBy(answer => answer.Sort).Select(answer => new GetConumserQuestionnaireResultResponseDto.Answer
                    {
                        AnswerLabel = answer.AnswerLabel,
                        IsSelected = d.AnswerGuids == null ? false : d.AnswerGuids.Contains(answer.AnswerGuid)
                    }).ToList()
                }).OrderBy(a => a.Sort).ToList()
            };
            return Success(result);
        }
        /// <summary>
        /// 提交问卷
        /// </summary>
        /// <param name="resultGuid"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> SubmitQuestionnaireAsync(string resultGuid)
        {
            var resultModel = await new QuestionnaireResultBiz().GetAsync(resultGuid);
            if (resultModel == null || resultModel.UserGuid != UserID)
            {
                return Failed(ErrorCode.UserData, "无此问卷数据");
            }
            if (resultModel.FillStatus)
            {
                return Failed(ErrorCode.UserData, "问卷已提交");
            }
            resultModel.LastUpdatedBy = UserID;
            resultModel.LastUpdatedDate = DateTime.Now;
            resultModel.FillStatus = true;//已提交
            var result = await new QuestionnaireResultBiz().UpdateAsync(resultModel);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "成功提交问卷");
        }
        /// <summary>
        /// 删除用户问卷
        /// </summary>
        /// <param name="questionnaireGuid">问卷Id</param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> DeleteQuestionnaireAsync(string questionnaireGuid)
        {
            //判断问卷是否存在
            var questionnaireModel = await new QuestionnaireBiz().GetAsync(questionnaireGuid);
            if (questionnaireModel == null || !questionnaireModel.Enable)
            {
                return Success(false, "问卷不存在");
            }
            var checkQuestionnaireHide = await new QuestionnaireHideBiz().GetQuestionnaireHideModelAsync(UserID, questionnaireModel.QuestionnaireGuid);
            if (checkQuestionnaireHide != null)
            {
                return Failed(ErrorCode.DataBaseError, "已被删除,请勿重复操作");
            }
            //判断问卷状态
            var questionnaireResultModel = await new QuestionnaireResultBiz().GetQuestionnaireResultModelAsync(UserID, questionnaireGuid);
            List<QuestionnaireResultDetailModel> questionnaireResultDetailList = null;
            if (questionnaireResultModel != null)
            {
                //用户问卷明细答案
                questionnaireResultDetailList = await new QuestionnaireResultDetailBiz().GetModelsByResultGuidAsync(questionnaireResultModel.ResultGuid);
            }
            //删除问卷标记数据
            QuestionnaireHideModel questionnaireHideModel = new QuestionnaireHideModel()
            {
                HideGuid = Guid.NewGuid().ToString("N"),
                UserGuid = UserID,
                QuestionnaireGuid = questionnaireModel.QuestionnaireGuid,
                CreatedBy = UserID,
                CreationDate = DateTime.Now,
                LastUpdatedBy = UserID,
                LastUpdatedDate = DateTime.Now
            };
            var result = await new QuestionnaireResultBiz().DeleteQuestionnaireAsync(questionnaireHideModel, questionnaireResultModel);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "删除失败");
        }
    }
}
