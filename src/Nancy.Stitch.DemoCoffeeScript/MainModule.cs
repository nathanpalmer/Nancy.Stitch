using Nancy;

namespace Stitch.Tests.Nancy
{
    public class MainModule : NancyModule
    {
        public MainModule()
        {
            Get["/"] = _ => View["Index"];
        }
    }
}