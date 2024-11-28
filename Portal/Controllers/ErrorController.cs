using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace SecurityCRM.Controllers
{
    [Route("Error")]

    public class ErrorController : Controller
    {
        //
        // GET: /Error/
        [HttpGet]
        [Route("Index")]
        public ActionResult Index()
        {
            return View("Error");
        }
        [HttpGet]
        [Route("NotFound")]
        public ActionResult NotFound()
        {
            return View("Not_Found");
        }
        [HttpGet]
        [Route("Unavailable")]
        public ActionResult Unavailable()
        {
            return View("Unavailable");
        }
        [HttpGet]
        [Route("AccessDenied")]
        public ActionResult AccessDenied()
        {
            return View("AccessDenied");
        }
    }
}