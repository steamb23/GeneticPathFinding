using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using TerraText;

namespace GeneticPathFinding
{
    struct PathFinderDescription
    {
        public PathFindingMap PathFindingMap
        {
            get;
            set;
        }
        public int PopulationSize
        {
            get;
            set;
        }
        public int ChromosomeSize
        {
            get;
            set;
        }
        public double CrossoverRate
        {
            get;
            set;
        }
        public double MutationRate
        {
            get;
            set;
        }
    }
    class PathFinderPopulation
    {
        public PathFinderDescription Description
        {
            get;
            private set;
        }

        public int Generation
        {
            get;
            private set;
        }

        public ReadOnlyCollection<Chromosome> Chromosomes => Array.AsReadOnly(chromosomes);
        public Chromosome[] chromosomes;

        public Chromosome[] oldChromosomes;
        public double[] fitnessRates;
        public double[] fitnessRateCumulatives;

        public PathFinderPopulation(PathFinderDescription description)
        {
            Reset(description);
        }

        public void Reset(PathFinderDescription description)
        {
            Description = description;
            // null 염색체 시작점
            int chromosomeNullStartIndex = 0;
            if (chromosomes == null)
            {
                // 염색체 군 새로 할당
                chromosomes = new Chromosome[description.PopulationSize];

                ResizeDataArrays();
            }
            else if (chromosomes.Length != description.PopulationSize)
            {
                // 염색체 군 크기 조정
                chromosomeNullStartIndex = chromosomes.Length;
                var newArray = new Chromosome[description.PopulationSize];
                Array.Copy(chromosomes, newArray, description.PopulationSize);
                chromosomes = newArray;

                ResizeDataArrays();
            }

            // null 염색체에 새로 할당
            for (int i = chromosomeNullStartIndex; i < description.PopulationSize; i++)
            {
                chromosomes[i] = new Chromosome(description.ChromosomeSize);
            }

            Reset();

            void ResizeDataArrays()
            {
                // 각 데이터 배열 재할당
                oldChromosomes = new Chromosome[description.PopulationSize];
                fitnessRates = new double[description.PopulationSize];
                fitnessRateCumulatives = new double[description.PopulationSize];
            }
        }

        public void Reset()
        {
            var description = Description;
            Generation = 0;
            for (int i = 0; i < description.PopulationSize; i++)
            {
                chromosomes[i].Reset();
                fitnessRates[i] = 0;
            }
        }

        bool isEmergencyStop;
        /// <summary>
        /// 지정된 세대만큼 실행합니다.
        /// </summary>
        /// <param name="nextGeneration">실행할 세대 수 입니다. 0미만의 값을 넣으면 무한히 실행됩니다.</param>
        public async Task Run(int nextGeneration = -1)
        {
            await Task.Run(() =>
            {
                isEmergencyStop = false;

                var description = Description;
                while (nextGeneration != 0)
                {
                    // 세대 교체
                    // 현재 세대가 시조 세대이면 세대 교체 일어나지 않음.
                    if (Generation > 0)
                    {
                        // 구세대 복사
                        Array.Copy(chromosomes, oldChromosomes, description.PopulationSize);
                        for (int i = 0; i < description.PopulationSize; i++)
                        {
                            // 선택
                            chromosomes[i] = Select();
                            // 교배
                            if (Program.Random.NextDouble() < description.CrossoverRate)
                            {
                                chromosomes[i].Crossover(Select());
                            }
                            // 변이
                            chromosomes[i].Mutate(description.MutationRate);

                            // 평가
                            /*
                             * 고려해야할 사항:
                             * 라우트의 길이
                             * 라우트 끝점과 목적지간의 거리
                             */

                        }
                    }
                    Generation += 1;


                    // 비상 정지
                    if (isEmergencyStop)
                    {
                        break;
                    }
                    // 다음 세대 처리 준비
                    if (nextGeneration > 0)
                        nextGeneration -= 1;
                }
            });
        }

        public void EmergencyStop()
        {
            isEmergencyStop = true;
        }

        Chromosome Select()
        {
            // 룰렛휠 선택
            var totalFitnessRate = fitnessRateCumulatives[^0];

            var rouletRate = Program.Random.NextDouble() * totalFitnessRate;
            var length = fitnessRateCumulatives.Length;
            for (int i = 0; i < length - 1; i++)
            {
                if (rouletRate > fitnessRateCumulatives[i] && rouletRate < fitnessRateCumulatives[i + 1])
                {
                    return oldChromosomes[i];
                }
            }

            // 비상용
            //return oldChromosomes[^0];
            return null;
        }
    }
}
