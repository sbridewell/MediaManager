using System.Web.Mvc;
using StructureMap;

[assembly: WebActivator.PreApplicationStartMethod(typeof(MediaManager.Mvc.App_Start.StructuremapMvc), "Start")]

namespace MediaManager.Mvc.App_Start
{
    /// <summary>
    /// This class comes with the StructureMap.MVC nuget package for dependency injection.
    /// We shouldn't need to touch this class.
    /// </summary>
    public static class StructuremapMvc
    {
        /// <summary>
        /// Called by the runtime.
        /// </summary>
        public static void Start()
        {
            var container = (IContainer) IoC.Initialize();
            DependencyResolver.SetResolver(new SmDependencyResolver(container));
        }
    }
}