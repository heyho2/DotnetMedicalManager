using GD.API.Code;
using GD.Common;
using GD.Dtos.Health;
using GD.Manager.Health;
using GD.Models.Health;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace GD.API.Controllers.Health
{
    /// <summary>
    /// 健康管理师控制器
    /// </summary>
    public class HealthManagerController : HealthBaseController
    {
        /// <summary>
        /// 添加健康管理师
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> Create([FromBody] CreateHealthManagerRequestDto request)
        {
            if (request.QualificationCertificateGuids.Count <= 0)
            {
                return Failed(ErrorCode.Empty, "职业资格证书未上传");
            }

            var managerBiz = new HealthManagerBiz();

            var checkPhone = await managerBiz.CheckPhone(request.Phone);
            if (checkPhone)
            {
                return Failed(ErrorCode.Empty, $"手机号【{request.Phone}】已存在");
            }

            var checkJobNumber = await managerBiz.CheckJobNumber(request.JobNumber);
            if (checkJobNumber)
            {
                return Failed(ErrorCode.Empty, $"工号【{request.JobNumber}】已存在");
            }

            var model = request.ToModel<HealthManagerModel>();
            model.ManagerGuid = Guid.NewGuid().ToString("N");
            model.Enable = request.Enable;
            model.QualificationCertificateGuid = JsonConvert.SerializeObject(request.QualificationCertificateGuids);
            model.CreatedBy = UserID;
            model.LastUpdatedBy = UserID;
            model.OrgGuid = "";

            var result = await managerBiz.InsertAsync(model);
            if (!result)
            {
                return Failed(ErrorCode.Empty, "添加健康管理师失败，请稍后重试");
            }

            return Success();
        }

        /// <summary>
        /// 获取指定健康管理师信息
        /// </summary>
        /// <param name="managerGuid"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetHealthManagerResponseDto>))]
        public async Task<IActionResult> Get(string managerGuid)
        {
            var managerBiz = new HealthManagerBiz();

            var result = await managerBiz.GetManagerInfoAsync(managerGuid);
            return Success(result);
        }

        /// <summary>
        /// 更新健康管理师
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> Update([FromBody] UpdateHealthManagerRequestDto request)
        {
            if (request.QualificationCertificateGuids.Count <= 0)
            {
                return Failed(ErrorCode.Empty, "职业资格证书未上传");
            }

            var managerBiz = new HealthManagerBiz();
            var checkManager = await managerBiz.GetAsync(request.ManagerGuid);
            if (checkManager is null)
            {
                return Failed(ErrorCode.Empty, $"健康管理师【{request.UserName}】不存在");
            }

            var checkPhone = await managerBiz.CheckPhone(request.Phone, request.ManagerGuid);
            if (checkPhone)
            {
                return Failed(ErrorCode.Empty, $"手机号【{request.Phone}】已存在");
            }

            var checkJobNumber = await managerBiz.CheckJobNumber(request.JobNumber, request.ManagerGuid);
            if (checkJobNumber)
            {
                return Failed(ErrorCode.Empty, $"工号【{request.JobNumber}】已存在");
            }

            var model = request.ToModel<HealthManagerModel>();
            model.CreatedBy = checkManager.CreatedBy;
            model.Enable = request.Enable;
            model.CreationDate = checkManager.CreationDate;
            model.QualificationCertificateGuid = JsonConvert.SerializeObject(request.QualificationCertificateGuids);
            model.LastUpdatedBy = UserID;
            model.OrgGuid = "";
            model.LastUpdatedDate = DateTime.Now;

            var result = await managerBiz.UpdateAsync(model);
            if (!result)
            {
                return Failed(ErrorCode.Empty, "更新健康管理师失败，请稍后重试");
            }

            return Success();
        }

        /// <summary>
        /// 查询健康管理师分页列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetHealthManagerListResponseDto>))]
        public async Task<IActionResult> GetHealthManagers([FromQuery]
        GetHealthManagerListRequestDto request)
        {
            var managerBiz = new HealthManagerBiz();

            var response = await managerBiz.GetHealthManagers(request);

            return Success(response);
        }

        /// <summary>
        /// 启用或禁用指定健康管理师
        /// </summary>
        /// <param name="managerGuid"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> UpdateStatus(string managerGuid)
        {
            if (string.IsNullOrEmpty(managerGuid))
            {
                return Failed(ErrorCode.Empty, "请求参数错误");
            }

            var managerBiz = new HealthManagerBiz();

            var result = await managerBiz.UpdateStatus(managerGuid);

            if (!result)
            {
                return Failed(ErrorCode.Empty, "更新状态失败，请稍后重试");
            }

            return Success();
        }

        /// <summary>
        /// 查询指定健康管理师已绑定会员分页列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetHealthManagerConsumerListResponseDto>))]
        public async Task<IActionResult> GetHealthManagerConsumers([FromQuery]
        GetHealthManagerConsumerListRequestDto request)
        {
            var managerBiz = new HealthManagerBiz();

            var response = await managerBiz.GetHealthManagerConsumers(request);

            return Success(response);
        }

        /// <summary>
        /// 解除会员绑定健康管理师
        /// </summary>
        /// <param name="consumerGuid"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> CancelBindHealthManager(string consumerGuid)
        {
            var managerBiz = new HealthManagerBiz();

            var response = await managerBiz.CancelBindHealthManager(consumerGuid);

            return Success(response);
        }

        /// <summary>
        /// 获取更换健康管理师分页列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetMealAccountListResponseDto<GetChangeHealthManagerItem>>))]
        public async Task<IActionResult> GetChangeHealthManagers([FromQuery]
        GetChangeHealthManagerListRequestDto request)
        {
            var managerBiz = new HealthManagerBiz();
            var response = await managerBiz.GetChangeHealthManagers(request);
            return Success(response);
        }

        /// <summary>
        /// 会员绑定健康管理师
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> BindHealthManager([FromBody] UpdateConsumerBindMangerRequestDto request)
        {
            var managerBiz = new HealthManagerBiz();

            var response = await managerBiz.BindHealthManager(request);

            return Success(response);
        }

        /// <summary>
        /// 批量会员绑定健康管理师
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> BatchBindHealthManager([FromBody] BatchUpdateConsumerBindMangerRequestDto request)
        {
            if (request.ConsumerGuids.Count <= 0)
            {
                return Failed(ErrorCode.Empty, "会员参数未提供，请检查");
            }

            var managerBiz = new HealthManagerBiz();
            var manager = await managerBiz.GetManagerInfoAsync(request.ManagerGuid);
            if (manager is null)
            {
                return Failed(ErrorCode.Empty, "健康管理师不存在，请检查");
            }

            var response = await managerBiz.BatchBindHealthManager(request);

            return Success(response);
        }

        /// <summary>
        /// 获取未被禁用健康管理员简单列表信息
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> GetSimpleHealthManagers()
        {
            var managerBiz = new HealthManagerBiz();

            var response = await managerBiz.GetSimpleHealthManagers();

            return Success(response);
        }

        /// <summary>
        /// 查询指定会员随访记录分页列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetHealthManagerFllowupRecordsResponseDto>))]
        public async Task<IActionResult> GetHealthManagerFllowupRecords([FromQuery]
        GetHealthManagerFllowupRecordsRequestDto request)
        {
            var managerBiz = new HealthManagerBiz();

            var response = await managerBiz.GetHealthManagerFllowupRecords(request);

            return Success(response);
        }
    }
}
