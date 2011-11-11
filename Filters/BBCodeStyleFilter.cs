using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orchard.Mvc.Filters;
using Orchard.Environment.Extensions;
using Orchard.UI.Resources;

namespace Piedone.BBCode.Filters
{
    /// <summary>
    /// Includes the BBCode stylesheet, as in some installations IResourceManager can't be resolved when requesting in the
    /// BBCodeFilter constructor.
    /// </summary>
    [OrchardFeature("Piedone.BBCode")]
    public class BBCodeStyleFilter : FilterProvider, IResultFilter
    {
        private readonly IResourceManager _resourceManager;

        public static bool RequireStylesheet { get; set; }

        public BBCodeStyleFilter(IResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
            RequireStylesheet = false;
        }

        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
        }

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            if (RequireStylesheet)
            {
                _resourceManager.Require("stylesheet", "BBCode");
            }
        }
    }
}