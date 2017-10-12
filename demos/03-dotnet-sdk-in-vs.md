# .NET Core SDK in Visual Studio

> Open `demo6.sln` in Visual Studio 2017.

> Show that `ConsoleApp.NetFramework` builds for .NET 4.6.1 by opening properties.

## Using NETStandard packages from .NET Framework

> Install a .NETStandard only NuGet package: Inflector.NETStandard. No warnings, no problems :)

> Add to `Program.cs`:

    var inflector = new Inflector.Inflector(CultureInfo.CreateSpecificCulture("en-US"));
    Console.WriteLine(inflector.Pluralize("entity"));

> (and fix errors like using statements).

> Run program to show effects.

## Using own NETStandard libraries from .NET Framework

> Install the Inflector.NETStandard package into ClassLibrary.NetStandard.

> Create a class with the following code:

    public class Pluralizer
    {
        private readonly Inflector.Inflector _inflector;

        public Pluralizer()
        {
            _inflector = new Inflector.Inflector(CultureInfo.CreateSpecificCulture("en-US"));
        }

        public string Pluralize(string singular)
        {
            return _inflector.Pluralize("entity");
        }
    }

> Replace the program implementation with

    Console.WriteLine(new Pluralizer().Pluralize("entity"));

and fix build errors by adding references as necessary.

> Demonstrate that it builds and runs without warnings.

## Using .NET Framework libraries from .NET Standard

There's nothing that stops you from adding a reference to a .NET Framework
library in a project that targets .NET Standard.

> Add EntityFramework to ClassLibrary.NetStandard.

> Show build warning

The warning appears because just because although .NET Framework 4.6.1
has everything in .NET Standard 2.0, .NET Standard 2.0 doesn't have
everything in .NET Framework 4.6.1. So this *might* be fine, if the
package doesn't use anything outside of the standard (or at least you
don't hit any of those code paths). It might also blow up at runtime.

If you know that you're fine, you can supress the warning

> Edit the EF reference in ClassLibrary.NetStandard.csproj to be:

    <PackageReference Include="EntityFramework" Version="6.1.3">
      <NoWarn>NU1701</NoWarn>
    </PackageReference>

> Demonstrate that the project builds and runs without warnings.

However, there's a better way to fix it: by changing the target framework in your project.

> Revert the changes to the reference node

> Change the target framework to `net461`

> Build and show that no warnings appear.

## Fixing DLL Hell

There's an interesting thing that happens now, though, because there's
a bunch of system assemblies that are resolved differently depending on
whether you're on the .NET Standard or the .NET Framework.

> Run the program, show that it blows up with an assembly load error

This is the same problem (but for a different dll) as we had in the
`#dotnet` channel on Slack the other week.

To fix this; first ensure that you remove all troublesome references.

> Uninstall .NET Standard Library with force and remove deps options

> If the GUI doesn't show the options, use Package Manager Console:  

    > Uninstall-Package -RemoveDependencies -Force NETStandard.Library -ProjectName ClassLibrary.NetStandard
    > Uninstall-Package -RemoveDependencies -Force NETStandard.Library -ProjectName ConsoleApp.NetFramework

> This might not be good enough; double check that the build fails
> (missing Inflector), and if it doesn't, manually remove assembly
> references and the packages folder.

With a clean, failing build, install the *latest* version of the
NETStandard.Library NuGet package into *all* projects.

Then, install whatever missing packages (Inflector.NETStandard).

> Show that now everything builds and runs again.

## Converting an old project to the new SDK

Microsoft's recommendation is actually that you should do this for
*all* your projects, regardless of runtime; in a solution with a number
of libraries and an entry point application project, convert everything
but the application project (or that too, if possible) to the new SDK.

> Add existing project ClassLibrary.NetFramework to solution.
> Show contents (Foo, Baz, ref to Newtonsoft.Json, targets net461).

The easiest way to convert this, is to create a new project, copy the
code into there and fix build errors.

Normally, you'd do this by remove from sln, rename folder, add new project
in its place, since you want to keep the names, but I'll simplify and add
a new project with a different name.

> Add new .NETStandard class library ClassLibrary.NetCoreSDK. Delete Class1.cs.

> Copy the code into the new project.

> Open the new project file - again, code is included by default, so no
> changes! :)

> Change target framework to net461

> Show build error in Foo.cs - fix by installing Newtonsoft.Json

The project is now converted! The build output is equivalent, and can be
used in all the same places as before.
