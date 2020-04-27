using GD.AppSettings;

namespace GD.Manager.WeChat
{

    public class PlatformSettings
    {
        /// <summary>
        /// host.json配置
        /// </summary>
        private static readonly Settings settings = Factory.GetSettings("host.json");

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
        public static readonly string UserAppointmentCompletedNotificationTemplate= settings["WeChat:Client:AppointmentCompletedNotificationTemplate"];

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
        /// 企业微信ID
        /// </summary>
        public static readonly string EnterpriseWeChatAppid = settings["EnterpriseWeChat:Appid"];
        /// <summary>
        /// 企业平台管理端应用ID
        /// </summary>
        public static readonly string EnterpriseWeChatPlatformAgentid = settings["EnterpriseWeChat:PlatformAgentid"];
        /// <summary>
        /// 企业平台管理端Secret
        /// </summary>
        public static readonly string EnterpriseWeChatPlatformSecret = settings["EnterpriseWeChat:PlatformSecret"];
    }
}
