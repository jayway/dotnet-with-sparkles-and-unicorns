layout: true

---

class: center, middle

## .NET Core CLI and .NET Standard

.right.bottom.bold[**@tlycken** on Slack, GitHub, Twitter...]

???

My name is Tomas Lycken. I've met most of you, and the rest of you have
probably seen my handle on Slack, or - at the very least - gotten a couple
of emails about GitHub permissions lately...

I work with .NET, primarily backend but if I have to work with web frontend
I prefer React, and I'm curious about TypeScript...

In the competence assistance this talk is titled
".NET Core CLI and .NET Standard", but I've decided to instead call it...

---

class: center, middle

## `dotnet`, with ✨ and 🦄

.right.bottom.bold[**@tlycken** on Slack, GitHub, Twitter...]

???

... dotnet with sparkles and unicorns.

There's a lot of ground to cover, and Microsoft have introduced a lot of
new and shiny features, so I'll do my best to show off the things I think
are most awesome.

I'll start with a few slides on the various parts of this new .NET eco-
system, like the .NET Standard, .NET Core, .NET Framework and how they all
relate to each-other. Once we've covered that, the rest of the talk is mostly
demos of various aspects of the new SDK and how you can start using it today.

---

class: right, middle

#So, what is the .NET Standard?

???

So, what is the .NET Standard? What problem does it solve?

The main pain point is multi-targeting.

---

## The .NET world we knew

![yesterday](/slides/images/dotnet-yesterday.png)

.bottom.right[Image from the announcement blog post: https://blogs.msdn.microsoft.com/dotnet/2016/09/26/introducing-net-standard/]

???

So, before the .NET Standard was introduced, each .NET runtime had its own
base library. Many components were available on multiple, or even all, platforms,
such as generic collections and LINQ, but even if the interface was the same,
there was no guarantee that the actual implementation was the same.

This made writing apps for multiple platforms a pain.

---

## Brave new .NET

![yesterday](/slides/images/dotnet-today.png)

.bottom.right[Image from the announcement blog post: https://blogs.msdn.microsoft.com/dotnet/2016/09/26/introducing-net-standard/]

???

With the .NET Standard, there's a way for the application to specify what it needs,
and to figure out whether a particular runtime supports its needs.

---

class: middle, center

# ...?
???

But if you are like me, this isn't really enough to *grok* what the .NET Standard is. There has to be a better way to explain it, right?

David Fowler on the ASP.NET Core team found a way that I think is way
superior to anything else I've seen. It's brilliant mainly because it talks
about these new things in a language you all already know quite well: C#.

Source: https://gist.github.com/davidfowl/8939f305567e1755412d6dc0b8baf1b7

---

## The .NET Standard defines available APIs

```c#
interface INetStandard10
{
    void Primitives();
    void Reflection();
    void Tasks();
    void Collections();
    void Linq();
}

interface INetStandard11 : INetStandard10
{
    void ConcurrentCollections();
    void InteropServices();
}

// ...and so on until

interface INetStandard20 : INetStandard16
{
    // lots of stuff
}
```

???

So, we can see the .NET Standard as a set of interfaces, defining APIs that should
be available for a given runtime to meet the standard. The classes that eventually
implement these interfaces are *.NET Platforms*, on which you can run your applications.


---

## Full .NET Framework implements the standard

```c#
interface INetFramework45 : INetStandard11
{
    void FileSystem();
    void Console();
    void ThreadPool();
    void Crypto();
    void WebSockets();
    void Process();
    void Sockets();

    void AppDomain();
    void Xml();
    void Drawing();
    void SystemWeb();
    void WPF();
    void WindowsForms();
    void WCF();
}

interface INetFramework451 : INetFramework45, INetStandard12
{
    // ...
}
```

###.right[...and may provide extra APIs]


???

But of course, these platforms might have other interfaces too, not specified
in the standard, so lets create interfaces for the platforms as well.

---

## Other platforms also implement the standard

```c#
interface IMono43 : INetFramework46
{
    void MonoSpecificApi();
}

interface IWindowsUniversalPlatform : INetStandard14 // Windows Universal Platform
{
    void GPS();
    void Xaml();
}

interface IXamarinIOS : INetStandard15
{
    void AppleAPIs();
}

interface IXamarinAndroid : INetStandard15
{
    void GoogleAPIs();
}
```

???

Mind you, there are other platforms than the full .NET Framework - as I'm sure
all the #winclient people here are eager to point out. Those fit well into this
analogy as well.

---

## ...and the standard helps define what a future platform can do!

```c#
interface ISomeFuturePlatform : INetStandard13
{
    // A future platform chooses to implement a specific .NET Standard version.
    // All libraries that target that version are instantly compatible with this new
    // platform
}
```

???

We can even use the standard to define what some *future* platform will be
capable of, even if that platform hasn't been built yet!

By making a platform implement a specific version of the standard, all apps
and libraries compatible with that standard are instantly compatible with
the new platform.

---

class: middle, right

# So, what is .NET Core?

???

So, where does .NET Core fit into this analogy?

---

## .NET Core is a platform, just like the others!

```c#
interface INetCoreApp10 : INetStandard15
{
}

interface INetCoreApp11 : INetStandard16
{
}

interface INetCoreApp20 : INetStandard20
{
}
```

???

In this analogy, .NET Core is a platform implementation, just like .NET Framework,
Mono or UWP.

There are currently three versions of the .NET Core runtime platform: 1.0, 1.1
and 2.0.

---

## Targeting the .NET Standard

```c#
public void Net45Application(INetFramework45 platform)
{
    platform.FileSystem();
    platform.Console();

    NetStandardLibrary13(platform);
}

public void NetStandardLibrary11(INetStandard11 platform)
{
    platform.FileSystem();
    platform.Console();
}
```

???

So, when you build your app - or library - you'll target one of the runtime platforms.

If you want to use libraries targeting a specific version of the .NET Standard, you
must ensure that the platform you've built your app for implements that version of the
standard.


---

## Targeting the .NET Standard

```c#
public void Net45Application(INetFramework45 platform)
{
    platform.FileSystem();
    platform.Console();

    NetStandardLibrary13(platform);
}

public void NetStandardLibrary13(INetStandard13 platform) // <-- note: it's now 1.3
{
    platform.FileSystem();
    platform.Console();
}

// reminder: .NET Framework 4.5 supports .NETStandard 1.1
interface INetFramework45 : INetStandard11 { /*...*/ } 
```

???

So, with a library targeting a higher version of the standard than what your platform
supports, you'll get errors.

This example fails because we're targeting .NET 4.5, which implements version 1.1 of
the standard, and we're trying to use a library that requires version 1.3.

---

## Multi-targeting on the .NET Standard

```c#
public void Net451Application(INetFramework451 platform)
{
    MultipleTargetsLibrary(platform);
}

public void NetCoreApplication(INetCoreApp10 platform)
{
    MultipleTargetsLibrary(platform);
}

public void MultipleTargetsLibrary(INetFramework45 platform)
{
    platform.FileSystem();
}

public void MultipleTargetsLibrary(INetStandard13 platform)
{
    platform.FileSystem();
}
```

???

Here we have two applications that depend on a library, and the library is built
for multiple targets. (I'll demo later how you build a library to support this.)

For the full .NET Framework 4.5.1 application, we select the best compatible
version of the library, which happens to be the one built for .NET 4.5.

For the .NET Core application, we can't use a framework for .NET 4.5, but we *can*
use one for .NET Standard 1.3, since the .NETCoreApp1.0 target supports .NET Standard
1.5, which is a superset of 1.3.

---

## What version can I use?

<table cellspacing="0">
<thead>
<tr><th>.NET Standard</th>
<th>1.0</th><th>1.1</th><th>1.2</th><th>1.3</th><th>1.4</th><th>1.5</th><th>1.6</th><th>2.0</th></tr>
</thead>
<tbody>
<tr><td>.NET Core</td><td>→</td><td>→</td><td>→</td><td>→</td><td>→</td><td>→</td><td>1.0</td><td>2.0</td></tr>
<tr><td>.NET Framework (with .NET Core 1.x SDK)</td><td>→</td><td>4.5</td><td>4.5.1</td><td>4.6</td><td>4.6.1</td><td>4.6.2</td><td></td><td></td></tr><tr><td>.NET Framework (with .NET Core 2.0 SDK)</td><td>→</td><td>4.5</td><td>4.5.1</td><td>4.6</td><td>4.6.1</td><td>→</td><td>→</td><td>4.6.1</td></tr>
<tr><td>Mono</td><td>→</td><td>→</td><td>→</td><td>→</td><td>→</td><td>→</td><td>4.6</td><td>5.4</td><tr>
<tr><td>Xamarin.iOS</td><td>→</td><td>→</td><td>→</td><td>→</td><td>→</td><td>→</td><td>10.0</td><td>10.14</td><tr>
<tr><td>Xamarin.Mac</td><td>→</td><td>→</td><td>→</td><td>→</td><td>→</td><td>→</td><td>3.0</td><td>3.8</td><tr>
<tr><td>Xamarin.Android</td><td>→</td><td>→</td><td>→</td><td>→</td><td>→</td><td>→</td><td>7.0</td><td>8.0</td><tr>
<tr><td>Universal Windows Platform</td><td>→</td><td>→</td><td>→</td><td>→</td><td>10.0</td><td>vNext</td><td>→</td><td>vNext</td><tr>
<tr><td>Windows</td><td>→</td><td>8.0</td><td>8.1</td><td></td><td></td><td></td><td></td><td></td><tr>
<tr><td>Windows Phone</td><td>→</td><td>→</td><td>8.1</td><td></td><td></td><td></td><td></td><td></td><tr>
<tr><td>Windows Phone Silverlight</td><td>8.0</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><tr>
</tbody>
</table>

???

So, you've all probably googled the .NET Standard to see if you could grok it,
ended up with this table, and given up. I know I have.

Now that you know how to think about what a .NET Standard version is, what a
runtime platform is, and how they relate to each-other, it shouldn't be too hard,
though.

There are two questions this table answers, depending on if you're writing a library
or an application:

* For library authors: what version of .NET Standard do I need to support to be
able to support platform X?
* For application authors: what version of .NET Standard must the package I want
to use support, in order to work in my production environment?

---

## What version can I use?

<table cellspacing="0">
<thead>
<tr><th>.NET Standard</th><th class="hilite">1.0</th><th class="hilite">1.1</th><th class="hilite">1.2</th><th class="hilite">1.3</th><th class="hilite">1.4</th><th class="hilite">1.5</th><th class="hilite">1.6</th><th class="hilite">2.0</th></tr>
</thead>
<tbody>
<tr><td>.NET Core</td><td>→</td><td>→</td><td>→</td><td>→</td><td>→</td><td>→</td><td>1.0</td><td class="hilite">2.0</td><tr>
<tr><td>.NET Framework (with .NET Core 1.x SDK)</td><td>→</td><td>4.5</td><td>4.5.1</td><td>4.6</td><td>4.6.1</td><td>4.6.2</td><td></td><td class="hilite"></td><tr>
<tr class="hilite"><td>.NET Framework (with .NET Core 2.0 SDK)</td><td>→</td><td>4.5</td><td>4.5.1</td><td>4.6</td><td>4.6.1</td><td>→</td><td>→</td><td class="warn">4.6.1</td><tr>
<tr><td>Mono</td><td>→</td><td>→</td><td>→</td><td>→</td><td>→</td><td>→</td><td>4.6</td><td>5.4</td><tr>
<tr><td>Xamarin.iOS</td><td>→</td><td>→</td><td>→</td><td>→</td><td>→</td><td>→</td><td>10.0</td><td>10.14</td><tr>
<tr><td>Xamarin.Mac</td><td>→</td><td>→</td><td>→</td><td>→</td><td>→</td><td>→</td><td>3.0</td><td>3.8</td><tr>
<tr><td>Xamarin.Android</td><td>→</td><td>→</td><td>→</td><td>→</td><td>→</td><td>→</td><td>7.0</td><td>8.0</td><tr>
<tr><td>Universal Windows Platform</td><td>→</td><td>→</td><td>→</td><td>→</td><td>10.0</td><td>vNext</td><td>→</td><td>vNext</td><tr>
<tr><td>Windows</td><td>→</td><td>8.0</td><td>8.1</td><td></td><td></td><td></td><td></td><td></td><tr>
<tr><td>Windows Phone</td><td>→</td><td>→</td><td>8.1</td><td></td><td></td><td></td><td></td><td></td><tr>
<tr><td>Windows Phone Silverlight</td><td>8.0</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><tr>
</tbody>
</table>

???

Let's consider an example:

You're writing a web application that needs to run on .NET Framework 4.6.1. You can use whatever tools you want to build the app, though, so of course you're on the new and shiny SDK.

That means that *all* .NET Standard versions are available to you.

That means that you can use any library targeting .NET Standard 1.2 *or lower*.

---

## What version can I use?

<table cellspacing="0">
<thead>
<tr><th>.NET Standard</th><th class="hilite">1.0</th><th class="hilite">1.1</th><th class="hilite">1.2</th><th class="hilite">1.3</th><th class="hilite">1.4</th><th>1.5</th><th>1.6</th><th>2.0</th></tr>
</thead>
<tbody>
<tr><td>.NET Core</td><td>→</td><td>→</td><td>→</td><td>→</td><td class="hilite">→</td><td>→</td><td>1.0</td><td>2.0</td><tr>
<tr class="hilite"><td>.NET Framework (with .NET Core 1.x SDK)</td><td>→</td><td>4.5</td><td>4.5.1</td><td>4.6</td><td class="warn">4.6.1</td><td>4.6.2</td><td></td><td></td><tr>
<tr><td>.NET Framework (with .NET Core 2.0 SDK)</td><td>→</td><td>4.5</td><td>4.5.1</td><td>4.6</td><td>4.6.1</td><td>→</td><td>→</td><td>4.6.1</td><tr>
<tr><td>Mono</td><td>→</td><td>→</td><td>→</td><td>→</td><td>→</td><td>→</td><td>4.6</td><td>5.4</td><tr>
<tr><td>Xamarin.iOS</td><td>→</td><td>→</td><td>→</td><td>→</td><td>→</td><td>→</td><td>10.0</td><td>10.14</td><tr>
<tr><td>Xamarin.Mac</td><td>→</td><td>→</td><td>→</td><td>→</td><td>→</td><td>→</td><td>3.0</td><td>3.8</td><tr>
<tr><td>Xamarin.Android</td><td>→</td><td>→</td><td>→</td><td>→</td><td>→</td><td>→</td><td>7.0</td><td>8.0</td><tr>
<tr><td>Universal Windows Platform</td><td>→</td><td>→</td><td>→</td><td>→</td><td>10.0</td><td>vNext</td><td>→</td><td>vNext</td><tr>
<tr><td>Windows</td><td>→</td><td>8.0</td><td>8.1</td><td></td><td></td><td></td><td></td><td></td><tr>
<tr><td>Windows Phone</td><td>→</td><td>→</td><td>8.1</td><td></td><td></td><td></td><td></td><td></td><tr>
<tr><td>Windows Phone Silverlight</td><td>8.0</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><tr>
</tbody>
</table>

???

Now, if you could only use version 1.x of the .NET Core SDK (for whatever reason), your options would be more limited. Instead of using libraries for whatever version of the
.NET Standard, you can now only use libraries targeting .NET Standard 1.4 or lower.

---

## What version can I use?

<table cellspacing="0">
<thead>
<tr><th>.NET Standard</th><th class="hilite">1.0</th><th class="hilite">1.1</th><th class="hilite">1.2</th><th class="hilite">1.3</th><th class="hilite">1.4</th><th>1.5</th><th>1.6</th><th>2.0</th></tr>
</thead>
<tbody>
<tr><td>.NET Core</td><td>→</td><td>→</td><td>→</td><td>→</td><td class="hilite">→</td><td>→</td><td>1.0</td><td>2.0</td><tr>
<tr><td>.NET Framework (with .NET Core 1.x SDK)</td><td>→</td><td>4.5</td><td>4.5.1</td><td>4.6</td><td class="hilite">4.6.1</td><td>4.6.2</td><td></td><td></td><tr>
<tr><td>.NET Framework (with .NET Core 2.0 SDK)</td><td>→</td><td>4.5</td><td>4.5.1</td><td>4.6</td><td class="hilite">4.6.1</td><td>→</td><td>→</td><td>4.6.1</td><tr>
<tr><td>Mono</td><td>→</td><td>→</td><td>→</td><td>→</td><td class="hilite">→</td><td>→</td><td>4.6</td><td>5.4</td><tr>
<tr><td>Xamarin.iOS</td><td>→</td><td>→</td><td>→</td><td>→</td><td class="hilite">→</td><td>→</td><td>10.0</td><td>10.14</td><tr>
<tr><td>Xamarin.Mac</td><td>→</td><td>→</td><td>→</td><td>→</td><td class="hilite">→</td><td>→</td><td>3.0</td><td>3.8</td><tr>
<tr><td>Xamarin.Android</td><td>→</td><td>→</td><td>→</td><td>→</td><td class="hilite">→</td><td>→</td><td>7.0</td><td>8.0</td><tr>
<tr class="hilite"><td>Universal Windows Platform</td><td>→</td><td>→</td><td>→</td><td>→</td><td class="warn">10.0</td><td>vNext</td><td>→</td><td>vNext</td><tr>
<tr><td>Windows</td><td>→</td><td>8.0</td><td>8.1</td><td></td><td class="hilite"></td><td></td><td></td><td></td><tr>
<tr><td>Windows Phone</td><td>→</td><td>→</td><td>8.1</td><td></td><td class="hilite"></td><td></td><td></td><td></td><tr>
<tr><td>Windows Phone Silverlight</td><td>8.0</td><td></td><td></td><td></td><td class="hilite"></td><td></td><td></td><td></td><tr>
</tbody>
</table>

???

Another one:

You're writing a library, that needs to support both the full .NET Framework and the UWP. Versions aren't so important, as long as they're actually released.

The current release of UWP supports .NET Standard 1.4, so you can use that, but not 1.5 or above.
