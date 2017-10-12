using System.Globalization;

namespace ClassLibrary.NetStandard
{
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
}