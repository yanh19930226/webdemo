using Microsoft.AspNetCore.Mvc.Rendering;

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
    }
}
