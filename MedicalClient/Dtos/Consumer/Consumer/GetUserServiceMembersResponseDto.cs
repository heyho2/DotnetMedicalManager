namespace GD.Dtos.Consumer.Consumer
{
    /// <summary>
    /// 获取账号下服务对象
    /// </summary>
    public class GetUserServiceMembersResponseDto
    {
        /// <summary>
        /// 服务对象Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 服务对象姓名
        /// </summary>
        public string Name { get; set; }
    }
}
