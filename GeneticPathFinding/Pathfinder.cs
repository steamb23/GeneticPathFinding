using System;
using System.Collections.Generic;
using System.Text;

namespace GeneticPathFinding
{
    /// <summary>
    /// PathfinderPopulation의 팩토리/래퍼 클래스
    /// </summary>
    class Pathfinder
    {
        public PathfinderPopulation Population { get; private set; }

        public Tilemap Tilemap
        {
            get;
            set;
        }
        public int PopulationSize
        {
            get;
            set;
        } = 200;
        public int ChromosomeSize
        {
            get;
            set;
        } = 100;
        public double CrossoverRate
        {
            get;
            set;
        } = 0.7;
        public double MutationRate
        {
            get;
            set;
        } = 0.01;

        public PathfinderDescription GetDescription()
        {
            return new PathfinderDescription()
            {
                Tilemap = Tilemap,
                PopulationSize = PopulationSize,
                ChromosomeSize = ChromosomeSize,
                CrossoverRate = CrossoverRate,
                MutationRate = MutationRate
            };
        }

        public Pathfinder(Tilemap tilemap)
        {
            Tilemap = tilemap;
        }

        // Population 재생성 및 리셋 절차
        public void Initialize()
        {
            Population = new PathfinderPopulation(GetDescription());
        }

        // 데이터 초기화
        public void Reset(bool isFullReset = false)
        {
            if (isFullReset)
            {
                Population.Reset(GetDescription());
            }
            else
            {
                Population.Reset();
            }
        }
    }
}
