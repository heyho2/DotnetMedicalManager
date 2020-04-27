using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Doctor.Hospital
{
    public class GetEnterpriseWeChatResponse
    {
        /// <summary>
        /// 企业Id
        /// </summary>
        public string Appid { get; set; }
        /// <summary>
        /// 应用Id
        /// </summary>
        public string Agentid { get; set; }
    }

    public class EnterpriseWeChatTokes
    {
        public int errcode { get; set; }
        public string errmsg { get; set; }
        public string access_token { get; set; }
        public int expires_in { get; set; }
    }

    public class EnterpriseWeChatUserInfo
    {
        public int errcode { get; set; }
        public string errmsg { get; set; }
        public string CorpId { get; set; }
        public string UserId { get; set; }
        public string DeviceId { get; set; }
        public string user_ticket { get; set; }
        public string expires_in { get; set; }
    }

    public class UserModel
    {
        public int errcode { get; set; }
        public string errmsg { get; set; }
        public string UserId { get; set; }
        public string DeviceId { get; set; }
    }

    public class UserDetail
    {
        public int errcode { get; set; }
        public string errmsg { get; set; }
        public string userid { get; set; }
        public string name { get; set; }
        public int[] department { get; set; }
        public int[] order { get; set; }
        public string position { get; set; }
        public string mobile { get; set; }
        public string gender { get; set; }
        public string email { get; set; }
        public int[] is_leader_in_dept { get; set; }
        public string avatar { get; set; }
        public string thumb_avatar { get; set; }
        public string telephone { get; set; }
        public int enable { get; set; }
        public string alias { get; set; }
        public string address { get; set; }
        public string open_userid { get; set; }
        public Extattr extattr { get; set; }
        public int status { get; set; }
        public string qr_code { get; set; }
        public string external_position { get; set; }
        public External_Profile external_profile { get; set; }
    }

    public class Extattr
    {
        public Attr[] attrs { get; set; }
    }

    public class Attr
    {
        public int type { get; set; }
        public string value { get; set; }
        public string name { get; set; }
        public Text text { get; set; }
        public Web web { get; set; }
    }

    public class Text
    {
        public string value { get; set; }
    }

    public class Web
    {
        public string url { get; set; }
        public string title { get; set; }
    }

    public class External_Profile
    {
        public string external_corp_name { get; set; }
        public External_Attr[] external_attr { get; set; }
    }

    public class External_Attr
    {
        public int type { get; set; }
        public string name { get; set; }
        public Text1 text { get; set; }
        public Web1 web { get; set; }
        public Miniprogram miniprogram { get; set; }
    }

    public class Text1
    {
        public string value { get; set; }
    }

    public class Web1
    {
        public string url { get; set; }
        public string title { get; set; }
    }

    public class Miniprogram
    {
        public string appid { get; set; }
        public string pagepath { get; set; }
        public string title { get; set; }
    }

    public class EnterpriseWeChat
    {
        public List<Mapping> Mapping { get; set; }
    }
    public class Mapping
    {
        public string HosId { get; set; }
        public int DepartmentId { get; set; }
    }
}
