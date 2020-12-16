using System;
using System.Reflection;
using System.Text;
using System.Threading;

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
            Console.OutputEncoding = Encoding.UTF8;
            Console.Title = Name;
            Console.WriteLine(Name);
            Console.WriteLine("청강문화산업대학교 201613018 최지헌, 2020-12");
            Console.WriteLine();
            Console.WriteLine("타일맵 테스트");

            Console.WriteLine(Tilemap.Load("Tilemap.txt").ToMapString(true));
            Console.Write("실행중...");
            for(int i = 0; i < 1000; i++)
            {
                Console.Write('-');
                Thread.Sleep(100);
                Console.CursorLeft--;
                Console.Write('\\');
                Thread.Sleep(100);
                Console.CursorLeft--;
                Console.Write('|');
                Thread.Sleep(100);
                Console.CursorLeft--;
                Console.Write('/');
                Thread.Sleep(100);
                Console.CursorLeft--;
            }
        }
    }
}
