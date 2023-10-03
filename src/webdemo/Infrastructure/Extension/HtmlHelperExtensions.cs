using Microsoft.AspNetCore.Mvc.Rendering;
using webdemo.Models.Enums;

namespace webdemo.Infrastructure.Extension
{
    public class HtmlHelperExtensions
    {
        /// <summary>
        /// 获取服务来源
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> GetService()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Text = "全部",
                Value = "-1"
            });
            foreach (服务来源 item in Enum.GetValues(typeof(服务来源)))
            {
                list.Add(new SelectListItem()
                {
                    Value = Convert.ToInt32(item).ToString(),
                    Text = item.ToString()
                });
            }
            return list;
        }

        /// <summary>
        /// 获取状态
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> GetStatusList()
        {
            List<SelectListItem> list = new List<SelectListItem>()
            {
                 new SelectListItem()
                {
                    Value = "0",
                    Text = "启用"
                },
                new SelectListItem()
                {
                   Value = "1",
                   Text = "禁用"
                }
            };

            return list;
        }

        /// <summary>
        /// 获取菜单类型
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> GetMenuTypeList()
        {
            List<SelectListItem> list = new List<SelectListItem>()
            {
                 new SelectListItem()
                {
                    Value = "1",
                    Text = "菜单"
                },
                new SelectListItem()
                {
                   Value = "2",
                   Text = "链接"
                },
                new SelectListItem()
                {
                   Value = "3",
                   Text = "按钮"
                }
            };

            return list;
        }

        /// <summary>
        /// 获取机构类型
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> GetOrganizationTypeList()
        {
            List<SelectListItem> list = new List<SelectListItem>()
            {
                 new SelectListItem()
                {
                    Value = "1",
                    Text = "集团"
                },
                new SelectListItem()
                {
                   Value = "2",
                   Text = "公司"
                },
                new SelectListItem()
                {
                   Value = "3",
                   Text = "部门"
                }
            };

            return list;
        }
    }
}
