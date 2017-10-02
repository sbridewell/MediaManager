using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MediaManager.Mvc.Controllers
{
    /// <summary>
    /// Controller for the ~/Home URL.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Responds to the ~/Home/Index URL.
        /// </summary>
        /// <returns>
        /// The default view in the MVC5 web application template - I haven't changed it.
        /// </returns>
        public ActionResult Index()
        {
            return View();
        }
    }
}