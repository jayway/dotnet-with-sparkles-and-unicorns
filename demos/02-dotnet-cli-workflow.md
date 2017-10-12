# 2. `dotnet` CLI workflow

## Set up a solution with a few projects

    > mkdir demo5
    > cd demo5

Start with creating a solution:

    > dotnet new sln

The solution format is same old same old.

Create a couple of projects:

    > dotnet new console -o app
    > dotnet new classlib -o lib
    > dotnet new xunit -o tests

Add the projects to the solution:

    > dotnet sln add .\app\app.csproj
    > dotnet sln add .\lib\lib.csproj
    > dotnet sln add .\tests\tests.csproj

Add project references:

    > dotnet add .\app\app.csproj reference .\lib\lib.csproj
    > dotnet add .\tests\tests.csproj reference .\lib\lib.csproj

Make the following changes:

* In `lib`, rename `Class1` to `Greeter` and create a new property:

       public string Greeting { get; } = "Hello, there!";

* In `Program.cs`, add `using lib;` and change the writeline statement to

       Console.WriteLine(new Greeter().Greeting);

Run the application

    > dotnet run --project .\app\app.csproj

Note: CLI doesn't have a notion of a "startup project" for a solution, so
you have to manually specify which project you want to start.

## Cool thing: live reloading!

Install the `dotnet-watch` tool, by adding to the app project:

    <ItemGroup>
        <DotNetCliToolReference Include="Microsoft.DotNet.Watcher.Tools" Version="2.0.0" />
    </ItemGroup>

Restore:

    > dotnet restore

Start the watcher from inside the app project:

    > cd app
    > dotnet run watch

Change the greeting string in the library, and see it run again in the console.

Show what happens with a build error.

Fix the build error, stop the watch (^C) and go back out to the solution directory:

    > cd ..

## Testing

Implement a test:

1. Rename `UnitTest1` to `GreetingTests`, the test method to `GreeterHasAMessageForYou`
2. Add `using lib;` at the top of the file
3. Add the following assertion:

       Assert.Equal("Hello, handsome!", new Greeter().Greeting);

Show that the test fails:

    > dotnet test .\tests\tests.csproj

Note that the syntax here is different; `dotnet run` uses `--project`, while
`dotnet test` does not.

You can also do solution-wide test runs with `dotnet test`, but it will give
errors for any projects that don't have any tests (e.g. `app` and `lib`).

Test also supports live watching:

Add the tool reference to the test project:

    <ItemGroup>
        <DotNetCliToolReference Include="Microsoft.DotNet.Watcher.Tools" Version="2.0.0" />
    </ItemGroup>

Navigate into the test directory and start the tests:

    > cd tests
    > dotnet watch test

Change the greeting message in `lib` to `"Hello, handsome!"` to make the test pass.

Navigate back to solution dir:

    > cd ..

## Creating deployment artifacts

### NuGet packages

`dotnet` CLI supports nuget packing natively:

    > dotnet pack -c Release

This creates a NuGet package in each project's bin folder. The package has
dependencies filled in for you.

Other metadata can be specified in the project file.

Add to lib.csproj:

    <Version>0.1.0</Version>
    <PackageVersion>0.1.0</PackageVersion>
    <Authors>tlycken</Authors>
    <Owners>tlycken</Owners>    
    <Copyright>2017 Tomas Lycken</Copyright>
    <Description>A cool demo package</Description>

Pack again:

    > dotnet pack -c Release

Open lib.0.1.0.nupkg and show the metadata and version.

### Applications

You can also publish a folder with the application:

    > dotnet publish -c Release .\app\app.csproj
    > ls .\app\bin\Release\netcoreapp2.0\publish

This directory + the `dotnet` runtime is all you need to run the app:

    > cd .\app\bin\Release\netcoreapp2.0\publish
    > dotnet app.dll

You can also bundle the runtime, creating a self-contained exe.

    > cd ..\..\..\..\..
    > dotnet publish .\app\app.csproj -C Release --self-contained -r win10-x64

This requires you to specify the runtime environment (go to `dotnet help publish`, look at the help for the `--runtime` flag and navigate to the RID catalog to see the list of possible values).

The output folder now contains everything you need to run the app without having the .NET Core Runtime installed:

    > .\app\bin\Release\netcoreapp2.0\win10-x64\publish\app.exe

You can compile for one runtime environment while working on another, i.e. cross-compile, so you can build e.g. OSX binaries on a Windows machine (but not test them, of course...).

    > dotnet publish .\app\app.csproj --self-contained -c Release -r osx.10.11-x64
