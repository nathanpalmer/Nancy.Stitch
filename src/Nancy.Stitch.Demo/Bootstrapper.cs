using Nancy;

namespace Stitch.Tests.Nancy
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void InitialiseInternal(TinyIoC.TinyIoCContainer container)
        {
            base.InitialiseInternal(container);

            var configuration = new StitchConfiguration()
                                    {
                                        /*Compilers = new[]
                                                        {
                                                            new StitchConfigurationCompiler {Type = typeof (JavaScriptCompiler).AssemblyQualifiedName, Extension = ".js"},
                                                            new StitchConfigurationCompiler {Type = typeof (CoffeeScriptCompiler).AssemblyQualifiedName, Extension = ".coffee"},
                                                            new StitchConfigurationCompiler {Type = typeof (jQueryTemplateCompiler).AssemblyQualifiedName, Extension = ".tmpl"}
                                                        },*/
                                        Files = new[]
                                                    {
                                                        new StitchConfiguration()
                                                            {
                                                                Name = "/Scripts/app.stitch",
                                                                Paths = new[]
                                                                            {
                                                                                "Scripts/App"
                                                                            }
                                                            }
                                                    }
                                    };

            container.Register<IStitchConfiguration>(configuration);

            Stitch.Enable(this, container.Resolve<IRootPathProvider>(), container.Resolve<IStitchConfiguration>());
        }
    }
}