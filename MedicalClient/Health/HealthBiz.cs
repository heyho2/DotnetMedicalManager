using Dapper;
using GD.DataAccess;
using GD.Dtos.Health;
using GD.Models.Consumer;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GD.Manager.Health
{
    /// <summary>
    /// 
    /// </summary>
    public class HealthBiz
    {
        /// <summary>
        /// 获取会员问卷列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetConsumerQuestionnairesPageListResponseDto> GetConsumerQuestionnairesPageList(GetConsumerQuestionnairesPageListRequestDto requestDto)
        {
            var sqlWhere = string.Empty;

            if (!string.IsNullOrEmpty(requestDto.Name))
            {
                sqlWhere = "AND q.questionnaire_name LIKE @Name";
                requestDto.Name = $"%{requestDto.Name}%";
            }

            var sqlSort = string.Empty;

            var sortCriterias = requestDto.SortCriterias;

            if (sortCriterias != null && sortCriterias.Count > 0)
            {
                var removeDuplicates = new List<string>();

                foreach (var fieldCriteria in sortCriterias)
                {
                    if (!fieldCriteria.Exist())
                    {
                        continue;
                    }

                    if (removeDuplicates.Contains(fieldCriteria.Field))
                    {
                        continue;
                    }

                    removeDuplicates.Add(fieldCriteria.Field);

                    if (fieldCriteria.IsFill())
                    {
                        //已填写
                        if (fieldCriteria.SortOrder == SortOrder.DESC)
                        {
                            sqlSort = $@"{sqlSort} qr.fill_status desc,qr.last_updated_date desc,";
                        }
                        else
                        {
                            sqlSort = $"{sqlSort} qr.fill_status asc, q.issuing_date desc,";
                        }
                    }
                    else if (fieldCriteria.IsCommented())
                    {
                        //已评论
                        if (fieldCriteria.SortOrder == SortOrder.DESC)
                        {
                            sqlSort = $@"{sqlSort} qr.commented desc, qr.fill_status desc, qr.last_updated_date desc,";
                        }
                        else
                        {
                            sqlSort = $@"{sqlSort} qr.fill_status desc,qr.last_updated_date desc,";
                        }                       
                    }
                }

                if (!string.IsNullOrEmpty(sqlSort))
                {
                    sqlSort = $"ORDER BY {sqlSort.TrimEnd(',')}";
                }
            }

            var sql = $@"SELECT 
                    qr.result_guid as ResultGuid,
                    q.questionnaire_guid as QuestionnaireGuid,
	                q.questionnaire_name as `Name`,
	                q.issuing_date as IssuingDate,
	                qr.fill_status as FillStatus,
	                qr.commented as CommentedStatus
                FROM  t_questionnaire_result as qr
	                INNER JOIN `t_questionnaire` as q ON qr.questionnaire_guid = q.questionnaire_guid
                WHERE qr.user_guid = @UserGuid {sqlWhere} ";

            if (!string.IsNullOrEmpty(sqlSort))
            {
                sql = $"{sql} {sqlSort}";
            }
            else
            {
                sql = $@"{sql} ORDER BY CASE 
                    WHEN  qr.fill_status = 0 THEN q.issuing_date
                    ELSE  qr.last_updated_date 
                END DESC";
            }

            return await MySqlHelper.QueryByPageAsync<GetConsumerQuestionnairesPageListRequestDto,
                 GetConsumerQuestionnairesPageListResponseDto,
                 GetQuestionnaireItem>(sql, requestDto);
        }

        /// <summary>
        /// 获取会员检验报告列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetConsumerHealthReportPageListResponseDto> GetConsumerHealthReportPageList(GetConsumerHealthReportPageListRequestDto requestDto)
        {
            var sqlWhere = string.Empty;

            if (!string.IsNullOrEmpty(requestDto.Name))
            {
                sqlWhere = "AND r.report_name LIKE @Name";
                requestDto.Name = $"%{requestDto.Name}%";
            }

            var sql = $@"SELECT
                    r.report_guid as ReportGuid,
	                r.report_name as `Name`,
	                r.creation_date as UploadedDate,
	                r.last_updated_date as LastUpdatedDate
                FROM  t_consumer_health_report as r
                WHERE r.user_guid = @UserGuid AND r.enable = 1 {sqlWhere}
                ORDER BY r.last_updated_date DESC";

            return await MySqlHelper.QueryByPageAsync<GetConsumerHealthReportPageListRequestDto,
                 GetConsumerHealthReportPageListResponseDto,
                 GetConsumerHealthReportItem>(sql, requestDto);
        }

        /// <summary>
        /// 获取会员指定报告详情
        /// </summary>
        /// <param name="reportGuid"></param>
        /// <returns></returns>
        public async Task<GetConsumerHealthReportResponseDto> GetConsumerHealthReportDetail(string reportGuid)
        {
            var response = (GetConsumerHealthReportResponseDto)null;

            var sql = @"SELECT
	                u.nick_name as NickName,	
	                u.phone as Phone,
	                m.report_name as ReportName,
                    m.suggestion as Suggestion
                FROM
	                t_consumer_health_report AS m
	                INNER JOIN t_utility_user as u ON u.user_guid = m.user_guid
                WHERE  m.report_guid = @reportGuid";

            using (var conn = MySqlHelper.GetConnection())
            {
                response = await conn.QueryFirstOrDefaultAsync
                    <GetConsumerHealthReportResponseDto>(sql, new
                    {
                        reportGuid
                    });

                if (response is null)
                {
                    return response;
                }

                sql = @"SELECT 
                    d.accessory_guid as ReportDetailGuid,
	                CONCAT(a.base_path,a.relative_path) as Url,
	                d.accessory_name as `Name`
                FROM t_consumer_health_report_detail as d
	                INNER JOIN t_utility_accessory as a ON a.accessory_guid = d.accessory_guid
                WHERE d.report_guid = @reportGuid";

                response.Attachments = (await conn.QueryAsync
                    <ReportAttachment>(sql, new
                    {
                        reportGuid
                    })).ToList();
            }

            return response;
        }

        /// <summary>
        /// 获取指定检验报告
        /// </summary>
        /// <param name="reportGuid"></param>
        /// <returns></returns>
        public async Task<ConsumerHealthReportModel> GetConsumerHealthReport(string reportGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.GetAsync<ConsumerHealthReportModel>(reportGuid);
            }
        }

        /// <summary>
        /// 删除检验报告
        /// </summary>
        /// <param name="reportGuid"></param>
        /// <returns></returns>
        public async Task<bool> DeleteConsumerHealthReport(string reportGuid)
        {
            var reportGuids = new string[] { reportGuid };

            return await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {
                await conn.DeleteAsync<ConsumerHealthReportModel>(reportGuid);

                await conn.DeleteListAsync<ConsumerHealthReportDetailModel>("where report_guid in @reportGuids", new
                {
                    reportGuids
                });

                return true;
            });
        }

        /// <summary>
        /// 上传检验报告
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> CreateHealthReport(ConsumerHealthReportModel model,
            List<ConsumerHealthReportDetailModel> detailModels)
        {
            return await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {
                await conn.InsertAsync<string, ConsumerHealthReportModel>(model);

                detailModels.InsertBatch();

                return true;
            });
        }

        /// <summary>
        /// 编辑检验报告
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateHealthReport(ConsumerHealthReportModel model,
            List<ConsumerHealthReportDetailModel> detailModels)
        {
            var reportGuids = new string[] { model.ReportGuid };

            return await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {
                await conn.UpdateAsync(model);

                await conn.DeleteListAsync<ConsumerHealthReportDetailModel>("where report_guid in @reportGuids", new
                {
                    reportGuids
                });

                detailModels.InsertBatch(conn);

                return true;
            });
        }

        /// <summary>
        /// 校验规则名称是否已存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<bool> CheckReportName(string name, string reportGuid = null)
        {
            var sql = "SELECT 1 FROM t_consumer_health_report WHERE report_name = @name";

            if (!string.IsNullOrEmpty(reportGuid))
            {
                sql = $" {sql} and report_guid <> @reportGuid";
            }

            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.ExecuteScalarAsync(sql, new
                {
                    name,
                    reportGuid
                })) != null;
            }
        }
    }
}