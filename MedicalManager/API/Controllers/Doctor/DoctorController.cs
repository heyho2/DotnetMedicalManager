using GD.AppSettings;
using GD.Common;
using GD.Dtos.Common;
using GD.Dtos.Doctor;
using GD.Dtos.Doctor.Doctor;
using GD.Dtos.Score;
using GD.Manager.Consumer;
using GD.Manager.Doctor;
using GD.Manager.Manager;
using GD.Manager.Utility;
using GD.Models.Consumer;
using GD.Models.Doctor;
using GD.Models.Manager;
using GD.Models.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GD.API.Controllers.Doctor
{
    /// <summary>
    /// 医生控制器
    /// </summary>
    public class DoctorController : DoctorBaseController
    {
        /// <summary>
        /// 搜索医生
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<SearchDoctorResponseDto>))]
        public async Task<IActionResult> SearchDoctorAsync([FromBody]SearchDoctorRequestDto request)
        {
            var response = await new DoctorBiz().SearchDoctorAsync(request);
            return Success(response);
        }

        /// <summary>
        /// 审核医生列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetReviewDoctorPageResponseDto>))]
        public async Task<IActionResult> GetReviewDoctorPageAsync([FromBody]GetReviewDoctorPageRequestDto request)
        {
            var response = await new DoctorBiz().GetReviewDoctorPageAsync(request);
            foreach (var item in response.CurrentPage)
            {
                if (item.Status == DoctorModel.StatusEnum.Submit.ToString())
                {
                    item.LastUpdatedDate = null;
                };
            }
            return Success(response);
        }

        /// <summary>
        /// 审核驳回
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> ReviewRejectDoctorAsync([FromBody] ReviewRejectDoctorRequestDto request)
        {
            DoctorBiz doctorBiz = new DoctorBiz();
            var entity = await doctorBiz.GetAsync(request.OwnerGuid);
            if (entity == null)
            {
                return Failed(ErrorCode.DataBaseError);
            }
            if (entity.Status.ToLower() == DoctorModel.StatusEnum.Reject.ToString().ToLower())
            {
                return Failed(ErrorCode.DataBaseError, "请不要重复审核");
            }
            entity.Status = DoctorModel.StatusEnum.Reject.ToString();
            entity.LastUpdatedBy = UserID;
            entity.LastUpdatedDate = DateTime.Now;
            var response = await doctorBiz.ReviewDoctorAsync(entity, request.RejectReason);
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
        public async Task<IActionResult> ReviewApprovedDoctorAsync([FromBody] ReviewApprovedDoctorRequestDto request)
        {
            DoctorBiz doctorBiz = new DoctorBiz();
            var entity = await doctorBiz.GetAsync(request.OwnerGuid);
            if (entity == null)
            {
                return Failed(ErrorCode.DataBaseError);
            }
            if (entity.Status.ToLower() == DoctorModel.StatusEnum.Approved.ToString().ToLower())
            {
                return Failed(ErrorCode.DataBaseError, "请不要重复审核");
            }
            entity.Status = DoctorModel.StatusEnum.Approved.ToString();
            entity.LastUpdatedBy = UserID;
            entity.LastUpdatedDate = DateTime.Now;
            var response = await doctorBiz.ReviewDoctorAsync(entity, "审核通过");
            if (!response)
            {
                return Failed(ErrorCode.DataBaseError, "审核失败");
            }
            return Success();
        }

        /// <summary>
        /// 获取医生列表（医生管理）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetDoctorPageResponseDto>)), AllowAnonymous]
        public async Task<IActionResult> GetDoctorPageAsync([FromBody]GetDoctorPageRequestDto request)
        {
            var settings = Factory.GetSettings("host.json");
            var presence = settings["XMPP:presence"];
            var domain = settings["XMPP:domain"];
            var response = await new DoctorBiz().GetDoctorPageAsync(request);
            ScoreBiz scoreBiz = new ScoreBiz();
            var users = response.CurrentPage.Select(a => a.DoctorGuid).ToArray();
            var totalPoints = await scoreBiz.GetUserPointsAsync(users);
            var wechatSubscriptionRecommendCounts = await new WechatSubscriptionBiz().GetWechatSubscriptionRecommendCountsAsync(WechatSubscriptionModel.EntranceEnum.Doctor, users);
            foreach (var item in response.CurrentPage)
            {
                item.PresenceIcon = $"{presence}?jid={item.DoctorGuid}@{domain}";
                item.TotalPoints = totalPoints.FirstOrDefault(a => a.UserGuid == item.DoctorGuid)?.Variation ?? 0;
                item.RecommendCount = wechatSubscriptionRecommendCounts.FirstOrDefault(a => a.UserGuid == item.DoctorGuid)?.Count ?? 0;
            }
            return Success(response);
        }

        /// <summary>
        /// 禁用医生
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> DisableEnableDoctorAsync([FromBody]DisableEnableRequestDto request)
        {
            var doctorBiz = new DoctorBiz();
            var entity = await doctorBiz.GetAsync(request.Guid);
            if (entity == null)
            {
                return Failed(ErrorCode.UserData, "找不到数据");
            }
            entity.LastUpdatedBy = UserID;
            entity.LastUpdatedDate = DateTime.Now;
            entity.Enable = request.Enable;
            var result = await doctorBiz.UpdateAsync(entity);
            if (!result)
            {
                return Failed(ErrorCode.UserData, "修改失败");
            }
            return Success();
        }

        /// <summary>
        /// 推荐医生
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> RecommendDoctorAsync([FromBody]RecommendDoctorRequestDto request)
        {
            var doctorBiz = new DoctorBiz();
            var entity = await doctorBiz.GetAsync(request.Guid);
            if (entity == null)
            {
                return Failed(ErrorCode.UserData, "找不到数据");
            }
            entity.LastUpdatedBy = UserID;
            entity.LastUpdatedDate = DateTime.Now;
            entity.IsRecommend = request.IsRecommend;
            var result = await doctorBiz.UpdateAsync(entity);
            if (!result)
            {
                return Failed(ErrorCode.UserData, "修改失败");
            }
            return Success();
        }
        /// <summary>
        /// 获取消息对话列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetDoctorTopicResponseDto>))]
        public async Task<IActionResult> GetDoctorTopicAsync([FromBody]GetDoctorTopicRequestDto request)
        {
            var doctorBiz = new DoctorBiz();
            var response = await doctorBiz.GetDoctorTopicAsync(request);

            foreach (var item in response.CurrentPage)
            {
                if (item.SponsorGuid != request.DoctorGuid)
                {
                    item.Consultants = item.SponsorName;
                }
                else if (item.ReceiverGuid != request.DoctorGuid)
                {
                    item.Consultants = item.ReceiverName;
                }
                item.LastMessage = (await doctorBiz.GetTopicLastMessageAsync(item.TopicGuid));
            }
            return Success(response);
        }

        /// <summary>
        /// 对话消息列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<TopicMessageResponseDto>))]
        public async Task<IActionResult> TopicMessageAsync([FromBody]TopicMessageRequestDto request)
        {
            var doctorBiz = new DoctorBiz();
            var response = await doctorBiz.TopicMessageAsync(request);
            return Success(response);
        }
        /// <summary>
        /// 获取医生信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetDoctorInfoResponseDto>))]
        public async Task<IActionResult> GetDoctorInfoAsync([FromBody]GetDoctorInfoRequestDto request)
        {
            var doctorBiz = new DoctorBiz();
            var doctorModel = await doctorBiz.GetAsync(request.DoctorGuid);
            var userModel = await new UserBiz().GetAsync(request.DoctorGuid);
            var accModel = await new AccessoryBiz().GetAsync(doctorModel.PortraitGuid);
            var WorkCitys = doctorModel.WorkCity == "未知" || string.IsNullOrWhiteSpace(doctorModel.WorkCity) ? new string[] { } : doctorModel.WorkCity.Split('-');
            string[] adeptTags = null;
            try
            {
                adeptTags = Newtonsoft.Json.JsonConvert.DeserializeObject<string[]>(string.IsNullOrWhiteSpace(doctorModel.AdeptTags) ? "[]" : doctorModel.AdeptTags);
            }
            catch (Exception)
            {
            };
            var response = new GetDoctorInfoResponseDto
            {
                WorkCity = WorkCitys,
                DoctorGuid = doctorModel.DoctorGuid,
                AdeptTags = adeptTags,
                Background = doctorModel.Background,
                Birthday = userModel?.Birthday ?? DateTime.Now,
                OfficeGuid = doctorModel.OfficeGuid,
                TitleGuid = doctorModel.TitleGuid,
                Gender = userModel?.Gender,
                Honor = doctorModel.Honor,
                HospitalGuid = doctorModel.HospitalGuid,
                IdentityNumber = userModel?.IdentityNumber,
                PortraitGuid = doctorModel.PortraitGuid,
                PractisingHospital = doctorModel.PractisingHospital,
                SignatureGuid = doctorModel.SignatureGuid,
                UserName = userModel?.UserName,
                PortraitUrl = $"{accModel?.BasePath}{accModel?.RelativePath}",
                Phone = userModel?.Phone,
                Enable = doctorModel.Enable,
                IsRecommend = doctorModel.IsRecommend,
                WorkAge = doctorModel.WorkAge,
                JobNumber = doctorModel.JobNumber
            };
            return Success(response);
        }
        /// <summary>
        /// 修改医生数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> UpdateDoctorInfoAsync([FromBody]UpdateDoctorInfoRequestDto request)
        {
            var doctorBiz = new DoctorBiz();
            var doctorModel = await doctorBiz.GetAsync(request.DoctorGuid);
            if (doctorModel == null)
            {
                return Failed(ErrorCode.DataBaseError, "该用户未注册医生！");
            }
            var userModel = await new UserBiz().GetAsync(request.DoctorGuid);
            if (userModel.Phone != request.Phone)
            {
                UserModel userPhoneModel = await new UserBiz().GetByPnoneAsync(request.Phone);
                if (userPhoneModel != null)
                {
                    return Failed(ErrorCode.UserData, "用户手机已注册！");
                }
            }

            var existNumber = await doctorBiz.ExistJobNumber(request.JobNumber, doctorModel.DoctorGuid);

            if (existNumber)
            {
                return Failed(ErrorCode.DataBaseError, $"工号【{request.JobNumber}】已存在");
            }

            userModel.UserName = request.UserName;
            userModel.IdentityNumber = request.IdentityNumber;
            userModel.Birthday = request.Birthday;
            userModel.Gender = request.Gender;
            userModel.Phone = request.Phone;
            userModel.LastUpdatedBy = UserID;
            userModel.LastUpdatedDate = DateTime.Now;

            //医生数据
            doctorModel.HospitalGuid = request.HospitalGuid;
            doctorModel.OfficeGuid = request.OfficeGuid;
            doctorModel.WorkCity = string.Join('-', request.WorkCity);
            doctorModel.JobNumber = request.JobNumber;
            doctorModel.PractisingHospital = request.PractisingHospital;
            doctorModel.Honor = request.Honor;
            doctorModel.Background = request.Background;
            doctorModel.TitleGuid = request.TitleGuid;
            doctorModel.AdeptTags = Newtonsoft.Json.JsonConvert.SerializeObject(request.AdeptTags);
            doctorModel.IsRecommend = request.IsRecommend;
            doctorModel.RecommendSort = request.RecommendSort;
            doctorModel.LastUpdatedBy = UserID;
            doctorModel.LastUpdatedDate = DateTime.Now;
            doctorModel.PortraitGuid = request.PortraitGuid;

            //医院名称
            var hospitalBiz = new HospitalBiz();
            var hospitalModel = await hospitalBiz.GetAsync(doctorModel.HospitalGuid);
            if (hospitalModel == null)
            {
                return Failed(ErrorCode.Empty, "未查到医院数据");
            }
            doctorModel.HospitalName = hospitalModel?.HosName;
            //科室名称
            var officeModel = await new OfficeBiz().GetAsync(doctorModel.OfficeGuid);
            if (officeModel == null)
            {
                return Failed(ErrorCode.Empty, "未查到科室数据");
            }
            doctorModel.OfficeName = officeModel?.OfficeName;
            //商户配置项证书信息 & 配置项证书附件信息
            var clientDicGuids = request.Certificates.Select(b => b.DicGuid);
            CertificateBiz certificateBiz = new CertificateBiz();
            var dbCertificate = await certificateBiz.GetListAsync(doctorModel.DoctorGuid);
            var dbDicGuids = dbCertificate.Select(b => b.DicGuid);

            var updateCertificate = dbCertificate.Where(a => clientDicGuids.Contains(a.DicGuid)).Select(a =>
            {
                var cc = request.Certificates.FirstOrDefault(c => c.DicGuid == a.DicGuid);
                a.PictureGuid = cc.AccessoryGuid;
                a.LastUpdatedBy = UserID;
                a.LastUpdatedDate = DateTime.Now;
                return a;
            });
            var addCertificate = request.Certificates.Where(a => !dbDicGuids.Contains(a.DicGuid)).Select(item => new CertificateModel
            {
                CertificateGuid = Guid.NewGuid().ToString("N"),
                PictureGuid = item.AccessoryGuid,
                OwnerGuid = doctorModel.DoctorGuid,
                DicGuid = item.DicGuid,
                CreatedBy = UserID,
                OrgGuid = string.Empty,
                LastUpdatedBy = UserID
            });
            var delCertificate = dbCertificate.Where(a => !clientDicGuids.Contains(a.DicGuid));
            var result = await doctorBiz.UpdateDoctorAsync(doctorModel, userModel, addCertificate, delCertificate, updateCertificate);
            if (!result)
            {
                return Failed(ErrorCode.DataBaseError, "医生数据修改失败!");
            }
            return Success();
        }
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> ResetPasswordAsync([FromBody]ResetPasswordResponseDto request)
        {
            UserBiz userBiz = new UserBiz();
            var userModel = await userBiz.GetAsync(request.DoctorGuid);
            if (userModel == null)
            {
                return Failed(ErrorCode.DataBaseError);
            }
            var password = "123456";//默认密码
            userModel.Password = Common.Helper.CryptoHelper.AddSalt(userModel.UserGuid, GD.Common.Helper.CryptoHelper.Md5(password));
            userModel.LastUpdatedBy = UserID;
            userModel.LastUpdatedDate = DateTime.Now;
            await userBiz.UpdateAsync(userModel);
            return Success();
        }
        /// <summary>
        /// 添加医生
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> AddDoctorInfoAsync([FromBody]AddDoctorInfoRequestDto request)
        {
            var doctorBiz = new DoctorBiz();
            var userIsInsert = false;
            var userModel = await new UserBiz().GetByPnoneAsync(request.Phone);
            if (userModel != null)
            {
                var doctor = await doctorBiz.GetAsync(userModel.UserGuid);
                if (doctor != null)
                {
                    return Failed(ErrorCode.DataBaseError, "该用户已注册医生！");
                }
                userModel.UserName = request.UserName;
                userModel.IdentityNumber = request.IdentityNumber;
                userModel.Birthday = request.Birthday;
                userModel.Gender = request.Gender;
                userModel.Phone = request.Phone;
            }
            else
            {
                //var password = request.Phone.Substring(request.Phone.Length - 6);
                var password = "123456";//默认密码
                var userid = Guid.NewGuid().ToString("N");
                userModel = new UserModel
                {
                    Birthday = request.Birthday,
                    CreatedBy = userid,
                    Enable = true,
                    Gender = request.Gender,
                    LastUpdatedBy = userid,
                    NickName = request.UserName,
                    OrgGuid = string.Empty,
                    Password = Common.Helper.CryptoHelper.AddSalt(userid, GD.Common.Helper.CryptoHelper.Md5(password)),
                    Phone = request.Phone,
                    UserGuid = userid,
                    UserName = request.UserName,
                    IdentityNumber = request.IdentityNumber
                };
                userIsInsert = true;
            }

            var existNumber = await doctorBiz.ExistJobNumber(request.JobNumber);
            if (existNumber)
            {
                return Failed(ErrorCode.DataBaseError, $"工号【{request.JobNumber}】已存在");
            }

            //医生数据
            var doctorModel = new DoctorModel
            {
                DoctorGuid = userModel.UserGuid,
                HospitalGuid = request.HospitalGuid,
                OfficeGuid = request.OfficeGuid,
                WorkCity = string.Join('-', request.WorkCity),
                PractisingHospital = request.PractisingHospital,
                Honor = request.Honor,
                Background = request.Background,
                TitleGuid = request.TitleGuid,
                AdeptTags = Newtonsoft.Json.JsonConvert.SerializeObject(request.AdeptTags),
                Status = DoctorModel.StatusEnum.Approved.ToString(),
                SignatureGuid = string.Empty,
                CreatedBy = userModel.UserGuid,
                OrgGuid = string.Empty,
                LastUpdatedBy = userModel.UserGuid,
                PortraitGuid = request.PortraitGuid,
                //HospitalName = string.Empty,
                //OfficeName = string.Empty,
                CreationDate = DateTime.Now,
                IsRecommend = request.IsRecommend,
                RecommendSort = request.RecommendSort,
                Enable = request.Enable,
                WorkAge = request.WorkAge,
                JobNumber = request.JobNumber
            };
            //医院名称
            var hospitalBiz = new HospitalBiz();
            var hospitalModel = await hospitalBiz.GetAsync(doctorModel.HospitalGuid);
            if (hospitalModel == null)
            {
                return Failed(ErrorCode.Empty, "未查到医院数据");
            }
            doctorModel.HospitalName = hospitalModel?.HosName;
            //科室名称
            var officeModel = await new OfficeBiz().GetAsync(doctorModel.OfficeGuid);
            if (officeModel == null)
            {
                return Failed(ErrorCode.Empty, "未查到科室数据");
            }
            doctorModel.OfficeName = officeModel?.OfficeName;
            //商户配置项证书信息 & 配置项证书附件信息
            var lstCertificate = request.Certificates.Select(item => new CertificateModel
            {
                CertificateGuid = Guid.NewGuid().ToString("N"),
                PictureGuid = item.AccessoryGuid,
                OwnerGuid = doctorModel.DoctorGuid,
                DicGuid = item.DicGuid,
                CreatedBy = UserID,
                OrgGuid = string.Empty,
                LastUpdatedBy = UserID
            });
            var result = await doctorBiz.AddDoctorAsync(doctorModel, userModel, userIsInsert, lstCertificate);
            if (!result)
            {
                return Failed(ErrorCode.DataBaseError, "医生数据修改失败!");
            }
            return Success();
        }
        /// <summary>
        /// 获取医生职称列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetDoctorJobTitlesResponseDto>>))]
        public async Task<IActionResult> GetDoctorJobTitlesAsync()
        {
            var dictionaryBiz = new DictionaryBiz();
            var doctorDic = await dictionaryBiz.GetAsync(DictionaryType.DoctorTitle);
            if (doctorDic == null)
            {
                return Failed(ErrorCode.Empty, "缺乏医生职称元数据配置项!");
            }
            var dics = await dictionaryBiz.GetListAsync(doctorDic.DicGuid, true);
            if (!dics.Any())
            {
                return Failed(ErrorCode.Empty, "不存在配置项");
            }
            var response = dics.Select(a => new GetDoctorJobTitlesResponseDto
            {
                DicGuid = a.DicGuid,
                ConfigCode = a.ConfigCode,
                ConfigName = a.ConfigName
            });
            return Success(response);
        }
        /// <summary>
        /// 获取医生注册资料配置项明细（所有配置项+配置项对应的值）
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetDoctorCertificateDetailItemDto>>))]
        public async Task<IActionResult> GetDoctorCertificateDetailAsync(string doctorGuid)
        {
            var response = await new CertificateBiz().GetCertificateDetailAsync(DictionaryType.DoctorDicConfig, doctorGuid);

            return Success(response);
        }
        /// <summary>
        /// 保存证书
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> SaveDoctorCertificateDetailAsync([FromBody]SaveDoctorCertificateDetailRequestDto request)
        {
            CertificateBiz certificateBiz = new CertificateBiz();
            var certificate = await certificateBiz.GetAsync(request.DicGuid, request.OwnerGuid);
            if (certificate == null)
            {
                request.CertificateGuid = Guid.NewGuid().ToString("N");
                await new CertificateBiz().InsertAsync(new CertificateModel
                {
                    CertificateGuid = request.CertificateGuid,
                    DicGuid = request.DicGuid,
                    CreatedBy = UserID,
                    Enable = true,
                    LastUpdatedBy = UserID,
                    OrgGuid = string.Empty,
                    OwnerGuid = request.OwnerGuid,
                    PictureGuid = request.AccessoryGuid
                });
                return Success();
            }
            else
            {
                await new CertificateBiz().UpdateAsync(new CertificateModel
                {
                    CertificateGuid = request.CertificateGuid,
                    DicGuid = request.DicGuid,
                    CreatedBy = UserID,
                    Enable = true,
                    LastUpdatedBy = UserID,
                    OrgGuid = string.Empty,
                    OwnerGuid = request.OwnerGuid,
                    PictureGuid = request.AccessoryGuid
                });
            }
            return Success(request.CertificateGuid);
        }

        /// <summary>
        /// 医生积分列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetDoctorIntegralPageResponseDto>))]
        public async Task<IActionResult> GetDoctorIntegralPageAsync([FromBody]GetDoctorIntegralPageRequestDto request)
        {
            DoctorBiz doctorBiz = new DoctorBiz();
            ScoreBiz scoreBiz = new ScoreBiz();
            var response = await doctorBiz.GetDoctorIntegralAsync(request);
            if (!response.CurrentPage.Any())
            {
                return Success(response);
            }
            var users = response.CurrentPage.Select(a => a.DoctorGuid).ToArray();
            var totalPoints = await scoreBiz.GetUserPointsAsync(users);
            var earnPoints = await scoreBiz.GetUserEarnPointsAsync(users);
            var usePoints = await scoreBiz.GetUserUsePointsAsync(users);
            foreach (var item in response.CurrentPage)
            {
                item.TotalPoints = totalPoints.FirstOrDefault(a => a.UserGuid == item.DoctorGuid)?.Variation ?? 0;
                item.EarnPoints = earnPoints.FirstOrDefault(a => a.UserGuid == item.DoctorGuid)?.Variation ?? 0;
                item.UsePoints = usePoints.FirstOrDefault(a => a.UserGuid == item.DoctorGuid)?.Variation ?? 0;
            }
            return Success(response);
        }
        /// <summary>
        /// 积分详细
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetIntegralInfoPageResponseDto>))]
        public async Task<IActionResult> GetIntegralInfoPageAsync([FromBody]GetIntegralInfoPageRequestDto request)
        {
            ScoreBiz scoreBiz = new ScoreBiz();
            var response = await scoreBiz.GetIntegralInfoPageAsync(request);
            return Success(response);
        }
        /// <summary>
        /// 导出医生积分
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<ExportDoctorIntegralResponseDto>))]
        public async Task<IActionResult> ExportDoctorIntegralAsync([FromBody]ExportDoctorIntegralRequestDto request)
        {
            DoctorBiz doctorBiz = new DoctorBiz();
            var response = await doctorBiz.ExportDoctorIntegralAsync(request);
            return Success(new ExportDoctorIntegralResponseDto { Items = response });
        }
        /// <summary>
        /// 导出用户积分
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<ExportIntegralInfoResponseDto>))]
        public async Task<IActionResult> ExportIntegralInfoAsync([FromBody]ExportIntegralInfoRequestDto request)
        {
            ScoreBiz scoreBiz = new ScoreBiz();
            var response = await scoreBiz.ExportIntegralInfoAsync(request);
            return Success(new ExportIntegralInfoResponseDto { Items = response });
        }
    }
}
