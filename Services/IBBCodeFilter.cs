using System;
using Orchard.Services;

namespace Piedone.BBCode.Services
{
    public interface IBBCodeFilter : IHtmlFilter
    {
        void AddTag(CodeKicker.BBCode.BBTag tag);
        string Parse(string text);
        void RemoveTag(string name);
    }
}
