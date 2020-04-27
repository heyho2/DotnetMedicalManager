using System.Diagnostics;
using GD.AppSettings;
using GD.Common.Helper;
using GD.Crontab;
using GD.Utility;

namespace GD.Scheduler
{
    public class MessageSaver : BaseJob
    {
        /// <summary>
        /// 任务间隔分钟
        /// </summary>
        private const int INTERVAL_MINUES = (1000 * 60) * 1;

        /// <summary>
        /// 是否启用XMPP
        /// </summary>
        private static readonly bool enableXmpp = false;

        public MessageSaver()
        {
            Interval = INTERVAL_MINUES;
        }

        static MessageSaver()
        {
            var settings = Factory.GetSettings("host.json");

            if (!bool.TryParse(settings["XMPP:enable"], out enableXmpp))
            {
                Logger.Warn("XMPP:enable 配置不正确");
                enableXmpp = false;
            }
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
            if (!enableXmpp)
            {
                return;
            }

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            Logger.Info("开始保存聊天消息数据");

            MessageBiz biz = new MessageBiz();
            int count = biz.Save2DB();

            stopwatch.Stop();

            if (count >= 0)
            {
                Logger.Info($"聊天消息数据保存完成，共处理条 {count} 数据。耗时：{ToTime(stopwatch.ElapsedMilliseconds)}");
            }
            else
            {
                Logger.Error("聊天消息数据保存失败");
            }
        }

        /// <summary>
        /// 毫秒数转时分秒毫秒字符串格式
        /// </summary>
        /// <param name="milliSeconds"></param>
        /// <returns></returns>
        private static string ToTime(long milliSeconds)
        {
            var ms = milliSeconds % 1000;

            var secondes = milliSeconds / 1000;
            var minutes = secondes / 60;
            secondes %= 60;

            var hours = minutes / 60;
            minutes %= 60;

            return $"{hours}时{minutes}分{secondes}秒{ms}毫秒";
        }
    }
}