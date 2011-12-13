using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Piedone.BBCode.Services;
using Orchard.Mvc.Html;

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