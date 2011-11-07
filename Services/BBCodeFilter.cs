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
    [OrchardFeature("Piedone.BBCode")]
    public class BBCodeFilter : IBBCodeFilter
    {
        private readonly IResourceManager _resourceManager;
        private Dictionary<string, BBTag> tags;

        #region Caching fields
        private readonly ICacheManager _cacheManager;
        private readonly ISignals _signals;
        private readonly string CachePrefix = "Piedone.BBCode.";
        private readonly string ParserSignal = "Piedone.BBCode.Parser";
        #endregion

        private bool styleIncluded = false;

        public BBCodeFilter(
            IResourceManager resourceManager,
            ICacheManager cacheManager, 
            ISignals signals)
        {
            _resourceManager = resourceManager;

            _cacheManager = cacheManager;
            _signals = signals;
        }

        public void AddTag(BBTag tag)
        {
            TryLoadDefaultTags(); // This is so defaults can be modified
            tags[tag.Name] = tag;
            TriggerParserChangedSignal();
        }

        public void RemoveTag(string name)
        {
            TryLoadDefaultTags(); // This is so defaults can be modified
            tags.Remove(name);
            TriggerParserChangedSignal();
        }

        public string ProcessContent(string text, string flavor)
        {
            if (String.IsNullOrEmpty(text))
                return "";

            if (flavor.Equals("bbcode", StringComparison.OrdinalIgnoreCase))
            {
                if (!styleIncluded)
                {
                    _resourceManager.Require("stylesheet", "BBCode");
                    styleIncluded = true;
                }
                return Parse(text);
            }

            return text;
        }

        public string Parse(string text)
        {
            text = BuildParser().ToHtml(text);
            text = "<p>" + text.Replace(Environment.NewLine, "</p>" + Environment.NewLine + "<p>") + "</p>";

            return text;
        }

        private BBCodeParser BuildParser()
        {
            return _cacheManager.Get(CachePrefix + "Parser", ctx =>
            {
                MonitorParserChangedSignal(ctx);
                
                TryLoadDefaultTags();

                return new BBCodeParser(
                    ErrorMode.ErrorFree,
                    null,
                    tags.Select(kvp => kvp.Value).ToList());
            });
        }

        private void TryLoadDefaultTags()
        {
            if (tags == null) LoadDefaultTags();
        }

        private void LoadDefaultTags()
        {
            tags = new Dictionary<string, BBTag>(10);
            tags["b"] = new BBTag("b", "<strong class=\"bbcode-bold\">", "</strong>");
            tags["i"] = new BBTag("i", "<em class=\"bbcode-italic\">", "</em>");
            tags["u"] = new BBTag("u", "<em class=\"bbcode-underline\">", "</em>");
            tags["s"] = new BBTag("s", "<del class=\"bbcode-strikethrough\">", "</del>");
            tags["code"] = new BBTag("code", "<code class=\"bbcode-code\">", "</code>");
            tags["img"] = new BBTag("img", "<img src=\"${content}\" class=\"bbcode-image\" />", "", false, true);
            tags["quote"] = new BBTag("quote", "<blockquote class=\"bbcode-quote\">", "</blockquote>");
            tags["sup"] = new BBTag("sup", "<sup class=\"bbcode-superscript\">", "</sup>");
            tags["sub"] = new BBTag("sub", "<sub class=\"bbcode-subscript\">", "</sub>");
            tags["url"] = new BBTag("url", "<a href=\"${href}\" class=\"bbcode-url\">", "</a>", new BBAttribute("href", ""), new BBAttribute("href", "href"));
        }

        private void MonitorParserChangedSignal(AcquireContext<string> ctx)
        {
            ctx.Monitor(_signals.When(ParserSignal));
        }

        private void TriggerParserChangedSignal()
        {
            _signals.Trigger(ParserSignal);
        }
    }
}
