using GD.AppSettings;
using GD.Common;
using GD.Common.Base;
using GD.Dtos.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GD.API.Controllers.Utility
{
    /// <summary>
    /// 天气预报接口控制器
    /// </summary>
    public class WeatherController : BaseController
    {
        /// <summary>
        /// 获取天气预报接口
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetWeatherSimpleResponseDto>))]
        public async Task<IActionResult> GetWeatherForecast([FromQuery] string ip)
        {
            var settings = Factory.GetSettings("host.json");
            var url = settings.Get("WeatherForecast:Url");
            var shenzhen = settings.Get("WeatherForecast:ShenZhen");

            var result = string.Empty;

            if (!string.IsNullOrEmpty(ip) && ValidateIPv4(ip))
            {
                result = await Invoke($"{url}&ip={ip}");
            }
            else
            {
                result = await Invoke($"{url}&cityid={shenzhen}");
            }

            result = UnicodeToString(result);

            var weather = JsonConvert.DeserializeObject<GetWeatherResponseDto>(result);

            var simpleWeather = new GetWeatherSimpleResponseDto()
            {
                CityName = weather.City,
                Tem = weather.Tem,
                Low = weather.Tem2,
                High = weather.Tem1,
                Wea = weather.Wea
            };

            return Success(simpleWeather);
        }

        bool ValidateIPv4(string ipString)
        {
            if (string.IsNullOrEmpty(ipString))
            {
                return false;
            }

            string[] splitValues = ipString.Split('.');
            if (splitValues.Length != 4)
            {
                return false;
            }

            return splitValues.All(r => byte.TryParse(r, out byte tempForParsing));
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        async Task<string> Invoke(string url)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
        }

        /// <summary>
        /// 将unicode转中文
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        string UnicodeToString(string source)
        {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(
                         source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        }
    }
}
