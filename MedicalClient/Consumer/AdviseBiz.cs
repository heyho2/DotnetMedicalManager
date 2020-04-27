using Dapper;
using GD.Common.EnumDefine;
using GD.DataAccess;
using GD.Dtos.Admin.Advise;
using GD.Models.Consumer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GD.Consumer
{
    /// <summary>
    /// 意见反馈模块业务类
    /// </summary>
    public class AdviseBiz
    {
        #region 查询
        /// <summary>
        /// 获取单个意见反馈记录
        /// </summary>
        /// <param name="adviseGuid">意见反馈Guid</param>
        /// <returns>单个意见反馈实例</returns>
        public AddressModel GetAdvise(string adviseGuid, bool enable = true)
        {
            var sql = "select * from t_consumer_advise where advise_guid=@adviseGuid and enable=@enable";
            var model = MySqlHelper.SelectFirst<AddressModel>(sql, new { adviseGuid, enable });

            return model;
        }


        #endregion

        #region 修改
        /// <summary>
        /// 新增意见反馈记录
        /// </summary>
        /// <param name="adviseModel">意见反馈实例</param>
        /// <returns>是否成功</returns>
        public bool InsertAdvise(AdviseModel adviseModel)
        {
            return !string.IsNullOrEmpty(adviseModel.Insert());
        }
        #endregion


        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(AdviseModel model)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.InsertAsync<string, AdviseModel>(model);
                return !string.IsNullOrWhiteSpace(result);
            }
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(AdviseModel model)
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
        public async Task<AdviseModel> GetAsync(string id)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.GetAsync<AdviseModel>(id);
            }
        }
        public async Task<GetAdvisePageResponseDto> GetAdvisePageAsync(GetAdvisePageRequestDto request)
        {
            var sqlWhere = $@"1 = 1";
            if (request.BeginDate != null)
            {
                request.BeginDate = request.BeginDate?.Date;
                sqlWhere = $"{sqlWhere} AND creation_date > @BeginDate";
            }
            if (request.EndDate != null)
            {
                request.EndDate = request.EndDate?.AddDays(1).Date;
                sqlWhere = $"{sqlWhere} AND creation_date < @EndDate";
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
            var sqlOrderBy = "creation_date desc";
            var sql = $@"
SELECT * FROM
    t_consumer_advise
 WHERE
	{sqlWhere}
ORDER BY
	{sqlOrderBy}";
            return await MySqlHelper.QueryByPageAsync<GetAdvisePageRequestDto, GetAdvisePageResponseDto, GetAdvisePageItemDto>(sql, request);
        }
    }
}
