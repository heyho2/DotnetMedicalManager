using GD.API.Code;
using GD.Common;
using GD.Common.EnumDefine;
using GD.Component;
using GD.Consumer;
using GD.Doctor;
using GD.Dtos.Admin.Doctor;
using GD.Dtos.Doctor.Doctor;
using GD.Dtos.Doctor.Hospital;
using GD.Dtos.Manager.Banner;
using GD.Dtos.Utility.Utility;
using GD.Fushion.CompositeBiz;
using GD.Manager;
using GD.Merchant;
using GD.Models.Consumer;
using GD.Models.Doctor;
using GD.Models.Manager;
using GD.Models.Utility;
using GD.Module.WeChat;
using GD.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.API.Controllers.Doctor
{
    /// <inheritdoc />
    /// <summary>
    /// 医生控制器
    /// </summary>
    public class DoctorController : DoctorBaseController
    {
        /// <summary>
        /// 获取企业微信医生移动端配置信息
        /// </summary>
        /// <returns></returns>
        [HttpGet, AllowAnonymous, Produces(typeof(ResponseDto<GetEnterpriseWeChatResponse>))]
        public IActionResult GetWeChatMobileConfig()
        {
            GetEnterpriseWeChatResponse response = new GetEnterpriseWeChatResponse();
            response.Appid = PlatformSettings.EnterpriseWeChatAppid;
            response.Agentid = PlatformSettings.EnterpriseWeChatMobileAgentid;
            return Success(response);
        }
        /// <summary>
        /// 获取企业微信医生PC端配置信息
        /// </summary>
        /// <returns></returns>
        [HttpGet, AllowAnonymous, Produces(typeof(ResponseDto<GetEnterpriseWeChatResponse>))]
        public IActionResult GetWeChatPCConfig()
        {
            GetEnterpriseWeChatResponse response = new GetEnterpriseWeChatResponse();
            response.Appid = PlatformSettings.EnterpriseWeChatAppid;
            response.Agentid = PlatformSettings.EnterpriseWeChatPCAgentid;
            return Success(response);
        }
        /// <summary>
        /// 注册医生
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> RegisterDoctor([FromBody]RegisterDoctorRequestDto doctorDto)
        {
            var doctorModelGuid = UserID;
            var doctorBiz = new DoctorBiz();
            var checkModel = await doctorBiz.GetAsync(doctorModelGuid, true, true);
            bool isAdd = checkModel == null; ;//当前为更新操作还是新增操作
            var statusCheck = string.Equals(checkModel?.Status, StatusEnum.Submit.ToString(), StringComparison.OrdinalIgnoreCase) || string.Equals(checkModel?.Status, StatusEnum.Approved.ToString(), StringComparison.OrdinalIgnoreCase);
            if (checkModel != null && statusCheck && checkModel.Enable)
            {
                return Failed(ErrorCode.DataBaseError, "该用户已注册过医生！");
            }

            var doctorCertificates = await new DictionaryBiz().GetListByParentGuidAsync(DictionaryType.DoctorDicConfig);
            foreach (var item in doctorCertificates)
            {
                if (doctorDto.Certificates.FirstOrDefault(a => a.DicGuid == item.DicGuid) == null)
                {
                    var eMsg = $"[{item.ConfigName}]没有上传";
                    return Failed(ErrorCode.UserData, $"上传的医生证书项和系统配置的项不符，请核对，详情：{eMsg}");
                }

            }

            var userModel = await new UserBiz().GetModelAsync(UserID);
            userModel.UserName = doctorDto.UserName;
            userModel.IdentityNumber = doctorDto.IdentityNumber;
            userModel.Birthday = doctorDto.Birthday;
            userModel.Gender = doctorDto.Gender;
            var hospitalBiz = new HospitalBiz();
            var officeBiz = new OfficeBiz();
            var accessoryBiz = new AccessoryBiz();
            var doctorModel = new DoctorModel();
            if (!isAdd)
            {
                doctorModel = checkModel;
            }
            //医生数据
            doctorModel.DoctorGuid = doctorModelGuid;
            doctorModel.HospitalGuid = doctorDto.HospitalGuid;
            doctorModel.OfficeGuid = doctorDto.DocOffice;
            doctorModel.WorkCity = doctorDto.City;
            doctorModel.PractisingHospital = doctorDto.PractisingHospital;
            doctorModel.Honor = doctorDto.Honor;
            doctorModel.Background = doctorDto.Background;
            doctorModel.TitleGuid = doctorDto.DocTitle;
            doctorModel.AdeptTags = Newtonsoft.Json.JsonConvert.SerializeObject(doctorDto.Adepts);
            doctorModel.Status = StatusEnum.Submit.ToString();
            doctorModel.SignatureGuid = doctorDto.SignatureGuid;
            doctorModel.CreatedBy = UserID;
            doctorModel.OrgGuid = "";
            doctorModel.LastUpdatedBy = UserID;
            doctorModel.PortraitGuid = doctorDto.PortraitGuid;
            doctorModel.LastUpdatedDate = DateTime.Now;
            doctorModel.Enable = true;

            //医院名称
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

            //医生证书配置项 & 附件
            var lstCertificate = new List<CertificateModel>();
            var lstAccessory = new List<AccessoryModel>();
            if (doctorDto.Certificates.Any())
            {
                foreach (var certificate in doctorDto.Certificates)
                {
                    var certificateModel = new CertificateModel
                    {
                        CertificateGuid = Guid.NewGuid().ToString("N"),
                        PictureGuid = certificate.AccessoryGuid,
                        OwnerGuid = doctorModel.DoctorGuid,
                        DicGuid = certificate.DicGuid,
                        CreatedBy = UserID,
                        OrgGuid = "",
                        LastUpdatedBy = UserID
                    };
                    lstCertificate.Add(certificateModel);
                    var accModel = await accessoryBiz.GetAsync(certificate.AccessoryGuid);
                    if (accModel != null)
                    {
                        accModel.OwnerGuid = certificateModel.CertificateGuid;
                        accModel.LastUpdatedDate = DateTime.Now;
                        lstAccessory.Add(accModel);
                    }
                }
            }
            var doctorCompositeBiz = new DoctorCompositeBiz();
            var result = await doctorCompositeBiz.RegisterDoctor(doctorModel, lstCertificate, lstAccessory, userModel, isAdd);
            if (result)
            {
                new DoctorActionBiz().RegisterDoctor(this.UserID);
            }
            return result ? Success() : Failed(ErrorCode.DataBaseError, "医生注册数据插入不成功!");
        }

        /// <summary>
        /// 获取医生职称列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetDoctorJobTitlesResponseDto>>)), AllowAnonymous]
        public IActionResult GetDoctorJobTitles()
        {
            var dictionaryBiz = new DictionaryBiz();
            var doctorDic = dictionaryBiz.GetModelById(DictionaryType.DoctorTitle);
            if (doctorDic == null)
            {
                return Failed(ErrorCode.Empty, "缺乏医生职称元数据配置项!");
            }
            var dics = dictionaryBiz.GetListByParentGuid(doctorDic.DicGuid);
            if (!dics.Any()) return Failed(ErrorCode.Empty, "不存在配置项");
            var response = dics.Select(a => new GetDoctorJobTitlesResponseDto
            {
                DicGuid = a.DicGuid,
                ConfigCode = a.ConfigCode,
                ConfigName = a.ConfigName
            });
            return Success(response);
        }

        /// <summary>
        /// 查看当前用户是否注册医生,若注册提供注册状态
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetDoctorRegisterStateResponseDto>))]
        public async Task<IActionResult> GetDoctorRegisterState()
        {
            var doctorBiz = new DoctorBiz();
            var model = doctorBiz.GetDoctor(UserID);
            var result = await new ReviewRecordBiz().GetLatestReviewRecordByTargetGuidAsync(UserID, ReviewRecordModel.TypeEnum.Doctors.ToString());
            return Success(new GetDoctorRegisterStateResponseDto
            {
                WhetherRegister = model != null,
                RegisterState = model?.Status,
                ApprovalMessage = result?.RejectReason
            });
        }

        /// <summary>
        /// 获取下属科室
        /// </summary>
        /// <param name="officeDto">获取科室列表传入Dto</param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<List<GetOfficesByParentResponseDto>>)), AllowAnonymous]
        public IActionResult GetOfficesByParent([FromBody]GetOfficesByParentRequestDto officeDto)
        {
            OfficeBiz officeBiz = new OfficeBiz();
            var offices = officeBiz.GetHospitalOffice(officeDto.HospitalGuid, officeDto.ParentOfficeGuid);
            if (offices == null || !offices.Any()) return Failed(ErrorCode.Empty, "没有获取到下属科室");
            var responseDto = offices.Select(a => new GetOfficesByParentResponseDto
            {
                OfficeGuid = a.OfficeGuid,
                OfficeName = a.OfficeName
            });
            return Success(responseDto);
        }

        /// <summary>
        /// 获取医院科室树结构数据
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetHospitalOfficeTreeItemDto>>)), AllowAnonymous]
        public IActionResult GetHospitalOfficeTree()
        {
            var hospitalBiz = new HospitalBiz();
            var officeBiz = new OfficeBiz();
            var hospitals = hospitalBiz.GetAllHospital();
            if (hospitals == null)
            {
                return Failed(ErrorCode.Empty, "没有查到医院数据！");
            }
            var hospitalDto = hospitals.Select(a => a.ToDto<GetHospitalOfficeTreeItemDto>()).ToList();

            foreach (var hospital in hospitalDto)
            {
                var topOffices = officeBiz.GetHospitalOffice(hospital.HospitalGuid, null);
                if (topOffices == null || !topOffices.Any()) continue;
                var officeDtos = topOffices.Select(a => a.ToDto<GetHospitalOfficeTreeOfficeItemDto>()).ToList();
                officeDtos.ForEach(GetSubordinateOffeces);
                hospital.Offeces = officeDtos;
            }
            return Success(hospitalDto);
        }

        /// <summary>
        /// 递归获取下属科室
        /// </summary>
        /// <param name="offceDto"></param>
        private void GetSubordinateOffeces(GetHospitalOfficeTreeOfficeItemDto offceDto)
        {
            var officeBiz = new OfficeBiz();
            var offices = officeBiz.GetHospitalOffice(offceDto.HospitalGuid, offceDto.OfficeGuid);
            if (offices == null || !offices.Any()) return;
            var tmps = offices.Select(a => a.ToDto<GetHospitalOfficeTreeOfficeItemDto>()).ToList();
            offceDto.SubordinateOffeces = tmps;
            foreach (var item in tmps)
            {
                GetSubordinateOffeces(item);
            }
        }

        /// <summary>
        /// 获取医生注册资料配置项
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetDoctorCertificateConfigResponseDto>>))]
        public IActionResult GetDoctorCertificateConfig()
        {
            var dictionaryBiz = new DictionaryBiz();
            var doctorDic = dictionaryBiz.GetModelById(DictionaryType.DoctorDicConfig);
            if (doctorDic == null)
            {
                return Failed(ErrorCode.DataBaseError, "缺乏医生证书元数据配置项!");
            }
            var dics = dictionaryBiz.GetListByParentGuid(doctorDic.DicGuid);
            if (dics.Any())
            {
                var response = dics.Select(a => new GetDoctorCertificateConfigResponseDto
                {
                    DicGuid = a.DicGuid,
                    ConfigCode = a.ConfigCode,
                    ConfigName = a.ConfigName
                });
                return Success(response);
            }
            else
            {
                return Failed(ErrorCode.Empty, "不存在配置项");
            }
        }

        /// <summary>
        /// 获取医生注册资料配置项明细（所有配置项+配置项对应的值）
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<CertificateDetailDto>>))]
        public async Task<IActionResult> GetDoctorCertificateDetailAsync()
        {
            var response = await new CertificateBiz().GetCertificateDetailAsync(DictionaryType.DoctorDicConfig, UserID);
            return Success(response);
        }

        /// <summary>
        /// 获取医生详情
        /// </summary>
        /// <param name="doctorGuid"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetDoctorDetailsResponseDto>)), AllowAnonymous]
        public IActionResult GetDoctorDetails(string doctorGuid)
        {
            var doctorBiz = new DoctorBiz();
            var model = doctorBiz.GetDoctor(doctorGuid);
            if (model == null)
            {
                return Failed(ErrorCode.Empty);
            }
            var userBiz = new UserBiz();
            var dictionaryBiz = new DictionaryBiz();
            var accessoryBiz = new AccessoryBiz();
            var topicBiz = new TopicBiz();
            var dto = model.ToDto<GetDoctorDetailsResponseDto>();
            var accessoryModel = accessoryBiz.GetAccessoryModelByGuid(model.PortraitGuid);
            dto.DoctorName = userBiz.GetUser(doctorGuid)?.UserName;
            dto.Title = dictionaryBiz.GetModelById(model.TitleGuid)?.ConfigName;
            dto.Portrait = $"{accessoryModel?.BasePath}{accessoryModel?.RelativePath}";
            var signature = accessoryBiz.GetAccessoryModelByGuid(model.SignatureGuid);
            dto.SignatureUrl = $"{signature?.BasePath}{signature?.RelativePath}";

            dto.CommentScore = new CommentBiz().GetTargetAvgScoreAsync(model.DoctorGuid).Result;
            dto.FansVolume = new CollectionBiz().GetListCountByTarget(model.DoctorGuid);
            dto.ConsultationVolume = doctorBiz.GetDocotrConsultationVolumeAsync(model.DoctorGuid).Result;

            return Success(dto);
        }

        /// <summary>
        /// 获取医生头像和姓名
        /// </summary>
        /// <param name="doctorGuid">医生guid</param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetDoctorNameAndNameResponseDto>)), AllowAnonymous]
        public async Task<IActionResult> GetDoctorNameAndNameAsync(string doctorGuid)
        {
            var response = await new DoctorBiz().GetDoctorNameAndNameAsync(doctorGuid);
            if (response == null)
            {
                //消息列表中，可能传入的不是医生，而是商户，此处写法为了兼容这个情况
                var merchantModel = await new MerchantBiz().GetAsync(doctorGuid);
                if (merchantModel != null)
                {
                    var merchantPic = await new AccessoryBiz().GetAsync(merchantModel?.MerchantPicture);
                    response = new GetDoctorNameAndNameResponseDto
                    {
                        DoctorGuid = doctorGuid,
                        DoctorName = merchantModel.MerchantName,
                        DoctorPortraitUrl = $"{merchantPic?.BasePath}{merchantPic?.RelativePath}"
                    };
                }
            }
            return Success(response);
        }

        #region 问医
        /// <summary>
        /// 获取问医服务的医生列表
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetAskedDoctorsResponseDto>)), AllowAnonymous]
        public async Task<IActionResult> GetAskedDoctors([FromBody]GetAskedDoctorsRequestDto requestDto)
        {
            var response = await new DoctorBiz().GetAskedDoctors(requestDto);
            if (response == null)
            {
                return Failed(ErrorCode.Empty);
            }
            return Success(response);
        }

        /// <summary>
        /// 获取医生文章详情
        /// </summary>
        /// <param name="articleGuid">文章Guid</param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetDoctorArticleDetailsResponseDto>)), AllowAnonymous]
        public IActionResult GetDoctorArticleDetails(string articleGuid)
        {
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
            var dto = model.ToDto<GetDoctorArticleDetailsResponseDto>();
            dto.Content = richtextBiz.GetModel(model.ContentGuid)?.Content;
            dto.DoctorName = userBiz.GetUser(model.AuthorGuid)?.UserName;
            var doctorModel = doctorBiz.GetDoctor(model.AuthorGuid);
            if (doctorModel != null)
            {
                var accessoryModel = accessoryBiz.GetAccessoryModelByGuid(doctorModel.PortraitGuid);
                dto.DoctorPortrait = accessoryModel?.BasePath + accessoryModel?.RelativePath;
                dto.HospitalGuid = doctorModel.HospitalGuid;
                dto.HospitalName = doctorModel.HospitalName;
                dto.OfficeGuid = doctorModel.OfficeGuid;
                dto.OfficeName = doctorModel.OfficeName;
            }
            dto.LikeNumber = likeBiz.GetLikeNumByTargetGuid(articleGuid);
            dto.Liked = likeBiz.GetLikeState(UserID, articleGuid);
            return Success(dto);
        }

        #region 问医热点
        /// <summary>
        /// 获取问医热点文章
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetAskedDoctorHotArticleResponseDto>>)), AllowAnonymous]
        public async Task<IActionResult> GetAskedDoctorHotArticle()
        {
            var articleBiz = new ConsumerBiz();
            var response = await articleBiz.GetAskedDoctorHotArticlesAsync(3.0M, 3.0M);
            if (response == null)
            {
                return Failed(ErrorCode.Empty, "未查询到数据");
            }
            return Success(response);
        }

        /// <summary>
        /// 获取问医热点Banner
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<BannerBaseDto>>)), AllowAnonymous]
        public async Task<IActionResult> GetAskedDoctorHotBanner()
        {
            var bannerBiz = new BannerBiz();
            var dtos = await bannerBiz.GetBannerInfoAsync(DictionaryType.AskedDoctorHot);
            return Success(dtos);
        }
        #endregion

        #region 问医讲堂
        /// <summary>
        /// 获取问医讲堂文章
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetAskedDoctorLectureHallArticleResponseDto>)), AllowAnonymous]
        public IActionResult GetAskedDoctorLectureHallArticle([FromBody]GetAskedDoctorLectureHallArticleRequestDto requestDto)
        {
            var articleBiz = new ConsumerBiz();
            var responseDtos = articleBiz.GetAskedDoctorLectureHallArticle(requestDto);
            if (responseDtos == null)
            {
                return Failed(ErrorCode.Empty);
            }

            return Success(responseDtos);
        }

        /// <summary>
        /// 获取问医讲堂Banner
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<BannerBaseDto>>)), AllowAnonymous]
        public async Task<IActionResult> GetAskedDoctorLectureHallBanner()
        {
            var bannerBiz = new BannerBiz();
            var dtos = await bannerBiz.GetBannerInfoAsync(DictionaryType.AskedDoctorLectureHall);
            return Success(dtos);
        }
        #endregion

        #region 问医经典问答
        /// <summary>
        /// 获取问医经典问答列表
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<List<GetAskedDoctorClassicQaResponseDto>>)), AllowAnonymous]
        public IActionResult GetAskedDoctorClassicQa([FromBody]GetAskedDoctorClassicQaRequestDto requestDto)
        {
            QaBiz qaBiz = new QaBiz();
            var qaModels = qaBiz.GetQaModels(requestDto.PageIndex, requestDto.PageSize, "where enable=true", "last_updated_date desc");
            if (qaModels == null)
            {
                return Failed(ErrorCode.Empty);
            }

            DoctorBiz doctorBiz = new DoctorBiz();
            UserBiz userBiz = new UserBiz();
            AccessoryBiz accessoryBiz = new AccessoryBiz();
            LikeBiz likeBiz = new LikeBiz();
            List<GetAskedDoctorClassicQaResponseDto> dtos = new List<GetAskedDoctorClassicQaResponseDto>();
            foreach (var model in qaModels)
            {
                var dto = model.ToDto<GetAskedDoctorClassicQaResponseDto>();
                dto.DoctorName = userBiz.GetUser(model.AuthorGuid)?.UserName;
                dto.LikeNumber = likeBiz.GetLikeNumByTargetGuid(model.QaGuid);
                var doctorModel = doctorBiz.GetDoctor(model.AuthorGuid);
                if (doctorModel != null)
                {
                    var accessoryModel = accessoryBiz.GetAccessoryModelByGuid(doctorModel.PortraitGuid);
                    dto.DoctorPortrait = accessoryModel?.BasePath + accessoryModel?.RelativePath;
                }
                dtos.Add(dto);
            }
            return Success(dtos);
        }

        /// <summary>
        /// 获取问医经典问答详情
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetAskedDoctorClassicQaDetailsResponseDto>)), AllowAnonymous]
        public IActionResult GetAskedDoctorClassicQaDetails(string qaGuid)
        {
            var qaBiz = new QaBiz();
            var doctorBiz = new DoctorBiz();
            var accessoryBiz = new AccessoryBiz();
            var userBiz = new UserBiz();
            var likeBiz = new LikeBiz();
            var dictionaryBiz = new DictionaryBiz();
            var model = qaBiz.GetModel(qaGuid);

            if (model == null)
            {
                return Failed(ErrorCode.Empty);
            }
            var dto = model.ToDto<GetAskedDoctorClassicQaDetailsResponseDto>();
            dto.DoctorName = userBiz.GetUser(model.AuthorGuid)?.UserName;
            var doctorModel = doctorBiz.GetDoctor(model.AuthorGuid);

            if (doctorModel != null)
            {
                var accessoryModel = accessoryBiz.GetAccessoryModelByGuid(doctorModel.PortraitGuid);
                dto.DoctorPortrait = $"{accessoryModel?.BasePath}{accessoryModel?.RelativePath}";
                dto.HospitalGuid = doctorModel.HospitalGuid;
                dto.HospitalName = doctorModel.HospitalName;
                dto.JobTitle = dictionaryBiz.GetModelById(doctorModel.TitleGuid)?.ConfigName;
            }
            dto.LikeNumber = likeBiz.GetLikeNumByTargetGuid(qaGuid);
            dto.Liked = likeBiz.GetLikeState(UserID, qaGuid);
            return Success(dto);
        }
        #endregion

        #endregion

        /// <summary>
        /// 搜索医生
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<SearchDoctorResponseDto>)), AllowAnonymous]
        public async Task<IActionResult> SearchDoctorAsync([FromBody]SearchDoctorRequestDto request)
        {
            CommonBiz commonBiz = new CommonBiz();
            if (!string.IsNullOrEmpty(UserID))
            {
                commonBiz.SearchHistory(UserID, request.Keyword);
            }
            commonBiz.HotWordSearch(request.Keyword);
            var response = await new DoctorBiz().SearchDoctorAsync(request);
            return Success(response);
        }

        /// <summary>
        /// 获取医生列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetDoctorListResponseDto>)), AllowAnonymous]
        public async Task<IActionResult> GetDoctorListAsync([FromBody]GetDoctorListRequestDto request)
        {
            var response = await new DoctorBiz().GetDoctorListAsync(request);
            return Success(response);
        }

        /// <summary>
        /// 审核医生注册信息
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public IActionResult AuditDoctorRegisterInfo([FromBody]AuditDoctorRegisterInfoRequestDto requestDto)
        {
            DoctorBiz doctorBiz = new DoctorBiz();
            var model = doctorBiz.GetDoctor(requestDto.DoctorGuid);
            if (model == null)
            {
                return Failed(ErrorCode.Empty, "未查到该医生数据");
            }
            model.Status = requestDto.Status.ToString();
            model.LastUpdatedBy = UserID;
            model.LastUpdatedDate = DateTime.Now;
            return doctorBiz.UpdateModel(model) ? Success() : Failed(ErrorCode.DataBaseError, "审核医生数据出现错误");

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
        /// 判断用户是否是医生用户
        /// </summary>
        /// <param name="userGuid">用户Id</param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<bool>)), AllowAnonymous]
        public async Task<IActionResult> CheckIsDoctorUserAsync(string userGuid)
        {
            var result = await new DoctorBiz().GetAsync(userGuid);
            bool response = false;
            if (result != null && result.Status.ToLower() == StatusEnum.Approved.ToString().ToLower())
            {
                response = true;
            }
            return Success(response, null);
        }

        /// <summary>
        /// 医生修改自己的医生数据
        /// </summary>
        /// <param name="doctorDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> ModifyDoctorInfoAsync([FromBody]ModifyDoctorInfoRequestDto doctorDto)
        {
            var doctorModelGuid = UserID;
            var doctorBiz = new DoctorBiz();
            var checkDoctor = await doctorBiz.GetAsync(doctorModelGuid);
            if (checkDoctor == null)
            {
                return Failed(ErrorCode.DataBaseError, "该用户未注册医生！");
            }

            var userModel = await new UserBiz().GetModelAsync(UserID);
            userModel.UserName = doctorDto.UserName;
            userModel.IdentityNumber = doctorDto.IdentityNumber;
            userModel.Birthday = doctorDto.Birthday;
            userModel.Gender = doctorDto.Gender;

            var hospitalBiz = new HospitalBiz();
            var officeBiz = new OfficeBiz();
            var accessoryBiz = new AccessoryBiz();
            var doctorModel = new DoctorModel();

            doctorModel = checkDoctor;

            //医生数据
            doctorModel.HospitalGuid = doctorDto.HospitalGuid;
            doctorModel.OfficeGuid = doctorDto.DocOffice;
            doctorModel.WorkCity = doctorDto.City;
            doctorModel.PractisingHospital = doctorDto.PractisingHospital;
            doctorModel.Honor = doctorDto.Honor;
            doctorModel.Background = doctorDto.Background;
            doctorModel.TitleGuid = doctorDto.DocTitle;
            doctorModel.AdeptTags = doctorDto.Adepts;
            doctorModel.Status = StatusEnum.Submit.ToString();
            //doctorModel.SignatureGuid = doctorDto.SignatureGuid;
            doctorModel.CreatedBy = UserID;
            doctorModel.OrgGuid = "";
            doctorModel.LastUpdatedBy = UserID;
            doctorModel.PortraitGuid = doctorDto.PortraitGuid;
            doctorModel.LastUpdatedDate = DateTime.Now;

            //医院名称
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

            //医生证书配置项 & 附件
            var lstCertificate = new List<CertificateModel>();
            var lstAccessory = new List<AccessoryModel>();
            if (doctorDto.Certificates.Any())
            {
                foreach (var certificate in doctorDto.Certificates)
                {
                    var certificateModel = new CertificateModel
                    {
                        CertificateGuid = Guid.NewGuid().ToString("N"),
                        PictureGuid = certificate.AccessoryGuid,
                        OwnerGuid = doctorModel.DoctorGuid,
                        DicGuid = certificate.DicGuid,
                        CreatedBy = UserID,
                        OrgGuid = "",
                        LastUpdatedBy = UserID
                    };
                    lstCertificate.Add(certificateModel);
                    var accModel = await accessoryBiz.GetAsync(certificate.AccessoryGuid);
                    if (accModel != null)
                    {
                        accModel.OwnerGuid = certificateModel.CertificateGuid;
                        accModel.LastUpdatedDate = DateTime.Now;
                        lstAccessory.Add(accModel);
                    }
                }
            }
            var doctorCompositeBiz = new DoctorCompositeBiz();
            var result = await doctorCompositeBiz.RegisterDoctor(doctorModel, lstCertificate, lstAccessory, userModel, false);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "医生数据修改失败!");
        }

        /// <summary>
        /// 获取当前科室和下属所有科室的医生列表
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetDoctorListByParentOfficeNodeResponseDto>)), AllowAnonymous]
        public async Task<IActionResult> GetDoctorListByParentOfficeNodeAsync([FromBody]GetDoctorListByParentOfficeNodeRequestDto requestDto)
        {
            var officeBiz = new OfficeBiz();

            //获取选择的科室model
            var officeModel = await officeBiz.GetAsync(requestDto.OfficeGuid);
            var officeIds = new List<string>();
            //获取医院下所有科室
            var officeModels = await officeBiz.GetHospitalOfficeAllAsync(requestDto.HospitalGuid);

            if (string.IsNullOrWhiteSpace(requestDto.OfficeGuid))
            {
                var levelOneOffices = officeBiz.GetHospitalOffice(requestDto.HospitalGuid, null);
                foreach (var item in levelOneOffices)
                {
                    officeIds.AddRange(officeBiz.GetOfficeListByParentOfficeNode(item.OfficeGuid, officeModels));
                }
            }
            else
            {
                officeIds.AddRange(officeBiz.GetOfficeListByParentOfficeNode(officeModel.OfficeGuid, officeModels));
            }

            requestDto.OfficeIds = officeIds;
            var response = await new DoctorBiz().GetDoctorListByParentOfficeNodeAsync(requestDto);
            return Success(response);
        }

        /// <summary>
        /// 获取医院下推荐医生列表
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetHospitalRecommendDoctorListResponseDto>)), AllowAnonymous]
        public async Task<IActionResult> GetHospitalRecommendDoctorListAsync([FromBody]GetHospitalRecommendDoctorListRequestDto requestDto)
        {
            var result = await new DoctorBiz().GetHospitalRecommendDoctorListAsync(requestDto);
            return Success(result);
        }

        /// <summary>
        /// 获取患者详情信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetPatientInfoResponseDto>))]
        public async Task<IActionResult> GetPatientInfoAsync(string userId)
        {
            var res = await new DoctorBiz().GetPatientInfoAsync(userId, UserID);

            var chaBiz = new CharacterBiz();
            var dicBiz = new DictionaryBiz();

            var chaModelList = chaBiz.GetCharacterModels(userId);//用户所有的列值
            chaModelList = chaModelList ?? new List<CharacterModel>();
            var dicModel = dicBiz.GetModelById(DictionaryType.UserPersonalInfo);//约定的GUID指定某类型的数据
            if (dicModel == null) return Failed(ErrorCode.FormatError, "查询不到约定ID,请联系管理员！");
            var dicModelList = dicBiz.GetListByParentGuid(dicModel.DicGuid);//个人资料所有的值
            res.CharacterInfos = (from dml in dicModelList
                                  join cml in chaModelList
                                      on dml.DicGuid equals cml.ConfGuid into temp
                                  from tt in temp.DefaultIfEmpty()
                                  select new GetPatientInfoResponseDto.CharacterInfo
                                  {
                                      ValueType = dml?.ValueType,
                                      CharacterName = dml?.ConfigName,
                                      CharacterValue = tt?.ConfValue
                                  }).ToList();

            return Success(res);
        }

        /// <summary>
        /// 通过一级科室名称获取医生
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetDoctorByFirstLevelOfficeNameResponseDto>)), AllowAnonymous]
        public async Task<IActionResult> GetDoctorByFirstLevelOfficeNameAsync([FromQuery]GetDoctorByFirstLevelOfficeNameRequestDto requestDto)
        {
            var officeBiz = new OfficeBiz();
            var levelOneOffices = await officeBiz.GetModelByNameAsync(requestDto.OfficeName);
            var officeIds = new List<string>();
            var officeModels = await officeBiz.GetAllAsync();
            foreach (var item in levelOneOffices)
            {
                officeIds.AddRange(officeBiz.GetOfficeListByParentOfficeNode(item.OfficeGuid, officeModels));
            }
            requestDto.OfficeIds = officeIds;
            var response = await new DoctorBiz().GetDoctorByFirstLevelOfficeNameAsync(requestDto);
            return Success(response);
        }

        /// <summary>
        /// 获取医生粉丝列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetDoctorFansListItemDto>>)), AllowAnonymous]
        public async Task<IActionResult> GetDoctorFansListAsync([FromQuery]GetDoctorFansListRequestDto requestDto)
        {
            var result = await new DoctorBiz().GetDoctorFansListAsync(requestDto);
            return Success(result);
        }

        /// <summary>
        /// 获取医生端口AppId
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetDoctorClientAppId()
        {
            var appId = PlatformSettings.DoctorClientAppId;
            return Success(appId, null);
        }

        /// <summary>
        /// 更新医生OpenId
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UpdateDoctorOpenIdAsync([FromBody]UpdateDoctorOpenIdRequestDto requestDto)
        {
            var result = await WeChartApi.Oauth2AccessTokenAsync(PlatformSettings.DoctorClientAppId, PlatformSettings.DoctorClientAppSecret, requestDto.Code);
            if (result.Errcode != 0)
            {
                return Failed(ErrorCode.SystemException, "获取医生openId失败");
            }
            var model = await new DoctorBiz().GetAsync(UserID);
            if (model == null)
            {
                return Failed(ErrorCode.UserData, "无此医生数据");
            }
            if (!string.IsNullOrWhiteSpace(result.OpenId) && model.WechatOpenid != result.OpenId)
            {
                model.WechatOpenid = result.OpenId;
                model.LastUpdatedDate = DateTime.Now;
                model.LastUpdatedBy = UserID;
                var res = await new DoctorBiz().UpdateAsync(model);
            }
            return Success();
        }
        /// <summary>
        /// 获取名医推荐医生列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetFamousRecommendDoctorListResponseDto>)), AllowAnonymous]
        public async Task<IActionResult> GetFamousRecommendDoctorListAsync([FromQuery]GetFamousRecommendDoctorListRequestDto requestDto)
        {
            var result = await new DoctorBiz().GetFamousRecommendDoctorListAsync(requestDto);
            return Success(result);
        }

        /// <summary>
        /// 获取医生所在单位是否是医院
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(ResponseDto<bool>))]
        public async Task<IActionResult> GetWhetherIsHospitalOfDoctorHospitalAsync()
        {
            var doctorModel = await new DoctorBiz().GetAsync(UserID);
            if (doctorModel == null || !doctorModel.Enable || doctorModel.Status != "approved")
            {
                return Failed(ErrorCode.UserData, "当前登录用户不是医生或已不是有效的医生");
            }
            var hospitalModel = await new HospitalBiz().GetAsync(doctorModel.HospitalGuid);
            return Success(hospitalModel.IsHospital);
        }
    }
}