using GD.Common.Helper;
using GD.Crontab;
using GD.FAQs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace GD.Scheduler
{
    /// <summary>
    /// 问答分配悬赏金-定时器
    /// </summary>
    public class FAQsAllotTimer : BaseJob
    {
        /// <summary>
        /// 任务间隔分钟
        /// (1000 * 60) * 60 * 12
        /// </summary>
        private const int INTERVAL_MINUES = (1000 * 60) * 24 * 60;


        public FAQsAllotTimer()
        {
            Interval = INTERVAL_MINUES;
        }
        /// <summary>
        /// 自动的执行任务
        /// </summary>
        /// <remarks>
        /// Interval 单位为毫秒
        /// Begin 从 2019-1-1 0:0:0 开始计时执行
        /// </remarks>
        public override void Run()
        {
            try
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                //Logger.Info("开始问答分配悬赏金");
                var num = new FaqsAnswerBiz().FaqsAllotFeeAsync().Result;
                stopwatch.Stop();
                if (num >= 0)
                {
                    Logger.Info($"问答分配悬赏金完成，共处理条 {num} 数据。耗时：{stopwatch.ElapsedMilliseconds / 1000}秒");
                }
            }
            catch (Exception ex)
            {
                Logger.Info($"问答自动分配悬赏金报错，Error：{ex.Message} ");
            }
        }




    }
}
