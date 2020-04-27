using GD.API.Code;
using GD.Common;
using GD.Dtos.Enum.Questionnaire;
using GD.Dtos.Questionnaire;
using GD.Manager.Health;
using GD.Manager.Questionnaire;
using GD.Manager.Utility;
using GD.Models.Questionnaire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GD.API.Controllers.Questionnaire
{
    /// <summary>
    /// 问卷控制器
    /// </summary>
    public class QuestionnaireController : QuestionnaireBaseController
    {
        /// <summary>
        /// 获取健康问卷分页列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(ResponseDto<GetQuestionnairePageListResponseDto>))]
        public async Task<IActionResult> GetQuestionnairePageListAsync(GetQuestionnairePageListRequestDto requestDto)
        {
            var result = await new QuestionnaireBiz().GetQuestionnairePageListAsync(requestDto);
            return Success(result);
        }

        /// <summary>
        /// 查看问卷基础数据详情
        /// </summary>
        /// <param name="questionnaireGuid"></param>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(ResponseDto<GetQuestionnaireInfoResponseDto>))]
        public async Task<IActionResult> GetQuestionnaireInfoAsync(string questionnaireGuid)
        {
            //获取问卷model
            var model = await new QuestionnaireBiz().GetAsync(questionnaireGuid);
            var questionModels = await new QuestionnaireQuestionBiz().GetModelsByQuestionnaireGuidAsync(questionnaireGuid);
            var answerModels = await new QuestionnaireAnswerBiz().GetModelsByQuestionnaireGuidAsync(questionnaireGuid);

            var result = new GetQuestionnaireInfoResponseDto
            {
                QuestionnaireGuid = model.QuestionnaireGuid,
                QuestionnaireName = model.QuestionnaireName,
                Subhead = model.Subhead,
                Questions = questionModels.Select(q => new GetQuestionnaireInfoResponseDto.Question
                {
                    QuestionGuid = q.QuestionGuid,
                    QuestionName = q.QuestionName,
                    QuestionType = Enum.Parse<QuestionnaireQuestionTypeEnum>(q.QuestionType),
                    IsDepend = q.IsDepend,
                    DependAnswer = q.DependAnswer,
                    DependQuestion = q.DependQuestion,
                    Sort = q.Sort,
                    Unit = q.Unit,
                    PromptText = q.PromptText,
                    DependDescription = GetDependDescription(questionModels, answerModels, q),
                    Answers = answerModels == null ? new List<GetQuestionnaireInfoResponseDto.Answer>() : answerModels.Where(a => a.QuestionGuid == q.QuestionGuid).OrderBy(a => a.Sort).Select(a => new GetQuestionnaireInfoResponseDto.Answer
                    {
                        AnswerGuid = a.AnswerGuid,
                        AnswerLabel = a.AnswerLabel,
                        IsDefault = a.IsDefault
                    }).ToList()
                }).OrderBy(q => q.Sort).ToList()
            };
            return Success(result);
        }

        /// <summary>
        /// 获取某一个问卷下消费者答题结果分页列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(ResponseDto<GetConsumerQuestionnairesPageListResponseDto>))]
        public async Task<IActionResult> GetConsumerQuestionnairesPageLisAsync(GetConsumerQuestionnairesPageListRequestDto requestDto)
        {
            var result = await new QuestionnaireResultBiz().GetConsumerQuestionnairesPageListAsync(requestDto);
            return Success(result);
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
            var questionnaire = await new QuestionnaireBiz().GetAsync(resultModel.QuestionnaireGuid);
            var questionModels = await new QuestionnaireQuestionBiz().GetModelsByQuestionnaireGuidAsync(questionnaire.QuestionnaireGuid);
            var answerModels = await new QuestionnaireAnswerBiz().GetModelsByQuestionnaireGuidAsync(questionnaire.QuestionnaireGuid);
            var resultDetailModels = await new QuestionnaireResultDetailBiz().GetModelsByResultGuidAsync(resultGuid);
            var result = new GetConumserQuestionnaireResultResponseDto
            {
                QuestionnaireName = questionnaire.QuestionnaireName,
                Subhead = questionnaire.Subhead,
                Comment = resultModel.Comment,
                Questions = resultDetailModels.Join(questionModels, d => d.QuestionGuid, q => q.QuestionGuid, (d, q) => new GetConumserQuestionnaireResultResponseDto.Question
                {
                    QuestionName = q.QuestionName,
                    QuestionType = Enum.Parse<QuestionnaireQuestionTypeEnum>(q.QuestionType),
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
        /// 评价用户填写的问卷
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Produces(typeof(ResponseDto))]
        public async Task<IActionResult> CommentConsumerQuestionnaireAsync([FromBody]CommentConsumerQuestionnaireRequestDto requestDto)
        {
            var biz = new QuestionnaireResultBiz();
            var questionnaireResult = await biz.GetAsync(requestDto.ResultGuid);
            if (questionnaireResult == null)
            {
                return Failed(ErrorCode.UserData, "用户问卷结果不存在");
            }
            if (!questionnaireResult.FillStatus)
            {
                return Failed(ErrorCode.UserData, "用户问卷未提交，不可评价");
            }
            questionnaireResult.Commented = true;
            questionnaireResult.Comment = requestDto.Comment;
            questionnaireResult.LastUpdatedBy = UserID;
            questionnaireResult.LastUpdatedDate = DateTime.Now;
            var result = await biz.UpdateAsync(questionnaireResult);
            if (result)
            {
                //rabbitMQ通知用户问卷结果被评价
                new HealthRabbitMQNotificationBiz().HealthRabbitMQNotification(new HealthMessageDto
                {
                    Title = "您填写的问卷得到医生回复啦",
                    Content = questionnaireResult.Comment,
                    HealthType = HealthMessageDto.HealthTypeEnum.Questionnaire,
                    ResultGuid = questionnaireResult.ResultGuid
                }, questionnaireResult.UserGuid);
            }
            return result ? Success() : Failed(ErrorCode.DataBaseError, "评价用户问卷结果失败");
        }

        /// <summary>
        /// 切换问卷显示状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Produces(typeof(ResponseDto))]
        public async Task<IActionResult> ChangerQuestionnaireDisplayAsync([FromBody]ChangerQuestionnaireDisplayRequestDto requestDto)
        {
            var biz = new QuestionnaireBiz();
            var model = await biz.GetAsync(requestDto.QuestionnaireGuid);
            if (model == null)
            {
                return Failed(ErrorCode.UserData, "为获取到问卷数据");
            }
            model.Display = requestDto.Display;
            model.LastUpdatedBy = UserID;
            model.LastUpdatedDate = DateTime.Now;
            var result = await biz.UpdateAsync(model);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "切换问卷显示状态失败");
        }

        /// <summary>
        /// 删除问卷
        /// </summary>
        /// <param name="questionnaireGuid"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces(typeof(ResponseDto))]
        public async Task<IActionResult> DeleteQuestionnaireAsync(string questionnaireGuid)
        {
            var biz = new QuestionnaireBiz();
            var model = await biz.GetAsync(questionnaireGuid);
            if (model == null)
            {
                return Failed(ErrorCode.UserData, "未找到此问卷");
            }
            var result = await biz.DeleteQuestionnaireAsync(model);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "删除问卷失败");

        }

        /// <summary>
        /// 发布问卷
        /// </summary>
        /// <param name="questionnaireGuid"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces(typeof(ResponseDto))]
        public async Task<IActionResult> IssueQuestionnaireAsync(string questionnaireGuid)
        {
            var biz = new QuestionnaireBiz();
            //1.获取文件状态，数据验证
            var model = await biz.GetAsync(questionnaireGuid);
            if (model == null)
            {
                return Failed(ErrorCode.UserData, "无此问卷数据");
            }
            if (model.IssuingStatus)
            {
                return Failed(ErrorCode.UserData, "问卷已发放，无需重复发放");
            }

            //2.切换问卷状态
            model.IssuingStatus = true;
            model.IssuingDate = DateTime.Now;
            model.LastUpdatedBy = UserID;
            model.LastUpdatedDate = DateTime.Now;
            await biz.UpdateAsync(model);


            //3.发送订阅消息

            return Success();
        }

        /// <summary>
        /// 创建问卷前的初始化工作
        /// 点击创建问卷需调用此接口生成问卷草稿
        /// </summary>
        /// <returns>问卷guid</returns>
        [HttpPost]
        [Produces(typeof(ResponseDto<string>))]
        public async Task<IActionResult> InitQuestionnaireForCreateQuestionnaireAsync()
        {
            var model = new QuestionnaireModel
            {
                QuestionnaireGuid = Guid.NewGuid().ToString("N"),
                QuestionnaireName = $"新建问卷{DateTime.Now.ToString("yyyyMMddHHmmss")}",
                Subhead = "问卷副标题",
                CreatedBy = UserID,
                LastUpdatedBy = UserID
            };
            var result = await new QuestionnaireBiz().InsertAsync(model);
            if (!result)
            {
                return Failed(ErrorCode.DataBaseError, "创建问卷前的初始化失败，请重新点击创建按钮");
            }
            return Success(model.ToDto<InitQuestionnaireResponseDto>());
        }

        /// <summary>
        /// 获取问卷问题类型key value 数组
        /// key:枚举数值
        /// value:枚举描述
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(ResponseDto<List<KeyValuePair<int, string>>>)), AllowAnonymous]
        public IActionResult GetQuestionTypeList()
        {
            List<KeyValuePair<int, string>> results = new List<KeyValuePair<int, string>>();
            foreach (var v in Enum.GetNames(typeof(QuestionnaireQuestionTypeEnum)))
            {
                var item = Enum.Parse<QuestionnaireQuestionTypeEnum>(v);
                results.Add(new KeyValuePair<int, string>((int)item, item.GetDescription()));
            }
            return Success(results);
        }

        /// <summary>
        /// 暂存问卷问题
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Produces(typeof(ResponseDto<CreateQuestionnaireQuestionResponseDto>))]
        public async Task<IActionResult> CreateQuestionnaireQuestionAsync([FromBody]CreateQuestionnaireQuestionRequestDto requestDto)
        {
            QuestionnaireQuestionModel model = null;
            var answers = new List<QuestionnaireAnswerModel>();
            var questionBiz = new QuestionnaireQuestionBiz();
            var cancelDependQuestionModels = new List<QuestionnaireQuestionModel>();//由于问题类型变化或答案选项被删除，需要删除依赖关系的问题列表
            var isCreate = true;//是否是创建问题
            //新增
            if (string.IsNullOrWhiteSpace(requestDto.QuestionGuid))
            {
                model = new QuestionnaireQuestionModel
                {
                    QuestionGuid = Guid.NewGuid().ToString("N"),
                    CreatedBy = UserID,
                    LastUpdatedBy = UserID
                };
            }
            else//编辑
            {
                model = await questionBiz.GetAsync(requestDto.QuestionGuid);
                if (model == null)
                {
                    return Failed(ErrorCode.UserData, $"未找到编辑的问题");
                }
                if (model.QuestionnaireGuid != requestDto.QuestionnaireGuid)
                {
                    return Failed(ErrorCode.UserData, $"传入的问卷guid和待编辑问题的问卷guid不一致");
                }

                //由于问题类型变化或答案选项被删除，需要删除依赖关系的问题列表
                if (model.QuestionType != requestDto.QuestionType.ToString())
                {
                    //获取依赖此问题的问题列表
                    var dependonQuestionModel = await questionBiz.GetModelsByDependQuestionGuidAsync(model.QuestionGuid);
                    dependonQuestionModel.ForEach(a =>
                    {
                        a.DependAnswer = string.Empty;
                        a.DependQuestion = string.Empty;
                        a.IsDepend = false;
                    });
                    cancelDependQuestionModels.AddRange(dependonQuestionModel);
                }
                else if (model.QuestionType == QuestionnaireQuestionTypeEnum.Bool.ToString() || model.QuestionType == QuestionnaireQuestionTypeEnum.Enum.ToString())
                {
                    var answerModels = await new QuestionnaireAnswerBiz().GetModelsByQuestionGuidAsync(model.QuestionGuid);
                    //获取被删除的选项id集合
                    var exceptAnswerIds = answerModels.Select(a => a.AnswerGuid)
                        .Except(requestDto.Answers
                                .Where(a => !string.IsNullOrWhiteSpace(a.AnswerGuid))
                                .Select(a => a.AnswerGuid));

                    //获取依赖此问题的问题列表
                    var dependonQuestionModel = await questionBiz.GetModelsByDependQuestionGuidAsync(model.QuestionGuid);
                    //获取依赖此问题中被删除选项的问题列表，这些问题的对此问题的依赖将被取消
                    var cancelDependQuestionModelList = dependonQuestionModel.Join(exceptAnswerIds, d => d.DependAnswer, a => a, (d, a) => d).ToList();
                    cancelDependQuestionModelList.ForEach(a =>
                    {
                        a.DependAnswer = string.Empty;
                        a.DependQuestion = string.Empty;
                        a.IsDepend = false;
                    });
                    cancelDependQuestionModels.AddRange(cancelDependQuestionModelList);
                }
                model.LastUpdatedBy = UserID;
                model.LastUpdatedDate = DateTime.Now;
                isCreate = false;
            }

            model.QuestionnaireGuid = requestDto.QuestionnaireGuid;
            model.QuestionName = requestDto.QuestionName;
            model.QuestionType = requestDto.QuestionType.ToString();
            model.Unit = requestDto.Unit;
            model.Sort = requestDto.Sort;//判断有无重复的sort
            model.PromptText = requestDto.PromptText;
            if (!string.IsNullOrWhiteSpace(requestDto.DependAnswer))
            {
                var dependAnswerModel = await new QuestionnaireAnswerBiz().GetAsync(requestDto.DependAnswer);
                if (dependAnswerModel == null)
                {
                    return Failed(ErrorCode.UserData, $"依赖的答案未找到");
                }
                model.DependAnswer = requestDto.DependAnswer;
                model.DependQuestion = dependAnswerModel.QuestionGuid;
                model.IsDepend = true;
            }
            else
            {
                model.DependAnswer = string.Empty;
                model.DependQuestion = string.Empty;
                model.IsDepend = false;
            }
            if (requestDto.Answers != null)
            {
                answers = requestDto.Answers.Select((item, index) => new QuestionnaireAnswerModel
                {
                    AnswerGuid = Guid.NewGuid().ToString("N"),
                    QuestionnaireGuid = model.QuestionnaireGuid,
                    QuestionGuid = model.QuestionGuid,
                    AnswerLabel = item.AnswerLabel,
                    IsDefault = item.IsDefault,
                    Sort = index + 1,
                    CreatedBy = UserID,
                    LastUpdatedBy = UserID
                }).ToList();
            }

            var result = await questionBiz.CreateQuestionnaireQuestionAsync(model, answers, isCreate, cancelDependQuestionModels);
            if (!result)
            {
                return Failed(ErrorCode.UserData, $"保存问题失败");
            }

            var dependQuestions = await questionBiz.GetQuestionWithDependAsync(requestDto.QuestionnaireGuid);
            var questionnaireModel = await new QuestionnaireBiz().GetAsync(requestDto.QuestionnaireGuid);
            if (questionnaireModel.HasDepend != dependQuestions.Any())
            {
                questionnaireModel.HasDepend = dependQuestions.Any();
                await new QuestionnaireBiz().UpdateAsync(questionnaireModel);
            }
            

            #region 将提交的数据返回（携带问题guid）
            var response = new CreateQuestionnaireQuestionResponseDto
            {
                QuestionnaireGuid = model.QuestionnaireGuid,
                QuestionGuid = model.QuestionGuid,
                QuestionName = model.QuestionName,
                QuestionType = requestDto.QuestionType,
                IsDepend = model.IsDepend,
                DependAnswer = model.DependAnswer,
                Sort = model.Sort,
                Answers = requestDto.Answers
            };
            if (model.IsDepend)
            {
                var dependAnswerModel = await new QuestionnaireAnswerBiz().GetAsync(model.DependAnswer);
                var dependQuestionModel = await questionBiz.GetAsync(model.DependQuestion);
                response.DependDescription = $"*依赖于第{dependQuestionModel?.Sort}题的第{dependAnswerModel?.Sort}个选项，当题目{dependQuestionModel?.Sort}选择[{dependAnswerModel?.AnswerLabel}]时，此题才显示。";
            }
            #endregion
            return Success(response);
        }

        /// <summary>
        /// 保存问卷
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces(typeof(ResponseDto))]
        public async Task<IActionResult> SaveQuestionnaireAsync([FromBody]SaveQuestionnaireRequestDto requestDto)
        {
            var biz = new QuestionnaireBiz();
            var model = await biz.GetAsync(requestDto.QuestionnaireGuid);
            if (model == null)
            {
                return Failed(ErrorCode.UserData, "不存在此问卷");
            }
            model.QuestionnaireName = requestDto.QuestionnaireName;
            model.Subhead = requestDto.Subhead;
            model.LastUpdatedBy = UserID;
            model.LastUpdatedDate = DateTime.Now;
            var result = await biz.UpdateAsync(model);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "保存文件失败");
        }


        /// <summary>
        /// 移除问卷问题
        /// </summary>
        /// <param name="questionGuid"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces(typeof(ResponseDto))]
        public async Task<IActionResult> RemoveQuestionnaireQuestionAsync(string questionGuid)
        {
            var questionBiz = new QuestionnaireQuestionBiz();
            var model = await questionBiz.GetAsync(questionGuid);
            if (model == null)
            {
                return Failed(ErrorCode.UserData, "未找到此问题");
            }
            var questionnaireModel = await new QuestionnaireBiz().GetAsync(model.QuestionnaireGuid);
            if (questionnaireModel.IssuingStatus)
            {
                return Failed(ErrorCode.UserData, "问卷已发放不能修改");
            }
            var dependModels = await questionBiz.GetModelsByDependQuestionGuidAsync(model.QuestionGuid);
            if (dependModels.Any())
            {
                return Failed(ErrorCode.UserData, $"该问题被第{string.Join("、", dependModels.OrderBy(a => a.Sort).Select(a => a.Sort))}题依赖，无法删除");
            }
            var result = await questionBiz.RemoveQuestionAsync(model);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "移除问题失败");
        }

        /// <summary>
        /// 改变问题序号
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Produces(typeof(ResponseDto))]
        public async Task<IActionResult> ChangeQuestionSortAsync([FromBody]ChangeQuestionSortRequestDto requestDto)
        {
            var biz = new QuestionnaireQuestionBiz();
            var model = await biz.GetAsync(requestDto.QuestionGuid);
            if (model == null)
            {
                return Failed(ErrorCode.UserData, "未找到此问题");
            }
            var newSort = requestDto.Sort;
            if (model.Sort == newSort)
            {
                return Failed(ErrorCode.UserData, "问题序号未产生变化，请核对");
            }
            if (model.IsDepend)
            {
                var dependQuestionModel = await biz.GetAsync(model.DependQuestion);
                if (dependQuestionModel == null)
                {
                    return Failed(ErrorCode.UserData, "当前问题的依赖数据异常");
                }
                if (newSort <= dependQuestionModel.Sort)
                {
                    return Failed(ErrorCode.UserData, $"当前问题的序号不能先于其依赖问题[第{dependQuestionModel.Sort}题]");
                }
            }
            //查询依赖当前问题的题目列表
            var dependModels = await biz.GetModelsByDependQuestionGuidAsync(model.QuestionGuid);
            if (dependModels.Any())
            {
                var minSortModel = dependModels.OrderBy(a => a.Sort).FirstOrDefault();
                if (minSortModel.Sort <= newSort)
                {
                    return Failed(ErrorCode.UserData, $"第{minSortModel.Sort}题依赖当前题目，移动序号不能大于{minSortModel.Sort}");
                }
            }
            // 一.其他问题为待变化问题顺次移动位置
            //  1.若向前移动，则原序号和新序号之间所有问题序号+1 (不包括待变化问题)
            //  2.若向后移动，则原序号和新序号之间所有问题序号-1 (不包括待变化问题)
            // 二.将待变化问题序号变为新序号
            var result = await biz.ChangeQuestionSortAsync(model, newSort, UserID);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "问题变化序号失败");
        }

        /// <summary>
        /// 获取当前问题可依赖的问题列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetQuestionListWhileCanDependAsync(GetQuestionListCanDependRequestDto requestDto)
        {
            var details = await new QuestionnaireQuestionBiz().GetQuestionListCanDependAsync(requestDto);
            //以问题分组
            var group = details.GroupBy(a => new
            {
                a.QuestionGuid,
                a.QuestionName,
                a.QuestionSort
            });
            var result = group.Select(g => new GetQuestionListCanDependResponseDto
            {
                QuestionGuid = g.Key.QuestionGuid,
                QuestionName = g.Key.QuestionName,
                Sort = g.Key.QuestionSort,
                Answers = g.Select(gd => new GetQuestionListCanDependResponseDto.Answer
                {
                    AnswerGuid = gd.AnswerGuid,
                    AnswerLabel = gd.AnswerLabel,
                    Sort = gd.AnswerSort
                }).OrderBy(answer => answer.Sort).ToList()
            }).OrderBy(q => q.Sort);
            return Success(result);
        }

        #region rabbitMQ
        //[HttpGet]
        //public IActionResult RabbitMqFanout()
        //{
        //    try
        //    {
        //        var bus = Communication.MQ.Client.CreateConnection();
        //        var advancedBus = bus.Advanced;
        //        if (advancedBus.IsConnected)
        //        {

        //            var queueName = "hzl";
        //            //var queue = advancedBus.QueueDeclare(queueName);

        //            ////获取员工的审批中待审批的数量 和 我的绩效中待处理的绩效
        //            //advancedBus.Publish(EasyNetQ.Topology.Exchange.GetDefault(), queue.Name, false, new EasyNetQ.Message<ApprovalNotificationDto>(new ApprovalNotificationDto
        //            //{
        //            //    MyApprovalCount = 1,
        //            //    MyPerformanceCount = 20
        //            //}));

        //            var exchange = advancedBus.ExchangeDeclare("HZLExchange", EasyNetQ.Topology.ExchangeType.Fanout);
        //            var queue1 = advancedBus.QueueDeclare("hzl1");
        //            var queue2 = advancedBus.QueueDeclare("hzl2");
        //            advancedBus.Bind(exchange, queue1, queue1.Name);
        //            advancedBus.Bind(exchange, queue2, queue2.Name);
        //            advancedBus.Publish(exchange, "", false, new EasyNetQ.Message<ApprovalNotificationDto>(new ApprovalNotificationDto
        //            {
        //                MyApprovalCount = 10,
        //                MyPerformanceCount = 11
        //            }));


        //        }
        //        bus.Dispose();
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return Success();
        //}
        #endregion

        

        /// <summary>
        /// 获取问题的依赖描述
        /// </summary>
        /// <param name="questions">问题集合</param>
        /// <param name="answers">答案项集合</param>
        /// <param name="targetQuestion">目标问题</param>
        /// <returns></returns>
        private string GetDependDescription(List<QuestionnaireQuestionModel> questions, List<QuestionnaireAnswerModel> answers, QuestionnaireQuestionModel targetQuestion)
        {
            if (!targetQuestion.IsDepend)
            {
                return null;
            }
            if (answers == null)
            {
                return null;
            }
            var dependQuestionModel = questions.FirstOrDefault(a => a.QuestionGuid == targetQuestion.DependQuestion);
            var dependAnswerModel = answers.FirstOrDefault(a => a.AnswerGuid == targetQuestion.DependAnswer);
            return $"*依赖于第{dependQuestionModel.Sort}题的第{dependAnswerModel?.Sort}个选项，当题目{dependQuestionModel.Sort}选择[{dependAnswerModel?.AnswerLabel}]时，此题才显示。";
        }


    }
}
