using GD.Common;
using GD.Decoration;
using GD.Dtos.Decoration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace GD.API.Controllers.Decoration
{
    /// <summary>
    /// 装修模块控制器
    /// </summary>
    public class DecorationController : DecorationBaseController
    {
        /// <summary>
        /// 获取装修记录内容
        /// </summary>
        /// <param name="decorationGuid"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetDecorationResponseDto>)), AllowAnonymous]
        public async Task<IActionResult> GetDecorationContentAsync(string decorationGuid)
        {
            var decoationModel = await new DecorationBiz().GetAsync(decorationGuid);
            var content = await new DecorationBiz().GetDecorationContentAsync(decorationGuid);
            var contents = JsonConvert.DeserializeObject<List<GetDecorationContentResponseDto>>(content);
            var result = ClearEmptyRow(contents);
            return Success(new GetDecorationResponseDto
            {
                Title = decoationModel?.DecorationName,
                Contents = result
            });
        }

        /// <summary>
        /// 清除空行
        /// </summary>
        /// <param name="contents"></param>
        /// <returns></returns>
        private List<GetDecorationContentResponseDto> ClearEmptyRow(List<GetDecorationContentResponseDto> contents)
        {
            var result = new List<GetDecorationContentResponseDto>();
            for (int i = 0; i < contents.Count; i++)
            {
                var item = contents[i];
                if (item.Columns.FirstOrDefault(a => a.Picture != null) != null)
                {
                    result.Add(item);
                }
            }
            return result;
        }

        /// <summary>
        /// 获取云购首页banner装修记录内容
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetDecorationContentResponseDto>>)), AllowAnonymous]
        public async Task<IActionResult> GetHomeBannersContentAsync()
        {
            string decorationGuid = "65bbf0bcb4d111e986ac00163e0c4296";
            var content = await new DecorationBiz().GetDecorationContentAsync(decorationGuid);
            var contents = JsonConvert.DeserializeObject<List<GetDecorationContentResponseDto>>(content);
            var result = ClearEmptyRow(contents);
            return Success(result);
        }

        /// <summary>
        /// 获取云购分类入口装修记录内容
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetDecorationContentResponseDto>>)), AllowAnonymous]
        public async Task<IActionResult> GetClassificationEntranceContentAsync()
        {
            string decorationGuid = "bd786652b4d111e986ac00163e0c4296";
            var content = await new DecorationBiz().GetDecorationContentAsync(decorationGuid);
            var contents = JsonConvert.DeserializeObject<List<GetDecorationContentResponseDto>>(content);
            var result = ClearEmptyRow(contents);
            return Success(result);
        }

        /// <summary>
        /// 获取云购即时活动装修记录内容
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetDecorationContentResponseDto>>)), AllowAnonymous]
        public async Task<IActionResult> GetInstantActivityContentAsync()
        {
            string decorationGuid = "e9c8708eb4d111e986ac00163e0c4296";
            var content = await new DecorationBiz().GetDecorationContentAsync(decorationGuid);
            var contents = JsonConvert.DeserializeObject<List<GetDecorationContentResponseDto>>(content);
            var result = ClearEmptyRow(contents);
            return Success(result);
        }
    }
}
