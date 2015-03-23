**NOTICE**: This project is no longer maintained. If you would like commit access to the repository please open up an issue. If you are still using this in production I would like to hear from you as well. It was fun putting this together but we eventually moved to different framework.

# Usage
To use Stitch in a Nancy project you simply need to add the Stitch package to your project and optionally the Stitch.Compilers.CoffeeScript if you'd like to compile coffeescript files.

> Install-Package Stitch

> Install-Package Stitch.Compilers.CoffeeScript

Then create the StitchConfiguration and Enable.

```csharp
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
                                                                },
                                                    Dependencies = new[]
                                                                {
                                                                    "Scripts/jquery.js"
                                                                }
                                                }
                                        }
                        };

Stitch.Enable(this, container.Resolve<IRootPathProvider>(), configuration);
```

