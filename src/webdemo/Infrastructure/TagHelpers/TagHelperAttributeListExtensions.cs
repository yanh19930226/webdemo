using Microsoft.AspNetCore.Razor.TagHelpers;

namespace webdemo.Infrastructure.TagHelpers
{
    public static class TagHelperAttributeListExtensions
    {
        public static void AddClass(this TagHelperAttributeList attributes, string className)
        {
            if (!string.IsNullOrWhiteSpace(className))
            {
                TagHelperAttribute tagHelperAttribute = attributes["class"];
                if (tagHelperAttribute == null)
                {
                    attributes.Add("class", className);
                    return;
                }

                List<string> list = tagHelperAttribute.Value.ToString()!.Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                if (!list.Contains(className))
                {
                    list.Add(className);
                }
                attributes.SetAttribute("class", string.Join(" ", list));
            }
        }

        public static void RemoveClass(this TagHelperAttributeList attributes, string className)
        {
            if (string.IsNullOrWhiteSpace(className))
            {
                return;
            }

            TagHelperAttribute tagHelperAttribute = attributes["class"];
            if (tagHelperAttribute != null)
            {
                List<string> list = tagHelperAttribute.Value.ToString()!.Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                list.RemoveAll((string c) => c == className);
                attributes.SetAttribute("class", string.Join(" ", list));
            }
        }

        public static void AddIfNotContains(this TagHelperAttributeList attributes, string name, object value)
        {
            if (!attributes.ContainsName(name))
            {
                attributes.Add(name, value);
            }
        }
    }
}
