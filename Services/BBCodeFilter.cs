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
        private Dictionary<string, BBTag> _tags;
        private bool styleIncluded = false;

        /// <summary>
        /// Parser instance cache
        /// </summary>
        private static BBCodeParser _parser;

        public BBCodeFilter(IResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
        }

        public void AddTag(BBTag tag)
        {
            TryLoadDefaultTags(); // This is so defaults can be modified
            _tags[tag.Name] = tag;
            _parser = null;
        }

        public void RemoveTag(string name)
        {
            TryLoadDefaultTags(); // This is so defaults can be modified
            _tags.Remove(name);
            _parser = null;
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
            if (_parser != null) return _parser;

            TryLoadDefaultTags();

            _parser = new BBCodeParser(
                ErrorMode.ErrorFree,
                null,
                _tags.Select(kvp => kvp.Value).ToList());

            return _parser;
        }

        private void TryLoadDefaultTags()
        {
            if (_tags == null) LoadDefaultTags();
        }

        private void LoadDefaultTags()
        {
            _tags = new Dictionary<string, BBTag>(10);
            _tags["b"] = new BBTag("b", "<strong class=\"bbcode-bold\">", "</strong>");
            _tags["i"] = new BBTag("i", "<em class=\"bbcode-italic\">", "</em>");
            _tags["u"] = new BBTag("u", "<em class=\"bbcode-underline\">", "</em>");
            _tags["s"] = new BBTag("s", "<del class=\"bbcode-strikethrough\">", "</del>");
            _tags["code"] = new BBTag("code", "<code class=\"bbcode-code\">", "</code>");
            _tags["img"] = new BBTag("img", "<img src=\"${content}\" class=\"bbcode-image\" />", "", false, true);
            _tags["quote"] = new BBTag("quote", "<blockquote class=\"bbcode-quote\">", "</blockquote>");
            _tags["sup"] = new BBTag("sup", "<sup class=\"bbcode-superscript\">", "</sup>");
            _tags["sub"] = new BBTag("sub", "<sub class=\"bbcode-subscript\">", "</sub>");
            _tags["url"] = new BBTag("url", "<a href=\"${href}\" class=\"bbcode-url\">", "</a>", new BBAttribute("href", ""), new BBAttribute("href", "href"));
        }
    }
}
