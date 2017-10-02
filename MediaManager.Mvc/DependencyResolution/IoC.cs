using StructureMap;
using MediaManager.MvcViewModels;

namespace MediaManager.Mvc
{
    /// <summary>
    /// This class comes with the StructureMap.NVC nuget package.
    /// </summary>
    public static class IoC
    {
        /// <summary>
        /// This is where we register the concrete types which StructureMap should inject
        /// at runtime in place of the specified interfaces.
        /// This method is key to dependency injections.
        /// In this example, most of the code in the application uses
        /// <see cref="IMediaManagerDataSource"/>, but here it is registered so that the
        /// runtime creates an instance of <see cref="MediaManagerContext"/> to inject
        /// where that interface is used, e.g. as a parameter for a controller class's
        /// action method.
        /// Unit tests can inject some kind of test double (stub, fake, mock, etc) where
        /// this interface is used.
        /// </summary>
        /// <returns>Something that the runtime uses.</returns>
        public static IContainer Initialize()
        {
            ObjectFactory.Initialize(x =>
                        {
                            x.Scan(scan =>
                                    {
                                        scan.TheCallingAssembly();
                                        scan.WithDefaultConventions();
                                    });
                            x.For<IMediaManagerDataSource>().HttpContextScoped().Use<MediaManagerContext>();
            //                x.For<IExample>().Use<Example>();
                        });
            return ObjectFactory.Container;
        }
    }
}