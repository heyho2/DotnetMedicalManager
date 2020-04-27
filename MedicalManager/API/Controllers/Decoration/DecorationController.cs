using GD.API.Code;
using GD.AppSettings;
using GD.Common;
using GD.Dtos.Decoration;
using GD.Manager.Decoration;
using GD.Models.Decoration;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static GD.Dtos.Decoration.DecorationDto;

namespace GD.API.Controllers.Decoration
{
    /// <summary>
    /// 装修模块控制器
    /// </summary>
    public class DecorationController : DecorationBaseController
    {
        /// <summary>
        /// 获取装修记录分页列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetDecorationPageListResponseDto>))]
        public async Task<IActionResult> GetDecorationPageListAsync([FromQuery]GetDecorationPageListRequestDto requestDto)
        {
            var result = await new DecorationBiz().GetDecorationPageListAsync(requestDto);
            return Success(result);
        }

        /// <summary>
        /// 获取装修记录分类列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetDecorationClassificationResponseDto>>))]
        public async Task<IActionResult> GetDecorationClassificationAsync()
        {
            var models = await new DecorationClassificationBiz().GetListAsync(true);
            var result = models.Select(a => a.ToDto<GetDecorationClassificationResponseDto>());
            return Success(result);
        }

        /// <summary>
        /// 获取装修记录详情
        /// </summary>
        /// <param name="decorationGuid">装修记录guid</param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<DecorationDto>))]
        public async Task<IActionResult> GetDecorationDetailsAsync(string decorationGuid)
        {
            var decorationBiz = new DecorationBiz();
            var result = new DecorationDto();
            var decorationModel = await decorationBiz.GetAsync(decorationGuid);
            if (decorationModel == null)
            {
                return Failed(ErrorCode.UserData, "未查询到装修记录数据");
            }
            var classificationModel = await new DecorationClassificationBiz().GetAsync(decorationModel.ClassificationGuid);
            result.DecorationGuid = decorationModel.DecorationGuid;
            result.DecorationName = decorationModel.DecorationName;
            result.ClassificationGuid = decorationModel.ClassificationGuid;
            result.RuleMode = classificationModel.RuleMode.ToEnum<RuleModeEnum>();
            var ruleModels = await new DecorationRuleConfigBiz().GetRulesByClassificationAsync(decorationModel.ClassificationGuid);
            result.DecorationRules = ruleModels.Select(a => a.ToDto<DecorationRule>()).ToList();
            var content = await decorationBiz.GetDecorationContentAsync(decorationGuid);
            result.Rows = string.IsNullOrWhiteSpace(content) ? new List<DecorationRow>() : JsonConvert.DeserializeObject<List<DecorationRow>>(content);
            return Success(result);
        }

        /// <summary>
        /// 根据类别获取装修规则设置
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetRuleConfigResponseDto>))]
        public async Task<IActionResult> GetRuleConfigAsync(string classificationGuid)
        {
            var classificationModel = await new DecorationClassificationBiz().GetAsync(classificationGuid);
            if (classificationModel == null)
            {
                return Failed(ErrorCode.UserData, "未查询到分类数据");
            }
            var ruleModels = await new DecorationRuleConfigBiz().GetRulesByClassificationAsync(classificationModel.ClassificationGuid);
            var result = new GetRuleConfigResponseDto
            {
                RuleMode = classificationModel.RuleMode.ToEnum<RuleModeEnum>(),
                DecorationRules = ruleModels.Select(a => a.ToDto<DecorationRule>()).ToList()
            };
            return Success(result);

        }

        /// <summary>
        /// 创建/修改装修记录
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> CreateDecorationRecordAsync([FromBody]CreateDecorationRecordRequestDto requestDto)
        {

            if (!requestDto.Rows.Any())
            {
                return Failed(ErrorCode.UserData, "内容不可为空");
            }
            var biz = new DecorationBiz();
            var model = new DecorationModel
            {
                DecorationGuid = Guid.NewGuid().ToString("N"),
                DecorationName = requestDto.DecorationName,
                ClassificationGuid = requestDto.ClassificationGuid,
                Content = JsonConvert.SerializeObject(requestDto.Rows),
                Sort = 100,
                CreatedBy = UserID,
                CreationDate = DateTime.Now,
                LastUpdatedBy = UserID,
                LastUpdatedDate = DateTime.Now,
                OrgGuid = string.Empty
            };
            if (!string.IsNullOrWhiteSpace(requestDto.DecorationGuid))
            {
                model = await biz.GetAsync(requestDto.DecorationGuid);
                model.DecorationName = requestDto.DecorationName;
                model.LastUpdatedBy = UserID;
                model.LastUpdatedDate = DateTime.Now;
                model.Content = JsonConvert.SerializeObject(requestDto.Rows);
            }
            var result = true;
            if (!string.IsNullOrWhiteSpace(requestDto.DecorationGuid))
            {
                result = await biz.UpdateAsync(model);
            }
            else
            {
                result = await biz.InsertAsync(model);
            }
            return result ? Success() : Failed(ErrorCode.DataBaseError);
        }

        /// <summary>
        /// 修改装修记录启用状态接口
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> DisableEnableDecorationAsync([FromBody]DisableEnableDecorationRequestDto request)
        {
            var result = await new DecorationBiz().DisableEnableAsync(request.DecorationGuid, request.Status, UserID);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "修改启用状态失败");
        }

    }
}
