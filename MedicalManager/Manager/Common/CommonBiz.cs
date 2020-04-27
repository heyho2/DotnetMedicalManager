﻿using GD.DataAccess;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using GD.Dtos;

namespace GD.Manager.Common
{
    public class CommonBiz
    {
        static CommonBiz()
        {
            {
                string key = RedisKeys.HotSearchHistoryDate;

                string[] hotWords = new string[] { "减肥", "感冒", "流鼻涕", "发烧", "咽喉痛", "腰间盘突出,", "颈椎病", "糖尿病", "银屑病" };

                //初始化一些热门词
                foreach (var item in hotWords)
                {
                    RedisHelper.Database.HashSet(key, item,
                        //初始化分数20起步
                        $"{20 + Array.IndexOf(hotWords, item)}|{DateTime.Now:yyyy-MM-dd}");
                }

            }
        }

        /// <summary>
        /// 用户搜索历史
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public bool SearchHistory(string userId, string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return false;
            }

            var key = string.Format(RedisKeys.UserSearchHistory, userId);
            if (RedisHelper.Database.SortedSetLength(key) > 10)
            {
                var value = RedisHelper.Database.SortedSetRangeByRank(key).FirstOrDefault();
                RedisHelper.Database.SortedSetRemove(key, value);
            }

            return RedisHelper.Database.SortedSetAdd(key, keyword, TimeStamp());
        }

        /// <summary>
        /// 获取用户搜索历史
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<string> GetUserSearchHistory(string userId)
        {
            var key = string.Format(RedisKeys.UserSearchHistory, userId);
            var values = RedisHelper.Database.SortedSetRangeByRank(key, 0, 9, Order.Descending);
            return values.Select(a => a.ToString());
        }

        /// <summary>
        /// 清除搜素历史
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task RemoveSearchHistoryAsync(string userId)
        {

            var key = string.Format(RedisKeys.UserSearchHistory, userId);
            await RedisHelper.Database.KeyDeleteAsync(key);
        }

        /// <summary>
        /// 获取用户搜索历史
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetHotSearchHistory()
        {
            var key = RedisKeys.HotSearchHistoryDate;
            var values = RedisHelper.Database.HashGetAll(key);
            var hotWords = values.Select(a =>
            {
                var aaa = a.Value.ToString().Split('|');
                return new HotWord
                {
                    Date = DateTime.Parse(aaa[1]),
                    Keyword = a.Name,
                    Sorted = int.Parse(aaa[0])
                };
            }).OrderByDescending(a => a.Sorted).Take(10);
            return hotWords.Select(a => a.Keyword);
        }

        /// <summary>
        /// 热词
        /// </summary>
        /// <returns></returns>
        public void HotWordSearch(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return;
            }

            if (keyword.Length < 2)
            {
                return;
            }

            var key = RedisKeys.HotSearchHistoryDate;
            var values = RedisHelper.Database.HashGetAll(key);
            var hotWords = values.Select(a =>
            {
                var aaa = a.Value.ToString().Split('|');
                return new HotWord
                {
                    Date = DateTime.Parse(aaa[1]),
                    Keyword = a.Name,
                    Sorted = int.Parse(aaa[0])
                };
            }).ToList();

            var pipei = hotWords.Where(a =>
            {
                var match = StrSimilarityUtils.GetSimilarityRatio(keyword, a.Keyword);
                return match >= 0.4; //匹配度大于40% +1
            }).OrderByDescending(a =>
            {
                var match = StrSimilarityUtils.GetSimilarityRatio(keyword, a.Keyword);
                return match; //匹配度大于40% +1
            }).FirstOrDefault();

            ///删除多余的热词
            if (pipei == null && hotWords.Count() > 100)
            {
                var delHotWords = hotWords.Where(a => (DateTime.Now.Day - a.Date.Day) > 30).Take(50).ToList();
                if (!delHotWords.Any())
                {
                    delHotWords.AddRange(hotWords.OrderBy(a => a.Date).ThenBy(a => a.Sorted).Take(10));
                }

                delHotWords.ForEach(a =>
                {
                    RedisHelper.Database.HashDelete(key, a.Keyword);
                    hotWords.Remove(a);
                });
                RedisHelper.Database.HashSet(key, keyword, $"{1}|{DateTime.Now:yyyy-MM-dd}");
            }
            else if (pipei != null)
            {
                RedisHelper.Database.HashSet(key, pipei.Keyword,
                    $"{pipei.Sorted + 1}|{DateTime.Now:yyyy-MM-dd}");
            }
            else
            {
                RedisHelper.Database.HashSet(key, keyword, $"{1}|{DateTime.Now:yyyy-MM-dd}");
            }
        }

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static double TimeStamp(DateTime? dateTime = null)
        {
            if (dateTime == null)
            {
                dateTime = DateTime.Now;
            }

            TimeSpan ts = dateTime.Value.ToUniversalTime() - new DateTime(1970, 1, 1);
            return ts.TotalMilliseconds; //精确到毫秒
        }
    }
    public class HotWord
    {
        public string Keyword { get; set; }
        public int Sorted { get; set; }
        public DateTime Date { get; set; }
    }

}