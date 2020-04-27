using Newtonsoft.Json;

namespace GD.Dtos.Doctor.Hospital
{
    public class GetHospitalEvaluvationResponseDto
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public string UserGuid { get; set; }
        /// <summary>
        /// 科室GUID
        /// </summary>
        public string OfficeGuid { get; set; }
        /// <summary>
        /// 科室名称
        /// </summary>
        public string OfficeName { get; set; }
        /// <summary>
        /// 评价标签
        /// </summary>
        [JsonIgnore]
        public string Tags { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string[] EvaluationTag
        {
            get
            {
                return JsonConvert.DeserializeObject<string[]>(Tags);
            }
        }
        /// <summary>
        /// 评分
        /// </summary>
        public decimal Score { get; set; }
        /// <summary>
        /// 病情详情
        /// </summary>
        public string ConditionDetail { get; set; }
        /// <summary>
        /// 是否匿名
        /// </summary>
        public bool Anonymous { get; set; }
    }
}
