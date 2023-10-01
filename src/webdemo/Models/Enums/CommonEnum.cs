using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace webdemo.Models.Enums
{
    /// <summary>
    /// 状态
    /// </summary>
    public enum StatusEnum
    {
        /// <summary>
        /// 审核中
        /// </summary>
        [Description("审核中")]
        Verifing = 0,
        /// <summary>
        /// 审核成功
        /// </summary>
        [Description("审核成功")]
        Success = 1,
        /// <summary>
        /// 审核失败
        /// </summary>
        [Description("审核失败")]
        Failed = 2
    }

    /// <summary>
    /// 菜单类型
    /// </summary>
    public enum MenuTypeEnum
    {
        /// <summary>
        /// 菜单
        /// </summary>
        [Description("菜单")]
        Menu = 1,
        /// <summary>
        /// 链接
        /// </summary>
        [Description("链接")]
        Link = 2,
        /// <summary>
        /// 按钮
        /// </summary>
        [Description("按钮")]
        Button = 3
    }

    /// <summary>
    /// 服务来源
    /// </summary>
    public enum 服务来源
    {
        Douyin,
        Xiaohongshu
    }
}
