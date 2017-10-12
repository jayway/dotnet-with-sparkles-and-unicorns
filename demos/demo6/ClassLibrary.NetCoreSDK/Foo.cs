using ClassLibrary.NetFramework.Bar;
using Newtonsoft.Json;

namespace ClassLibrary.NetFramework
{
    public class Foo
    {
        public string DoStuff() => JsonConvert.SerializeObject(new Baz());
    }
}
