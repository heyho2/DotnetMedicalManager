using GD.Common;
using GD.Common.Helper;
using GD.Dtos.Certificate;
using GD.Dtos.Common;
using GD.Dtos.Merchant;
using GD.Manager.Doctor;
using GD.Manager.Manager;
using GD.Manager.Merchant;
using GD.Manager.Utility;
using GD.Models.Manager;
using GD.Models.Merchant;
using GD.Models.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GD.API.Controllers.Merchant
{
    /// <summary>
    /// 商户控制器
    /// </summary>
    public class MerchantController : MerchantBaseController
    {
        /// <summary>
        /// 商户列表（审核）
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetReviewMerchantPageResponseDto>))]
        public async Task<IActionResult> GetReviewMerchantPageAsync([FromBody]GetReviewMerchantPageRequestDto request)
        {
            var response = await new MerchantBiz().GetReviewMerchantPageAsync(request);
            return Success(response);
        }
        /// <summary>
        /// 商家详细（审核）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetReviewMerchantInfoResponseDto>))]
        public async Task<IActionResult> GetReviewMerchantInfoAsync([FromBody]GetReviewMerchantInfoRequestDto request)
        {
            var merchantEntity = await new MerchantBiz().GetAsync(request.MerchantGuid);
            if (merchantEntity == null)
            {
                return Failed(ErrorCode.DataBaseError);
            }
            return Success();
        }
        /// <summary>
        /// 获取商户信息（用于编辑）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetMerchantInfoResponseDto>))]
        public async Task<IActionResult> GetMerchantInfoAsync([FromBody]GetMerchantInfoRequestDto request)
        {
            var merchantBiz = new MerchantBiz();
            var merchantModel = await merchantBiz.GetAsync(request.MerchantGuid);
            var accModel = await new AccessoryBiz().GetAsync(merchantModel.MerchantPicture);
            var response = new GetMerchantInfoResponseDto
            {
                Address = merchantModel.MerchantAddress,
                MerchantGuid = merchantModel.MerchantGuid,
                MerchantName = merchantModel.MerchantName,
                MerchantPicture = merchantModel.MerchantPicture,
                MerchantPictureUrl = $"{ accModel?.BasePath}{accModel?.RelativePath}",
                SignatureGuid = merchantModel.SignatureGuid,
                Telephone = merchantModel.Telephone,
                Longitude = merchantModel.Longitude ?? 0,
                Latitude = merchantModel.Latitude ?? 0,
                Account = merchantModel.Account,
                Area = merchantModel.Area,
                City = merchantModel.City,
                Province = merchantModel.Province,
                Street = merchantModel.Street,
                HospitalGuid = merchantModel.HospitalGuid
            };
            return Success(response);
        }
        /// <summary>
        /// 审核驳回
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> ReviewRejectMerchantAsync([FromBody] ReviewRejectMerchantRequestDto request)
        {
            MerchantBiz merchantBiz = new MerchantBiz();
            var entity = await merchantBiz.GetAsync(request.OwnerGuid);
            if (entity == null)
            {
                return Failed(ErrorCode.DataBaseError);
            }
            if (entity.Status.ToLower() == MerchantModel.StatusEnum.Reject.ToString().ToLower())
            {
                return Failed(ErrorCode.DataBaseError, "请不要重复审核");
            }
            entity.Status = MerchantModel.StatusEnum.Reject.ToString();
            entity.LastUpdatedBy = UserID;
            entity.LastUpdatedDate = DateTime.Now;
            var response = await merchantBiz.ReviewMerchantAsync(entity, request.RejectReason);
            if (!response)
            {
                return Failed(ErrorCode.DataBaseError, "审核失败");
            }
            return Success();
        }
        /// <summary>
        /// 审核通过
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> ReviewApprovedMerchantAsync([FromBody] ReviewApprovedMerchantRequestDto request)
        {
            MerchantBiz merchantBiz = new MerchantBiz();
            var entity = await merchantBiz.GetAsync(request.OwnerGuid);
            if (entity == null)
            {
                return Failed(ErrorCode.DataBaseError);
            }
            if (entity.Status.ToLower() == MerchantModel.StatusEnum.Approved.ToString().ToLower())
            {
                return Failed(ErrorCode.DataBaseError, "请不要重复审核");
            }
            entity.Status = MerchantModel.StatusEnum.Approved.ToString();
            entity.LastUpdatedBy = UserID;
            entity.LastUpdatedDate = DateTime.Now;
            var response = await merchantBiz.ReviewMerchantAsync(entity, "审核通过");
            if (!response)
            {
                return Failed(ErrorCode.DataBaseError, "审核失败");
            }
            return Success();
        }

        /// <summary>
        /// 商户列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetMerchantPageResponseDto>))]
        public async Task<IActionResult> GetMerchantPageAsync([FromBody]GetMerchantPageRequestDto request)
        {
            var response = await new MerchantBiz().GetMerchantPageAsync(request);
            return Success(response);
        }

        /// <summary>
        /// 禁用商家
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> DisableEnableMerchantAsync([FromBody]DisableEnableMerchantRequestDto request)
        {
            var doctorBiz = new MerchantBiz();
            var result = await doctorBiz.DisableEnableAsync(request.Guid, request.Enable, UserID);
            if (!result)
            {
                return Failed(ErrorCode.UserData, "修改失败");
            }
            return Success();
        }

        /// <summary>
        /// 获取经范围
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetBusinessScopeListItemDto>>))]
        public async Task<IActionResult> GetBusinessScopeListAsync()
        {
            var list = await new DictionaryBiz().GetListAsync(DictionaryType.BusinessScopeDic, true);
            var response = list.Select(a => new GetBusinessScopeListItemDto
            {
                Guid = a.DicGuid,
                Name = a.ConfigName
            });
            return Success(response);
        }
        /// <summary>
        /// 获取经营范围许可证
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetBusinessScopeLicenseItemDto>>))]
        public async Task<IActionResult> GetBusinessScopeLicenseListAsync(string merchantGuid)
        {
            var response = await new MerchantBiz().GetBusinessScopeLicenseListAsync(merchantGuid);
            return Success(response);
        }

        /// <summary>
        /// 获取经营范围许可证
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<List<GetBusinessScopeLicenseItemDto>>))]
        public async Task<IActionResult> DeleteBusinessScopeAsync(string scopeGuid)
        {
            var merchantScope = new MerchantScopeBiz();
            var scope = await merchantScope.GetAsync(scopeGuid);
            if (scope == null)
            {
                return Failed(ErrorCode.UserData, "数据不存在！");
            }
            var dictionary = new DictionaryBiz();
            var sss = await dictionary.GetListAsync();
            List<string> dicGuids = new List<string>();
            dictionary.GetAllSubsetGuids(sss, scope.ScopeDicGuid, ref dicGuids);
            if (await merchantScope.AnyMerchantScopeProductAsync(scope.MerchantGuid, dicGuids))
            {
                return Failed(ErrorCode.UserData, "该商户经营范围已经添加过商品！");
            }
            var response = merchantScope.DeleteAsync(scopeGuid);
            return Success(response);
        }

        /// <summary>
        /// 订单详细数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetMerchantOrderDetailPageResponseDto>))]
        public async Task<IActionResult> GetMerchantOrderDetailPageAsync([FromBody]GetMerchantOrderDetailPageRequestDto request)
        {
            var response = await new MerchantBiz().GetMerchantOrderDetailPageAsync(request);
            return Success(response);
        }

        /// <summary>
        /// 获取用户商品购买次数
        /// </summary>
        /// <param name="userGuid">userGuid</param>
        /// <param name="productGuid">productGuid</param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<int>))]
        public async Task<IActionResult> GetUserShopCountAsync(string userGuid, string productGuid)
        {
            var response = await new MerchantBiz().GetUserShopCountAsync(userGuid, productGuid);
            return Success(response);
        }
        /// <summary>
        /// 注册商户
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> RegisterMerchantAsync([FromBody]RegisterMerchantRequestDto request)
        {
            if (!request.Scopes.Any())
            {
                return Failed(ErrorCode.UserData, "经营范围数据为空！");
            }
            var merchantBiz = new MerchantBiz();
            if (await merchantBiz.AnyAccountAsync(request.Account))
            {
                return Failed(ErrorCode.UserData, "已经存在相同的账号！");
            }
            //商户信息
            string merchantGuid = Guid.NewGuid().ToString("N");
            var merchantModel = new MerchantModel
            {
                Status = MerchantModel.StatusEnum.Approved.ToString(),
                MerchantGuid = merchantGuid,
                MerchantPicture = request.MerchantPicture,
                MerchantName = request.MerchantName,
                CreatedBy = merchantGuid,
                SignatureGuid = request.SignatureGuid,
                Telephone = request.Telephone,
                OrgGuid = string.Empty,
                MerchantAddress = $"{request.Province}{request.City}{request.Area}{request.Street}",
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                LastUpdatedBy = merchantGuid,
                Password = CryptoHelper.AddSalt(merchantGuid, request.Password),
                Account = request.Account,
                Enable = true,
                Area = request.Area,
                City = request.City,
                Province = request.Province,
                Street = request.Street,
                HospitalGuid = request.HospitalGuid ?? string.Empty

            };
            //商户经营范围信息
            var lstScope = request.Scopes.Select(scope => new ScopeModel
            {
                ScopeGuid = Guid.NewGuid().ToString("N"),
                ScopeDicGuid = scope.ScopeDicGuid,
                MerchantGuid = merchantModel.MerchantGuid,
                PictureGuid = scope.AccessoryGuid,
                CreatedBy = merchantGuid,
                OrgGuid = string.Empty,
                LastUpdatedBy = merchantGuid
            }).ToList();
            //商户配置项证书信息 & 配置项证书附件信息
            var lstCertificate = request.Certificates.Select(item => new CertificateModel
            {
                CertificateGuid = Guid.NewGuid().ToString("N"),
                PictureGuid = item.AccessoryGuid,
                OwnerGuid = merchantModel.MerchantGuid,
                DicGuid = item.DicGuid,
                CreatedBy = UserID,
                OrgGuid = string.Empty,
                LastUpdatedBy = UserID
            });
            var lstAccessory = (await new AccessoryBiz().GetListAsync(request.Certificates.Select(a => a.AccessoryGuid).ToArray())).ToList();
            lstAccessory.ForEach(a =>
            {
                a.OwnerGuid = lstCertificate.FirstOrDefault(b => b.PictureGuid == a.AccessoryGuid)?.CertificateGuid;
                a.LastUpdatedDate = DateTime.Now;
                a.CreatedBy = UserID;
            });
            var result = await merchantBiz.RegisterMerchantAsync(merchantModel, lstScope, lstCertificate, lstAccessory);
            if (!result)
            {
                Failed(ErrorCode.DataBaseError, "商户注册数据插入不成功!");
            }
            return Success();
        }

        /// <summary>
        /// 修改商户
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> UpdateMerchantAsync([FromBody]UpdateMerchantRequestDto request)
        {
            var merchantBiz = new MerchantBiz();
            var merchantModel = await merchantBiz.GetAsync(request.MerchantGuid);
            if (merchantModel == null)
            {
                return Failed(ErrorCode.UserData, "商户不存在！");
            }

            if (request.Account != merchantModel.Account && await merchantBiz.AnyAccountAsync(request.Account))
            {
                return Failed(ErrorCode.UserData, "已经存在相同的账号！");
            }
            //商户信息
            merchantModel.MerchantPicture = request.MerchantPicture;
            merchantModel.MerchantName = request.MerchantName;
            merchantModel.Telephone = request.Telephone;
            merchantModel.LastUpdatedBy = merchantModel.MerchantGuid;
            merchantModel.LastUpdatedDate = DateTime.Now;
            merchantModel.Latitude = request.Latitude;
            merchantModel.Longitude = request.Longitude;
            merchantModel.Account = request.Account;
            if (null != request.Password)
            {
                merchantModel.Password = CryptoHelper.AddSalt(merchantModel.MerchantGuid, request.Password);
            }
            merchantModel.MerchantAddress = $"{request.Province}{request.City}{request.Area}{request.Street}";
            merchantModel.Area = request.Area;
            merchantModel.City = request.City;
            merchantModel.Province = request.Province;
            merchantModel.Street = request.Street;
            merchantModel.HospitalGuid = request.HospitalGuid ?? string.Empty;
            //商户经营范围信息
            var scopes = request.Scopes.Select(scope => new ScopeModel
            {
                ScopeGuid = Guid.NewGuid().ToString("N"),
                ScopeDicGuid = scope.ScopeDicGuid,
                MerchantGuid = merchantModel.MerchantGuid,
                PictureGuid = scope.AccessoryGuid,
                CreatedBy = merchantModel.MerchantGuid,
                OrgGuid = string.Empty,
                LastUpdatedBy = merchantModel.MerchantGuid
            });
            //商户配置项证书信息 & 配置项证书附件信息
            var lstCertificate = request.Certificates.Select(item => new CertificateModel
            {
                CertificateGuid = Guid.NewGuid().ToString("N"),
                PictureGuid = item.AccessoryGuid,
                OwnerGuid = merchantModel.MerchantGuid,
                DicGuid = item.DicGuid,
                CreatedBy = UserID,
                OrgGuid = string.Empty,
                LastUpdatedBy = UserID
            });
            var result = await merchantBiz.UpdateMerchantAsync(merchantModel, scopes, lstCertificate);
            if (!result)
            {
                Failed(ErrorCode.DataBaseError, "商户修改失败!");
            }
            return Success();
        }
        /// <summary>
        /// 商户(下拉框)
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<IList<SelectItemDto>>))]
        public async Task<IActionResult> GetMerchantSelectAsync()
        {
            var response = await new MerchantBiz().GetMerchantSelectAsync();
            return Success(response);
        }
        /// <summary>
        /// 医院列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<IList<SelectItemDto>>))]
        public async Task<IActionResult> GetHospitalSelectAsync()
        {
            var response = await new HospitalBiz().GetHospitalSelectAsync();
            return Success(response);
        }
        /// <summary>
        /// 获取证书
        /// </summary>
        /// <param name="merchantGuid"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetCertificateItemDto>>))]
        public async Task<IActionResult> GetMerchantCertificateAsync(string merchantGuid)
        {
            var response = await new CertificateBiz().GetCertificateDetailAsync(DictionaryType.MerchantDicConfig, merchantGuid);

            return Success(response);
        }

        
    }
}
