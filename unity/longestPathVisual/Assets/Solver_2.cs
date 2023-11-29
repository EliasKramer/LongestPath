using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    public class Solver_2 : IPathSolver
    {
        public PathResult CalculatePath(int n)
        {
            List<int> route = new List<int>();
            bool[,] neighbourVisited = new bool[n, n];
            int[] freeNeighbours = new int[n];

            for (int i = 0; i < n; i++)
            {
                freeNeighbours[i] = n - 1;
                neighbourVisited[i, i] = true;
            }

            int currentNode = Array.IndexOf(freeNeighbours, freeNeighbours.Max());

            while (true)
            {
                route.Add(currentNode);

                int maxFreeNeighbours = int.MinValue;
                int bestNeighbourIndex = -1;

                for (int i = 0; i < freeNeighbours.Length; i++)
                {
                    if (!neighbourVisited[currentNode, i] && maxFreeNeighbours < freeNeighbours[i])
                    {
                        maxFreeNeighbours = freeNeighbours[i];
                        bestNeighbourIndex = i;
                    }
                }

                if (bestNeighbourIndex == -1)
                {
                    break;
                }
                else
                {
                    freeNeighbours[currentNode]--;
                    freeNeighbours[bestNeighbourIndex]--;

                    neighbourVisited[currentNode, bestNeighbourIndex] = true;
                    neighbourVisited[bestNeighbourIndex, currentNode] = true;

                    currentNode = bestNeighbourIndex;
                }
            }

            return new PathResult()
            {
                NodeCount = n,
                Path = route
            };
        }
    }
}
