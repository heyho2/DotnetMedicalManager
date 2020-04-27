using System;
using System.Collections.Generic;
using Dapper;
using GD.DataAccess;
using GD.Models.Manager;

namespace GD.Component
{
    /// <summary>
    /// 对Key的管理
    /// </summary>
    public static class RedisKeys
    {
        /// <summary>
        /// 本地关系缓存
        /// </summary>
        /// <remarks>
        /// 是否也可以保存到Redis中？
        /// </remarks>
        private static Dictionary<string, string> keyRelation = new Dictionary<string, string>();

        /// <summary>
        /// 测试Key
        /// </summary>
        public static string TestKey
        {
            get { return GetKey(nameof(TestKey)); }
            set { SetKey(nameof(TestKey), value); }
        }

        #region 私有方法，只管调用，不要随意修改

        /// <summary>
        /// 持久化到mysql
        /// </summary>
        /// <param name="mysqlKey"></param>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        private static bool SetKey(string entityProperty, string redisKey)
        {
            var result = true;

            if (keyRelation.ContainsKey(entityProperty) && string.Equals(keyRelation[entityProperty], redisKey))
            {
                return result;
            }

            var model = MySqlHelper.SelectFirst<RedisKeysModel>("select * from t_manager_redis_keys where entity_property=@entity_property", new { entityProperty });
            if (model == null)
            {
                model = new RedisKeysModel
                {
                    KeyGuid = Guid.NewGuid().ToString("N"),
                    EntityProperty = entityProperty,
                    RedisKey = redisKey,
                    CreatedBy = "admin",
                    LastUpdatedBy = "admin"
                };

                result = string.Equals(model.Insert(), model.KeyGuid); // 插入
            }
            else
            {
                model.RedisKey = redisKey;
                result = (model.Update() == 1); // 更新
            }

            // 如果持久化成功，则更新本地关系
            if (result)
            {
                if (keyRelation.ContainsKey(entityProperty))
                {
                    keyRelation[entityProperty] = redisKey;
                }
                else
                {
                    keyRelation.Add(entityProperty, redisKey);
                }
            }

            return result;
        }

        /// <summary>
        /// 初始化从mysql加载数据
        /// </summary>
        /// <param name="entityProperty"></param>
        /// <returns></returns>
        private static string GetKey(string entityProperty)
        {
            if (keyRelation.ContainsKey(entityProperty))
            {
                return keyRelation[entityProperty];
            }

            var model = MySqlHelper.SelectFirst<RedisKeysModel>("select * from t_manager_redis_keys where entity_property=@entity_property", new { entityProperty });
            if (model == null)
            {
                return string.Empty;
            }

            keyRelation.Add(entityProperty, model.RedisKey);

            return model.RedisKey;
        }

        #endregion
    }
}