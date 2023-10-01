namespace webdemo.Models.Domain.System
{
    public class UserRole : Entity
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 角色id
        /// </summary>
        public long RoleId { get; set; }
    }
}
