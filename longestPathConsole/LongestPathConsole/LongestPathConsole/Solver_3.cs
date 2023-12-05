using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    public class Solver_3 : IPathSolver
    {
        public PathResult CalculatePath(int n)
        {
            List<int> path = new List<int>();
            if (n % 2 == 0 || n == 1 || n == 0) return new PathResult() { Path = path, NodeCount = n };

            path.Add(0);
            path.Add(1);
            path.Add(2);
            path.Add(0);

            for (int i = 3; i < n; i += 2)
            {
                int x = 1;
                for (int j = 1; j < i; j++)
                {
                    path.Add(i + x);
                    path.Add(j);
                    x = (x + 1) % 2;
                }
                path.Add(i + 1);
                path.Add(i);
                path.Add(0);
            }

            return new PathResult() { Path = path, NodeCount = n };
        }
    }
}
