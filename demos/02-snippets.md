    public class Greeter
    {
        public string Greeting { get; } = "Hello, there!";
    }

---

    Console.WriteLine(new Greeter().Greeting);

---

  <ItemGroup>
    <DotNetCliToolReference Include="Microsoft.DotNet.Watcher.Tools" Version="2.0.0" />
  </ItemGroup>

    public class GreeterTests
    {
        [Fact]
        public void GreeterHasAMessageForYou()
        {
            Assert.Equal("Hello, handsome!", new Greeter().Greeting);
        }
    }

---

    <Version>0.1.0</Version>
    <PackageVersion>0.1.0</PackageVersion>
    <Authors>tlycken</Authors>
    <Owners>tlycken</Owners>    
    <Copyright>2017 Tomas Lycken</Copyright>
    <Description>A cool demo package</Description>
