using System;
using System.Reflection;
using System.Text;
using System.Threading;

namespace GeneticPathFinding
{
    class Program
    {
        public static Random Random { get; } = new Random();
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
            Console.CursorVisible = false;
            Console.WriteLine(Name);
            Console.WriteLine("청강문화산업대학교 201613018 최지헌, 2020-12");
            Console.WriteLine();
            Console.WriteLine("타일맵 테스트");
            var tilemap = Tilemap.Load("Tilemap.txt");
            Console.WriteLine(tilemap.ToMapString(true));
            Console.WriteLine();
            Console.WriteLine("패스파인딩맵 테스트");
            var pathFindingMap = new PathFindingMap(tilemap, new Point(1,1));
            pathFindingMap.Path = new System.Collections.Generic.List<Direction>()
            {
                Direction.Right,
                Direction.Right,
                Direction.Right,
                Direction.Right,
                Direction.Down,
                Direction.Right,
                Direction.Right,
                Direction.Right,
                Direction.Right,
                Direction.Right,
                Direction.Right,
                Direction.Right,
                Direction.Down,
                Direction.Up,
            };
            Console.WriteLine(pathFindingMap.ToMapString(true));




            //Console.Write("생각중...");
            //for (int i = 0; i < 1000; i++)
            //{
            //    Console.Write('|');
            //    Thread.Sleep(100);
            //    Console.CursorLeft -= 1;
            //    Console.Write('/');
            //    Thread.Sleep(100);
            //    Console.CursorLeft -= 1;
            //    Console.Write('-');
            //    Thread.Sleep(100);
            //    Console.CursorLeft -= 1;
            //    Console.Write('\\');
            //    Thread.Sleep(100);
            //    Console.CursorLeft -= 1;
            //}
        }
    }
}
