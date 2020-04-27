using Newtonsoft.Json;
using System.Collections.Generic;

namespace GD.Dtos.Utility
{
    /// <summary>
    /// 
    /// </summary>
    public class GetWeatherResponseDto
    {
        static readonly Dictionary<string, string> dict = new Dictionary<string, string>()
        {
            {"xue","雪" },
            {"lei","雷" },
            {"shachen","沙尘" },
            {"wu","雾" },
            {"bingbao","冰雹" },
            {"yun","云" },
            {"yu","雨" },
            {"yin","阴" },
            {"qing","晴" }
        };

        /// <summary>
        /// 城市ID
        /// </summary>
        [JsonProperty("city_id")]
        public string CityId { get; set; }
        /// <summary>
        /// 当前日期
        /// </summary>
        [JsonProperty("date")]
        public string Date { get; set; }
        /// <summary>
        /// 当前星期
        /// </summary>
        [JsonProperty("week")]
        public string Week { get; set; }
        /// <summary>
        /// 气象台更新时间
        /// </summary>
        [JsonProperty("update_time")]
        public string Update_time { get; set; }
        /// <summary>
        /// 城市名称
        /// </summary>
        [JsonProperty("city")]
        public string City { get; set; }
        /// <summary>
        /// 城市英文名称
        /// </summary>
        [JsonProperty("city_en")]
        public string CityEn { get; set; }
        /// <summary>
        /// 国家名称
        /// </summary>
        [JsonProperty("country")]
        public string Country { get; set; }
        /// <summary>
        /// 国家英文名称
        /// </summary>
        [JsonProperty("country_en")]
        public string CountryEn { get; set; }
        /// <summary>
        /// 天气情况
        /// </summary>
        [JsonProperty("wea")]
        public string Wea { get; set; }
        /// <summary>
        /// 天气对应图标(xue、lei、shachen、wu、bingbao、yun、yu、yin、qing)
        /// </summary>
        [JsonProperty("wea_img")]
        public string WeaImg { get; set; }
        /// <summary>
        /// 实时温度
        /// </summary>
        [JsonProperty("tem")]
        public string Tem { get; set; }
        /// <summary>
        /// 高温
        /// </summary>
        [JsonProperty("tem1")]
        public string Tem1 { get; set; }
        /// <summary>
        /// 低温
        /// </summary>
        [JsonProperty("tem2")]
        public string Tem2 { get; set; }
        /// <summary>
        /// 风向
        /// </summary>
        [JsonProperty("win")]
        public string Win { get; set; }
        /// <summary>
        /// 风力等级
        /// </summary>
        [JsonProperty("win_speed")]
        public string WinSpeed { get; set; }
        /// <summary>
        /// 风速
        /// </summary>
        [JsonProperty("win_meter")]
        public string WinMeter { get; set; }
        /// <summary>
        /// 湿度
        /// </summary>
        [JsonProperty("humidity")]
        public string Humidity { get; set; }
        /// <summary>
        /// 能见度
        /// </summary>
        [JsonProperty("visibility")]
        public string Visibility { get; set; }
        /// <summary>
        /// 气压hPa
        /// </summary>
        [JsonProperty("pressure")]
        public string Pressure { get; set; }
        /// <summary>
        /// 空气质量
        /// </summary>
        [JsonProperty("air")]
        public string Air { get; set; }
        [JsonProperty("air_pm25")]
        public string AirPm25 { get; set; }
        /// <summary>
        /// 空气质量等级
        /// </summary>
        [JsonProperty("air_level")]
        public string AirLevel { get; set; }
        [JsonProperty("air_tips")]
        public string Airtips { get; set; }
        /// <summary>
        /// 空气质量描述
        /// </summary>
        [JsonProperty("alarm")]
        public Alarm Alarm { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class Alarm
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("alarm_type")]
        public string AlarmType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("alarm_level")]
        public string AlarmLevel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("alarm_content")]
        public string AlarmContent { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetWeatherSimpleResponseDto
    {
        /// <summary>
        /// 城市名称
        /// </summary>
        public string CityName { get; set; }
        /// <summary>
        /// 实时温度
        /// </summary>
        public string Tem { get; set; }
        /// <summary>
        /// 最低温度
        /// </summary>
        public string Low { get; set; }
        /// <summary>
        /// 最高温度
        /// </summary>
        public string High { get; set; }
        /// <summary>
        /// 天气情况
        /// </summary>
        public string Wea { get; set; }
    }
}
