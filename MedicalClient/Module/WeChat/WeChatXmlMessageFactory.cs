using GD.Dtos.WeChat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace GD.Module.WeChat
{
    public class WeChatXmlMessageFactory
    {
        private static List<BaseWeChatXmlMsg> _queue;
        public static BaseWeChatXmlMsg CreateMessage(string xml)
        {
            if (_queue == null)
            {
                _queue = new List<BaseWeChatXmlMsg>();
            }
            else if (_queue.Count >= 50)
            {
                _queue = _queue.Where(q => { return q.CreatedTime.AddSeconds(20) > DateTime.Now; }).ToList();//保留20秒内未响应的消息
            }
            XElement xdoc = XElement.Parse(xml);
            var msgtype = xdoc.Element("MsgType").Value.ToUpper();
            var FromUserName = xdoc.Element("FromUserName").Value;
            var CreateTime = xdoc.Element("CreateTime").Value;
            WeChartMsgType type = (WeChartMsgType)Enum.Parse(typeof(WeChartMsgType), msgtype);
            if (type != WeChartMsgType.EVENT)
            {
                var MsgId = xdoc.Element("MsgId").Value;
                if (_queue.FirstOrDefault(m => { return m.MsgFlag == MsgId; }) == null)
                {
                    _queue.Add(new BaseWeChatXmlMsg
                    {
                        CreatedTime = DateTime.Now,
                        FromUserName = FromUserName,
                        MsgFlag = MsgId
                    });
                }
                else
                {
                    return null;
                }
            }
            else
            {
                if (_queue.FirstOrDefault(m => { return m.MsgFlag == CreateTime && m.FromUserName == FromUserName; }) == null)
                {
                    _queue.Add(new BaseWeChatXmlMsg
                    {
                        CreatedTime = DateTime.Now,
                        FromUserName = FromUserName,
                        MsgFlag = CreateTime
                    });
                }
                else
                {
                    return null;
                }
            }
            switch (type)
            {
                case WeChartMsgType.TEXT:
                    return WeChatUtils.ConvertXmlToObj<WeChatXmlTextMsg>(xml);
                case WeChartMsgType.IMAGE:
                    return WeChatUtils.ConvertXmlToObj<BaseWeChatXmlMsg>(xml);
                case WeChartMsgType.VOICE:
                    return WeChatUtils.ConvertXmlToObj<BaseWeChatXmlMsg>(xml);
                case WeChartMsgType.VIDEO:
                    return WeChatUtils.ConvertXmlToObj<BaseWeChatXmlMsg>(xml);
                case WeChartMsgType.LOCATION:
                    return WeChatUtils.ConvertXmlToObj<BaseWeChatXmlMsg>(xml);
                case WeChartMsgType.LINK:
                    return WeChatUtils.ConvertXmlToObj<BaseWeChatXmlMsg>(xml);
                case WeChartMsgType.EVENT:
                    var enumEventType = (WeChartEvent)Enum.Parse(typeof(WeChartEvent), xdoc.Element("Event").Value.ToUpper());
                    switch (enumEventType)
                    {
                        case WeChartEvent.CLICK:
                            return WeChatUtils.ConvertXmlToObj<WeChatXmlNormalMenuEventMsg>(xml);
                        case WeChartEvent.VIEW: return WeChatUtils.ConvertXmlToObj<WeChatXmlNormalMenuEventMsg>(xml);
                        case WeChartEvent.LOCATION: return WeChatUtils.ConvertXmlToObj<BaseWeChatEventXmlMsg>(xml);
                        case WeChartEvent.LOCATION_SELECT: return WeChatUtils.ConvertXmlToObj<BaseWeChatEventXmlMsg>(xml);
                        case WeChartEvent.SCAN: return WeChatUtils.ConvertXmlToObj<WeChatScanEventMessage>(xml);
                        case WeChartEvent.SUBSCRIBE: return WeChatUtils.ConvertXmlToObj<WeChatSubEventXmlMsg>(xml);
                        case WeChartEvent.UNSUBSCRIBE: return WeChatUtils.ConvertXmlToObj<BaseWeChatEventXmlMsg>(xml);
                        case WeChartEvent.SCANCODE_WAITMSG: return WeChatUtils.ConvertXmlToObj<BaseWeChatEventXmlMsg>(xml);
                        default:
                            return WeChatUtils.ConvertXmlToObj<BaseWeChatEventXmlMsg>(xml);
                    }
                default:
                    return WeChatUtils.ConvertXmlToObj<BaseWeChatXmlMsg>(xml);
            }
        }
    }
}
