# 1. `dotnet new`

## First look

    > dotnet new 

Help output shows installed templates, what languages etc they support,
and common options.

    > dotnet help new

Opens a new browser tab with more help.

    > dotnet new console -o demo1

The generated project file:

- Starts with SDK declaration
- Defines output type and targets
- Note: no references to `*.cs` files!

`Program.cs` is just a Simple hello world app.

    > cd demo1
    > dotnet run

Restores, builds and runs app!

## Other templates

    > cd ..
    > dotnet new -lang f# classlib -o demo2

F# references files, because order is important.

Library targets `netstandard2.0`.

## All the flags!

    > cd ..
    > dotnet new classlib --language c# --framework netstandard2.0 --output demo3 --name ademoproject

## Templates are open and NuGet-hosted

At https://github.com/dotnet/templating/wiki/Available-templates-for-dotnet-new
there's a curated list of templates - templates are nuget-hosted, so they can
be published by anyone. (New and open MSFT!)

Install Giraffe template:

    > dotnet new -i "giraffe-template::*"

Create a Giraffe project:

    > dotnet new giraffe -o demo4
    > cd demo4
    > dotnet restore
