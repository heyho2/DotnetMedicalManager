using GD.DataAccess;
using GD.Dtos.Advise;
using GD.Models.Consumer;
using System.Threading.Tasks;

namespace GD.Manager.Consumer
{
    /// <summary>
    /// 意见反馈模块业务类
    /// </summary>
    public class AdviseBiz : BaseBiz<AdviseModel>
    {
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
