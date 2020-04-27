namespace GD.Dtos.Merchant.Category
{
    public class GetConsumerProjectsResponseDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string ProjectGuid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string GoodsItemGuid { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
    }


    public class ConsumerProjectItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string GoodsItemGuid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ProjectGuid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Remain { get; set; }
    }
}
