using Microsoft.AspNetCore.Mvc.Razor;
using System.Collections.Generic;

namespace IntroducaoAoMediatR.Utilities
{
    public class FeatureViewLocator : IViewLocationExpander
    {
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            return new[] 
            {
                "~/Features/{1}/{0}.cshtml",
                "~/Features/Shared/{0}.cshtml"
            };
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            context.Values["customviewlocation"] = nameof(FeatureViewLocator);
        }
    }
}