using Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LongestPathConsole
{
    public class Solver_4 : IPathSolver
    {
        public PathResult CalculatePath(int n)
        {
            List<int> path = new List<int>();
            bool[,] connected = new bool[n, n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    connected[i, j] = false;
                }
                connected[i, i] = true;
            }

            path.Add(0);
            int currentIdx = 0;
            bool connectionFound;
            do
            {
                connectionFound = false;
                for (int i = 1; i < n; i++)
                {
                    int neighborIdx = (currentIdx + i) % n;

                    if (connected[currentIdx, neighborIdx] == false)
                    {
                        path.Add(neighborIdx);
                        connected[currentIdx, neighborIdx] = true;
                        connected[neighborIdx, currentIdx] = true;
                        currentIdx = neighborIdx;
                        connectionFound = true;
                        break;
                    }
                }
            } while (connectionFound);

            return new PathResult()
            {
                Path = path,
                NodeCount = n
            };
        }
    }
}
