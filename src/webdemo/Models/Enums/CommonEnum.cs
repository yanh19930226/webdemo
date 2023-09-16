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
}
