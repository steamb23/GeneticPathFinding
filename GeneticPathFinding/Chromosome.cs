using System;
using System.Collections.Generic;
using System.Text;

namespace GeneticPathFinding
{
    class Chromosome
    {
        public Direction[] Datas { get; set; }

        public Chromosome(Direction[] datas)
        {
            this.Datas = datas;
        }

        public Chromosome(int size)
        {
            this.Datas = new Direction[size];
        }

        /// <summary>
        /// 다른 염색체와 교배합니다.
        /// </summary>
        /// <param name="other">교배할 염색체입니다. 해당 염색체의 값은 변경되지 않습니다.</param>
        public void Crossover(Chromosome other)
        {
            // 가장 작은 인덱스
            int minIndex = Math.Min(this.Datas.Length, other.Datas.Length);

            int p1 = Program.Random.Next(0, minIndex);
            int p2 = Program.Random.Next(p1, minIndex);

            Array.Copy(other.Datas, p1, this.Datas, p1, p2 - p1);
        }

        /// <summary>
        /// 지정된 변이 확률 만큼 데이터를 무작위로 변이시킵니다.
        /// </summary>
        /// <param name="mutationRate"></param>
        public void Mutate(double mutationRate)
        {
            var length = Datas.Length;
            for (int i = 0; i < length; i++)
            {
                if (Program.Random.NextDouble() < mutationRate)
                {
                    // 4개중 택1
                    Datas[i] = (Direction)Program.Random.Next(0, 4);
                }
            }
        }

        public void Reset()
        {
            var length = Datas.Length;
            for (int i = 0; i < length; i++)
            {
                // 4개중 택1
                Datas[i] = (Direction)Program.Random.Next(0, 4);
            }
        }

    }
}
