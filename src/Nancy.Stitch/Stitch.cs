using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Nancy;
using Nancy.Bootstrapper;
using Stitch.Compilers;

namespace Stitch.Tests.Nancy
{
    public class Stitch
    {
        public static void Enable(IApplicationPipelines applicationPipelines, IRootPathProvider rootPathProvider, IStitchConfiguration configuration)
        {
            if (applicationPipelines == null)
            {
                throw new ArgumentNullException("applicationPipelines");
            }

            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

            if (rootPathProvider == null)
            {
                throw new ArgumentNullException("rootPathProvider");
            }

            var compilerTypes =
                from type in AppDomainAssemblyTypeScanner.Types
                where typeof(ICompile).IsAssignableFrom(type)
                select type;

            var compilers = compilerTypes.Select(compiler => (ICompile) Activator.CreateInstance(compiler)).ToList();

            applicationPipelines.BeforeRequest.AddItemToStartOfPipeline(GetStitchResponse(compilers, rootPathProvider, configuration));
        }

        private static Func<NancyContext, Response> GetStitchResponse(IEnumerable<ICompile> compilers, IRootPathProvider rootPathProvider, IStitchConfiguration configuration)
        {
            return ctx =>
                {
                    if (ctx.Request == null) return null;
                    if (compilers == null) return null;
                    if (rootPathProvider == null) return null;

                    if (ctx.Request.Uri.ToLowerInvariant().EndsWith(".stitch"))
                    {
                        Package package = null;
                        if (configuration.Files != null)
                        {
                            var file = configuration.Files.Where(f => f.Name.Equals(ctx.Request.Uri, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                            if (file != null)
                            {
                                package = new Package(
                                    rootPathProvider.GetRootPath(),
                                    file.Paths,
                                    file.Dependencies,
                                    file.Identifier ?? configuration.Identifier ?? "require",
                                    compilers);
                            }
                        }

                        if (package == null)
                        {
                            package = new Package(
                                rootPathProvider.GetRootPath(),
                                configuration.Paths,
                                configuration.Dependencies,
                                configuration.Identifier ?? "require",
                                compilers);
                        }

                        var response = new Response
                                           {
                                               StatusCode = HttpStatusCode.OK,
                                               ContentType = "application/x-javascript",
                                               Contents = s =>
                                                              {
                                                                  using (var writer = new StreamWriter(s))
                                                                  {
                                                                      writer.Write(package.Compile());
                                                                      writer.Flush();
                                                                  }
                                                              }
                                           };

                        return response;
                    }

                    return null;
                };
        }
    }
}