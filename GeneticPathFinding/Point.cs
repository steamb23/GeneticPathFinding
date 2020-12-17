using System;
using System.Collections.Generic;
using System.Text;

namespace GeneticPathFinding
{
    struct Point
    {
        public int x;
        public int y;

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override int GetHashCode()
        {
            return (x + y) ^ y;
        }

        public override bool Equals(object obj)
        {
            if (obj is Point point)
            {
                return this == point;
            }
            return base.Equals(obj);
        }

        public static bool operator ==(Point v1, Point v2)
        {
            return v1.x == v2.x && v1.y == v2.y;
        }

        public static bool operator !=(Point v1, Point v2) => !(v1 == v2);
    }
}
