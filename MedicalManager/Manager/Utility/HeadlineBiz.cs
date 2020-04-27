using Dapper;
using GD.DataAccess;
using GD.Dtos.Headline;
using GD.Models.Manager;
using System.Threading.Tasks;

namespace GD.Manager.Utility
{
    public class HeadlineBiz : BaseBiz<HeadlineModel>
    {
        public async Task<GetHeadlinePageResponseDto> GetHeadlinePageAsync(GetHeadlinePageRequestDto request)
        {
            var whereSql = "1=1";
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                whereSql = $"{whereSql} AND headline_name like @Name";
            }
            var sql = $@"
SELECT * FROM (
	SELECT
	    a.*
    FROM
	    t_manager_headline a
) T 
WHERE {whereSql}
	ORDER BY creation_date
";
            request.Name = $"%{request.Name}%";
            return await MySqlHelper.QueryByPageAsync<GetHeadlinePageRequestDto, GetHeadlinePageResponseDto, GetHeadlinePageItemDto>(sql, request);
        }
    }
}
