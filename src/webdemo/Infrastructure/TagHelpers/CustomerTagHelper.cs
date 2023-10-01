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
            var textDate = (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate)) ? $"{startDate}-{endDate}" : "";
            var sbHtml = new StringBuilder();
            sbHtml.AppendLine($"<label class=\"col-mb-1 col-form-label\">{Label}</label>");
            sbHtml.AppendLine($"<input type = \"text\" class=\"form-control\" data-toggle=\"date-picker\" data-cancel-class=\"btn-warning\" id=\"{Label}\" name=\"{Label}\" value=\"{textDate}\">");
            sbHtml.AppendLine($"<input type = \"hidden\" name=\"{startName}\">");
            sbHtml.AppendLine($"<input type = \"hidden\" name=\"{endName}\">");
            sbHtml.AppendLine("</div>");
            sbHtml.AppendLine("</div>");
            output.TagMode = TagMode.StartTagAndEndTag;
            output.PreContent.AppendHtml(sbHtml.ToString());
            output.PostElement.SetHtmlContent($@"
            <script>
             $('[name={Label}]').val('');
             $('#{Label}').daterangepicker({{
             autoApply: false,
             autoUpdateInput: false,
             locale: {{
                format: 'YYYY-MM-DD',
                separator: '',
                applyLabel: '确定',
                cancelLabel: '取消',
                daysOfWeek: ['日', '一', '二', '三', '四', '五', '六'],
                monthNames: ['一月', '二月', '三月', '四月', '五月', '六月','七月', '八月', '九月', '十月', '十一月', '十二月'],
            }}
            }});

            $('#{Label}').on('apply.daterangepicker', function(ev, picker) {{
             debugger;

            $('[name={startName}]').val(picker.startDate.format('YYYY-MM-DD'));
            $('[name={endName}]').val(picker.endDate.format('YYYY-MM-DD'));
            $('[name={Label}]').val(picker.startDate.format('YYYY-MM-DD') + ' - ' + picker.endDate.format('YYYY-MM-DD'));
            }}).on('cancel.daterangepicker', function (ev, picker) {{
            $('[name={startName}]').val('');
            $('[name={endName}]').val('');
            $('[name={Label}]').val('');
            }});
             </script>");

            base.Process(context, output);
        }
    }
}
