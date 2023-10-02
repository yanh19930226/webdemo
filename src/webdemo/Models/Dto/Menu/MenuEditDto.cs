namespace webdemo.Models.Dto.Menu
{
    public class MenuEditDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }
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
        public bool IsShow { get; set; }
    }
}
