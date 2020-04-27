using System;
using System.Linq;
using Dapper;
using GD.DataAccess;
using GD.Models.Utility;

namespace GD.Manager.Utility
{
    /// <summary>
    /// test
    /// </summary>
    public class TestBiz
    {
        /// <summary>
        /// 对数据库的测试表进行CRUD测试
        /// </summary>
        /// <returns></returns>
        public static bool Crud()
        {
            var item = new TestModel
            {
                TestGuid = Guid.NewGuid().ToString("N"),
                TestName = "Insert",
                Enable = true,
                CreatedBy = "zhy",
                CreationDate = DateTime.Now,
                LastUpdatedBy = "zhy",
                LastUpdatedDate = DateTime.Now,
                OrgGuid = "guodan"
            };

            if (string.IsNullOrEmpty(item.Insert())) // 增
            {
                return false;
            }

            var query = MySqlHelper.Select<TestModel>($"select * from {item.GetTableName()} where test_guid = @test_guid", new { test_guid = item.TestGuid }); // 查
            if (!query.Any())
            {
                return false;
            }

            item.TestName = "Update";
            item.LastUpdatedDate = DateTime.Now;

            return item.Update() == 1 && item.Delete() == 1; // 先改后删
        }
    }
}
