using System;
using GD.DataAccess;
using GD.Models.Utility;

namespace GD.Utility
{
    /// <summary>
    /// 标签Biz
    /// </summary>
    public class TaggedBiz
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Add(TaggedModel model)
        {
            return model.Insert();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int? Delete(TaggedModel model)
        {
            return model.Delete();
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int? Update(TaggedModel model)
        {
            model.LastUpdatedDate = DateTime.Now;
            return model.Update();
        }
    }
}
