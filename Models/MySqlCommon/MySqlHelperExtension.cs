using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GD.Models.MySqlCommon
{
    /// <summary>
    /// 
    /// </summary>
    public static class MySqlHelperExtension
    {
        /// <summary>
        /// 生成model批量插入语句
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="models"></param>
        /// <returns></returns>
        public static List<string> CreateBatchInsertSqls<TModel>(List<TModel> models) where TModel : BaseModel 
        {
            var sqlFields = new StringBuilder();
            var sqlVaues = new StringBuilder();
            if (models == null || !models.Any())
            {
                return new List<string>();
            }
            List<string> sqls = new List<string>();
            var modelattr = typeof(TModel).GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.Schema.TableAttribute), true).FirstOrDefault();
            if (modelattr != null)
            {
                var tableAttribute = modelattr as System.ComponentModel.DataAnnotations.Schema.TableAttribute;
                var properties = typeof(TModel).GetProperties();
                foreach (var info in properties)
                {
                    var objAttrs = info.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.Schema.ColumnAttribute), true).FirstOrDefault();
                    if (objAttrs != null)
                    {
                        var fieldAttr = objAttrs as System.ComponentModel.DataAnnotations.Schema.ColumnAttribute;
                        sqlFields.Append($"`{fieldAttr.Name}`,");
                    }
                }

                var sqlTemplate = $"insert into `{tableAttribute.Name}` ({sqlFields.ToString().Trim(',')}) values ";
                int modelItemIndex = 0;
                int theMaxRecord = 10000;
                foreach (var item in models)
                {
                    modelItemIndex++;
                    sqlVaues.Append("(");
                    int i = 0;
                    foreach (var info in properties)
                    {
                        i++;
                        Type Ts = item.GetType();
                        object o = Ts.GetProperty(info.Name).GetValue(item, null);
                        if (o == null)
                        {
                            sqlVaues.Append("null");
                        }
                        else
                        {
                            if (info.PropertyType == typeof(string))
                            {
                                sqlVaues.Append($"'{Convert.ToString(o).Replace("'", "\\'")}'");
                            }
                            else if (info.PropertyType == typeof(DateTime) || info.PropertyType == typeof(DateTime?))
                            {
                                sqlVaues.Append($"'{Convert.ToDateTime(o).ToString("yyyy-MM-dd HH:mm:ss")}'");
                            }
                            else if (info.PropertyType == typeof(decimal) || info.PropertyType == typeof(decimal?))
                            {
                                sqlVaues.Append($"{Convert.ToDecimal(o)}");
                            }
                            else if (info.PropertyType == typeof(bool) || info.PropertyType == typeof(bool?))
                            {
                                sqlVaues.Append($"{Convert.ToBoolean(o)}");
                            }
                            else if (info.PropertyType == typeof(int) || info.PropertyType == typeof(int?))
                            {
                                sqlVaues.Append($"{Convert.ToInt32(o)}");
                            }
                            else if (info.PropertyType == typeof(long) || info.PropertyType == typeof(long?))
                            {
                                sqlVaues.Append($"{Convert.ToInt64(o)}");
                            }
                            else if (info.PropertyType == typeof(double) || info.PropertyType == typeof(double?))
                            {
                                sqlVaues.Append($"{Convert.ToDouble(o)}");
                            }
                        }
                        if (i < properties.Length)
                        {
                            sqlVaues.Append(",");
                        }
                    }
                    sqlVaues.Append("),");
                    if (modelItemIndex % theMaxRecord == 0)
                    {
                        sqls.Add($"{sqlTemplate}{sqlVaues.ToString().Trim(',')}");
                        sqlVaues.Clear();
                    }
                }
                if ((modelItemIndex % theMaxRecord) > 0)
                {
                    sqls.Add($"{sqlTemplate}{sqlVaues.ToString().Trim(',')}");
                }
            }
            return sqls;

        }

       
    }
}
