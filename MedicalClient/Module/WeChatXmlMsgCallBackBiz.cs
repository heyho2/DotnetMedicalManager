using Dapper;
using GD.Common.Helper;
using GD.DataAccess;
using GD.Dtos.WeChat;
using GD.Models.Consumer;
using GD.Models.Manager;
using GD.Models.Payment;
using GD.Module.WeChat;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Web;
using static GD.Models.Consumer.WechatSubscriptionModel;

namespace GD.Module
{
    /// <summary>
    /// 微信消息回调Biz
    /// </summary>
    public class WeChatXmlMsgCallBackBiz
    {
        public void MessageHandling(BaseWeChatXmlMsg msg)
        {
            //Text消息
            if (msg is WeChatXmlTextMsg)
            {
                TextMessageHandling((WeChatXmlTextMsg)msg);
            }
            //菜单事件消息
            else if (msg is WeChatXmlNormalMenuEventMsg)
            {
                MenuEventMessageHandling((WeChatXmlNormalMenuEventMsg)msg);
            }
            //订阅/取消订阅事件
            else if (msg is WeChatSubEventXmlMsg)
            {
                SubEventMessageHandling((WeChatSubEventXmlMsg)msg);
            }
            else if (msg is WeChatScanEventMessage)
            {
                ScanEventMessageHandling((WeChatScanEventMessage)msg);
            }
        }



        /// <summary>
        /// Text消息回调处理
        /// </summary>
        /// <param name="tmpMsg"></param>
        private void TextMessageHandling(WeChatXmlTextMsg tmpMsg)
        {
            //if (tmpMsg.Content.Equals("绑定美疗师"))
            //{
            //    var msgParam2 = new NewsCustomMsg
            //    {
            //        Touser = tmpMsg.FromUserName,
            //        MsgType = "news",
            //        News = new NewsCustomMsg.NewsMsg
            //        {
            //            Articles = new List<NewsCustomMsg.NewsArticle>()
            //                {
            //                    new NewsCustomMsg.NewsArticle{
            //                        Title = "美疗师绑定",
            //                        Description = $"请点击进行美疗师绑定",
            //                        Url = PlatformSettings.GetThrapistBindUrl(tmpMsg.FromUserName),
            //                        PicUrl = "http://f.gd-cosmetic.com/images/2019/4/23/logo.png"
            //                    }
            //                }
            //        }
            //    };
            //    var response2 = WeChartApi.SendCustomMsg(msgParam2, WeChartAccessToken.GetAccessToken(out string outMessage));
            //    var strmsg = outMessage;
            //}
        }

        /// <summary>
        /// 菜单事件消息回调处理
        /// </summary>
        /// <param name="tmpMsg"></param>
        private void MenuEventMessageHandling(WeChatXmlNormalMenuEventMsg tmpMsg)
        {
            //if (tmpMsg.EventKey == "key_get_link")
            //{
            //    var token = WeChartAccessToken.GetAccessToken(out string message);
            //    var msgParam = new NewsCustomMsg
            //    {
            //        Touser = tmpMsg.FromUserName,
            //        MsgType = "news",
            //        News = new NewsCustomMsg.NewsMsg
            //        {
            //            Articles = new List<NewsCustomMsg.NewsArticle>()
            //                {
            //                    new NewsCustomMsg.NewsArticle{
            //                        Title = "美疗师登录",
            //                        Description = $"获取时间{DateTime.Now.ToString("yyyy年MM月dd日HH:mm")},请点击进入美疗师执行端",
            //                        Url = PlatformSettings.GetThrapistExeUrl(),
            //                        PicUrl = "http://f.gd-cosmetic.com/images/2019/4/23/logo.png"
            //                    }
            //                }
            //        }
            //    };
            //    var response = WeChartApi.SendCustomMsg(msgParam, token);
            //}
        }

        /// <summary>
        /// 订阅/取消订阅事件消息回调处理
        /// </summary>
        /// <param name="tmpMsg"></param>
        private void SubEventMessageHandling(WeChatSubEventXmlMsg tmpMsg)
        {
            var accessToken = GetAccessToken(tmpMsg);

            if (string.IsNullOrEmpty(accessToken)) { return; }

            Logger.Debug($"订阅事件消息回调处理({tmpMsg.FromUserName})-enventKey:{tmpMsg.EventKey}");

            if (string.IsNullOrEmpty(tmpMsg.EventKey)) { return; }

            var scene = GetScene(tmpMsg.EventKey);

            if (scene is null || string.IsNullOrEmpty(scene.Extension)) { return; }

            dynamic extension = JObject.Parse(scene.Extension);

            if (tmpMsg.Event == WeChartEvent.SUBSCRIBE)
            {
                #region 关注公众号发送欢迎关注相关消息
                var welcomeTextModel = new DictionaryModel();
                using (var conn = MySqlHelper.GetConnection())
                {
                    welcomeTextModel = conn.GetAsync<DictionaryModel>(DictionaryType.SubscribeWelcome).Result;
                }

                TextCustomMsg msgParam = new TextCustomMsg
                {
                    Touser = tmpMsg.FromUserName,
                    MsgType = "text",
                    Text = new TextCustomMsg.TextContent
                    {
                        Content = string.IsNullOrWhiteSpace(welcomeTextModel?.ExtensionField) ? $@"您已成功关注公众号，欢迎您。" : welcomeTextModel.ExtensionField
                    }
                };

                var response = WeChartApi.SendCustomMsg(msgParam, accessToken).Result;
                #endregion

                #region 若是用户分享关注公众号时,记录是谁推荐关注的，并记录是从哪个端口推荐的

                if (scene.Action == WeChatSceneActionEnum.share.ToString())
                {
                    var entrance = (string)extension.entrance;
                    var recommendUserGuid = (string)extension.value;

                    Logger.Debug($"订阅事件消息回调处理-recommendUserGuid:{recommendUserGuid}  entrance:{entrance}");

                    if (string.IsNullOrEmpty(entrance) || string.IsNullOrEmpty(recommendUserGuid))
                    {
                        return;
                    }

                    var model = new WechatSubscriptionModel
                    {
                        SubscriptionGuid = Guid.NewGuid().ToString("N"),
                        RecommendUserGuid = recommendUserGuid,
                        Entrance = entrance,
                        OpenId = tmpMsg.FromUserName,
                        CreatedBy = "WeChatXmlMsgCallBack",
                        LastUpdatedBy = "WeChatXmlMsgCallBack",
                        OrgGuid = string.Empty
                    };

                    using (var conn = MySqlHelper.GetConnection())
                    {
                        var result = conn.Insert<string, WechatSubscriptionModel>(model);
                        Logger.Debug($"订阅事件消息回调处理-result:{result}");
                    }
                    #endregion

                    #region 扫码医生二维码推送医生个人中心链接
                    if (!entrance.Equals(EntranceEnum.Doctor.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        return;
                    }

                    var personalCenterTextModel = (DictionaryModel)null;

                    using (var conn = MySqlHelper.GetConnection())
                    {
                        personalCenterTextModel = conn.Get<DictionaryModel>(DictionaryType.DoctorPersonalCenterLink);
                    }

                    Logger.Info($"personalCenterTextModel:{JsonConvert.SerializeObject(personalCenterTextModel)}");

                    if (personalCenterTextModel != null && !string.IsNullOrEmpty(personalCenterTextModel.ExtensionField))
                    {
                        msgParam.Text = new TextCustomMsg.TextContent
                        {
                            Content = string.Format(
                                personalCenterTextModel.ExtensionField, recommendUserGuid)
                        };

                        var re = WeChartApi.SendCustomMsg(msgParam, accessToken).Result;
                    }
                    #endregion
                }
                else if (scene.Action == WeChatSceneActionEnum.pay.ToString())
                {
                    var name = (string)extension.name;
                    var url = $"{PlatformSettings.HospitalPayUrl}/PaymentAmount/{extension.value}/{name}";

                    //TODO 发送待缴费模板消息，暂用图文消息替代
                    NewCustomMsg newParam = new NewCustomMsg
                    {
                        Touser = tmpMsg.FromUserName,
                        MsgType = "news",
                        News = new NewCustomMsg.NewContent()
                        {
                            Articles = new List<NewCustomMsg.Article>()
                            {
                                new NewCustomMsg.Article()
                                {
                                    Url = url,
                                    Description = ">>点击此链接，进行在线缴费，祝您早日康复",
                                    Title = $"待缴费提醒({HttpUtility.UrlDecode(name)})",
                                    PicUrl = $"{PlatformSettings.Upload}/payment/pay.png"
                                }
                            }
                        }
                    };

                    var re = WeChartApi.SendCustomMsg(newParam, accessToken).Result;
                }
            }
        }

        /// <summary>
        /// 带参数二维码扫描事件消息回调处理
        /// </summary>
        /// <param name="msg"></param>
        private void ScanEventMessageHandling(WeChatScanEventMessage msg)
        {
            var accessToken = GetAccessToken(msg);

            if (string.IsNullOrEmpty(accessToken)) { return; }

            Logger.Debug($"订阅事件消息回调处理({msg.FromUserName})-enventKey:{msg.EventKey}");

            if (string.IsNullOrEmpty(msg.EventKey)) { return; }

            var scene = GetScene(msg.EventKey);

            if (scene is null || string.IsNullOrEmpty(scene.Extension)) { return; }

            dynamic extension = JObject.Parse(scene.Extension);

            if (msg.Event == WeChartEvent.SCAN)
            {
                if (scene.Action == WeChatSceneActionEnum.pay.ToString())
                {
                    var name = (string)extension.name;

                    var url = $"{PlatformSettings.HospitalPayUrl}/PaymentAmount/{extension.value}/{name}";

                    //TODO 发送待缴费模板消息，暂用图文消息替代
                    NewCustomMsg msgParam = new NewCustomMsg
                    {
                        Touser = msg.FromUserName,
                        MsgType = "news",
                        News = new NewCustomMsg.NewContent()
                        {
                            Articles = new List<NewCustomMsg.Article>()
                            {
                                new NewCustomMsg.Article()
                                {
                                    Url = url,
                                    Description = ">>点击此链接，进行在线缴费，祝您早日康复",
                                    Title = $"待缴费提醒({HttpUtility.UrlDecode(name)})",
                                    PicUrl = $"{PlatformSettings.Upload}/payment/pay.png"
                                }
                            }
                        }
                    };

                    var re = WeChartApi.SendCustomMsg(msgParam, accessToken).Result;
                }
            };
        }



        string GetAccessToken(BaseWeChatXmlMsg msg)
        {
            var resToken = WeChartApi.GetAccessToken(PlatformSettings.CDClientAppId, PlatformSettings.CDClientAppSecret).Result;

            Logger.Debug($"获取token-{JsonConvert.SerializeObject(resToken)}");

            if (string.IsNullOrWhiteSpace(resToken.AccessToken))
            {
                Logger.Error($"GD.API.Controllers.Consumer.{nameof(WeChatXmlMsgCallBackBiz)}.{nameof(SubEventMessageHandling)}  openId:[{msg.FromUserName}] {Environment.NewLine} error:获取token失败。{resToken.Errmsg}");
            }

            return resToken.AccessToken;
        }

        WechatSceneModel GetScene(string eventKey)
        {
            var sceneBiz = new WechatSceneBiz();

            var scene = sceneBiz.GetAsync(eventKey).Result;

            return scene;
        }
    }
}
