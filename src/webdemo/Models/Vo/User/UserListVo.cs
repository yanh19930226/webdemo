using webdemo.Models.Enums;

namespace webdemo.Models.Vo.User
{
    public class UserSearch:Search
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public 服务来源 Service { get; set; }

        public int Status { get; set; }
        public string Keyword { get; set; }
    }

    public class UserListVo
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Age { get; set; }
    }
}
