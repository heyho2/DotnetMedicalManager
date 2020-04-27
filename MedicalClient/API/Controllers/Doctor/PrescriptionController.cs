using GD.Common;
using GD.Common.Helper;
using GD.Consumer;
using GD.Doctor;
using GD.Dtos.Doctor.Doctor;
using GD.Dtos.Doctor.Prescription;
using GD.Dtos.Enum;
using GD.Dtos.Enum.DoctorAppointment;
using GD.Manager;
using GD.Models.Doctor;
using GD.Models.Manager;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GD.API.Controllers.Doctor
{
    /// <summary>
    /// 处方相关控制器
    /// </summary>
    public class PrescriptionController : DoctorBaseController
    {
        private readonly object locker = new object();

        /// <summary>
        /// 查询用药数据字典
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetMedicationDictionaryPageListResponseDto>))]
        public async Task<IActionResult> GetMedicationDictionaryPageListAsync([FromQuery]GetMedicationDictionaryRequestDto requestDto)
        {
            var response = await new MedicationDictionaryBiz().GetMedicationDictionaryPageListAsync(requestDto);
            return Success(response);
        }
        /// <summary>
        /// 添加字典数据
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateMedicationDictionary([FromBody]CreateMedicationDictionaryRequestDto requestDto)
        {
            //校验名称是否存在
            var medicationDictionaryBiz = new MedicationDictionaryBiz();
            var medication = medicationDictionaryBiz.GetModelByNameAsync(requestDto.Name).Result;
            if (medication != null)
            {
                return Failed(Common.ErrorCode.UserData, "名称已存在");
            }
            var model = new MedicationDictionaryModel();
            model.MedicationGuid = Guid.NewGuid().ToString("N");
            model.MedicationType = requestDto.PrescriptionEnum.ToString();
            model.Name = requestDto.Name;
            model.CreatedBy = UserID;
            model.LastUpdatedBy = UserID;
            var result = await medicationDictionaryBiz.InsertAsync(model);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "添加失败");
        }
        /// <summary>
        /// 删除字典数据
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DeleteMedicationDictionary([FromBody]DeleteMedicationDictionaryRequestDto requestDto)
        {
            if (!requestDto.MedicationGuids.Any())
            {
                return Failed(ErrorCode.UserData, "至少选择一条数据");
            }
            var result = await new MedicationDictionaryBiz().DeleteAsync(requestDto.MedicationGuids);
            return result ? Success() : Failed(Common.ErrorCode.DataBaseError, "删除数据失败");
        }
        /// <summary>
        /// 编辑用药字典信息
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UpdateMedicationDictionaryAsync([FromBody]UpdateMedicationDictionaryRequestDto requestDto)
        {
            var medicationDictionaryBiz = new MedicationDictionaryBiz();
            var model = await medicationDictionaryBiz.GetAsync(requestDto.MedicationGuid);
            if (model == null)
            {
                return Failed(Common.ErrorCode.UserData, "未找到数据");
            }
            var medication = medicationDictionaryBiz.GetModelByNameAsync(requestDto.Name).Result;
            if (medication != null)
            {
                return Failed(Common.ErrorCode.UserData, "名称已存在");
            }
            model.LastUpdatedBy = UserID;
            model.MedicationType = requestDto.PrescriptionEnum.ToString();
            model.Name = requestDto.Name;
            model.LastUpdatedDate = DateTime.Now;
            model.Name = requestDto.Name;
            var result = await medicationDictionaryBiz.UpdateAsync(model);
            return result ? Success() : Failed(Common.ErrorCode.DataBaseError, "编辑数据失败");
        }


        /// <summary>
        /// 预约确认收款
        /// </summary>
        /// <param name="appointmentGuid">预约Id</param>
        /// <returns></returns>
        [HttpPost]
        [Produces(typeof(ResponseDto<bool>))]
        public async Task<IActionResult> ConfirmAppointmentReceivables(string appointmentGuid)
        {
            if (string.IsNullOrEmpty(appointmentGuid))
            {
                return Failed(ErrorCode.UserData, "用户预约参数未提供");
            }

            var appointmentBiz = new DoctorAppointmentBiz();

            var appointment = await appointmentBiz.GetAsync(appointmentGuid);
            if (appointment is null)
            {
                return Failed(ErrorCode.UserData, "预约不存在，请检查参数是否正确");
            }

            var prescriptionBiz = new PrescriptionBiz();

            var result = await prescriptionBiz.UpdateReceptionStatus(appointmentGuid);

            return result ? Success() : Failed(ErrorCode.DataBaseError, "更新收款状态失败");
        }

        /// <summary>
        /// 今日(未)收款
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(ResponseDto<GetMoneyTodayResponseDto>))]
        public async Task<IActionResult> GetCollectionTodayAsync()
        {
            GetMoneyTodayResponseDto result = new GetMoneyTodayResponseDto();
            var biz = new PrescriptionInformationBiz();
            var collectionTodayTotal = await biz.GetAPrescriptionMoneyAsync(DateTime.Now.Date, DateTime.Now.Date.AddDays(1).AddSeconds(-1), UserID, PrescriptionStatusEnum.Paied);
            var yesterdayCollectionTotal = await biz.GetAPrescriptionMoneyAsync(DateTime.Now.Date.AddDays(-1), DateTime.Now.Date.AddSeconds(-1), UserID, PrescriptionStatusEnum.Paied);
            var increase = yesterdayCollectionTotal > 0 ? ((collectionTodayTotal - yesterdayCollectionTotal) / yesterdayCollectionTotal) : 1M;
            if (collectionTodayTotal == 0 && yesterdayCollectionTotal == 0)
            {
                increase = 0M;
            }
            result.Collected = new GetCollectionTodayResponseDto
            {
                AmountCollected = collectionTodayTotal,
                Increase = Math.Round(increase, 2) * 100
            };
            var collectionUnTodayTotal = await biz.GetAPrescriptionMoneyAsync(DateTime.Now.Date, DateTime.Now.Date.AddDays(1).AddSeconds(-1), UserID, PrescriptionStatusEnum.Obligation);
            var yesterdayUnCollectionTotal = await biz.GetAPrescriptionMoneyAsync(DateTime.Now.Date.AddDays(-1), DateTime.Now.Date.AddSeconds(-1), UserID, PrescriptionStatusEnum.Obligation);
            var increaseUn = yesterdayUnCollectionTotal > 0 ? ((collectionUnTodayTotal - yesterdayUnCollectionTotal) / yesterdayUnCollectionTotal) : 1M;
            if (collectionUnTodayTotal == 0 && yesterdayUnCollectionTotal == 0)
            {
                increaseUn = 0M;
            }
            result.Uncollected = new GetUncollectedTodayResponseDto
            {
                AmountUncollected = collectionUnTodayTotal,
                Increase = Math.Round(increaseUn, 2) * 100
            };
            if (collectionTodayTotal != 0 || collectionUnTodayTotal != 0)
            {
                result.Collected.Percentage = Math.Round(collectionTodayTotal / (collectionTodayTotal + collectionUnTodayTotal), 2) * 100;
                result.Uncollected.Percentage = Math.Round(collectionUnTodayTotal / (collectionTodayTotal + collectionUnTodayTotal), 2) * 100;
            }
            return Success(result);
        }
        /// <summary>
        /// 本月(本年)收款
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(ResponseDto<GetMoneyMonthsAndYearResponseDto>))]
        public async Task<IActionResult> GetCollectionToMonthsAndYearAsync()
        {
            GetMoneyMonthsAndYearResponseDto result = new GetMoneyMonthsAndYearResponseDto();
            var biz = new PrescriptionInformationBiz();
            //本月
            DateTime sDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var collectionTodayTotal = await biz.GetAPrescriptionMoneyAsync(sDate, DateTime.Now, UserID, PrescriptionStatusEnum.Paied);
            var yesterdayCollectionTotal = await biz.GetAPrescriptionMoneyAsync(sDate.AddMonths(-1), DateTime.Now.AddMonths(-1), UserID, PrescriptionStatusEnum.Paied);
            var increase = yesterdayCollectionTotal > 0 ? ((collectionTodayTotal - yesterdayCollectionTotal) / yesterdayCollectionTotal) : 1M;
            if (collectionTodayTotal == 0 && yesterdayCollectionTotal == 0)
            {
                increase = 0M;
            }
            result.MonthsCollected = new GetCollectionMonthsResponseDto
            {
                AmountCollected = collectionTodayTotal,
                Increase = Math.Round(increase, 2) * 100
            };
            //本年
            DateTime sDateYear = new DateTime(DateTime.Now.Year, 1, 1);
            var collectionUnTodayTotal = await biz.GetAPrescriptionMoneyAsync(sDateYear, DateTime.Now, UserID, PrescriptionStatusEnum.Paied);
            var yesterdayUnCollectionTotal = await biz.GetAPrescriptionMoneyAsync(sDateYear.AddYears(-1), DateTime.Now.AddYears(-1), UserID, PrescriptionStatusEnum.Paied);
            var increaseUn = yesterdayUnCollectionTotal > 0 ? ((collectionUnTodayTotal - yesterdayUnCollectionTotal) / yesterdayUnCollectionTotal) : 1M;
            if (collectionUnTodayTotal == 0 && yesterdayUnCollectionTotal == 0)
            {
                increaseUn = 0M;
            }
            result.YearCollected = new GetCollectionYearResponseDto
            {
                AmountCollected = collectionUnTodayTotal,
                Increase = Math.Round(increaseUn, 2) * 100
            };
            return Success(result);
        }
        /// <summary>
        /// 收款比例
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(ResponseDto<GetCollectionRatioResponse>))]
        public async Task<IActionResult> GetCollectionRatiosync([FromQuery]GetCollectionRatioRequest requestDto)
        {
            var biz = new PrescriptionInformationBiz();
            //已收款
            var collectionTotal = await biz.GetAPrescriptionMoneyAsync(requestDto.StartDate.Date, requestDto.EndDate.Date.AddDays(1).AddSeconds(-1), UserID, PrescriptionStatusEnum.Paied);
            //未收款
            var unCollectionTotal = await biz.GetAPrescriptionMoneyAsync(requestDto.StartDate.Date, requestDto.EndDate.Date.AddDays(1).AddSeconds(-1), UserID, PrescriptionStatusEnum.Obligation);
            GetCollectionRatioResponse result = new GetCollectionRatioResponse
            {
                Receivable = collectionTotal,
                Uncollected = unCollectionTotal
            };
            return Success(result);
        }
        /// <summary>
        /// 获取收款统计数据
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(ResponseDto<GetCollectionRatioResponse>))]
        public async Task<IActionResult> GetCollectionDataync([FromQuery]GetCollectionRatioRequest requestDto)
        {
            var biz = new PrescriptionInformationBiz();
            //已收款
            var collectionTotal = await biz.GetAPrescriptionMoneyAsync(requestDto.StartDate.Date, requestDto.EndDate.Date.AddDays(1).AddSeconds(-1), UserID, PrescriptionStatusEnum.Paied);
            //未收款
            var unCollectionTotal = await biz.GetAPrescriptionMoneyAsync(requestDto.StartDate, requestDto.EndDate.Date.AddDays(1).AddSeconds(-1), UserID, PrescriptionStatusEnum.Obligation);
            GetCollectionRatioResponse result = new GetCollectionRatioResponse
            {
                Receivable = collectionTotal,
                Uncollected = unCollectionTotal
            };
            return Success(result);
        }
        /// <summary>
        /// 获取科室收入统计
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(ResponseDto<GetCollectionResponseDto>))]
        public async Task<IActionResult> GetAPrescriptionDataAsync([FromQuery]GetCollectionRequest requestDto)
        {
            var biz = new PrescriptionInformationBiz();
            var result = await biz.GetAPrescriptionDataAsync(requestDto.IsMonths, requestDto.StartDate, requestDto.EndDate, UserID, PrescriptionStatusEnum.Paied);
            return Success(result);
        }

        #region 处方相关操作接口
        /// <summary>
        /// 获取药品用法字典数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(ResponseDto<List<string>>))]
        public IActionResult GetPrescriptionDrugUsage()
        {
            var dict = new DictionaryBiz();
            var usage = dict.GetListByParentGuid(DictionaryType.DrugUsage);
            var names = usage.Select(d => d.ConfigName);
            return Success(names);
        }

        /// <summary>
        /// 获取药品频度单位字典数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(ResponseDto<List<string>>))]
        public IActionResult GetPrescriptionDrugFrequency()
        {
            var dict = new DictionaryBiz();
            var usage = dict.GetListByParentGuid(DictionaryType.DrugFrequency);
            var names = usage.Select(d => d.ConfigName);
            return Success(names);
        }

        /// <summary>
        /// 创建处方
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces(typeof(ResponseDto<SubmitPrescriptionSuccessResponseDto>))]
        public async Task<IActionResult> CreatePrescription([FromBody]ProcessPrescriptionRequestDto request)
        {
            Logger.Info($"{nameof(CreatePrescription)}: {JsonConvert.SerializeObject(request)}");

            var context = new PrescriptionContext(request);

            var appointmentBiz = new DoctorAppointmentBiz();
            var appointment = await appointmentBiz.GetAsync(request.AppointmentGuid);
            if (appointment is null)
            {
                return Failed(ErrorCode.Empty, "预约不存在，请检查");
            }

            var informationBiz = new PrescriptionInformationBiz();
            var prescriptionInformation = await informationBiz.ExistInformation(appointment.AppointmentGuid);

            if (prescriptionInformation != null)
            {
                return Failed(ErrorCode.Empty, "该处方已提交，请勿重复提交");
            }
            context.AppointmentModel = appointment;

            var validateResult = Validate(context);
            if (validateResult.Code != ErrorCode.Success)
            {
                return validateResult;
            }
            context.AppointmentModel.Status = AppointmentStatusEnum.Treated.ToString();

            var prescriptionBiz = new PrescriptionBiz();
            var result = await prescriptionBiz.CreatePrescription(context);
            if (!result)
            {
                return Failed(ErrorCode.DataBaseError, "创建处方记录失败，请稍后重试");
            }

            var response = new SubmitPrescriptionSuccessResponseDto
            {
                InformationGuid = context.InformationModel.InformationGuid,
                PrescriptionSuccess = context.PrescriptionModels.Select(d => new SubmitPrescriptionSuccessItemDto()
                {
                    PrescriptionGuid = d.PrescriptionGuid,
                    PrescriptionName = d.PrescriptionName
                })
            };

            return Success(response);
        }

        /// <summary>
        /// 修改处方
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces(typeof(ResponseDto<SubmitPrescriptionSuccessResponseDto>))]
        public async Task<IActionResult> UpdatePrescription([FromBody]
        ProcessPrescriptionRequestDto request)
        {
            if (string.IsNullOrEmpty(request.InformationGuid))
            {
                return Failed(ErrorCode.Empty, "预约不存在，请检查");
            }

            Logger.Info($"{nameof(UpdatePrescription)}: {JsonConvert.SerializeObject(request)}");

            var context = new PrescriptionContext(request);

            var appointmentBiz = new DoctorAppointmentBiz();
            var appointment = await appointmentBiz.GetAsync(request.AppointmentGuid);
            if (appointment is null)
            {
                return Failed(ErrorCode.Empty, "预约不存在，请检查");
            }
            context.AppointmentModel = appointment;

            var informationBiz = new PrescriptionInformationBiz();
            var information = await informationBiz.GetAsync(request.InformationGuid);
            if (information is null)
            {
                return Failed(ErrorCode.Empty, "用户信息不存在，请检查");
            }

            if (!information.AppointmentGuid.Equals(request.AppointmentGuid))
            {
                return Failed(ErrorCode.Empty, "预约记录和用户信息不一致，无法操作");
            }

            context.InformationModel = information;

            var prescriptionBiz = new PrescriptionBiz();

            var prescriptionModels = await prescriptionBiz.GetAllPrescriptionsAsync(information.InformationGuid);

            prescriptionModels = prescriptionModels.Where(d =>
            d.Status != PrescriptionStatusEnum.Cancellation.ToString()).ToList();

            context.dbPrescriptionModels = prescriptionModels;

            var validateResult = Validate(context);
            if (validateResult.Code != ErrorCode.Success)
            {
                return validateResult;
            }

            context.dbPrescriptionModels = prescriptionModels
                .Concat(context.PrescriptionModels).ToList();

            UpdatePrescriptionInformationPaidStatus(context);

            var result = await prescriptionBiz.UpdatePrescription(context);

            if (!result)
            {
                return Failed(ErrorCode.DataBaseError, "更新处方记录失败");
            }

            var prescriptions = prescriptionModels.Where(d => d.Status != PrescriptionStatusEnum.Cancellation.ToString())
                 .OrderBy(d => d.CreationDate)
                 .Concat(context.PrescriptionModels)
                 .Select(d => new SubmitPrescriptionSuccessItemDto()
                 {
                     PrescriptionGuid = d.PrescriptionGuid,
                     PrescriptionName = d.PrescriptionName
                 });

            var response = new SubmitPrescriptionSuccessResponseDto()
            {
                InformationGuid = information.InformationGuid,
                PrescriptionSuccess = prescriptions
            };

            return Success(response);
        }

        /// <summary>
        /// 作废处方
        /// </summary>
        /// <param name="request">作废请求参数</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CancellPrescription([FromBody]CancellPrescriptionRequestDto request)
        {
            var prescriptionBiz = new PrescriptionBiz();

            var prescription = await prescriptionBiz.GetAsync(request.PrescriptionGuid);

            if (prescription is null)
            {
                return Failed(ErrorCode.Empty, "处方记录不存在，请检查");
            }

            var name = prescription.PrescriptionName;

            if (prescription.Status == PrescriptionStatusEnum.Paied.ToString())
            {
                return Failed(ErrorCode.Empty, $"处方【{name}】已完成付款不可作废");
            }

            if (prescription.Status == PrescriptionStatusEnum.Cancellation.ToString())
            {
                return Failed(ErrorCode.Empty, $"处方【{name}】已作废，请勿重复提交");
            }

            prescription.Status = PrescriptionStatusEnum.Cancellation.ToString();
            prescription.LastUpdatedBy = UserID;
            prescription.LastUpdatedDate = DateTime.Now;
            prescription.Reason = request.Reason;

            var informationBiz = new PrescriptionInformationBiz();
            var information = await informationBiz.GetAsync(prescription.InformationGuid);
            if (information is null)
            {
                return Failed(ErrorCode.Empty, "处方记录不存在");
            }

            lock (locker)
            {
                information.TotalCost -= prescription.TotalCost;
            }

            var context = new PrescriptionContext(null)
            {
                InformationModel = information
            };

            var prescriptionModels = await prescriptionBiz.GetAllPrescriptionsAsync(information.InformationGuid, prescription.PrescriptionGuid);

            context.dbPrescriptionModels = prescriptionModels;

            UpdatePrescriptionInformationPaidStatus(context);

            var result = await prescriptionBiz.CancellPrescription(context.InformationModel, prescription);
            if (!result)
            {
                return Failed(ErrorCode.DataBaseError, $"作废处方【{prescription.PrescriptionName}】失败");
            }

            return Success();
        }

        /// <summary>
        /// 获取预约处方详情
        /// </summary>
        /// <param name="appointmentGuid">预约Guid</param>
        /// <param name="actionType">操作（调用）</param>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(ResponseDto<GetPrescriptionResponseDto>))]
        public async Task<IActionResult> GetPrescription(string appointmentGuid, string actionType = null)
        {
            if (string.IsNullOrEmpty(appointmentGuid))
            {
                return Failed(ErrorCode.Empty, "参数不正确，请检查");
            }

            var prescriptionBiz = new PrescriptionBiz();

            var result = await prescriptionBiz.GetPrescription(appointmentGuid, actionType);

            return Success(result);
        }

        /// <summary>
        /// 验证参数
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        ResponseDto Validate(PrescriptionContext context)
        {
            var request = context.Request;
            var prescriptionBiz = new PrescriptionBiz();

            if (request.PatientAge <= 0)
            {
                return Failed(ErrorCode.Empty, "年龄必须大于0");
            }

            if (request.PatientAge > 120)
            {
                return Failed(ErrorCode.Empty, "年龄超过最大120岁限制");
            }

            if (!string.IsNullOrEmpty(request.PatientSymptoms) &&
                request.PatientSymptoms.Length > 1000)
            {
                return Failed(ErrorCode.Empty, "患者症状超过最大长度限制");
            }

            context.InformationModel.AppointmentGuid = request.AppointmentGuid;
            context.InformationModel.PatientName = request.PatientName;
            context.InformationModel.PatientGender = request.PatientGender.ToString();
            context.InformationModel.PatientAge = request.PatientAge;
            context.InformationModel.PatientPhone = request.PatientPhone;
            context.InformationModel.HasAllergy = request.HasAllergy;
            context.InformationModel.HasChronicDisease = request.HasChronicDisease;
            context.InformationModel.PatientProvince = request.PatientProvince;
            context.InformationModel.PatientCity = request.PatientCity;
            context.InformationModel.PatientDistrict = request.PatientDistrict;
            context.InformationModel.PatientAddress = request.PatientAddress;
            context.InformationModel.ReceptionType = request.ReceptionType.ToString();
            context.InformationModel.ClinicalDiagnosis = request.ClinicalDiagnosis;
            context.InformationModel.PatientSymptoms = request.PatientSymptoms;
            context.InformationModel.DoctorGuid = context.AppointmentModel.DoctorGuid;
            context.InformationModel.HospitalGuid = context.AppointmentModel.HospitalGuid;
            context.InformationModel.OfficeGuid = context.AppointmentModel.OfficeGuid;

            var prescriptions = request.BasePrescriptions;

            if (string.IsNullOrEmpty(context.InformationModel.InformationGuid))
            {
                if (prescriptions.Count <= 0)
                {
                    return Failed(ErrorCode.Empty, "处方数据为空，请检查");
                }

                context.InformationModel.InformationGuid = Guid.NewGuid().ToString("N");
                context.InformationModel.PaidStatus = PrescriptionInformationPaidStatus
                    .NotPaid.ToString();
                context.InformationModel.AppointmentTime = DateTime.Now;
                context.InformationModel.CreatedBy = UserID;
                context.InformationModel.LastUpdatedBy = UserID;
            }
            else
            {
                context.InformationModel.LastUpdatedDate = DateTime.Now;

                if (prescriptions.Count <= 0)
                {
                    return Success();
                }

            }

            if (prescriptions.Any(d => d.Receptions.Count <= 0))
            {
                return Failed(ErrorCode.Empty, "存在药品数据为空的处方，请检查");
            }

            var duplicatePreNameExists = prescriptions.GroupBy(d => d.PrescriptionName?.Trim())
                     .Any(g => g.Count() > 1);

            if (duplicatePreNameExists)
            {
                return Failed(ErrorCode.Empty, "处方名称有重复，请检查");
            }

            #region 随机生成七位纯数字处方编号

            var uniquePrescriptions = (ConcurrentBag<string>)null;

            lock (locker)
            {
                uniquePrescriptions = prescriptionBiz.GetUniquePrescriptionNos(prescriptions.Count);
            }
            #endregion

            foreach (var prescription in prescriptions)
            {
                var prescriptionModel = new PrescriptionModel();

                lock (locker)
                {
                    var precriptionTotalFee = prescription.Receptions
                        .Sum(d => d.ItemPrice * d.ItemQuantity);

                    context.InformationModel.TotalCost += precriptionTotalFee;

                    prescriptionModel.TotalCost = precriptionTotalFee;
                }

                var name = prescription.PrescriptionName;
                if (string.IsNullOrEmpty(name?.Trim()))
                {
                    return Failed(ErrorCode.Empty, "存在名称为空的处方，请检查");
                }

                if (name.Length > 50)
                {
                    return Failed(ErrorCode.Empty, $"处方【{name}】名称超过最大长度限制，请检查");
                }

                if (context.dbPrescriptionModels.Count > 0)
                {
                    if (context.dbPrescriptionModels.Any(d => d.PrescriptionName.Equals(name)))
                    {
                        return Failed(ErrorCode.Empty, "已提交，请勿重复提交");
                    }
                }

                var pewscriptionGuid = Guid.NewGuid().ToString("N");

                if (!uniquePrescriptions.TryTake(out var no))
                {
                    return Failed(ErrorCode.Empty, "服务器操作繁忙，请稍后重试");
                }

                prescriptionModel.PrescriptionGuid = pewscriptionGuid;
                prescriptionModel.InformationGuid = context.InformationModel.InformationGuid;
                prescriptionModel.PrescriptionName = name;
                prescriptionModel.PrescriptionNo = no;
                prescriptionModel.Status = PrescriptionStatusEnum.Obligation.ToString();
                prescriptionModel.CreatedBy = UserID;
                prescriptionModel.LastUpdatedBy = UserID;

                context.PrescriptionModels.Add(prescriptionModel);

                foreach (var reception in prescription.Receptions)
                {
                    if (string.IsNullOrEmpty(reception.ItemName?.Trim()))
                    {
                        return Failed(ErrorCode.Empty, $"处方【{name}】下存在名称为空的药品，请检查");
                    }

                    if (reception.ItemName.Length > 50)
                    {
                        return Failed(ErrorCode.Empty, $"药品【{reception.ItemName}】名称超过最大长度限制，请检查");
                    }

                    if (!string.IsNullOrEmpty(reception.ItemSpecification?.Trim()))
                    {
                        if (reception.ItemSpecification.Length > 50)
                        {
                            return Failed(ErrorCode.Empty, $"药品【{reception.ItemName}】名称超过最大长度限制，请检查");
                        }
                    }

                    if (reception.ItemPrice < 0)
                    {
                        return Failed(ErrorCode.Empty, $"处方【{name}】下存在单价小于0的药品，请检查");
                    }

                    if (reception.ItemQuantity < 1)
                    {
                        return Failed(ErrorCode.Empty, $"处方【{name}】下存在数量小于1的药品，请检查");
                    }

                    if (reception.ItemType == ReceptionRecipeTypeEnum.Drug)
                    {
                        if (!reception.DrugFrequencyQuantity.HasValue)
                        {
                            return Failed(ErrorCode.Empty, $"处方【{name}】下存在用药频度数量需提供，请检查");
                        }

                        if (reception.DrugFrequencyQuantity.Value <= 0)
                        {
                            return Failed(ErrorCode.Empty, $"处方【{name}】下存在用药频度数量小于0的药品，请检查");
                        }

                        if (!reception.DrugFrequencyQuantity.HasValue)
                        {
                            return Failed(ErrorCode.Empty, $"处方【{name}】下存在用药频度数量需提供，请检查");
                        }

                        if (reception.DrugFrequencyTimes.Value <= 0)
                        {
                            return Failed(ErrorCode.Empty, $"处方【{name}】下存在用药频度小于0的药品，请检查");
                        }

                        if (string.IsNullOrEmpty(reception.DrugFrequencyUnit?.Trim()))
                        {
                            return Failed(ErrorCode.Empty, $"处方【{name}】下存在用药频度单位为空的药品，请检查");
                        }

                        if (reception.DrugFrequencyUnit.Length > 50)
                        {
                            return Failed(ErrorCode.Empty, $"处方【{name}】下存在用药频度单位超过最大长度限制，请检查");
                        }
                    }

                    context.RecipeModels.Add(new PrescriptionRecipeModel()
                    {
                        RecipeGuid = Guid.NewGuid().ToString("N"),
                        PrescriptionGuid = pewscriptionGuid,
                        ItemType = reception.ItemType.ToString(),
                        ItemName = reception.ItemName,
                        ItemSpecification = reception.ItemSpecification,
                        ItemPrice = reception.ItemPrice,
                        ItemQuantity = reception.ItemQuantity,
                        DrugDosage = reception.DrugDosage,
                        DrugFrequencyQuantity = reception.DrugFrequencyQuantity,
                        DrugFrequencyTimes = reception.DrugFrequencyTimes,
                        DrugFrequencyUnit = reception.DrugFrequencyUnit,
                        DrugUsage = reception.DrugUsage,
                        LastUpdatedBy = UserID,
                        CreatedBy = UserID
                    });
                }
            }
            return Success();
        }

        /// <summary>
        /// 更新接诊记录收款状态
        /// </summary>
        /// <param name="context">操作上下文</param>
        /// <returns></returns>
        void UpdatePrescriptionInformationPaidStatus(
          PrescriptionContext context)
        {
            var prescriptionModels = context.dbPrescriptionModels;

            var isObligation = prescriptionModels.Any(d => d.Status == PrescriptionStatusEnum.Obligation.ToString());

            var isPaied = prescriptionModels.Any(d => d.Status == PrescriptionStatusEnum
            .Paied.ToString());

            if (isObligation && !isPaied)
            {
                context.InformationModel.PaidStatus
                    = PrescriptionInformationPaidStatus.NotPaid.ToString();
            }
            else if (!isObligation && isPaied)
            {
                context.InformationModel.PaidStatus
                    = PrescriptionInformationPaidStatus.Paided.ToString();
            }
            else if (isObligation && isPaied)
            {
                context.InformationModel.PaidStatus
                    = PrescriptionInformationPaidStatus.PartiallyUnpaid.ToString();
            }
            else
            {
                context.InformationModel.PaidStatus
                    = PrescriptionInformationPaidStatus.None.ToString();
            }

            context.InformationModel.LastUpdatedBy = UserID;
            context.InformationModel.LastUpdatedDate = DateTime.Now;
        }

        #endregion

        #region 处方记录

        /// <summary>
        /// 获取处方单据数据(打印处方单时的预览数据)
        /// </summary>
        /// <param name="informationGuid">诊疗记录guid</param>
        /// <param name="prescriptionIds">prescriptionIds（处方Id，多个处方用逗号隔开）</param>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(ResponseDto<List<GetPrescriptionBillsResponseDto>>))]
        public async Task<IActionResult> GetPrescriptionBillsAsync(string informationGuid, string prescriptionIds = null)
        {
            var result = await new PrescriptionBiz()
                .GetPrescriptionBillsAsync(informationGuid, prescriptionIds);

            return Success(result);
        }

        /// <summary>
        /// 获取处方记录分页列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(ResponseDto<GetPrescriptionRecordPageListResponseDto>))]
        public async Task<IActionResult> GetPrescriptionRecordPageListAsync([FromQuery]GetPrescriptionRecordPageListRequestDto requestDto)
        {
            var result = await new PrescriptionInformationBiz().GetPrescriptionRecordPageListAsync(requestDto);
            var unpaidPrescriptions = await new PrescriptionBiz().GetUnpaidPrescriptionsByInformationIdsAsync(result.CurrentPage.Select(a => a.InformationGuid).Distinct().ToList());
            foreach (var item in result.CurrentPage)
            {
                item.ObligationAmount = unpaidPrescriptions.Where(a => a.InformationGuid == item.InformationGuid).Sum(a => a.TotalCost);
            }
            return Success(result);
        }

        /// <summary>
        /// 获取历史处方记录分页列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(ResponseDto<GetHistoryPrescriptionRecordsResponseDto>))]
        public async Task<IActionResult> GetHistoryPrescriptionRecordPageListAsync([FromQuery]GetHistoryPrescriptionRecordsRequestDto requestDto)
        {
            var appointment = await new DoctorAppointmentBiz().GetAsync(requestDto.AppointmentGuid);

            if (appointment is null)
            {
                return Failed(ErrorCode.Empty, "预约记录不存在");
            }

            requestDto.UserGuid = appointment.UserGuid;

            var result = await new PrescriptionInformationBiz().GetHistoryPrescriptionRecordsAsync(requestDto);

            return Success(result);
        }
        #endregion
    }
}
