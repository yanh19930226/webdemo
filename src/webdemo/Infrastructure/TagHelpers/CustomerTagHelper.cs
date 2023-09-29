using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace webdemo.Infrastructure.TagHelpers
{
    [HtmlTargetElement("datetimerange")]
    public class DateTimeRangeTagHelper : TagHelper
    {
        public string Label { get; set; }

        public string StartName { get; set; }

        public string EndName { get; set; }

        public ModelExpression AspStartFor { get; set; }

        public ModelExpression AspEndFor { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var startName = string.IsNullOrEmpty(StartName) ? AspStartFor.Name : StartName;
            var endName = string.IsNullOrEmpty(EndName) ? AspEndFor.Name : EndName;
            var startDate = AspStartFor.Model == null ? "" : Convert.ToDateTime(AspStartFor.Model).ToString("yyyy-MM-dd");
            var endDate = AspEndFor.Model == null ? "" : Convert.ToDateTime(AspEndFor.Model).ToString("yyyy-MM-dd");
            var labelHtml = GetLabelAsHtml(context, output);
            var sbHtml = new StringBuilder();
            sbHtml.AppendLine(labelHtml);
            sbHtml.AppendLine("<div class=\"input-daterange form-control\">");
            sbHtml.AppendLine("<div class=\"input-daterange-item\">");
            sbHtml.AppendLine($"<input type=\"hidden\" name=\"{startName}\" />");
            sbHtml.AppendLine($"<input type=\"text\" value=\"{startDate}\" style=\"width:6rem;border:none\" />");
            sbHtml.AppendLine("</div>");
            sbHtml.AppendLine("<span class=\"input-group-addon\">-</span>");
            sbHtml.AppendLine("<div class=\"input-daterange-item\">");
            sbHtml.AppendLine($"<input type=\"hidden\" name=\"{endName}\" />");
            sbHtml.AppendLine($"<input type=\"text\" value=\"{endDate}\" style=\"width:6rem;border:none\" />");
            sbHtml.AppendLine("</div>");
            sbHtml.AppendLine("</div>");
            output.TagName = "div";
            output.Attributes.AddClass("form-group abp-datetime-range");
            output.TagMode = TagMode.StartTagAndEndTag;
            output.PreContent.AppendHtml(sbHtml.ToString());

            //output.PostElement.SetHtmlContent($@"
            //<script>
            // layui.use('laydate', function(){{
            //       var laydate = layui.laydate;

            //       //执行一个laydate实例
            //       laydate.render({{
            //         elem: '#{Name}' //指定元素
            //     ,change:function(value){{
            //           $('#{Name}').val(value);   
            //     }}
            //       }});
            //     }});
            // </script>");

            base.Process(context, output);
        }

        private string GetLabelAsHtml(TagHelperContext context, TagHelperOutput output)
        {
            if (!string.IsNullOrEmpty(Label))
            {
                var label = new TagBuilder("label");
                label.InnerHtml.AppendHtml(Label);
                return label.ToHtmlString();
            }
            return "";
        }
    }


    [HtmlTargetElement("test-button")]
    public class ButtonTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "button";
            base.Process(context, output);
        }
    }
}
