# Usage
To use Stitch in a Nancy project you simply need to add the Stitch package to your project and optionally the Stitch.Compilers.CoffeeScript if you'd like to compile coffeescript files.

> Install-Package Stitch
> Install-Package Stitch.Compilers.CoffeeScript

Then add the StitchConfiguration to your Bootstrapper, Register and Enable.

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

            container.Register<IStitchConfiguration>(configuration);

            Stitch.Enable(this, container.Resolve<IRootPathProvider>(), container.Resolve<IStitchConfiguration>());
```

