﻿namespace webdemo.Models.Vo.User
{
    public class UserSearch:Search
    {
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
    }
}