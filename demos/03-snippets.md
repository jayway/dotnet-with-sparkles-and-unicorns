    var inflector = new Inflector.Inflector(CultureInfo.CreateSpecificCulture("en-US"));
    Console.WriteLine(inflector.Pluralize("entity"));

---

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

---

    Console.WriteLine(new Pluralizer().Pluralize("entity"));

---

    <PackageReference Include="EntityFramework" Version="6.1.3">
      <NoWarn>NU1701</NoWarn>
    </PackageReference>
