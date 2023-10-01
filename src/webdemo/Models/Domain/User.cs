namespace webdemo.Models.Domain
{
    public class User : Entity
    {
        public string UserName { get; set; }

        public string Password { get; set; }
        public int Age { get; set; }
        public int Status { get; set; }
        public 服务来源 Service { get; set; }
    }

    public enum 服务来源
    {
        Douyin,
        Xiaohongshu
    }
}
