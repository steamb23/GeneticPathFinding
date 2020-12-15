using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GeneticPathFinding
{
    class Tilemap
    {
        TilemapData[,] tilemapDatas;

        public static Tilemap Load(string filename)
        {
            try
            {
                using var fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
                return Load(fileStream);
            }
            catch (IOException)
            {
                Console.WriteLine("파일을 열 수 없습니다.");
            }

            return null;
        }

        public static Tilemap Load(Stream stream)
        {
            // [G]enetic[P]ath[F]inding[M]ap
            const int GPFM = 1203747823;
            // 매직 넘버 판정
            var binaryReader = new BinaryReader(stream);
            //var test = binaryReader.ReadInt32();
            if (binaryReader.ReadInt32() == GPFM)
            {
                var textReader = new StreamReader(stream);
                textReader.ReadLine(); // 개행
                string tempString;
                string[] tempStrings;

                // 크기 헤더 읽기
                tempString = textReader.ReadLine();
                tempStrings = tempString.Split(',');
                int.TryParse(tempStrings[0], out int xSize);
                int.TryParse(tempStrings[1], out int ySize);

                if (xSize < 2 && ySize < 2)
                    return null;

                Tilemap tilemap = new Tilemap(xSize, ySize);

                // 맵 데이터 읽기
                for (int yPosition = 0; !textReader.EndOfStream && yPosition < ySize; yPosition++)
                {
                    tempString = textReader.ReadLine();
                    for (int xPosition = 0; xPosition < tempString.Length && xPosition < xSize; xPosition++)
                    {
                        tilemap.SetData(xPosition, yPosition, GetTilemapData(tempString[xPosition]));
                    }
                }

                return tilemap;
            }
            else
            {
                return null;
            }
        }

        private static TilemapData GetTilemapData(char mapCharacter)
        {
            return mapCharacter switch
            {
                '#' => TilemapData.Wall,
                '@' => TilemapData.Object,
                _ => TilemapData.Blank,
            };
        }

        public Tilemap(int xSize, int ySize)
        {
            tilemapDatas = new TilemapData[xSize, ySize];
        }

        public int XSize => tilemapDatas.GetLength(0);
        public int YSize => tilemapDatas.GetLength(1);

        public TilemapData GetData(int x, int y) => tilemapDatas[x, y];
        public void SetData(int x, int y, TilemapData value) => tilemapDatas[x, y] = value;

        public string ToMapString(bool fullWidth = false)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int y = 0; y < YSize; y++)
            {
                for (int x = 0; x < XSize; x++)
                {
                    stringBuilder.Append(TilemapDataToCharacter(tilemapDatas[x, y], fullWidth));
                }
                stringBuilder.AppendLine();
            }

            return stringBuilder.ToString();
        }

        private char TilemapDataToCharacter(TilemapData tilemapData, bool fullWidth = false)
        {
            if (fullWidth)
            {
                return tilemapData switch
                {
                    TilemapData.Wall => '＃',
                    TilemapData.Object => '＠',
                    _ => '．',
                };
            }
            else
            {
                return tilemapData switch
                {
                    TilemapData.Wall => '#',
                    TilemapData.Object => '@',
                    _ => '.',
                };
            }
        }
    }

    enum TilemapData
    {
        Blank,
        Wall,
        Object
    }
}
