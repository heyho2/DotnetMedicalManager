using GD.API.Code;
using GD.Common;
using GD.Common.EnumDefine;
using GD.Common.Helper;
using GD.Dtos.Doctor.Hospital;
using GD.Dtos.Hospital;
using GD.Manager.Doctor;
using GD.Manager.Utility;
using GD.Models.Doctor;
using GD.Models.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GD.API.Controllers.Doctor
{
    /// <summary>
    /// 医院控制器
    /// </summary>
    public class HospitalController : DoctorBaseController
    {
        /// <summary>
        /// 获取医院 列表
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetHospitalPageResponseDto>))]
        public async Task<IActionResult> GetHospitalPageAsync([FromBody]GetHospitalPageRequestDto request)
        {
            var response = await new HospitalBiz().GetHospitalPageAsync(request);
            return Success(response);
        }
        /// <summary>
        /// 医院详细
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetHospitalInfomResponseDto>))]
        public async Task<IActionResult> GetHospitalInfoAsync([FromBody]GetHospitalInfomRequestDto request)
        {
            var entity = await new HospitalBiz().GetAsync(request.HospitalGuid);
            if (entity == null)
            {
                return Failed(ErrorCode.DataBaseError, "数据错误");
            }
            var response = entity.ToDto<GetHospitalInfomResponseDto>();
            response.HosLevel = response.HosLevel?.ToLower();
            var richtextModel = await new RichtextBiz().GetAsync(entity.HosDetailGuid);
            response.Content = richtextModel?.Content;
            AccessoryModel accessory = await new AccessoryBiz().GetAsync(entity.LogoGuid);
            response.LogoUrl = $"{accessory?.BasePath}{accessory?.RelativePath}";
            return Success(response);
        }
        /// <summary>
        /// 医院级别
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetHospitalLevelItemDto>))]
        public IActionResult GetHospitalLevel()
        {
            var response = new List<GetHospitalLevelItemDto>();
            foreach (HospitalModel.HosLevelEnum item in Enum.GetValues(typeof(HospitalModel.HosLevelEnum)))
            {
                response.Add(new GetHospitalLevelItemDto
                {
                    Name = item.GetDescription(),
                    Value = item.ToString().ToLower()
                });
            }
            return Success(response);
        }
        /// <summary>
        /// 添加医院
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> AddHospitalAsync([FromBody]AddHospitalRequestDto request)
        {
            HospitalBiz hospitalBiz = new HospitalBiz();
            if (await hospitalBiz.AnyAccountAsync(request.Account))
            {
                return Failed(ErrorCode.UserData, "已经存在相同的账号！");
            }

            var hospitalGuid = Guid.NewGuid().ToString("N");
            var textGuid = Guid.NewGuid().ToString("N");
            request.Content = string.IsNullOrWhiteSpace(request.Content) ? "暂无详细" : request.Content;
            request.HosTag = string.IsNullOrWhiteSpace(request.HosTag) ? "暂无标签" : request.HosTag;

            var richtextModel = new RichtextModel
            {
                Content = request.Content,
                CreatedBy = UserID,
                CreationDate = DateTime.Now,
                Enable = true,
                LastUpdatedBy = UserID,
                LastUpdatedDate = DateTime.Now,
                OrgGuid = string.Empty,
                OwnerGuid = hospitalGuid,
                TextGuid = textGuid,
            };
            var hospitalModel = new HospitalModel
            {
                HosAbstract = request.HosAbstract,
                HosDetailGuid = textGuid,
                HosLevel = request.HosLevel,
                HosName = request.HosName,
                HosTag = request.HosTag,
                Location = request.Location,
                LogoGuid = request.LogoGuid,
                PlatformType = PlatformType.CloudDoctor.ToString(),
                RegisteredDate = request.RegisteredDate,
                Visibility = request.Visibility,
                HospitalGuid = hospitalGuid,
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
                Enable = request.Enable,
                OrgGuid = string.Empty,
                ContactNumber = request.ContactNumber,
                Sort = request.Sort,
                GuidanceUrl = request.GuidanceUrl ?? string.Empty,
                ExternalLink = request.ExternalLink ?? string.Empty,
                Password = CryptoHelper.AddSalt(hospitalGuid, request.Password),
                Account = request.Account,
                IsHospital = request.IsHospital,
                Longitude = request.Longitude,
                Latitude = request.Latitude
            };
            var officeAll = await new OfficeBiz().GetAllAsync2();
            var offices = officeAll.Select(a => (new
            {
                a.OfficeName,
                ParentName = officeAll.FirstOrDefault(b => b.OfficeGuid == a.ParentOfficeGuid)?.OfficeName,
                a.Sort,
                a.Enable,
                a.PictureGuid
            })).Distinct();
            var offices2 = new List<OfficeModel>();
            foreach (var item in offices)
            {
                GetOfficeModel(item.ParentName, item.OfficeName, item.Sort, item.Enable, item.PictureGuid, hospitalModel, offices2, offices);
            }
            var result = await hospitalBiz.AddAsync(hospitalModel, richtextModel, offices2);

            if (!result)
            {
                return Failed(ErrorCode.UserData, "添加失败");
            }
            return Success();
        }
        private OfficeModel GetOfficeModel(string pName, string name, int sort, bool enable, string PictureGuid, HospitalModel hospitalModel, List<OfficeModel> offices2, IEnumerable<dynamic> offices)
        {
            OfficeModel po = null;
            if (pName != null)
            {
                po = offices2.FirstOrDefault(a => a.OfficeName == pName);
            }
            if (po == null)
            {
                dynamic dd = offices.FirstOrDefault(c => c.OfficeName == pName);
                if (dd != null)
                {
                    po = GetOfficeModel(dd?.ParentName, pName, dd?.Sort, dd?.Enable, dd?.PictureGuid, hospitalModel, offices2, offices);
                }
            }
            var o = offices2.FirstOrDefault(a => a.OfficeName == name);
            if (o != null)
            {
                return o;
            }
            o = new OfficeModel
            {
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
                Enable = enable,
                OrgGuid = string.Empty,
                HospitalGuid = hospitalModel.HospitalGuid,
                HospitalName = hospitalModel.HosName,
                OfficeGuid = Guid.NewGuid().ToString("N"),
                OfficeName = name,
                ParentOfficeGuid = po?.OfficeGuid,
                Recommend = false,
                PictureGuid = PictureGuid,
                Sort = sort
            };
            offices2.Add(o);
            return o;
        }

        /// <summary>
        /// 修改医院
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> UpdateHospitalAsync([FromBody]UpdateHospitalRequestDto request)
        {
            var hospitalBiz = new HospitalBiz();
            var hospitalModel = await hospitalBiz.GetAsync(request.HospitalGuid);
            if (hospitalModel == null)
            {
                return Failed(ErrorCode.DataBaseError, "数据错误");
            }
            if (request.Account != hospitalModel.Account && await hospitalBiz.AnyAccountAsync(request.Account))
            {
                return Failed(ErrorCode.UserData, "已经存在相同的账号！");
            }
            var contentBiz = new RichtextBiz();
            request.Content = string.IsNullOrWhiteSpace(request.Content) ? "暂无详细" : request.Content;
            request.HosTag = string.IsNullOrWhiteSpace(request.HosTag) ? "暂无标签" : request.HosTag;
            var richtextModel = await contentBiz.GetAsync(hospitalModel.HosDetailGuid);
            var richtextIsAdd = false;
            if (richtextModel != null)
            {
                richtextModel.Content = request.Content;
                richtextModel.LastUpdatedBy = UserID;
                richtextModel.LastUpdatedDate = DateTime.Now;
                richtextModel.OrgGuid = string.Empty;
                richtextModel.OwnerGuid = request.HospitalGuid;
            }
            else
            {
                var textGuid = Guid.NewGuid().ToString("N");
                richtextModel = new RichtextModel
                {
                    Content = request.Content,
                    CreatedBy = UserID,
                    CreationDate = DateTime.Now,
                    Enable = true,
                    LastUpdatedBy = UserID,
                    LastUpdatedDate = DateTime.Now,
                    OrgGuid = string.Empty,
                    OwnerGuid = hospitalModel.HospitalGuid,
                    TextGuid = textGuid,
                };
                hospitalModel.HosDetailGuid = textGuid;
                richtextIsAdd = true;
            }

            hospitalModel.HosAbstract = request.HosAbstract;
            hospitalModel.HosLevel = request.HosLevel;
            hospitalModel.HosName = request.HosName;
            hospitalModel.HosTag = request.HosTag;
            hospitalModel.Location = request.Location;
            hospitalModel.LogoGuid = request.LogoGuid;
            hospitalModel.RegisteredDate = request.RegisteredDate;
            hospitalModel.Visibility = request.Visibility;
            hospitalModel.LastUpdatedBy = UserID;
            hospitalModel.LastUpdatedDate = DateTime.Now;
            hospitalModel.Enable = request.Enable;
            hospitalModel.ContactNumber = request.ContactNumber;
            hospitalModel.Sort = request.Sort;
            hospitalModel.GuidanceUrl = request.GuidanceUrl ?? string.Empty;
            hospitalModel.ExternalLink = request.ExternalLink ?? string.Empty;
            hospitalModel.Account = request.Account;
            hospitalModel.IsHospital = request.IsHospital;
            hospitalModel.Longitude = request.Longitude;
            hospitalModel.Latitude = request.Latitude;
            if (null != request.Password)
            {
                hospitalModel.Password = CryptoHelper.AddSalt(hospitalModel.HospitalGuid, request.Password);
            }

            var response = await hospitalBiz.UpdateAsync(hospitalModel, richtextModel, richtextIsAdd);
            if (!response)
            {
                return Failed(ErrorCode.DataBaseError, "修改失败");
            }
            return Success(response);
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> ResetPasswordAsync([FromBody]ResetPasswordResponseDto request)
        {
            HospitalBiz hospitalBiz = new HospitalBiz();
            var entity = await hospitalBiz.GetAsync(request.Guid);
            if (entity == null)
            {
                return Failed(ErrorCode.DataBaseError);
            }
            if (string.IsNullOrWhiteSpace(entity.Account))
            {
                return Failed(ErrorCode.DataBaseError, "请先设置账号");
            }
            var password = "123456";//默认密码
            entity.Password = CryptoHelper.AddSalt(entity.HospitalGuid, GD.Common.Helper.CryptoHelper.Md5(password));
            entity.LastUpdatedBy = UserID;
            entity.LastUpdatedDate = DateTime.Now;
            await hospitalBiz.UpdateAsync(entity);
            return Success();
        }

        /// <summary>
        /// 禁用医院
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> DisableEnableHospitalAsync([FromBody]DisableEnableHospitalRequestDto request)
        {
            var hospitalBiz = new HospitalBiz();
            var entity = await hospitalBiz.GetAsync(request.Guid);
            if (entity == null)
            {
                return Failed(ErrorCode.UserData, "找不到数据");
            }
            entity.LastUpdatedBy = UserID;
            entity.LastUpdatedDate = DateTime.Now;
            entity.Enable = request.Enable;
            var result = await hospitalBiz.UpdateAsync(entity);
            if (!result)
            {
                return Failed(ErrorCode.UserData, "修改失败");
            }
            return Success();
        }
        /// <summary>
        /// 搜索医院
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<SearchHospitalResponseDto>))]
        public async Task<IActionResult> SearchHospitalAsync([FromBody]SearchHospitalRequestDto request)
        {
            var response = await new HospitalBiz().SearchHospitalAsync(request);
            foreach (var item in response.CurrentPage)
            {
                item.HosLevel = GetHosLevelName(item.HosLevel);
            }
            return Success(response);
        }
        private string GetHosLevelName(string strEnum)
        {
            foreach (HospitalModel.HosLevelEnum item in Enum.GetValues(typeof(HospitalModel.HosLevelEnum)))
            {
                if (item.ToString().ToLower() == strEnum.ToLower())
                {
                    return item.GetDescription();
                }
            }
            return null;
        }
        /// <summary>
        /// 删除医院
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> DeleteHospitalAsync(string guid)
        {
            var hospitalBiz = new HospitalBiz();
            var result = await hospitalBiz.Delete2Async(guid);
            if (!result)
            {
                return Failed(ErrorCode.UserData, "删除失败");
            }
            return Success();
        }


        #region 科室
        /// <summary>
        /// 获取科室列表
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<List<GetOfficeTreeItemDto>>))]
        public async Task<IActionResult> GetOfficeListAsync([FromBody]GetOfficeListRequestDto request)
        {
            OfficeBiz officeBiz = new OfficeBiz();
            var officeModels = await officeBiz.GetOfficeListAsync(request);
            var response = officeModels.GetTree(null, a => a.ParentName, a => a.OfficeName, a => new GetOfficeTreeItemDto
            {
                OfficeName = a.OfficeName,
                ParentName = a.ParentName,
                Enable = a.Enable,
                Sort = a.Sort,
                PictureGuid = a.PictureGuid,
            });
            return Success(response);
        }
        /// <summary>
        /// 添加科室
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> AddOfficeAsync([FromBody]AddOfficeRequestDto request)
        {
            var officeBiz = new OfficeBiz();
            var officeAll = await officeBiz.GetAllAsync();
            if (officeAll.Any(a => a.OfficeName == request.OfficeName.Trim()))
            {
                return Failed(ErrorCode.UserData, "系统中存在相同的科室名称");
            }
            var hospitalBiz = new HospitalBiz();
            var hospitalAll = await hospitalBiz.GetAllAsync();

            var officeModelList = new List<OfficeModel>();
            foreach (var item in hospitalAll)
            {
                officeModelList.Add(new OfficeModel
                {
                    CreatedBy = UserID,
                    LastUpdatedBy = UserID,
                    Enable = request.Enable,
                    OrgGuid = string.Empty,
                    HospitalGuid = item.HospitalGuid,
                    HospitalName = item.HosName,
                    OfficeGuid = Guid.NewGuid().ToString("N"),
                    OfficeName = request.OfficeName,
                    ParentOfficeGuid = officeAll.FirstOrDefault(b => b.HospitalGuid == item.HospitalGuid && b.OfficeName == request.ParentName)?.OfficeGuid,
                    PictureGuid = request.PictureGuid,
                    Recommend = false,
                    Sort = request.Sort
                });
            }
            var result = await officeBiz.InsertListAsync(officeModelList);
            if (!result)
            {
                return Failed(ErrorCode.UserData, "添加失败");
            }
            return Success();
        }
        /// <summary>
        /// 修改科室
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> UpdataOfficeAsync([FromBody]UpdataOfficeRequestDto request)
        {
            var officeBiz = new OfficeBiz();
            var officeAll = await officeBiz.GetAllAsync();
            if (request.ParentName == request.OfficeName)
            {
                return Failed(ErrorCode.UserData, "一级科室不能是自己");
            }
            var officeModelList = officeAll.Where(a => a.OfficeName == request.OfficeName).ToList();

            var childOffice = officeAll.Where(a => officeAll.Where(aa => aa.OfficeName == request.OfficeName).Select(aa => aa.OfficeGuid).Contains(a.ParentOfficeGuid)).ToList();
            if (!string.IsNullOrWhiteSpace(request.ParentName) && childOffice.Any())
            {
                return Failed(ErrorCode.UserData, "此科室存二级科室，不能移动到其他科室下面");
            }
            //foreach (var item in childOffice)
            //{
            //    item.ParentOfficeGuid = officeAll.FirstOrDefault(b => b.OfficeName == request.OfficeName && b.HospitalGuid == item.HospitalGuid)?.ParentOfficeGuid;
            //}
            foreach (var item in officeModelList)
            {
                item.LastUpdatedBy = UserID;
                item.OfficeName = request.NewOfficeName;
                if (request.ParentName == null)
                {
                    item.ParentOfficeGuid = null;
                }
                else
                {
                    item.ParentOfficeGuid = officeAll.FirstOrDefault(b => b.OfficeName == request.ParentName && b.HospitalGuid == item.HospitalGuid)?.OfficeGuid;
                }
                item.Recommend = false;
                item.Enable = request.Enable;
                item.Sort = request.Sort;
                item.PictureGuid = request.PictureGuid;
            }
            //officeModelList.AddRange(childOffice);
            var result = await officeBiz.UpdateAsync(officeModelList, request.OfficeName, request.NewOfficeName);
            if (!result)
            {
                return Failed(ErrorCode.UserData, "添加失败");
            }
            return Success();
        }

        /// <summary>
        /// 搜索科室
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<SearchOfficeResponseDto>))]
        public async Task<IActionResult> SearchOfficeAsync([FromBody]SearchOfficeRequestDto request)
        {
            OfficeBiz officeBiz = new OfficeBiz();

            var officeModels = await officeBiz.SearchOfficeAsync(request);
            return Success(officeModels);
        }
        /// <summary>
        /// 启动禁用科室
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> DisableEnableOfficeAsync([FromBody]DisableEnableOfficeRequestDto request)
        {
            var officeBiz = new OfficeBiz();
            var officeAll = await officeBiz.GetAllAsync();
            var officeModelList = officeAll.Where(a => a.OfficeName == request.OfficeName).ToList();

            var childOffice = officeAll.Where(a => officeAll.Where(aa => aa.OfficeName == request.OfficeName).Select(aa => aa.OfficeGuid).Contains(a.ParentOfficeGuid)).ToList();
            foreach (var item in childOffice)
            {
                item.LastUpdatedBy = UserID;
                item.LastUpdatedDate = DateTime.Now;
                item.Enable = request.Enable;
            }
            foreach (var item in officeModelList)
            {
                item.LastUpdatedBy = UserID;
                item.LastUpdatedDate = DateTime.Now;
                item.Enable = request.Enable;
            }
            officeModelList.AddRange(childOffice);
            var result = await officeBiz.UpdateAsync(officeModelList);
            if (!result)
            {
                return Failed(ErrorCode.UserData, "修改失败");
            }
            return Success();
        }
        /// <summary>
        /// 获取一级科室(名称)
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<string>>))]
        public async Task<IActionResult> GetFirstOfficeListAsync()
        {
            var officeBiz = new OfficeBiz();
            var officeAll = await officeBiz.GetAllAsync(enable: true);
            officeAll = officeAll.Where(a => a.ParentOfficeGuid == null || !officeAll.Any(b => b.OfficeGuid == a.ParentOfficeGuid)).ToList();
            return Success(officeAll.Select(a => a.OfficeName).Distinct());
        }
        #endregion


        #region 其他
        /// <summary>
        /// 获取全部医院（下拉框）
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetHospitalAllSelectItemDto>>))]
        public async Task<IActionResult> GetHospitalAllSelectAsync()
        {
            var hospitalBiz = new HospitalBiz();
            var hospitalAll = await hospitalBiz.GetAllAsync();

            var response = hospitalAll.Select(a => new GetHospitalAllSelectItemDto
            {
                Name = a.HosName,
                Guid = a.HospitalGuid
            });
            return Success(response);
        }
        /// <summary>
        /// 获取科室
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetHospitalOfficeTreeItemDto>>))]
        public async Task<IActionResult> GetHospitalOfficeSelectAsync(string hospitalGuid)
        {
            var models = await new OfficeBiz().GetHospitalOfficeAllAsync(hospitalGuid);
            var response = models.GetTree(null, a => a.ParentOfficeGuid, a => a.OfficeGuid, a => new GetHospitalOfficeTreeItemDto
            {
                Name = a.OfficeName,
                ParentGuid = a.ParentOfficeGuid,
                Guid = a.OfficeGuid,
            });
            return Success(response);
        }
        #endregion
    }
}
