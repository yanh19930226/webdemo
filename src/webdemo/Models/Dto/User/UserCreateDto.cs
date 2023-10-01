using webdemo.Models.Enums;

namespace webdemo.Models.Dto.User
{
    public class UserCreateDto
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string UserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Password { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Age { get; set; }

        public int Status { get; set; }
        public 服务来源 Service { get; set; }
    }
}
