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