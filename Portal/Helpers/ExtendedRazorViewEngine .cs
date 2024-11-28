using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Encodings.Web;

namespace SecurityCRM.Helpers
{
    public class ExtendedRazorViewEngine : RazorViewEngine
    {
        public ExtendedRazorViewEngine(IRazorPageFactoryProvider pageFactory, IRazorPageActivator pageActivator, HtmlEncoder htmlEncoder, IOptions<RazorViewEngineOptions> optionsAccessor, ILoggerFactory loggerFactory, DiagnosticListener diagnosticListener) : base(pageFactory, pageActivator, htmlEncoder, optionsAccessor, loggerFactory, diagnosticListener)
        {
        }

        public void AddViewLocationFormat(string paths)
        {
            //List<string> existingPaths = new List<string>(ViewLocationExpanderContext);
            //existingPaths.Add(paths);

            //ViewLocationFormats = existingPaths.ToArray();
        }

        public void AddPartialViewLocationFormat(string paths)
        {
            //List<string> existingPaths = new List<string>(PartialViewLocationFormats);
            //existingPaths.Add(paths);

            //PartialViewLocationFormats = existingPaths.ToArray();
        }
    }
}