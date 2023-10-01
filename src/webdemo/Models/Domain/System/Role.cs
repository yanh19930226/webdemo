namespace webdemo.Models.Domain.System
{
    public class Role : Entity
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        public String RoleName { get; set; }
        /// <summary>
        /// 角色标识
        /// </summary>
        public String RoleCode { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String Remark { get; set; }
    }
}
