using Dapper;
using GD.BizBase;
using GD.DataAccess;
using GD.Dtos.Doctor.Prescription;
using GD.Dtos.Enum;
using GD.Models.Doctor;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GD.Doctor
{
    public class PrescriptionBiz : BaseBiz<PrescriptionModel>
    {
        private static readonly Random random = new Random();
        public async Task<bool> UpdateReceptionStatus(
            string appointmentGuid)
        {
            var sql = @"UPDATE t_doctor_prescription p
                        JOIN t_doctor_prescription_information pi ON p.information_guid = pi.information_guid
                        SET 
                            p.`status` = 2,
                            pi.`paid_status` = 3,
                            p.last_updated_date = CURRENT_TIMESTAMP
                        WHERE pi.appointment_guid = @appointmentGuid AND p.`status` = 1";

            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.ExecuteAsync(sql, new { appointmentGuid }) > 0;
            }
        }

        /// <summary>
        /// 获取处方单据数据(打印处方单时的预览数据)
        /// </summary>
        /// <param name="informationGuid"></param>
        /// <returns></returns>
        public async Task<List<GetPrescriptionBillsResponseDto>> GetPrescriptionBillsAsync(string informationGuid, string prescriptionIds = null)
        {
            var sql = @"SELECT
                            c.prescription_name,
							c.prescription_no,
                            c.`status` as prescription_status,
							ifnull(c.creation_date,a.creation_date) AS billing_date,
							a.patient_name,
							a.patient_gender,
							a.patient_age,
							a.patient_phone,
							a.reception_no,
							b.office_name,
                            f.hos_name,
							a.clinical_diagnosis,
                            a.patient_symptoms,
							CONCAT(a.patient_province,a.patient_city,a.patient_district,a.patient_address) as patient_address,
							e.user_name AS doctor_name,
							c.total_cost,
							d.item_name,
							d.item_specification,
							d.drug_usage,
							d.drug_dosage,
							d.drug_frequency_quantity,
                            d.drug_frequency_unit,
                            d.drug_frequency_times,
							d.item_quantity 
						FROM
							t_doctor_prescription_information a
							LEFT JOIN t_doctor_office b ON a.office_guid = b.office_guid
							LEFT JOIN t_doctor_prescription c ON a.information_guid = c.information_guid 
							AND c.`enable` = 1
							LEFT JOIN t_doctor_prescription_recipe d ON d.prescription_guid = c.prescription_guid
							LEFT JOIN t_utility_user e ON e.user_guid = a.doctor_guid 
                            LEFT JOIN t_doctor_hospital f on f.hospital_guid=a.hospital_guid
						WHERE
							a.`enable` = 1";

            var prescriptionGuIds = (string[])null;

            if (!string.IsNullOrEmpty(informationGuid))
            {
                sql = $"{sql} AND a.information_guid = @informationGuid";
            }

            if (!string.IsNullOrEmpty(prescriptionIds))
            {
                prescriptionGuIds = prescriptionIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                sql = $"{sql} AND c.prescription_guid in @prescriptionGuIds";
            }


            var data = new List<GetPrescriptionBillsItemDto>();
            using (var conn = MySqlHelper.GetConnection())
            {
                data = (await conn.QueryAsync<GetPrescriptionBillsItemDto>(sql,
                    new { informationGuid, prescriptionGuIds })).ToList();
            }
            var result = data.GroupBy(a => new
            {
                a.PrescriptionNo,
                a.PrescriptionName,
                a.PrescriptionStatus,
                a.BillingDate,
                a.PatientName,
                a.PatientGender,
                a.PatientAge,
                a.PatientPhone,
                a.ReceptionNo,
                a.HosName,
                a.OfficeName,
                a.ClinicalDiagnosis,
                a.PatientSymptoms,
                a.PatientAddress,
                a.DoctorName,
                a.TotalCost
            }).Select(a => new GetPrescriptionBillsResponseDto
            {
                PrescriptionNo = a.Key.PrescriptionNo,
                PrescriptionName = a.Key.PrescriptionName,
                PrescriptionStatus = a.Key.PrescriptionStatus,
                BillingDate = a.Key.BillingDate,
                PatientName = a.Key.PatientName,
                PatientGender = a.Key.PatientGender,
                PatientAge = a.Key.PatientAge,
                PatientPhone = a.Key.PatientPhone,
                ReceptionNo = a.Key.ReceptionNo,
                HosName = a.Key.HosName,
                OfficeName = a.Key.OfficeName,
                ClinicalDiagnosis = a.Key.ClinicalDiagnosis,
                PatientSymptoms = a.Key.PatientSymptoms,
                PatientAddress = a.Key.PatientAddress,
                DoctorName = a.Key.DoctorName,
                TotalCost = a.Key.TotalCost,
                RecipeItems = a.Select(item => new GetPrescriptionBillsResponseDto.RecipeItemDto
                {
                    ItemName = item.ItemName,
                    ItemSpecification = item.ItemSpecification,
                    DrugUsage = item.DrugUsage,
                    DrugDosage = item.DrugDosage,
                    DrugFrequencyQuantity = item.DrugFrequencyQuantity,
                    DrugFrequencyUnit = item.DrugFrequencyUnit,
                    DrugFrequencyTimes = item.DrugFrequencyTimes,
                    ItemQuantity = item.ItemQuantity
                }).OrderBy(item => item.ItemName).ToList()
            }).OrderBy(a => a.BillingDate).ToList();
            return result;
        }

        /// <summary>
        /// 创建处方
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<bool> CreatePrescription(PrescriptionContext context)
        {
            var today = DateTime.Now.ToString("yyyyMMdd");

            var maxReceptionNo = GetMaxReceptionNoForToday(today) ?? $"{today}0000";

            var nextReceptionNo = $"{Convert.ToInt64(maxReceptionNo) + 1}".ToString();

            context.InformationModel.ReceptionNo = nextReceptionNo;

            return await MySqlHelper.TransactionAsync(async (conn, trans) =>
            {
                var updateResult = await conn.UpdateAsync(context.AppointmentModel);

                if (updateResult <= 0) { return false; }

                var insertResult = await conn.InsertAsync<string, PrescriptionInformationModel>(context.InformationModel);

                if (string.IsNullOrEmpty(insertResult))
                {
                    return false;
                }

                var insertPrescriptionResult = context.PrescriptionModels.InsertBatch(conn);
                if (insertPrescriptionResult <= 0)
                {
                    return false;
                }

                var insertRecipeResult = context.RecipeModels.InsertBatch(conn);

                if (insertRecipeResult <= 0)
                {
                    return false;
                }

                return true;
            });
        }

        /// <summary>
        /// 更新处方
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<bool> UpdatePrescription(PrescriptionContext context)
        {
            return await MySqlHelper.TransactionAsync(async (conn, trans) =>
            {
                var updateResult = await conn.UpdateAsync(context.InformationModel);
                if (updateResult <= 0)
                {
                    return false;
                }

                if (context.PrescriptionModels.Count > 0)
                {
                    var prescriptionsResult = context.PrescriptionModels.InsertBatch(conn);
                    if (prescriptionsResult <= 0)
                    {
                        return false;
                    }

                    var recipeResult = context.RecipeModels.InsertBatch(conn);
                    if (recipeResult <= 0)
                    {
                        return false;
                    }
                }

                return true;
            });
        }

        /// <summary>
        /// 作废处方
        /// </summary>
        /// <param name="model"></param>
        /// <param name="prescriptionModel"></param>
        /// <returns></returns>
        public async Task<bool> CancellPrescription(PrescriptionInformationModel model, PrescriptionModel prescriptionModel)
        {
            return await MySqlHelper.TransactionAsync(async (conn, trans) =>
            {
                var updateResult = await conn.UpdateAsync(model);
                if (updateResult <= 0)
                {
                    return false;
                }

                var prescritionResult = await conn.UpdateAsync(prescriptionModel);

                if (prescritionResult <= 0)
                {
                    return false;
                }

                return true;
            });
        }

        /// <summary>
        /// 获取预约处方详情
        /// </summary>
        /// <param name="appointmentGuid"></param>
        /// <returns></returns>
        public async Task<GetPrescriptionResponseDto> GetPrescription(string appointmentGuid,
            string action)
        {
            var response = (GetPrescriptionResponseDto)null;

            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"
                SELECT 
                        information_guid,
                        patient_name,
                        patient_phone,
                        patient_gender,
                        patient_age,
                        patient_province,
                        patient_city,
                        patient_district,
                        patient_address,
                        clinical_diagnosis,
                        patient_symptoms,
                        reception_type,
                        has_allergy,
                        has_chronic_disease    
                FROM `t_doctor_prescription_information`
                WHERE appointment_guid = @appointmentGuid";

                response = await conn.QueryFirstOrDefaultAsync<GetPrescriptionResponseDto>(sql, new { appointmentGuid });

                if (response is null) { return null; }

                sql = @"
                SELECT 
                        prescription_guid,
                        prescription_name,
                        reason,
                        status
                FROM t_doctor_prescription
                WHERE information_guid = @InformationGuid";

                //页面上点击【调用】按钮，过滤去除“作废”处方
                if (!string.IsNullOrEmpty(action) && action.Equals("invoke"))
                {
                    sql = $"{sql} and `status` in(1,2)";
                }

                response.Prescriptions = (await conn.QueryAsync<GetPrescrtionDetail>(sql, new { response.InformationGuid })).ToList();

                if (response.Prescriptions.Count <= 0)
                {
                    return response;
                }

                var cancels = response.Prescriptions.Where(d => d.Status == PrescriptionStatusEnum.Cancellation.ToString()).ToList();

                var histories = response.Prescriptions.Where(d => d.Status == PrescriptionStatusEnum.Paied.ToString());

                var inserts = response.Prescriptions.Where(d => d.Status == PrescriptionStatusEnum.Obligation.ToString());

                response.Prescriptions = cancels.Concat(histories).Concat(inserts).ToList();

                var prescriptionGuids = response.Prescriptions.Select(d => d.PrescriptionGuid)
                    .ToArray();

                sql = @"
                SELECT 
                        prescription_guid,
                        recipe_guid,
                        item_name,
                        item_type,
                        item_specification,
                        item_price,
                        item_quantity,
                        drug_usage,
                        drug_dosage,
                        drug_frequency,
                        drug_frequency_quantity,
                        drug_frequency_unit,
                        drug_frequency_times
                FROM t_doctor_prescription_recipe
                WHERE prescription_guid IN @prescriptionGuids";

                var receptions = (await conn.QueryAsync<GetPrescriptionReception>(sql, new { prescriptionGuids })).ToList();

                foreach (var prescription in response.Prescriptions)
                {
                    prescription.Receptions = receptions.Where(d => d.PrescriptionGuid.Equals(prescription.PrescriptionGuid)).ToList();
                }

                return response;
            }
        }

        /// <summary>
        /// 获取当天已存储最大门诊编号
        /// </summary>
        /// <returns></returns>
        string GetMaxReceptionNoForToday(string today)
        {
            var maxReception = $"{today}%";
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"
                SELECT MAX(reception_no) FROM `t_doctor_prescription_information`
                WHERE reception_no LIKE @maxReception";

                return conn.ExecuteScalar<string>(sql, new { maxReception });
            }
        }

        /// <summary>
        /// 随机生成指定数量唯一处方编号
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public ConcurrentBag<string> GetUniquePrescriptionNos(int count)
        {
            var prescriptions = GetPrecriptions();

            var bag = new ConcurrentBag<string>();

            var random = new Random();

            for (int i = 0; ; i++)
            {
                if (bag.Count == count)
                {
                    break;
                }

                var number = (random.Next(0, 1000) * random.Next(0, 10000))
                .ToString().PadLeft(7, '0');

                if (prescriptions.Contains(number))
                {
                    continue;
                }

                if (bag.Contains(number))
                {
                    continue;
                }

                bag.Add(number);
            }

            return bag;
        }

        /// <summary>
        /// 获取已生成处方编号
        /// </summary>
        /// <returns></returns>
        List<string> GetPrecriptions()
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"SELECT prescription_no FROM t_doctor_prescription";

                return conn.Query<string>(sql).ToList();
            }
        }

        /// <summary>
        /// 获取接诊记录下所有处方记录（或排除指定处方）
        /// </summary>
        /// <param name="informationGuid"></param>
        /// <returns></returns>
        public async Task<List<PrescriptionModel>> GetAllPrescriptionsAsync(string informationGuid,
            string prescriptionGuid = null)
        {
            var sql = string.Empty;

            if (!string.IsNullOrEmpty(prescriptionGuid))
            {
                sql = " and prescription_guid != @prescriptionGuid ";
            }

            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.GetListAsync<PrescriptionModel>($"where information_guid = @informationGuid {sql} and `enable`=1", new { informationGuid, prescriptionGuid });
                return result.ToList();
            }
        }

        public async Task<List<PrescriptionModel>> GetUnpaidPrescriptionsByInformationIdsAsync(List<string> informationGuids)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.GetListAsync<PrescriptionModel>($"where information_guid in @informationGuids and `status`=1 and `enable`=1", new { informationGuids });
                return result.ToList();
            }
        }
    }
}
