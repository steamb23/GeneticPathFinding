using System;
using System.Collections.Generic;
using System.Text;

namespace GeneticPathFinding
{
    class PathFindingMap
    {

        public PathFindingMap(Tilemap baseTilemap, Point startPoint = default, Point targetPoint = default)
        {
            this.BaseTilemap = baseTilemap;
            this.StartPoint = startPoint;
            this.TargetPoint = targetPoint;
        }

        public List<Direction> Path { get; set; } = new List<Direction>();

        public Point StartPoint { get; }
        public Point TargetPoint { get; }

        public Tilemap BaseTilemap { get; }

        public static List<Point> GetRoute(Tilemap tilemap, Point startPoint, Point targetPoint, params Direction[] path)
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
                if (tilemap.GetData(nextPoint) == TilemapData.Blank &&
                    nextPoint.x > 0 && nextPoint.x < tilemap.XSize &&
                    nextPoint.y > 0 && nextPoint.y < tilemap.YSize)
                {
                    currentPoint = nextPoint;
                }

                // 이동한 위치가 끝지점이면 경로 빌드 종료.
                if(currentPoint == targetPoint)
                {
                    break;
                }
            }
            // 최종 위치
            route.Add(currentPoint);

            return route;
        }

        public string ToMapString(bool fullWidth = false)
        {
            var route = GetRoute(BaseTilemap, StartPoint, TargetPoint, Path.ToArray());

            // 원본 문자열 가져오기
            var originMapString = BaseTilemap.ToMapString(fullWidth);
            // 문자열 자르기
            var splitedMapString = originMapString.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            char[,] mapChars = new char[BaseTilemap.YSize, BaseTilemap.XSize];
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

        public PathFindingMap Clone()
        {
            var clone = (PathFindingMap)MemberwiseClone();

            clone.Path = new List<Direction>();

            return clone;
        }
    }
}
