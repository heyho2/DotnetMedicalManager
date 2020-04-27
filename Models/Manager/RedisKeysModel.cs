using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GD.Common.Base;

namespace GD.Models.Manager
{
    /// <summary>
    /// 持久化RedisKey
    /// </summary>
    [Table("t_manager_redis_keys")]
    public class RedisKeysModel : BaseModel
    {
        /// <summary>
        /// GUID
        /// </summary>
        [Column("key_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "GUID")]
        public string KeyGuid { get; set; }

        /// <summary>
        /// 后台定义的属性字段名
        /// </summary>
        [Column("entity_property"), Required(ErrorMessage = "{0}必填"), Display(Name = "后台定义的属性字段名")]
        public string EntityProperty { get; set; }

        /// <summary>
        /// redis关键字
        /// </summary>
        [Column("redis_key"), Required(ErrorMessage = "{0}必填"), Display(Name = "redis关键字")]
        public string RedisKey { get; set; }
    }
}
