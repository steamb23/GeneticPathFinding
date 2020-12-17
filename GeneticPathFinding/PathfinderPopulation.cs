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

        public Chromosome BestChromosome { get; private set; }
        public double BestFitness { get; private set; }

        public ReadOnlyCollection<Chromosome> Chromosomes => Array.AsReadOnly(chromosomes);
        public Chromosome[] chromosomes;

        public Chromosome[] oldChromosomes;
        public double[] fitnesses;
        public double[] fitnessCumulatives;

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
                fitnesses = new double[description.PopulationSize];
                fitnessCumulatives = new double[description.PopulationSize];
            }
        }

        public void Reset()
        {
            var description = Description;
            Generation = 0;
            for (int i = 0; i < description.PopulationSize; i++)
            {
                chromosomes[i].Reset();
                fitnesses[i] = 0;
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
                this.isEmergencyStop = false;

                var description = Description;
                while (nextGeneration != 0)
                {
                    lock (description.PathFindingMap)
                    {
                        // 세대 교체
                        // 현재 세대가 시조 세대이면 세대 교체 일어나지 않음.
                        if (Generation > 0)
                        {
                            double fitnessCumulative = 0;
                            this.BestFitness = 0;
                            // 구세대 복사
                            Array.Copy(this.chromosomes, this.oldChromosomes, description.PopulationSize);
                            for (int i = 0; i < description.PopulationSize; i++)
                            {
                                // 선택
                                var chromosome = this.chromosomes[i] = Select().Clone();
                                // 교배
                                if (Program.Random.NextDouble() < description.CrossoverRate)
                                {
                                    chromosome.Crossover(Select());
                                }
                                // 변이
                                chromosome.Mutate(description.MutationRate);

                                // 평가
                                var fitness = this.fitnesses[i] = chromosome.Evaluate(description.PathFindingMap);
                                this.fitnessCumulatives[i] = fitnessCumulative += fitness;

                                // 베스트 염색체 선정
                                if (fitness > this.BestFitness)
                                {
                                    this.BestFitness = fitness;
                                    this.BestChromosome = chromosome;
                                }
                            }
                        }

                        // 결과 반영
                        description.PathFindingMap.Path.Clear();
                        description.PathFindingMap.Path.AddRange(BestChromosome.Datas);
                        Generation += 1;
                    }


                    // 비상 정지
                    if (this.isEmergencyStop)
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
            var totalFitnessRate = fitnessCumulatives[^0];

            var rouletRate = Program.Random.NextDouble() * totalFitnessRate;
            var length = fitnessCumulatives.Length;
            for (int i = 0; i < length - 1; i++)
            {
                if (rouletRate > fitnessCumulatives[i] && rouletRate < fitnessCumulatives[i + 1])
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
