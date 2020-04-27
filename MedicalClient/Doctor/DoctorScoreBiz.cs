using GD.DataAccess;
using GD.Dtos.Doctor.Score;
using GD.Models.Doctor;
using Dapper;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GD.Models.Utility;
using GD.Common.EnumDefine;
using GD.Dtos.Utility.Score;
using System;

namespace GD.Doctor
{
    /// <summary>
    /// 医生积分逻辑处理类
    /// </summary>
    public class DoctorScoreBiz
    {
        /// <summary>
        /// 获取医生积分Dto
        /// </summary>
        /// <param name="doctorGuid">医生GUID</param>
        /// <param name="requestDto">医生积分查询dto</param>
        /// <returns></returns>
        public async Task<ScorePageDto<GetDoctorScoreResponseDto>> GetDoctorScoreResponseDtoList(string doctorGuid, GetDoctorScoreRequestDto requestDto)
        {
            ScorePageDto<GetDoctorScoreResponseDto> result = new ScorePageDto<GetDoctorScoreResponseDto>();
            UserModel user = new Utility.UserBiz().GetUser(doctorGuid);
            DoctorModel doctor = new DoctorBiz().GetDoctor(doctorGuid);
            if (doctor != null)
            {
                requestDto.userId = doctorGuid;
                requestDto.userType = UserType.Doctor.ToString();
                requestDto.startTime = GetStartDate(requestDto.startTime);
                requestDto.endTime = GetEndDate(requestDto.endTime);

                result = await GetScoreModels(requestDto);
                foreach (var item in result.CurrentPage)
                {
                    if (item.Variation > 0)
                    {
                        item.IncomeVariation = item.Variation;
                    }
                    else
                    {
                        item.OutlayVariation = item.Variation;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 获取医生积分所有信息
        /// </summary>
        /// <param name="userID">医生GUID</param>
        /// <param name="requestDto">医生积分查询dto</param>
        /// <returns></returns>
        public async Task<GetDoctorScoreAllInfoResponseDto> GetDoctorScoreAllInfo(string userID, GetDoctorScoreAllInfoRequestDto requestDto)
        {
            GetDoctorScoreAllInfoResponseDto scoreAll = new GetDoctorScoreAllInfoResponseDto();
            requestDto.startTime = GetStartDate(requestDto.startTime);
            requestDto.endTime = GetEndDate(requestDto.endTime);

            scoreAll.AllVariation = await GetVariationAll(userID, requestDto, UserType.Doctor) ?? 0;
            scoreAll.IncomeALLVariation = await GetIncomeVariationAll(userID, requestDto, UserType.Doctor) ?? 0;
            scoreAll.OutlayALLVariation = await GetOutlayVariationAll(userID, requestDto, UserType.Doctor) ?? 0;
            return scoreAll;
        }

        /// <summary>
        /// 获取积分规则Model
        /// </summary>
        /// <param name="userType">用户类型</param>
        /// <param name="actionCharacteristics">行为特征枚举</param>
        /// <returns></returns>
        public async Task<List<GetActionRulesResponseDto>> GetScoreRulesModel(UserType userType, ActionCharacteristicsEnum actionCharacteristics = ActionCharacteristicsEnum.Action)
        {
            string sqlstring = $@"select 
                                    sr.rules_guid AS RulesGuid,
                                    ac.action_characteristics_code AS ActionCharacteristicsCode,
                                    ac.action_characteristics_name AS ActionCharacteristicsName,
                                    ac.action_characteristics_type AS ActionCharacteristicsType
                                from t_utility_score_rules as sr 
                                     inner join t_utility_user_action as ua on sr.user_action_guid=ua.user_action_guid
                                     inner join t_utility_action_characteristics as ac on ac.action_characteristics_guid=ua.action_guid
                                where ac.action_characteristics_type=@action_characteristics_type 
                                     and ua.user_type_guid=@user_type_guid";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("action_characteristics_type", actionCharacteristics.ToString(), System.Data.DbType.String);
            parameters.Add("user_type_guid", userType.ToString(), System.Data.DbType.String);
            using (MySql.Data.MySqlClient.MySqlConnection conn = MySqlHelper.GetConnection())
            {
                return (await conn.QueryAsync<GetActionRulesResponseDto>(sqlstring, parameters)).AsList();
            }
        }

        /// <summary>
        /// 总积分
        /// </summary>
        /// <param name="userGuid">用户GUID</param>
        /// <param name="requestDto">查询Dto</param>
        /// <param name="userType">用户类型</param>
        /// <returns></returns>
        private async Task<int?> GetVariationAll(string userGuid, GetDoctorScoreAllInfoRequestDto requestDto, UserType userType)
        {
            StringBuilder sb = new StringBuilder("select sum(variation) from t_utility_score where user_guid=@user_guid and user_type_guid=@userType");
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("user_guid", userGuid, System.Data.DbType.String);
            parameters.Add("userType", userType.ToString(), System.Data.DbType.String);

            if (requestDto.startTime != null)
            {
                sb.Append(" and creation_date>@startTime ");
                parameters.Add("startTime", requestDto.startTime, System.Data.DbType.DateTime);
            }
            if (requestDto.endTime != null)
            {
                sb.Append(" and creation_date<@endTime ");
                parameters.Add("endTime", requestDto.endTime, System.Data.DbType.DateTime);
            }
            using (MySql.Data.MySqlClient.MySqlConnection conn = MySqlHelper.GetConnection())
            {
                return await conn.QuerySingleAsync<int?>(sb.ToString(), parameters);
            }
        }

        /// <summary>
        /// 支出总积分
        /// </summary>
        /// <param name="userGuid">用户GUID</param>
        /// <param name="requestDto">查询Dto</param>
        /// <param name="userType">用户类型</param>
        /// <returns></returns>
        private async Task<int?> GetIncomeVariationAll(string userGuid, GetDoctorScoreAllInfoRequestDto requestDto, UserType userType)
        {
            StringBuilder sb = new StringBuilder("select sum(variation) from t_utility_score where variation>0 and user_guid=@user_guid and user_type_guid=@userType");
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("user_guid", userGuid, System.Data.DbType.String);
            parameters.Add("userType", userType.ToString(), System.Data.DbType.String);

            if (requestDto.startTime != null)
            {
                sb.Append(" and creation_date>@startTime ");
                parameters.Add("startTime", requestDto.startTime, System.Data.DbType.DateTime);
            }
            if (requestDto.endTime != null)
            {
                sb.Append(" and creation_date<@endTime ");
                parameters.Add("endTime", requestDto.endTime, System.Data.DbType.DateTime);
            }
            using (MySql.Data.MySqlClient.MySqlConnection conn = MySqlHelper.GetConnection())
            {
                return await conn.QueryFirstAsync<int?>(sb.ToString(), parameters);
            }
        }

        /// <summary>
        /// 收入总积分
        /// </summary>
        /// <param name="userGuid">用户GUID</param>
        /// <param name="requestDto">查询Dto</param>
        /// <param name="userType">用户类型</param>
        /// <returns></returns>
        private async Task<int?> GetOutlayVariationAll(string userGuid, GetDoctorScoreAllInfoRequestDto requestDto, UserType userType)
        {
            StringBuilder sb = new StringBuilder("select sum(variation) from t_utility_score where variation<0 and user_guid=@user_guid and user_type_guid=@userType");
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("user_guid", userGuid, System.Data.DbType.String);
            parameters.Add("userType", userType.ToString(), System.Data.DbType.String);

            if (requestDto.startTime != null)
            {
                sb.Append(" and creation_date>@startTime ");
                parameters.Add("startTime", requestDto.startTime, System.Data.DbType.DateTime);
            }
            if (requestDto.endTime != null)
            {
                sb.Append(" and creation_date<@endTime ");
                parameters.Add("endTime", requestDto.endTime, System.Data.DbType.DateTime);
            }
            using (MySql.Data.MySqlClient.MySqlConnection conn = MySqlHelper.GetConnection())
            {
                return await conn.QueryFirstAsync<int?>(sb.ToString(), parameters);
            }
        }

        /// <summary>
        /// 获取积分Model
        /// </summary>
        /// <param name="requestDto">医生积分查询dto</param>
        /// <returns></returns>
        public async Task<ScorePageDto<GetDoctorScoreResponseDto>> GetScoreModels(GetDoctorScoreRequestDto requestDto)
        {
            StringBuilder sb = new StringBuilder("select creation_date as CreationDate,variation as Variation,reason as Reason from t_utility_score where user_guid=@userId and user_type_guid=@userType ");

            if (!string.IsNullOrWhiteSpace(requestDto.UserActionGuid))
            {
                sb.Append(@" and rules_guid = @UserActionGuid ");
                requestDto.UserActionGuid = $"{requestDto.UserActionGuid}";
            }
            if (requestDto.startTime != null)
            {
                sb.Append(" and creation_date>@startTime ");
            }
            if (requestDto.endTime != null)
            {
                sb.Append(" and creation_date<@endTime ");
            }
            sb.Append(" order by creation_date desc ");
            using (MySql.Data.MySqlClient.MySqlConnection conn = MySqlHelper.GetConnection())
            {
                return await MySqlHelper.QueryByPageAsync<GetDoctorScoreRequestDto, ScorePageDto<GetDoctorScoreResponseDto>, GetDoctorScoreResponseDto>(sb.ToString(), requestDto);
            }
        }

        /// <summary> 
        /// 获取一天中的开始时间
        /// </summary> 
        /// <param name="someDate">该周中任意一天</param> 
        /// <returns>返回礼拜一日期，后面的具体时、分、秒清零</returns> 
        private DateTime? GetStartDate(DateTime? startDate)
        {
            if (startDate != null)
            {
                return GetStartDate(startDate.Value);
            }
            return null;
        }

        /// <summary>
        /// 计算某日结束日期
        /// </summary>
        /// <param name="endDate">时间</param>
        /// <returns>时、分、秒改成最后1毫秒</returns>
        private DateTime? GetEndDate(DateTime? endDate)
        {
            if (endDate != null)
            {
                return GetEndDate(endDate.Value);
            }
            return null;
        }

        /// <summary>
        /// 获取一天中的开始时间
        /// </summary>
        /// <param name="startDate">时间</param>
        /// <returns>时、分、秒清零</returns>
        private DateTime GetStartDate(DateTime startDate)
        {
            return new DateTime(startDate.Year, startDate.Month, startDate.Day);
        }

        /// <summary> 
        /// 计算某日结束日期
        /// </summary> 
        /// <param name="someDate">该周中任意一天</param> 
        /// <returns>时、分、秒改成最后1毫秒</returns> 
        private DateTime GetEndDate(DateTime endDate)
        {
            return new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59, 999);
        }
    }
}