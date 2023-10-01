namespace webdemo.Models.Domain
{
    public class Category : Entity
    {
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
    }
}
