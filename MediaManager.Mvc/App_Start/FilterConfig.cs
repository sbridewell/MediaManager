using System.Web;
using System.Web.Mvc;

namespace MediaManager.Mvc
{
    /// <summary>
    /// No idea.
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// Called by the runtime.
        /// No idea what it does.
        /// </summary>
        /// <param name="filters">No idea.</param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
