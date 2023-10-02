namespace webdemo.Models.Vo.Category
{
    public class CategoryVo
    {
        public long Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Int32 ServiceId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String CategoryName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Int32 ParentId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Int32 Sort { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Int32 Status { get; set; }
        /// <summary>
        /// Level
        /// </summary>
        public int Level { get; set; }
    }
}
