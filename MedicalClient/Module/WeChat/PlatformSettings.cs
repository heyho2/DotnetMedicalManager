using GD.AppSettings;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Module.WeChat
{

    public class PlatformSettings
    {
        /// <summary>
        /// host.json配置
        /// </summary>
        private static Settings settings = Factory.GetSettings("host.json");

        /// <summary>
        /// 点餐客户端小程序AppId
        /// </summary>
        public static readonly string CDMealClientAppId = settings["WeChat:MealClient:AppId"];

        /// <summary>
        /// 点餐客户端小程序秘钥
        /// </summary>
        public static readonly string CDMealClientAppSecret = settings["WeChat:MealClient:AppSecret"];

        /// <summary>
        /// 云医客户端公众号AppId
        /// </summary>
        public static readonly string CDClientAppId = settings["WeChat:Client:AppId"];

        /// <summary>
        /// 云医客户端公众号秘钥
        /// </summary>
        public static readonly string CDClientAppSecret = settings["WeChat:Client:AppSecret"];


        /// <summary>
        /// 微信公众号数字id base64加密后的结果，用于跳转到关注公众号页面的参数
        /// </summary>
        public static readonly string CDClientAppBizId = settings["WeChat:Client:BizId"];

        /// <summary>
        /// 回调token
        /// </summary>
        public static readonly string CDClientAppToken = settings["WeChat:Client:Token"];

        /// <summary>
        /// 云医客户端客服聊天窗口链接
        /// </summary>
        public static readonly string CDClientCustomerServiceLink = settings["WeChat:Client:CustomerServiceLink"];

        /// <summary>
        /// 医生移动端公众号AppId
        /// </summary>
        public static readonly string DoctorClientAppId = settings["WeChat:DoctorClient:AppId"];

        /// <summary>
        /// 医生移动端公众号秘钥
        /// </summary>
        public static readonly string DoctorClientAppSecret = settings["WeChat:DoctorClient:AppSecret"];


        /// <summary>
        /// 医生离线消息通知模板
        /// </summary>
        public static readonly string DoctorOfflineMsgTemplate = settings["WeChat:DoctorClient:DoctorOfflineMsgTemplate"];

        /// <summary>
        /// 用户服务项目预约通知微信模板
        /// </summary>
        public static readonly string UserAppointmentNotificationTemplate = settings["WeChat:Client:AppointmentNotificationTemplate"];

        /// <summary>
        /// 用户服务项目完成通知微信模板
        /// </summary>
        public static readonly string UserAppointmentCompletedNotificationTemplate = settings["WeChat:Client:AppointmentCompletedNotificationTemplate"];

        /// <summary>
        /// 服务人员服务项目预约通知微信模板
        /// </summary>
        public static readonly string OperatorAppointmentNotificationTemplate = settings["WeChat:DoctorClient:AppointmentNotificationTemplate"];

        /// <summary>
        /// 平台客服Id
        /// </summary>
        public static readonly string PlatformCustomerService = settings["CustomerService:PlatformCustomerService"];

        /// <summary>
        /// 问医在线客服Id
        /// </summary>
        public static readonly string AskedDoctorOnlineCustomerService = settings["CustomerService:AskedDoctorOnlineCustomerService"];

        /// <summary>
        /// api服务主机域名
        /// </summary>
        public static readonly string APIDomain = settings["APIDomain"];

        /// <summary>
        /// 医院待缴费URL
        /// </summary>
        public static readonly string HospitalPayUrl = settings["HospitalPay:Pay"];

        /// <summary>
        /// 医院付款后调查页面url
        /// </summary>
        public static readonly string HospitalEvaluationUrl = settings["HospitalPay:HospitalEvaluationUrl"];

        /// <summary>
        /// 支付成功通知模板消息Id
        /// </summary>
        public static readonly string HospitalSuccessPayTemplateMsgId = settings["HospitalPay:SuccessPayTemplateMsgId"];

        /// <summary>
        /// 文件资源上传地址
        /// </summary>
        public static readonly string Upload = settings["Upload"];
        /// <summary>
        /// 企业微信ID
        /// </summary>
        public static readonly string EnterpriseWeChatAppid = settings["EnterpriseWeChat:Appid"];

        /// <summary>
        /// 企业医院端应用ID
        /// </summary>
        public static readonly string EnterpriseWeChatAgentid = settings["EnterpriseWeChat:Agentid"];
        /// <summary>
        /// 企业医院端Secret
        /// </summary>
        public static readonly string EnterpriseWeChatSecret = settings["EnterpriseWeChat:Secret"];
        /// <summary>
        /// 映射列表
        /// </summary>
        public static readonly IConfigurationSection Mappings = settings.GetSection("EnterpriseWeChat");
        /// <summary>
        /// 企业医生移动端应用ID
        /// </summary>
        public static readonly string EnterpriseWeChatMobileAgentid = settings["EnterpriseWeChat:DoctorMobileAgentid"];
        /// <summary>
        /// 企业医生移动端Secret
        /// </summary>
        public static readonly string EnterpriseWeChatMobileSecret = settings["EnterpriseWeChat:DoctorMobileSecret"];
        /// <summary>
        /// 企业医生PC端应用ID
        /// </summary>
        public static readonly string EnterpriseWeChatPCAgentid = settings["EnterpriseWeChat:DoctorPCAgentid"];
        /// <summary>
        /// 企业医生PC端Secret
        /// </summary>
        public static readonly string EnterpriseWeChatPCSecret = settings["EnterpriseWeChat:DoctorPCSecret"];

        /// <summary>
        /// 健康管理师移动端应用Id
        /// </summary>
        public static readonly string HealthManagerMobileAgentid = settings["EnterpriseWeChat:HealthManagerMobileAgentid"];

        /// <summary>
        /// 健康管理师移动端应用秘钥
        /// </summary>
        public static readonly string HealthManagerMobileSecret = settings["EnterpriseWeChat:HealthManagerMobileSecret"];
        /// <summary>
        /// 预警跳转Url地址
        /// </summary>
        public static readonly string WarningUrl = settings["EnterpriseWeChat:WarningUrl"];
    }
}
