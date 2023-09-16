using PagedList.Core;
using PagedList.Core.Mvc;

namespace webdemo.Models.Vo
{
    public class PageResult<T>
    {
        public IPagedList<T> Data { get; set; }
        public Search FilterData { get; set; }
    }

    public class SitePagedListRenderOptions
    {
        public static PagedListRenderOptions Boostrap4
        {
            get
            {
                var option = PagedListRenderOptions.Bootstrap4Full;
                option.MaximumPageNumbersToDisplay = 5;
                return option;
            }
        }

        public static PagedListRenderOptions Boostrap4CustomizedText
        {
            get
            {
                var option = PagedListRenderOptions.Bootstrap4Full;
                option.MaximumPageNumbersToDisplay = 5;
                option.LinkToPreviousPageFormat = "上一页";
                option.LinkToNextPageFormat = "下一页";
                option.LinkToFirstPageFormat = "首页";
                option.LinkToLastPageFormat = "尾页";
                return option;
            }
        }
    }
}
