using System.Web.Mvc;
using Orchard.Mvc.Html;
using Piedone.BBCode.Services;
using Orchard;

namespace Piedone.BBCode.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static string ParseBBCode(this HtmlHelper html, string text)
        {
            return html.ViewContext.RequestContext.GetWorkContext().Resolve<IBBCodeFilter>().Parse(text);
        }
    }
}