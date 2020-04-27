using Newtonsoft.Json;

namespace GD.Dtos.Merchant.Therapist
{
    /// <summary>
    /// 
    /// </summary>
    public class GetTherapistClassifyProjectDto
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public string TherapistProjectGuid { get; set; }
        /// <summary>
        /// 项目
        /// </summary>
        public string ProjectGuid { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
    }
}
