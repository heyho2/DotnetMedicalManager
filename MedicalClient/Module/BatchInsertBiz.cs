using GD.Common.Base;
using GD.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using MySql.Data.MySqlClient;
using dotNet = System.ComponentModel.DataAnnotations.Schema;

namespace GD.Module
{
    
    public class BatchInsertBiz
    {
        /// <summary>
        /// 批量插入
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="list"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public static int InsertBatch<TModel>(IEnumerable<TModel> list, MySqlConnection conn = null) where TModel : BaseModel
        {
            if (!list.Any())
            {
                return 0;
            }

            var item = list.First();
            var type = typeof(TModel);
            var properties = type.GetProperties();

            var columnList = new List<string>();
            var matrix = new object[list.Count(), properties.Length];
            var paras = new DynamicParameters();
            for (int i = 0; i < properties.Length; i++)
            {
                var prop = properties[i];

                var col = prop.GetCustomAttributes(typeof(dotNet.ColumnAttribute), false).FirstOrDefault() as dotNet.ColumnAttribute;
                if (col == null)
                {
                    continue;
                }

                columnList.Add(col.Name);

                for (int j = 0; j < list.Count(); j++)
                {
                    var value = prop.GetValue(list.ElementAt(j));
                    var paraName = $"@{prop.Name}{j}";
                    matrix[j, i] = paraName;
                    paras.Add(paraName, value);
                }
            }

            var valueList = new List<string>();

            for (int i = 0; i < list.Count(); i++)
            {
                var record = new List<object>();
                for (int j = 0; j < properties.Length; j++)
                {
                    record.Add(matrix[i, j]); // 数据类型没处理
                }

                valueList.Add("(" + string.Join(", ", record) + ")");
            }

            var sql = $"insert {item.GetTableName()} ({string.Join(", ", columnList)}) values {string.Join(", ", valueList)}";

            var con = conn ?? DataAccess.MySqlHelper.GetConnection();

            var result = con.Execute(sql, paras);

            if (conn == null)
            {
                con.Close();
                con.Dispose();
            }

            return result;
        }

    }
}
