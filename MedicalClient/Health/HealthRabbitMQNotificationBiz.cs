using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GD.Health
{
    public class HealthRabbitMQNotificationBiz
    {
        /// <summary>
        /// 问卷和日常指标有评价时RabbitMQ通知用户端
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="userId"></param>
        public void HealthRabbitMQNotification(HealthMessageDto dto, string userId)
        {
            Task.Run(() =>
            {
                try
                {
                    var bus = Communication.MQ.Client.CreateConnection();
                    var advancedBus = bus.Advanced;
                    if (advancedBus.IsConnected)
                    {

                        var queueName = $"health_{userId}";
                        var queue = advancedBus.QueueDeclare(queueName);

                        //获取员工的审批中待审批的数量 和 我的绩效中待处理的绩效
                        advancedBus.Publish(EasyNetQ.Topology.Exchange.GetDefault(), queue.Name, false, new EasyNetQ.Message<HealthMessageDto>(new HealthMessageDto
                        {
                            ResultGuid = dto.ResultGuid,
                            HealthType = dto.HealthType,
                            Title = dto.Title,
                            Content = dto.Content
                        }));
                    }
                    bus.Dispose();
                }
                catch (Exception ex)
                {

                }
            });


        }
    }

    public class HealthMessageDto
    {
        /// <summary>
        /// 问卷结果guid
        /// </summary>
        public string ResultGuid { get; set; }

        public HealthTypeEnum HealthType { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public enum HealthTypeEnum
        {
            /// <summary>
            /// 日常指标
            /// </summary>
            HealthIndicator,
            /// <summary>
            /// 健康问卷
            /// </summary>
            Questionnaire
        }


    }

}
