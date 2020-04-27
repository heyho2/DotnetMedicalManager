using GD.API.Code;
using GD.Common;
using GD.Common.EnumDefine;
using GD.Component;
using GD.Consumer;
using GD.DataAccess;
using GD.Doctor;
using GD.Dtos.Admin.Article;
using GD.Dtos.Manager.Banner;
using GD.Dtos.Utility.Article;
using GD.Manager;
using GD.Models.Manager;
using GD.Models.Utility;
using GD.Module;
using GD.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GD.API.Controllers.Utility
{
    /// <inheritdoc />
    /// <summary>
    /// 文章控制器
    /// </summary>
    public class ArticleController : UtilityBaseController
    {
        /// <summary>
        /// 职业病预防文章
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(List<ResponseDto<GetCcupationalDiseasePreventionResponseDto>>)), AllowAnonymous]
        public IActionResult GetCupationalDiseasePrevention([FromBody]GetCcupationalDiseasePreventionRequestDto dto)
        {
            ArticleBiz articleBiz = new ArticleBiz();
            var condition = "where article_type_dic=@article_type_dic and visible=true and enable=true ";
            var lst = articleBiz.GetArticles(dto.PageNumber, dto.PageSize, condition, "creation_date desc", new { article_type_dic = DictionaryType.OccupationalDiseasePrevent });
            AccessoryBiz accessoryBiz = new AccessoryBiz();
            var dtos = new List<GetCcupationalDiseasePreventionResponseDto>();
            foreach (var item in lst)
            {
                var model = item.ToDto<GetCcupationalDiseasePreventionResponseDto>();
                var picture = MySqlHelper.GetModelById<AccessoryModel>(item.PictureGuid);
                if (picture != null)
                {
                    model.Picture = $"{ picture?.BasePath}{picture?.RelativePath}";
                }
                dtos.Add(model);
            }
            if (dtos.Count == 0)
            {
                return Failed(ErrorCode.Empty);
            }
            return Success(dtos);
        }

        /// <summary>
        /// 职业病常识文章
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(List<ResponseDto<GetCcupationalDiseaseKnowledgeResponseDto>>)), AllowAnonymous]
        public IActionResult GetCupationalDiseaseKnowledge([FromBody]GetCcupationalDiseaseKnowledgeRequestDto dto)
        {
            ArticleBiz articleBiz = new ArticleBiz();
            var condition = "where article_type_dic=@article_type_dic and visible=true and enable=true ";
            var lst = articleBiz.GetArticles(dto.PageNumber, dto.PageSize, condition, "creation_date desc", new { article_type_dic = DictionaryType.OccupationalDiseaseKnowledge });
            var accessoryBiz = new AccessoryBiz();
            var dtos = new List<GetCcupationalDiseaseKnowledgeResponseDto>();
            foreach (var item in lst)
            {
                var model = item.ToDto<GetCcupationalDiseaseKnowledgeResponseDto>();
                var picture = MySqlHelper.GetModelById<AccessoryModel>(item.PictureGuid);
                model.Picture = $"{picture?.BasePath}{picture?.RelativePath}";
                dtos.Add(model);
            }
            if (dtos.Count == 0)
            {
                return Failed(ErrorCode.Empty);
            }
            return Success(dtos);
        }

        /// <summary>
        /// 获取文章类型
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetArticleTypeResponseDto>>)), AllowAnonymous]
        public IActionResult GetArticleType()
        {
            var dictionaryBiz = new DictionaryBiz();
            var scopeMetadataDic = dictionaryBiz.GetModelById(DictionaryType.ArticleTypeConfig);
            if (scopeMetadataDic == null)
            {
                return Failed(ErrorCode.Empty, "缺乏文章类型元数据配置项");
            }
            var dics = dictionaryBiz.GetListByParentGuid(scopeMetadataDic.DicGuid);
            if (!dics.Any()) return Failed(ErrorCode.Empty, "不存在配置项");
            var lstDic = dics.Select(dic => new GetArticleTypeResponseDto
            {
                DicGuid = dic.DicGuid,
                ConfigCode = dic.ConfigCode,
                ConfigName = dic.ConfigName,
                Sort = dic.Sort
            }).OrderBy(a => a.Sort).ToList();

            return Success(lstDic);
        }

        /// <summary>
        /// 获取用户文章列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<List<GetUserArticlesResponseDto>>)), AllowAnonymous]
        public IActionResult GetUserArticles([FromBody]GetUserArticleRequestDto requestDto)
        {
            ArticleBiz articleBiz = new ArticleBiz();
            var sourceTypeWhere = string.Empty;
            if (requestDto.SourceType != null)
            {
                sourceTypeWhere = $"and source_type='{requestDto.SourceType.Value.ToString()}'";
            }
            var models = articleBiz.GetArticles(requestDto.PageIndex, requestDto.PageSize, $"where author_guid=@author_guid and actcle_release_status='Release' {sourceTypeWhere}", "last_updated_date desc", new { author_guid = requestDto.AuthorGuid });
            if (models == null)
            {
                return Failed(ErrorCode.Empty);
            }
            var accessoryBiz = new AccessoryBiz();
            var dictionaryBiz = new DictionaryBiz();
            var responseDtos = new List<GetUserArticlesResponseDto>();
            foreach (var model in models)
            {
                var dto = model.ToDto<GetUserArticlesResponseDto>();
                var accessoryModel = accessoryBiz.GetAccessoryModelByGuid(model.PictureGuid);
                dto.Picture = $"{accessoryModel?.BasePath}{accessoryModel?.RelativePath}";
                dto.ArticleType = dictionaryBiz.GetModelById(model.ArticleTypeDic)?.ConfigName;
                dto.PageView = new ArticleViewBiz().CountNumByTargetIDAsync(model.ArticleGuid).Result;
                responseDtos.Add(dto);
            }
            return Success(responseDtos);
        }

        /// <summary>
        /// 获取首页头条
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<List<GetHomeHeadlineItemDto>>)), AllowAnonymous]
        public async Task<IActionResult> GetHomeHeadlineAsync([FromBody]GetHomeHeadlineRequestDto requestDto)
        {
            var response = await new ConsumerBiz().GetHomeHeadlineAsync(requestDto);
            return Success(response);
        }

        /// <summary>
        /// 搜索文章
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<SearchArticleResponseDto>)), AllowAnonymous]
        public async Task<IActionResult> SearchArticleAsync([FromBody]SearchArticleRequestDto requestDto)
        {
            CommonBiz commonBiz = new CommonBiz();
            if (!string.IsNullOrEmpty(UserID))
            {
                commonBiz.SearchHistory(UserID, requestDto.Keyword);
            }
            commonBiz.HotWordSearch(requestDto.Keyword);

            var response = await new ConsumerBiz().SearchArticleAsync(requestDto);
            return Success(response);
        }

        /// <summary>
        /// 获取文章列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetArticleListResponseDto>))]
        public async Task<IActionResult> GetArticleListAsync([FromBody]GetArticleListRequestDto request)
        {
            var response = await new ConsumerBiz().GetArticleListAsync(UserID, request);
            return Success(response);
        }

        /// <summary>
        /// 添加文章
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<string>))]
        public async Task<IActionResult> AddArticleAsync([FromBody]AddArticleRequestDto request)
        {
            var articleGuid = Guid.NewGuid().ToString("N");
            var textGuid = Guid.NewGuid().ToString("N");
            RichtextModel richtextModel = new RichtextModel
            {
                Content = request.Content,
                CreatedBy = UserID,
                CreationDate = DateTime.Now,
                Enable = true,
                LastUpdatedBy = UserID,
                LastUpdatedDate = DateTime.Now,
                OrgGuid = string.Empty,
                OwnerGuid = articleGuid,
                TextGuid = textGuid,
            };

            ArticleModel articleModel = new ArticleModel
            {
                Abstract = request.Abstract,
                ArticleGuid = articleGuid,
                ArticleTypeDic = request.ArticleTypeDic,
                AuthorGuid = UserID,
                ContentGuid = textGuid,
                CreatedBy = UserID,
                CreationDate = DateTime.Now,
                Enable = true,
                LastUpdatedBy = UserID,
                LastUpdatedDate = DateTime.Now,
                OrgGuid = string.Empty,
                PlatformType = PlatformType.CloudDoctor.ToString(),
                Sort = 1,
                Title = request.Title,
                Visible = request.Visible,
                PictureGuid = request.PictureGuid,
                ActcleReleaseStatus = Enum.Parse<ReleaseStatus>(request.ActcleReleaseStatus)
            };

            var response = await new ArticleBiz().AddAsync(richtextModel, articleModel);
            if (response)
            {
                //发布时才添加积分
                if (articleModel.ActcleReleaseStatus == ReleaseStatus.Release)
                {
                    new DoctorActionBiz().AddArticleAsync(this.UserID);
                }
                return Success(articleModel.ArticleGuid, null);
            }
            else
            {
                return Failed(ErrorCode.DataBaseError, "添加失败");
            }
        }

        /// <summary>
        /// 修改文章
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<bool>))]
        public async Task<IActionResult> UpdateArticleAsync([FromBody]UpdateArticleRequestDto request)
        {
            var articleBiz = new ArticleBiz();
            var contentBiz = new RichtextBiz();
            var articleModel = await articleBiz.GetAsync(request.ArticleGuid);
            if (articleModel == null)
            {
                return Failed(ErrorCode.DataBaseError, "数据错误");
            }

            var richtextModel = await contentBiz.GetAsync(articleModel.ContentGuid);

            richtextModel.Content = request.Content;
            richtextModel.LastUpdatedBy = UserID;
            richtextModel.LastUpdatedDate = DateTime.Now;
            richtextModel.OrgGuid = string.Empty;
            richtextModel.OwnerGuid = request.ArticleGuid;

            articleModel.Abstract = request.Abstract;
            articleModel.ArticleTypeDic = request.ArticleTypeDic;
            articleModel.LastUpdatedBy = UserID;
            articleModel.LastUpdatedDate = DateTime.Now;
            articleModel.Sort = 1;
            articleModel.Title = request.Title;
            articleModel.Visible = request.Visible;
            articleModel.PictureGuid = request.PictureGuid;
            articleModel.ActcleReleaseStatus = Enum.Parse<ReleaseStatus>(request.ActcleReleaseStatus);

            var response = await new ArticleBiz().UpdateAsync(richtextModel, articleModel);
            if (!response)
            {
                return Failed(ErrorCode.DataBaseError, "修改失败");
            }
            //发布时才添加积分
            if (articleModel.ActcleReleaseStatus == ReleaseStatus.Release)
            {
                new DoctorActionBiz().AddArticleAsync(this.UserID);
            }
            return Success(response);
        }

        /// <summary>
        /// 获取文章信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetArticleInfoResponseDto>)), AllowAnonymous]
        public IActionResult GetArticleInfoAsync([FromBody]GetArticleInfoRequestDto request)
        {
            var articleBiz = new ArticleBiz();
            var contentBiz = new RichtextBiz();
            AccessoryBiz accessoryBiz = new AccessoryBiz();
            CollectionBiz collection = new CollectionBiz();
            var articleModel = articleBiz.GetModel(request.ArticleGuid);
            if (articleModel == null)
            {
                return Failed(ErrorCode.DataBaseError, "数据错误");
            }
            var richtextModel = contentBiz.GetModel(articleModel.ContentGuid);
            var accessory = accessoryBiz.GetAccessoryModelByGuid(articleModel.PictureGuid);
            var likeCount = new LikeBiz().GetLikeNumByTargetGuid(articleModel.ArticleGuid);
            var pageViewCount = new ArticleViewBiz().CountNumByTargetIDAsync(articleModel.ArticleGuid).Result;
            int collectionCount = collection.GetListCountByTarget(articleModel.ArticleGuid);
            return Success(new GetArticleInfoResponseDto
            {
                ArticleTypeDic = articleModel.ArticleTypeDic,
                Abstract = articleModel.Abstract,
                Content = richtextModel?.Content,
                PictureGuid = articleModel.PictureGuid,
                Title = articleModel.Title,
                Visible = articleModel.Visible,
                PictureUrl = $"{accessory?.BasePath}{accessory?.RelativePath}",
                ArticleGuid = articleModel.ArticleGuid,
                ActcleReleaseStatus = articleModel.ActcleReleaseStatus.ToString(),
                CreationDate = articleModel.CreationDate,
                LikeCount = likeCount,
                VisitCount = pageViewCount,
                Collection = collectionCount
            });
        }

        /// <summary>
        /// 是否可阅读
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<int>))]
        public async Task<IActionResult> SetArticleVisibleAsync([FromBody]SetArticleVisibleRequestDto request)
        {
            var articleBiz = new ArticleBiz();
            var articleModel = await articleBiz.GetAsync(request.ArticleGuid);
            if (articleModel == null)
            {
                return Failed(ErrorCode.DataBaseError, "数据错误");
            }
            articleModel.Visible = request.Visible;
            var response = await articleBiz.UpdateArticleAsync(articleModel);
            if (response == 0)
            {
                return Failed(ErrorCode.DataBaseError, "跟新失败");
            }
            return Success();
        }

        /// <summary>
        /// 移除文章
        /// </summary>
        /// <param name="articleId">文章Id</param>
        /// <returns>操作是否成功</returns>
        [HttpGet, Produces(typeof(ResponseDto<bool>))]
        public async Task<IActionResult> RemoveArticleAsync(string articleId)
        {
            var articleBiz = new ArticleBiz();
            var articleModel = await articleBiz.GetAsync(articleId);
            if (articleModel == null)
            {
                return Failed(ErrorCode.Empty, "未查询到该文章数据");
            }
            articleModel.LastUpdatedBy = UserID;
            articleModel.LastUpdatedDate = DateTime.Now;
            var result = await articleBiz.DeleteArticleAsync(articleModel);
            if (result && articleModel.ActcleReleaseStatus == ReleaseStatus.Release)
            {
                new DoctorActionBiz().DeleteArticleAsync(this.UserID);
            }
            return Success(result);

        }

        /// <summary>
        /// 获取客户端综合文章分页列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetClientArticlePageListResponseDto>)), AllowAnonymous]
        public async Task<IActionResult> GetClientArticlePageListAsync([FromBody]GetClientArticlePageListRequestDto requestDto)
        {
            var response = await new UnionArticleBiz().GetClientArticlePageListAsync(requestDto);
            return Success(response);
        }

        /// <summary>
        /// 获取客户端综合文章详情
        /// </summary>
        /// <param name="articleGuid"></param>
        /// <param name="articleSource"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(GetClientArticleDetailResponseDto)), AllowAnonymous]
        public async Task<IActionResult> GetClientArticleDetailAsync(string articleGuid, ArticleModel.ArticleSourceTypeEnum articleSource = ArticleModel.ArticleSourceTypeEnum.Doctor)
        {
            if (string.IsNullOrWhiteSpace(articleGuid))
            {
                return Failed(ErrorCode.UserData, "文章Id articleGuid 不可为空");
            }


            var response = new GetClientArticleDetailResponseDto();
            ArticleBiz articleBiz = new ArticleBiz();
            var model = articleBiz.GetModel(articleGuid);
            if (model == null)
            {
                return Failed(ErrorCode.Empty);
            }
            var doctorBiz = new DoctorBiz();
            var accessoryBiz = new AccessoryBiz();
            var userBiz = new UserBiz();
            var likeBiz = new LikeBiz();
            var richtextBiz = new RichtextBiz();

            response.ArticleGuid = model.ArticleGuid;
            response.Title = model.Title;
            response.AuthorGuid = model.AuthorGuid;
            response.LastUpdatedDate = model.LastUpdatedDate;
            response.Content = richtextBiz.GetModel(model.ContentGuid)?.Content;
            response.LikeNumber = likeBiz.GetLikeNumByTargetGuid(articleGuid);
            response.Liked = likeBiz.GetLikeState(UserID, articleGuid);
            if (articleSource == ArticleModel.ArticleSourceTypeEnum.Doctor)
            {
                response.AuthorName = userBiz.GetUser(model.AuthorGuid)?.UserName;
                var doctorModel = doctorBiz.GetDoctor(model.AuthorGuid);
                if (doctorModel != null)
                {
                    var accessoryModel = accessoryBiz.GetAccessoryModelByGuid(doctorModel.PortraitGuid);
                    response.AuthorPortrait = accessoryModel?.BasePath + accessoryModel?.RelativePath;
                    response.HospitalName = doctorModel.HospitalName;
                    response.OfficeName = doctorModel.OfficeName;
                }
            }
            else if (articleSource == ArticleModel.ArticleSourceTypeEnum.Manager)
            {
                response.AuthorName = (await new ManagerAccountBiz().GetAsync(model.AuthorGuid))?.UserName;
            }
            else
            {
                return Failed(ErrorCode.UserData, $"文章来源 articleSource:{articleSource.ToString()} 数据值非法");
            }
            return Success(response);
        }

        /// <summary>
        /// 获取客户端推荐综合文章分页列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetClientRecommandArticleListResponseDto>)), AllowAnonymous]
        public async Task<IActionResult> GetClientRecommandArticleListAsync([FromBody]GetClientRecommandArticleListRequestDto requestDto)
        {
            var response = await new UnionArticleBiz().GetClientRecommandArticleListAsync(requestDto);
            return Success(response);
        }


    }
}