using Dapper;
using GD.DataAccess;
using GD.Dtos.Health;
using GD.Models.CommonEnum;
using GD.Models.Consumer;
using GD.Models.Health;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GD.Manager.Health
{
    public class HealthInformationBiz : BaseBiz<HealthInformationModel>
    {
        /// <summary>
        /// 获取用户健康基础信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<GetHealthInformationResponseDto>> GetHealthInformationList(string userId)
        {
            var informations = (List<GetHealthInformationResponseDto>)null;

            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"SELECT
	                            a.information_guid,
	                            a.information_type,
	                            a.subject_name,
	                            a.subject_unit,
	                            a.subject_prompt_text,
                                a.is_single_line,
	                            b.option_guids,
	                            b.result_value 
                            FROM
	                            t_health_information a
	                            LEFT JOIN t_consumer_health_info b ON a.information_guid = b.information_guid 
	                            AND a.`enable` = 1 and  b.user_guid = @userId
                            ORDER BY
	                            a.sort";

                informations = (await conn.QueryAsync<GetHealthInformationResponseDto>(sql,
                    new { userId })).ToList();


                if (informations is null || informations.Count <= 0)
                {
                    return informations;
                }

                var options = await GetHealthInformationOptions();
                if (options is null || options.Count <= 0)
                {
                    return informations;
                }

                var consumerInfos = await GetConsumerHealthInfos(userId);

                foreach (var information in informations)
                {
                    if (information.InformationType == HealthInformationEnum.Decimal.ToString()
                        || information.InformationType == HealthInformationEnum.String.ToString())
                    {
                        continue;
                    }

                    information.OptionList = options.Where(d => d.InformationGuid == information.InformationGuid).Select(s => new HealthInformationOptionResponse
                    {
                        OptionGuid = s.OptionGuid,
                        OptionLabel = s.OptionLabel,
                        IsDefault = s.IsDefault,
                        Sort = s.Sort
                    }).OrderBy(s => s.Sort).ToList();

                    var consumerInfo = consumerInfos.FirstOrDefault(d => d.InformationGuid == information.InformationGuid);

                    information.OptionValue = consumerInfo?.OptionGuids;
                }

                return informations;
            }
        }

        /// <summary>
        /// 查找用户信息集合
        /// </summary>
        /// <param name="informationGuid"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        async Task<List<ConsumerHealthInfoModel>> GetConsumerHealthInfos(string userId)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.GetListAsync<ConsumerHealthInfoModel>(@"
                    where user_guid = @userId and `enable`=1", new { userId })).ToList();
            }
        }

        /// <summary>
        /// 获取基础健康信息问题选项
        /// </summary>
        /// <param name="informationGuid"></param>
        /// <returns></returns>
        public async Task<List<HealthInformationOptionModel>> GetHealthInformationOptionAsync(string informationGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.GetListAsync<HealthInformationOptionModel>("where information_guid=@informationGuid and `enable`= 1", new { informationGuid })).ToList();
            }
        }

        /// <summary>
        /// 获取基础健康信息问题选项列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<HealthInformationOptionModel>> GetHealthInformationOptions()
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.GetListAsync<HealthInformationOptionModel>(@"order by sort")).ToList();
            }
        }

        /// <summary>
        /// 保存健康基础信息数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<bool> SaveHealthInformationAsync(SaveHealthInformationContext context)
        {
            return await MySqlHelper.TransactionAsync(async (conn, trans) =>
            {
                context.AddInfos.InsertBatch(conn);
                context.AddOptions.InsertBatch(conn);
                foreach (var item in context.UpdateInfos)
                {
                    await conn.UpdateAsync(item);
                }
                foreach (var item in context.UpdateOptions)
                {
                    await conn.UpdateAsync(item);
                }
                foreach (var item in context.DeleteInfos)
                {
                    await conn.DeleteAsync(item);
                }
                foreach (var item in context.DeleteOptions)
                {
                    await conn.DeleteAsync(item);
                }
                return true;
            });
        }

        /// <summary>
        /// 改变健康信息序号
        /// </summary>
        /// <param name="model"></param>
        /// <param name="newSort"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public async Task<bool> ChangeSortAsync(HealthInformationModel model, int newSort, string userID)
        {
            var sqlWhere = " information_guid<>@informationGuid ";
            var updateValue = "+1";
            var oldSort = model.Sort;
            if (oldSort > newSort)//往前移
            {
                sqlWhere = $"{sqlWhere} and sort >=@newSort and sort<=@oldSort";
                updateValue = "+1";
            }
            else//往后移
            {
                sqlWhere = $"{sqlWhere} and sort >=@oldSort and sort<=@newSort";
                updateValue = "-1";
            }
            var sql = $"update t_health_information set sort=sort{updateValue} where {sqlWhere}";
            return await MySqlHelper.TransactionAsync(async (conn, trans) =>
            {
                await conn.ExecuteAsync(sql, new
                {
                    informationGuid = model.InformationGuid,
                    oldSort,
                    newSort
                });
                model.Sort = newSort;//序号变为新序号
                model.LastUpdatedBy = userID;
                model.LastUpdatedDate = DateTime.Now;
                await conn.UpdateAsync(model);
                return true;
            });
        }
    }
}
