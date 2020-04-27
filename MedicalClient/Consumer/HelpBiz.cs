using Dapper;
using GD.Common.EnumDefine;
using GD.DataAccess;
using GD.Dtos.Admin.Help;
using GD.Models.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.Utility
{
    /// <summary>
    /// 平台帮助业务类
    /// </summary>
    public class HelpBiz
    {
        /// <summary>
        /// 分页获取双美平台的平台帮助
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<List<HelpModel>> GetHelpListOfCosmetologyAsync(int pageIndex, int pageSize)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                string sql = $"select * from t_utility_help where `enable`=1 and (platform_type='LifeCosmetology' or platform_type='MedicalCosmetology') order by creation_date desc limit @pageIndex,@pageSize";
                var sumScore = await conn.QueryAsync<HelpModel>(sql, new { pageIndex = (pageIndex - 1) * pageSize, pageSize });
                return sumScore?.ToList();
            }
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(HelpModel model)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.InsertAsync<string, HelpModel>(model);
                return !string.IsNullOrWhiteSpace(result);
            }
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(HelpModel model)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.UpdateAsync(model);
                return result > 0;
            }
        }
        /// <summary>
        /// 根据id获取model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<HelpModel> GetAsync(string id)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.GetAsync<HelpModel>(id);
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(string id)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.DeleteAsync<HelpModel>(id);
                return result > 0;
            }
        }
        /// <summary>
        /// 常见问题列表
        /// </summary>
        /// <returns></returns>
        public async Task<GetHelpPageResponseDto> GetHelpPageAsync(GetHelpPageRequestDto request)
        {
            var sqlWhere = $@"1 = 1";
            if (!string.IsNullOrWhiteSpace(request.Question))
            {
                sqlWhere = $"{sqlWhere} and question like @Question";
            }
            if (!string.IsNullOrWhiteSpace(request.PlatformType))
            {
                if (request.PlatformType.ToLower() == PlatformType.CloudDoctor.ToString().ToLower())
                {
                    sqlWhere = $"{sqlWhere} and platform_type = @PlatformType";
                }
                else
                {
                    sqlWhere = $"{sqlWhere} and platform_type != '{PlatformType.CloudDoctor.ToString()}'";
                }
            }
            var sqlOrderBy = "sort desc";
            var sql = $@"
SELECT * FROM
    t_utility_help
 WHERE
	{sqlWhere}
ORDER BY
	{sqlOrderBy}";
            request.Question = $"%{request.Question}%";
            return await MySqlHelper.QueryByPageAsync<GetHelpPageRequestDto, GetHelpPageResponseDto, GetHelpPageItemDto>(sql, request);
        }
    }
}
