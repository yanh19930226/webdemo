namespace webdemo.Models.Domain
{
    /// <summary>
    /// Article
    /// </summary>
    public partial class Article : Entity
    {
        /// <summary>
        /// 标题
        /// </summary>
        public String Title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public String Content { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        public String Author { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        public String Summary { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public String Tags { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Int32 CategoryId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String CategoryName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Int32 Sort { get; set; }
        /// <summary>
        /// 状态 0未发布 1已发布
        /// </summary>
        public Int32 Status { get; set; }
    }
}
