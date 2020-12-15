using System;
using System.Reflection;

namespace GeneticPathFinding
{
    class Program
    {
        public static string Name
        {
            get
            {
                var currentAssembly = Assembly.GetExecutingAssembly();
                var assemblyName = currentAssembly.GetName();
                return $"{assemblyName.Name} v{assemblyName.Version}";
            }
        }
        static void Main(string[] args)
        {
            Console.Title = Name;
            Console.WriteLine(Name);
            Console.WriteLine("청강문화산업대학교 201613018 최지헌, 2020-12");
            Console.WriteLine();
            Console.WriteLine("타일맵 테스트");

            Console.WriteLine(Tilemap.Load("Tilemap.txt").ToMapString(true));
        }
    }
}
