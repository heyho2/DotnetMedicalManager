using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using GD.DataAccess;
using GD.Dtos.Utility.Message;
using GD.Models.Utility;
using Newtonsoft.Json;

namespace GD.Utility
{
    /// <summary>
    /// 聊天记录处理业务
    /// </summary>
    public class MessageBiz : BizBase.BaseBiz<MessageModel>
    {
        /// <summary>
        /// 聊天记录 Redis Key
        /// </summary>
        public static string MessageKey { get; }

        static MessageBiz()
        {
            MessageKey = RedisHelper.CreateKey(string.Empty, 1);
            MessageKey = MessageKey.Replace(".cctor", "static"); // 静态构造函数：.cctor
        }

        /// <summary>
        /// 按ID查询
        /// </summary>
        /// <param name="msgGuid"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public MessageModel GetModelById(string msgGuid, bool enable = true)
        {
            const string sql = "select * from t_utility_message  where msg_guid=@msgGuid and enable=@enable";

            return MySqlHelper.SelectFirst<MessageModel>(sql, new { msgGuid, enable });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="strWhere"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public List<MessageModel> GetPageList(string strWhere, int pageSize, int pageIndex, string orderBy)
        {
            return MySqlHelper.Select<MessageModel>(pageSize, pageIndex, strWhere, orderBy).ToList();
        }

        /// <summary>
        /// 新增到Redis
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Push2Redis(MessageModel model)
        {
            var oldLengh = RedisHelper.Database.ListLength(MessageKey);
            var newLenth = RedisHelper.Database.ListRightPush(MessageKey, JsonConvert.SerializeObject(model)); // 右进左出
            return (newLenth == oldLengh + 1);
        }

        /// <summary>
        /// 新增到Redis
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool Push2Redis(IEnumerable<MessageModel> list)
        {
            if (list == null || !list.Any())
            {
                return false;
            }

            var oldLengh = RedisHelper.Database.ListLength(MessageKey);

            foreach (var model in list)
            {
                RedisHelper.Database.ListRightPush(MessageKey, JsonConvert.SerializeObject(model)); // 右进左出
            }

            var newLenth = RedisHelper.Database.ListLength(MessageKey);

            return (newLenth == oldLengh + list.Count());
        }

        /// <summary>
        /// 将Redis聊天数据持久化到数据库
        /// </summary>
        /// <returns>正数：成功处理消息的条数；0：没有要处理的数据，负数：处理失败</returns>
        public int Save2DB()
        {
            var query = from redisValue in RedisHelper.Database.ListRange(MessageKey)
                        select JsonConvert.DeserializeObject<MessageModel>(redisValue.ToString());

            int count = query.Count();

            if (count > 0)
            {
                if (InsertBatch(query))
                {
                    RedisHelper.Database.ListTrim(MessageKey, count, -1);
                }
                else
                {
                    count = -1;
                }
            }

            return count;
        }

        /// <summary>
        /// 批量插入到数据库
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private bool InsertBatch(IEnumerable<MessageModel> list)
        {
            if (list == null || !list.Any())
            {
                return false;
            }

            return MySqlHelper.Transaction((conn, tran) => { return list.InsertBatch(conn) == list.Count(); });
        }

        /// <summary>
        /// 通过顶部消息Id获取历史消息记录
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<List<GetHistoryMessageListStartByTopMsgResponseDto>> GetHistoryMessageListStartByTopMsgAsync(GetHistoryMessageListStartByTopMsgRequestDto requestDto)
        {
            var sqlWhere = string.Empty;
            if (!string.IsNullOrWhiteSpace(requestDto.TopMsgId))
            {
                sqlWhere = "and a.creation_date<@TopMsgDate";
            }
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = $@"SELECT
                                a.msg_guid,
	                            a.from_guid,
	                            a.to_guid,
	                            a.context,
	                            a.creation_date,
                                a.is_html
                            FROM
	                            t_utility_message a
	                            INNER JOIN t_utility_topic topic ON a.topic_guid = topic.topic_guid 
                            WHERE
	                            topic.about_type = 'Doctor' 
	                            AND a.`enable` = 1 
	                            AND topic.`enable` = 1 
	                            {sqlWhere}
	                            AND (
	                            ( topic.sponsor_guid = @UserId1 AND topic.receiver_guid = @UserId2 ) 
	                            OR ( topic.sponsor_guid = @UserId2 AND topic.receiver_guid = @UserId1 ) 
	                            ) 
                            ORDER BY
	                            a.creation_date DESC
	                            limit @PageSize";
                var result = await conn.QueryAsync<GetHistoryMessageListStartByTopMsgResponseDto>(sql, requestDto);
                return result?.ToList();
            }
        }
    }
}