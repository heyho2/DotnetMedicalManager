using GD.API.Code;
using GD.Common;
using GD.Dtos.Common;
using GD.Dtos.Dictionary;
using GD.Manager.Manager;
using GD.Manager.Merchant;
using GD.Manager.Utility;
using GD.Models.Manager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GD.API.Controllers.System
{
    /// <summary>
    /// 字典控制器
    /// </summary>
    public class DictionaryController : SystemBaseController
    {
        /// <summary>
        /// 字典列表
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetDictionaryPageResponseDto>))]
        public async Task<IActionResult> GetDictionaryPageAsync([FromBody]GetDictionaryPageRequestDto request)
        {
            var response = await new DictionaryBiz().GetDictionaryPageAsync(request);
            return Success(response);
        }
        /// <summary>
        /// 字典树
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<List<GetDictionaryTreeResponseDto>>))]
        public async Task<IActionResult> GetDictionaryTreeAsync([FromBody]GetDictionaryTreeRequestDto request)
        {
            var entityList = await new DictionaryBiz().GetListAsync();
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                entityList = entityList.Where(a => a.ConfigName.Contains(request.Name) || a.TypeName.Contains(request.Name));
            }
            var response = entityList.GetTree(request.Matedata, a => a.ParentGuid, a => a.DicGuid, a => new GetDictionaryTreeResponseDto
            {
                ConfigCode = a.ConfigCode,
                ConfigName = a.ConfigName,
                ExtensionField = a.ExtensionField,
                ParentGuid = a.ParentGuid,
                TypeCode = a.TypeCode,
                TypeName = a.TypeName,
                ValueRange = a.ValueRange,
                ValueType = a.ValueType,
                Enable = a.Enable,
                Sort = a.Sort,
                CreationDate = a.CreationDate,
                DicGuid = a.DicGuid
            });
            return Success(response);
        }
        /// <summary>
        /// 元数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<List<GetDictionaryItemDto>>))]
        public IActionResult GetDictionaryMatedata([FromBody]GetDictionaryTreeRequestDto request)
        {
            var list = new DictionaryBiz().GetDictionaryMatedata();

            var response = list.Select(a => new GetDictionaryItemDto
            {
                Code = a.ConfigCode,
                Name = a.ConfigName,
                Guid = a.DicGuid
            });
            return Success(response);
        }
        /// <summary>
        /// 获取子字典结果集
        /// </summary>
        /// <param name="dictionaryGuid"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetDictionaryItemDto>>))]
        public async Task<IActionResult> GetDictionaryAsync(string dictionaryGuid)
        {
            var list = await new DictionaryBiz().GetListAsync(DictionaryType.MerchantDicConfig, true);

            var response = list.Select(a => new GetDictionaryItemDto
            {
                Code = a.ConfigCode,
                Name = a.ConfigName,
                Guid = a.DicGuid
            });
            return Success(response);
        }
        /// <summary>
        /// 获取商户证书
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetDictionaryItemDto>>))]
        public async Task<IActionResult> GetMerchantDictionaryAsync()
        {
            var list = await new DictionaryBiz().GetListAsync(DictionaryType.MerchantDicConfig, true);

            var response = list.Select(a => new GetDictionaryItemDto
            {
                Code = a.ConfigCode,
                Name = a.ConfigName,
                Guid = a.DicGuid
            });
            return Success(response);
        }
        /// <summary>
        /// 添加Dictionary
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> AddDictionaryAsync([FromBody]AddDictionaryRequestDto request)
        {
            var dictionaryBiz = new DictionaryBiz();
            var entity2 = await dictionaryBiz.GetDictionaryByNameAsync(request.ConfigName);
            if (entity2 != null)
            {
                return Failed(ErrorCode.UserData, "已经存在相同的字典名");
            }
            //var entitys = await dictionaryBiz.GetListByCodeAsync(request.ConfigCode);
            var code = new CodeBiz().GetDictionaryCode("D", 6);
            var entitys = await dictionaryBiz.GetListByCodeAsync(code);
            if (entitys.Count() > 0)
            {
                return Failed(ErrorCode.UserData, "code不能重复");
            }
            var pentity = await dictionaryBiz.GetAsync(request.ParentGuid);
            if (pentity == null)
            {
                return Failed(ErrorCode.UserData, "数据被禁用");
            }
            var model = request.ToModel<DictionaryModel>();
            model.TypeName = pentity?.ConfigName;
            model.TypeCode = pentity?.ConfigCode;
            model.ParentGuid = pentity?.DicGuid;
            model.ConfigCode = code;
            model.ValueRange = string.IsNullOrWhiteSpace(request.ValueRange) ? "{}" : request.ValueRange;
            model.DicGuid = Guid.NewGuid().ToString("N");
            model.CreatedBy = UserID;
            model.LastUpdatedBy = UserID;
            model.OrgGuid = string.Empty;

            var result = await dictionaryBiz.InsertAsync(model);
            if (!result)
            {
                return Failed(ErrorCode.UserData, "添加失败");
            }
            return Success();
        }
        /// <summary>
        /// 修改Dictionary
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> UpdateDictionaryAsync([FromBody]UpdateDictionaryRequestDto request)
        {
            var sss = Converter.GetValues(new DictionaryType());
            //if (sss.Contains(request.DicGuid))
            //{
            //    return Failed(ErrorCode.UserData, "元数据不能编辑");
            //}
            if (request.DicGuid == request.ParentGuid)
            {
                return Failed(ErrorCode.UserData, "父节点不能是自己");
            }
            var dictionaryBiz = new DictionaryBiz();
            if ((await dictionaryBiz.GetDictionaryChildrenAsync(request.DicGuid, request.ParentGuid)) > 0)
            {
                return Failed(ErrorCode.UserData, "父节点不能移动到自己的子节点里面去");
            }
            var entity2 = await dictionaryBiz.GetDictionaryByNameAsync(request.ConfigName);
            if (entity2 != null && entity2.DicGuid != request.DicGuid)
            {
                return Failed(ErrorCode.UserData, "已经存在相同的字典名");
            }
            var entity = await dictionaryBiz.GetAsync(request.DicGuid);
            if (entity == null)
            {
                return Failed(ErrorCode.UserData, "找不到数据");
            }
            if (!entity.Enable)
            {
                return Failed(ErrorCode.UserData, "此数据被禁用");
            }
            if (string.IsNullOrWhiteSpace(request.ParentGuid))
            {
                entity.TypeCode = "matedata";
                entity.TypeName = "元数据";
                entity.ParentGuid = null;
            }
            else
            {
                var pentity = (await dictionaryBiz.GetAsync(request.ParentGuid));
                if (pentity == null)
                {
                    return Failed(ErrorCode.UserData, "父级数据被禁用");
                }
                entity.TypeCode = pentity.ConfigCode;
                entity.TypeName = pentity?.ConfigName ?? "元数据";
                entity.ParentGuid = pentity?.DicGuid;
            }
            entity.ConfigName = request.ConfigName;
            entity.ExtensionField = request.ExtensionField;
            entity.ValueType = request.ValueType;
            entity.ValueRange = string.IsNullOrWhiteSpace(request.ValueRange) ? "{}" : request.ValueRange;
            entity.ExtensionField = request.ExtensionField;
            entity.Sort = request.Sort;
            entity.Enable = request.Enable;
            entity.LastUpdatedDate = DateTime.Now;
            entity.LastUpdatedBy = UserID;
            var result = await dictionaryBiz.UpdateAsync(entity);
            if (!result)
            {
                return Failed(ErrorCode.UserData, "修改失败");
            }
            return Success();
        }
        /// <summary>
        /// 禁用Dictionary
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> DisableEnableDictionaryAsync([FromBody]DisableEnableDictionaryRequestDto request)
        {
            var sss = Converter.GetValues(new DictionaryType());
            if (sss.Contains(request.Guid))
            {
                return Failed(ErrorCode.UserData, "元数据不能编辑");
            }
            DictionaryBiz dictionaryBiz = new DictionaryBiz();
            var entity = await dictionaryBiz.GetAsync(request.Guid);
            if (entity == null)
            {
                return Failed(ErrorCode.UserData, "找不到数据");
            }
            if (entity.ParentGuid == DictionaryType.BusinessScopeDic)
            {
                if (await new MerchantScopeBiz().AnyMerchantScopeAsync(entity.DicGuid))
                {
                    return Failed(ErrorCode.UserData, "经营范围已经使用");
                }
            }
            var entityAll = await dictionaryBiz.GetListAsync();

            var entitys = dictionaryBiz.GetAllSubset(entityAll, request.Guid);
            entitys.Add(entity);
            foreach (var item in entitys)
            {
                item.LastUpdatedBy = UserID;
                item.LastUpdatedDate = DateTime.Now;
                item.Enable = request.Enable;
            }
            var result = await dictionaryBiz.UpdateAsync(entitys);
            if (!result)
            {
                return Failed(ErrorCode.UserData, "修改失败");
            }
            return Success();
        }

        /// <summary>
        /// 删除字典
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> DeleteDictionaryAsync([FromBody]DeleteDictionaryRequestDto request)
        {
            var sss = Converter.GetValues(new DictionaryType());
            if (sss.Contains(request.Guid))
            {
                return Failed(ErrorCode.UserData, "元数据不能删除");
            }
            var dictionaryBiz = new DictionaryBiz();
            var entity = await dictionaryBiz.GetAsync(request.Guid);
            if (entity == null)
            {
                return Failed(ErrorCode.UserData, "找不到数据");
            }
            if (entity.ParentGuid == DictionaryType.BusinessScopeDic)
            {
                if (await new MerchantScopeBiz().AnyMerchantScopeAsync(entity.DicGuid))
                {
                    return Failed(ErrorCode.UserData, "经营范围已经使用");
                }
            }
            var result = await dictionaryBiz.DeleteAsync(request.Guid);
            if (!result)
            {
                return Failed(ErrorCode.UserData, "删除失败");
            }
            return Success();
        }

        /// <summary>
        /// 经营范围字典列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<List<GetDictionaryTreeResponseDto>>))]
        public async Task<IActionResult> GetBusinessScopePageAsync([FromBody]GetDictionaryTreeRequestDto request)
        {
            request.Matedata = DictionaryType.BusinessScopeDic;
            var response = await GetDictionaryTreeAsync(request);
            return response;
        }
        /// <summary>
        /// 修改经营范围Dictionary
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> UpdateBusinessScopeAsync([FromBody]UpdateDictionaryRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.ParentGuid))
            {
                request.ParentGuid = DictionaryType.BusinessScopeDic;
            }
            var response = await UpdateDictionaryAsync(request);
            return response;
        }
        /// <summary>
        /// 添加经营范围Dictionary
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> AddBusinessScopeAsync([FromBody]AddDictionaryRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.ParentGuid))
            {
                request.ParentGuid = DictionaryType.BusinessScopeDic;
            }
            var response = await AddDictionaryAsync(request);
            return response;
        }
        /// <summary>
        /// 经营范围类型
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<SelectTreeItemDto>>))]
        public async Task<IActionResult> GetBusinessScopeTypeAsync()
        {
            var list = await new DictionaryBiz().GetListAsync(enable: true);
            var response = list.GetTree(DictionaryType.BusinessScopeDic, a => a.ParentGuid, a => a.DicGuid, a => new SelectTreeItemDto
            {
                Guid = a.DicGuid,
                Name = a.ConfigName,
                Code = a.ConfigCode
            });
            return Success(response);
        }
        /// <summary>
        /// 字典下拉框
        /// /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<SelectTreeItemDto>>))]
        public async Task<IActionResult> GetDictionaryTreeTypeAsync()
        {
            var list = await new DictionaryBiz().GetListAsync(enable: true);
            var response = list.GetTree(null, a => a.ParentGuid, a => a.DicGuid, a => new SelectTreeItemDto
            {
                Guid = a.DicGuid,
                Name = a.ConfigName,
                Code = a.ConfigCode
            });
            return Success(response);
        }
        /// <summary>
        /// 问答设置数据
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetFaqsSettingItemDto>>))]
        public async Task<IActionResult> GetFaqsSettingTypeAsync()
        {
            var list = await new DictionaryBiz().GetListAsync(DictionaryType.FaqsSetting, true);
            var response = list.Select(a => new GetFaqsSettingItemDto
            {
                ConfigName = a.ConfigName,
                DicGuid = a.DicGuid,
                ExtensionField = a.ExtensionField,
                ValueRange = a.ValueRange,
                ValueType = a.ValueType,
                ParentGuid = a.ParentGuid,
                Sort = a.Sort,
            });
            return Success(response);
        }
        /// <summary>
        /// 保存问答设置
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<List<GetFaqsSettingItemDto>>))]
        public async Task<IActionResult> SaveFaqsSettingAsync([FromBody]SaveFaqsSettingRequestDto[] request)
        {
            var dictionaryBiz = new DictionaryBiz();
            var list = await dictionaryBiz.GetListAsync(request.Select(a => a.DicGuid));
            foreach (var item in list)
            {
                item.ExtensionField = request.FirstOrDefault(a => a.DicGuid == item.DicGuid)?.ExtensionField;
                item.LastUpdatedDate = DateTime.Now;
                item.LastUpdatedBy = UserID;
            }
            var response = await dictionaryBiz.UpdateAsync(list);
            return Success(response);
        }
        
        /// <summary>
        /// ceshi
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<List<GetFaqsSettingItemDto>>)), AllowAnonymous]
        public IActionResult SaveFaqsSetting2(string postid,string temp,string phone,string type)
        {
            Random rd = new Random();
            int i = rd.Next();
            using (var client = new HttpClient())
            {
                List<KeyValuePair<string, string>> paramList = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("postid",postid),
                    new KeyValuePair<string, string>("id", "1"),
                    new KeyValuePair<string, string>("temp", temp),
                    new KeyValuePair<string, string>("type", type),
                    new KeyValuePair<string, string>("platform", "MWWW"),
                    new KeyValuePair<string, string>("token", ""),
                    new KeyValuePair<string, string>("phone", phone),
                    new KeyValuePair<string, string>("valicode", "")
                };
                HttpContent httpContent = new FormUrlEncodedContent(paramList);
                //设置Http的内容标头
                httpContent.Headers.Add("contenttype", $"application/x-www-form-urlencoded");
                //设置Http的内容标头的字符
                httpContent.Headers.ContentType.CharSet = "utf-8";


                var httpResponse = client.PostAsync("https://m.kuaidi100.com/query", new FormUrlEncodedContent(paramList)).Result;
                //确保Http响应成功
                if (httpResponse.IsSuccessStatusCode)
                {
                    //异步读取json
                   var result = httpResponse.Content.ReadAsStringAsync().Result;

                    return Success(result);
                }
                return Success(null);

            }
        }
    }
}
