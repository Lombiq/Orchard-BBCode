using System.Web.Mvc;
using Orchard.Mvc.Html;
using Piedone.BBCode.Services;

namespace Piedone.BBCode.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static string ParseBBCode(this HtmlHelper html, string text)
        {
            return html.Resolve<IBBCodeFilter>().Parse(text);
        }
    }
}