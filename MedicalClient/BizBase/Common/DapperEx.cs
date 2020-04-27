using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GD.BizBase.Common
{
    public class DapperEx
    {
        /// <summary>
        /// 获取model实体主键key名
        /// </summary>
        /// <typeparam name="T">Model 类型</typeparam>
        /// <returns></returns>
        public static string GetTablePrimaryKey<T>() where T : BaseModel
        {
            var type = typeof(T);
            var primaryProp = type.GetProperties().FirstOrDefault(a => a.IsDefined(typeof(System.ComponentModel.DataAnnotations.KeyAttribute), true));
            if (primaryProp == null)
            {
                throw new Exception("model实体中未找到主键");
            }
            var keyName = primaryProp.Name;
            if (primaryProp.IsDefined(typeof(System.ComponentModel.DataAnnotations.Schema.ColumnAttribute), true))
            {
                var columnAttr = (System.ComponentModel.DataAnnotations.Schema.ColumnAttribute)primaryProp.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.Schema.ColumnAttribute), true)[0];
                keyName = columnAttr.Name;
            }
            return keyName;
        }
    }
}
