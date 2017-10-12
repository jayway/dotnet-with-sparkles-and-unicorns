using ClassLibrary.NetStandard;
using System;

namespace ConsoleApp.NetFramework
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine(new Pluralizer().Pluralize("entity"));
        }
    }
}
