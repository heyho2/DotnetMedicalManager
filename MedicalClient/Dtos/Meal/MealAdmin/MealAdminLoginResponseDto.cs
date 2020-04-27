namespace GD.Dtos.Meal.MealAdmin
{
    /// <summary>
    /// 登录响应
    /// </summary>
    public class MealAdminLoginResponseDto
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 医院名称
        /// </summary>
        public string HospitalName { get; set; }
        /// <summary>
        /// 医院Id
        /// </summary>
        public string HospitalId { get; set; }
        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 构造
        /// </summary>
        public MealAdminLoginResponseDto()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hospitalName"></param>
        /// <param name="hospitalId"></param>
        /// <param name="userName"></param>
        public MealAdminLoginResponseDto(string hospitalName, string hospitalId, string userName)
        {
            HospitalName = hospitalName;
            HospitalId = hospitalId;
            UserName = userName;

        }
    }
}
