using GD.API.Code;
using GD.Common;
using GD.Consumer;
using GD.Dtos.Consumer.Consumer;
using GD.Dtos.Consumer.ServiceMember;
using GD.Models.Consumer;
using GD.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GD.API.Controllers.Consumer
{
    /// <summary>
    /// 服务对象控制器
    /// </summary>
    public class ServiceMemberController : ConsumerBaseController
    {
        /// <summary>
        /// 获取用户服务对象成员列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<ServiceMemberDto>>))]
        public async Task<IActionResult> GetServiceMemberListAsync()
        {
            var models = await new ServiceMemberBiz().GetServiceMemberListAsync(UserID);
            var result = models.Select(item => item.ToDto<ServiceMemberDto>());
            return Success(result);
        }

        /// <summary>
        /// 创建/修改服务对象成员数据请求
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> CreateEidtServiceMemberAsync([FromBody]CreateEidtServiceMemberRequestDto requestDto)
        {
            var biz = new ServiceMemberBiz();
            //是否为创建
            var isCreate = string.IsNullOrWhiteSpace(requestDto.ServiceMemberGuid);

            var model = requestDto.ToModel<ServiceMemberModel>();
            if (isCreate)
            {
                model.ServiceMemberGuid = Guid.NewGuid().ToString("N");
                model.UserGuid = UserID;
                model.CreatedBy = UserID;
                model.CreationDate = DateTime.Now;
                model.OrgGuid = string.Empty;
                model.Enable = true;
            }
            else
            {
                var tmpModel = await biz.GetAsync(requestDto.ServiceMemberGuid);
                model.CreatedBy = tmpModel.CreatedBy;
                model.CreationDate = tmpModel.CreationDate;
                model.OrgGuid = tmpModel.OrgGuid;
                model.Enable = tmpModel.Enable;
                model.UserGuid = tmpModel.UserGuid;
            }
            model.LastUpdatedBy = UserID;
            model.LastUpdatedDate = DateTime.Now;
            var result = await biz.CreateEidtServiceMemberAsync(model, isCreate);
            return result ? Success() : Failed(ErrorCode.DataBaseError, $@"{(isCreate ? "创建" : "编辑")}服务对象失败");

        }

        /// <summary>
        /// 删除服务对象成员记录
        /// </summary>
        /// <param name="serviceMemberId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DeleteServiceMemberAsync(string serviceMemberId)
        {
            var biz = new ServiceMemberBiz();
            var model = await biz.GetAsync(serviceMemberId);
            if (model == null)
            {
                return Failed(ErrorCode.UserData, "未查询到该记录");
            }
            model.Enable = false;
            model.LastUpdatedBy = UserID;
            model.LastUpdatedDate = DateTime.Now;
            var result = await biz.UpdateAsync(model);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "删除服务对象成员记录失败");
        }

        /// <summary>
        /// 获取用户账号下服务对象列表
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetUserServiceMembersResponseDto>>))]
        public async Task<IActionResult> GetUserServiceMembersAsync(string phone)
        {
            if (string.IsNullOrEmpty(phone) || phone.Length != 11)
            {
                return Failed(ErrorCode.UserData, "请输入正确的手机号");
            }

            var regex = new Regex("^[0-9]+$");
            if (!regex.IsMatch(phone))
            {
                return Failed(ErrorCode.UserData, "请输入正确的手机号");
            }

            var userModel = new UserBiz().GetUserByPhone(phone);
            if (userModel == null)
            {
                return Failed(ErrorCode.UserData, "该手机号未注册");
            }

            var biz = new ServiceMemberBiz();

            return Success(await biz.GetUserServiceMembersAsync(phone));
        }


    }
}
