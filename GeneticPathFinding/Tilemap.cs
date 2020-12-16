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
            Span<byte> bom = stackalloc byte[] { 0xEF, 0xBB, 0xBF };

            // 매직 넘버 판정
            Span<byte> buffer = stackalloc byte[4];
            stream.Read(buffer);
            // UTF-8 BOM 처리
            if (buffer.Slice(0, 3).SequenceEqual(bom))
            {
                stream.Position--;
                stream.Read(buffer);
            }

            Span<byte> GPFM = stackalloc byte[] { 0x47, 0x50, 0x46, 0x4D };
            if (buffer.SequenceEqual(GPFM))
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
            switch (mapCharacter)
            {
                default:
                    return TilemapData.Blank;
                case '#':
                case '＃':
                    return TilemapData.Wall;
                case '@':
                case '＠':
                    return TilemapData.Object;
            }
        }

        public Tilemap(int xSize, int ySize)
        {
            tilemapDatas = new TilemapData[ySize, xSize];
        }

        public int XSize => tilemapDatas.GetLength(1);
        public int YSize => tilemapDatas.GetLength(0);

        public TilemapData GetData(int x, int y) => tilemapDatas[y, x];
        public TilemapData GetData(Point point) => GetData(point.x, point.y);
        public void SetData(int x, int y, TilemapData value) => tilemapDatas[y, x] = value;

        public virtual string ToMapString(bool fullWidth = false)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int y = 0; y < YSize; y++)
            {
                for (int x = 0; x < XSize; x++)
                {
                    stringBuilder.Append(TilemapDataToCharacter(tilemapDatas[y, x], fullWidth));
                }
                stringBuilder.AppendLine();
            }

            return stringBuilder.ToString();
        }

        private char TilemapDataToCharacter(TilemapData tilemapData, bool fullWidth = false)
        {
            if (fullWidth)
            {
                switch (tilemapData)
                {
                    default:
                        return '．';
                    case TilemapData.Wall:
                        return '＃';
                    case TilemapData.Object:
                        return '＠';
                }
            }
            else
            {
                switch (tilemapData)
                {
                    default:
                        return '.';
                    case TilemapData.Wall:
                        return '#';
                    case TilemapData.Object:
                        return '@';
                }
            }
        }
    }

    enum TilemapData : byte
    {
        Blank,
        Wall,
        Object
    }
}
