using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    public static class ListExtensions
    {
        private static Random rng = new Random();
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
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

            int currentNode = 0;// Array.IndexOf(freeNeighbours, freeNeighbours.Max());

            while (true)
            {
                route.Add(currentNode);

                int maxFreeNeighbours = int.MinValue;
                int bestNeighbourIndex = -1;

                List<int> freeNeighboursIndices = new List<int>();

                for (int i = 0; i < freeNeighbours.Length; i++)
                {
                    freeNeighboursIndices.Add(i);
                }

                freeNeighboursIndices.Shuffle();

                foreach (int i in freeNeighboursIndices)
                {
                    if (!neighbourVisited[currentNode, i] && maxFreeNeighbours < freeNeighbours[i])
                    {
                        maxFreeNeighbours = freeNeighbours[i];
                        bestNeighbourIndex = i;
                    }
                }
                /*
                for (int i = currentNode; i < freeNeighbours.Length; i++)
                {
                    if (!neighbourVisited[currentNode, i] && maxFreeNeighbours < freeNeighbours[i])
                    {
                        maxFreeNeighbours = freeNeighbours[i];
                        bestNeighbourIndex = i;
                    }
                }
                for(int i = 0; i < currentNode; i++)
                {
                    if (!neighbourVisited[currentNode, i] && maxFreeNeighbours < freeNeighbours[i])
                    {
                        maxFreeNeighbours = freeNeighbours[i];
                        bestNeighbourIndex = i;
                    }
                }
                */
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
