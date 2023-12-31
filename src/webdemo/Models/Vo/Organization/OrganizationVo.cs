﻿namespace webdemo.Models.Vo.Organization
{
    public class OrganizationVo
    {
        public long Id { get; set; }
        /// <summary>
        /// 机构类型
        /// </summary>
        public Int32 OrganizationType { get; set; }
        /// <summary>
        /// 父级
        /// </summary>
        public long ParentId { get; set; }
        /// <summary>
        /// 机构名称
        /// </summary>
        public String OrganizationName { get; set; }
        /// <summary>
        /// 负责人id
        /// </summary>
        public long? LeaderId { get; set; }
        /// <summary>
        /// 排序号
        /// </summary>
        public Int32 Sort { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String Remark { get; set; }
        /// <summary>
        /// Level
        /// </summary>
        public int Level { get; set; }
    }
}
