using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Piedone.BBCode.Services;

namespace Piedone.BBCode.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static string ParseBBCode(this HtmlHelper helper, string text)
        {
            return BBCodeFilter.Instance.Parse(text);
        }
    }
}