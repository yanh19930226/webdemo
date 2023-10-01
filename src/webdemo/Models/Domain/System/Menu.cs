namespace webdemo.Models.Domain.System
{
    public class Menu : Entity
    {
        /// <summary>
        /// 1.菜单2.链接3.按钮
        /// </summary>
        public int MenuType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long ParentId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string MenuName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AuthorityCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Icon { get; set; }
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
