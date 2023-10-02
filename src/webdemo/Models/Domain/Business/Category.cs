namespace webdemo.Models.Domain.Business
{
    public class Category : Entity
    {
        /// <summary>
        /// 
        /// </summary>
        public int ServiceId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ParentId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Status { get; set; }
    }
}
