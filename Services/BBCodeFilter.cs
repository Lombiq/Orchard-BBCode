using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.Services;
using CodeKicker.BBCode;
using Orchard.Caching;
using Orchard.Environment.Extensions;
using Orchard.UI.Resources;

namespace Piedone.BBCode.Services
{
    // OrchardSuppressDependency is necessary as otherwise the built-in filter causes troubles
    [OrchardSuppressDependency("Orchard.Core.Common.Services.BbcodeFilter")]
    public class BBCodeFilter : IHtmlFilter
    {
        private readonly ICacheManager _cacheManager;
        private readonly IResourceManager _resourceManager;

        private bool styleIncluded = false;

        public BBCodeFilter(ICacheManager cacheManager, IResourceManager resourceManager)
        {
            _cacheManager = cacheManager;
            _resourceManager = resourceManager;
        }

        public string ProcessContent(string text, string flavor)
        {
            if (String.IsNullOrEmpty(text))
                return "";

            if (flavor.Equals("bbcode", StringComparison.OrdinalIgnoreCase))
            {
                if (!styleIncluded)
                {
                    _resourceManager.Require("stylesheet", "Piedone_BBCode");
                    styleIncluded = true;
                }
                return BBCodeReplace(text);
            }
            
            return text;
        }

        private string BBCodeReplace(string text)
        {
            var parser = new BBCodeParser(
                ErrorMode.ErrorFree, 
                null,
                new[]
                {
                    new BBTag("b", "<strong class=\"bbcode-strong\">", "</strong>"), 
                    new BBTag("i", "<span class=\"bbcode-italic\">", "</span>"), 
                    new BBTag("u", "<span class=\"bbcode-underline\">", "</span>"), 
                    new BBTag("s", "<span class=\"bbcode-strikethrough\">", "</span>"), 
                    new BBTag("code", "<code class=\"bbcode-code\">", "</code>"), 
                    new BBTag("img", "<img src=\"${content}\" class=\"bbcode-image\" />", "", false, true), 
                    new BBTag("quote", "<blockquote class=\"bbcode-quote\">", "</blockquote>"), 
                    //new BBTag("list", "<ul class=\"bbcode-list\">", "</ul>"), 
                    //new BBTag("*", "<li>", "</li>", true, false), 
                    new BBTag("sup", "<sup class=\"bbcode-superscript\">", "</sup>"), 
                    new BBTag("sub", "<sub class=\"bbcode-subscript\">", "</sub>"), 
                    new BBTag("url", "<a href=\"${href}\" class=\"bbcode-url\">", "</a>", new BBAttribute("href", ""), new BBAttribute("href", "href"))
                });


            text = parser.ToHtml(text);
            text = "<p>" + text.Replace(Environment.NewLine, "</p>" + Environment.NewLine + "<p>") + "</p>";

            return text;
        }
    }
}
