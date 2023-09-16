namespace webdemo.Models.Vo.Menu
{
    public class MenuVo
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        /// <summary>
        /// 1菜单2链接
        /// </summary>
        public int Type { get; set; }
        public string Name { get; set; }

        public string Url { get; set; }

        public string Icon { get; set; }
    }
}
