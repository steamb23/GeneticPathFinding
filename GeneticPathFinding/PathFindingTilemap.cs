using System;
using System.Collections.Generic;
using System.Text;

namespace GeneticPathFinding
{
    class PathFindingMap
    {
        Tilemap baseTilemap;
        List<Direction> path;

        Point startPoint;

        public PathFindingMap(Tilemap baseTilemap, Point startPoint = default)
        {
            this.baseTilemap = baseTilemap;
            this.startPoint = startPoint;
        }

        public List<Direction> Path
        {
            get => this.path;
            set => this.path = value;
        }

        public Point StartPoint => startPoint;

        public Tilemap BaseTilemap => baseTilemap;

        public List<Point> GetRoute()
        {
            Point currentPoint = startPoint;
            List<Point> route = new List<Point>();
            // build route
            foreach (var direction in path)
            {
                route.Add(currentPoint);

                var nextPoint = currentPoint;
                switch (direction)
                {
                    case Direction.Up:
                        nextPoint.y--;
                        break;
                    case Direction.Right:
                        nextPoint.x++;
                        break;
                    case Direction.Down:
                        nextPoint.y++;
                        break;
                    case Direction.Left:
                        nextPoint.x--;
                        break;
                }

                // 다음 위치가 빈 공간이면 그 위치로 이동
                if (baseTilemap.GetData(nextPoint) == TilemapData.Blank &&
                    nextPoint.x > 0 && nextPoint.x < baseTilemap.XSize &&
                    nextPoint.y > 0 && nextPoint.y < baseTilemap.YSize)
                {
                    currentPoint = nextPoint;
                }
            }
            // 최종 위치
            route.Add(currentPoint);

            return route;
        }

        public string ToMapString(bool fullWidth = false)
        {
            var route = GetRoute();

            // 원본 문자열 가져오기
            var originMapString = baseTilemap.ToMapString(fullWidth);
            // 문자열 자르기
            var splitedMapString = originMapString.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            char[,] mapChars = new char[baseTilemap.YSize, baseTilemap.XSize];
            for (int y = 0; y < splitedMapString.Length; y++)
            {
                var currentLine = splitedMapString[y];
                for (int x = 0; x < currentLine.Length; x++)
                {
                    mapChars[y, x] = currentLine[x];
                }
            }

            foreach (var point in route)
            {
                mapChars[point.y, point.x] = fullWidth ? 'Ｏ' : 'O';
            }

            // 재조립
            StringBuilder stringBuilder = new StringBuilder();
            for (int y = 0; y < mapChars.GetLength(1); y++)
            {
                for (int x = 0; x < mapChars.GetLength(0); x++)
                {
                    stringBuilder.Append(mapChars[y, x]);
                }
                stringBuilder.AppendLine();
            }

            return stringBuilder.ToString();
        }
    }
}
