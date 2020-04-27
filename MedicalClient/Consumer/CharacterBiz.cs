using GD.DataAccess;
using GD.Models.Consumer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GD.Consumer
{
    /// <summary>
    /// 消费者用户画像（特征）业务类
    /// </summary>
    public class CharacterBiz
    {
        #region 查询
        /// <summary>
        /// 通过消费者特征Guid获取消费者特征对象唯一实例
        /// </summary>
        /// <param name="characterGuid">消费者特征Guid</param>
        /// <returns>消费者特征对象唯一实例</returns>
        public CharacterModel GetCharacterModelByChaGuid(string characterGuid,string userId, bool enable = true)
        {
            var sql = "select * from t_consumer_character where character_guid=@characterGuid and user_guid=@userId and enable=@enable";
            var characterModel = MySqlHelper.SelectFirst<CharacterModel>(sql, new { characterGuid, userId, enable });

            return characterModel;
        }

        /// <summary>
        /// 通过消费者ConfigGuid获取消费者特征对象唯一实例
        /// </summary>
        /// <param name="configGuid">消费者特征Guid</param>
        /// <param name="userId"></param>
        /// <param name="enable"></param>
        /// <returns>消费者特征对象唯一实例</returns>
        public CharacterModel GetCharacterModelByConfigGuid(string configGuid, string userId, bool enable = true)
        {
            var sql = "select * from t_consumer_character where conf_guid=@configGuid and user_guid=@userId and enable=@enable";
            var characterModel = MySqlHelper.SelectFirst<CharacterModel>(sql, new { configGuid, userId, enable });

            return characterModel;
        }
        /// <summary>
        /// 通过用户Guid获取特征集合
        /// </summary>
        /// <param name="userGuid">用户Guid</param>
        /// <returns>消费者特征集合</returns>
        public List<CharacterModel> GetCharacterModels(string userGuid, bool enable = true)
        {
            var sql = "select * from t_consumer_character where user_guid=@userGuid and enable=@enable";
            var characterModels = MySqlHelper.Select<CharacterModel>(sql, new { userGuid, enable });
            return characterModels?.ToList();

        }
        #endregion

        /// <summary>
        /// 修改消费者特征记录
        /// </summary>
        /// <param name="characterModel">消费正特征实例</param>
        /// <returns>是否成功</returns>
        public bool UpdateCharacterModel(CharacterModel characterModel)
        {
            characterModel.LastUpdatedDate = DateTime.Now;
            return characterModel.Update() == 1;
        }

        /// <summary>
        /// 批量插入特征
        /// </summary>
        /// <param name="characterModelList"></param>
        /// <returns></returns>
        public bool InsertCharacterModelList(List<CharacterModel> characterModelList)
        {
            if (!characterModelList.Any())
            {
                return false;
            }
            const string sql = @"INSERT INTO `t_consumer_character`
                                    VALUES
	                                    (
		                                    @CharacterGuid,
		                                    @UserGuid,
		                                    @ConfGuid,
		                                    @ConfValue,
		                                    @CreatedBy,
		                                    @CreationDate,
		                                    @LastUpdatedBy,
		                                    @LastUpdatedDate,
		                                    @OrgGuid,
	                                        @ENABLE 
	                                    ); ";
            var conn = Dapper.SqlMapper.Execute(MySqlHelper.GetConnection(), sql, characterModelList);
            return conn > 0;
            #region 另外一种方式
            //可用
            //var result = MySqlHelper.Transaction((conn, tran) =>
            //{
            //    //执行insert
            //    foreach (var model in characterModelList)
            //    {
            //        if (string.IsNullOrEmpty(model.Insert(conn)))
            //        {
            //            return false;
            //        }
            //    }
            //    return true;
            //});
            //return result;
            #endregion
        }

        /// <summary>
        /// 检查特征是否存在
        /// </summary>
        /// <param name="configGuid"></param>
        /// <param name="userId"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public bool IsExistConfig(string configGuid, string userId, bool enable = true)
        {
            //可用
            return MySqlHelper.Count<CharacterModel>("where conf_guid=@configGuid and user_guid=@userId and enable=@enable", new { configGuid, userId, enable }) > 0; ;
        }


    }
}
