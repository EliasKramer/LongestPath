using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    public class Node
    {
        public int Idx { get; set; } = -1;
        public Node Next { get; set; } = null;
        public Node Previous { get; set; } = null;
    }
    public class Solver_1 : IPathSolver
    {
        public static void FindRouteLL(int points, List<int> route, bool[,] neighbourVisited, int[] freeNeighbours, Node[] pointsPerNeighbourCount)
        {
            // This variable is for optimization purposes. It tracks the highest number of unvisited neighbours that is currently possible.
            int currentPossibleMaxNeighbours = -1;

            // Find the highest possible amount of neighbours currently available
            for (int i = pointsPerNeighbourCount.Length - 1; i >= 0; i--)
            {
                if (pointsPerNeighbourCount[i].Next != null)
                {
                    currentPossibleMaxNeighbours = i;
                    break;
                }
            }

            // int currentNode = Array.IndexOf(freeNeighbours, freeNeighbours.Max());
            Node currentNode = pointsPerNeighbourCount[currentPossibleMaxNeighbours].Next;

            // Immediately remove currentNode
            pointsPerNeighbourCount[currentPossibleMaxNeighbours].Next = pointsPerNeighbourCount[currentPossibleMaxNeighbours].Next.Next;

            if (pointsPerNeighbourCount[currentPossibleMaxNeighbours].Next != null)
            {
                pointsPerNeighbourCount[currentPossibleMaxNeighbours].Next.Previous = pointsPerNeighbourCount[currentPossibleMaxNeighbours];
            }

            while (true)
            {
                route.Add(currentNode.Idx);

                Node bestNeighbour = null;

                for (int i = currentPossibleMaxNeighbours; i >= 0 && bestNeighbour == null; i--)
                {
                    Node curr = pointsPerNeighbourCount[i];

                    while (curr.Next != null && neighbourVisited[currentNode.Idx, curr.Next.Idx])
                    {
                        curr = curr.Next;
                    }

                    if (curr.Next != null)
                    {
                        bestNeighbour = curr.Next;

                        if (curr.Next.Next != null)
                        {
                            curr.Next.Next.Previous = curr;
                        }

                        curr.Next = curr.Next.Next;
                    }
                }

                if (bestNeighbour == null)
                {
                    break;
                }
                else
                {
                    freeNeighbours[currentNode.Idx]--;
                    freeNeighbours[bestNeighbour.Idx]--;

                    // bestNeighbour does not have to be inserted back into the linked list now since we can not travel to ourself anyways
                    // Only reinsert currentNode
                    currentNode.Next = pointsPerNeighbourCount[freeNeighbours[currentNode.Idx]].Next;
                    pointsPerNeighbourCount[freeNeighbours[currentNode.Idx]].Next = currentNode;
                    currentNode.Previous = pointsPerNeighbourCount[freeNeighbours[currentNode.Idx]];

                    if (currentNode.Next != null)
                    {
                        currentNode.Next.Previous = currentNode;
                    }

                    // Careful here: Don't decrement past our bestNeighbour
                    while (pointsPerNeighbourCount[currentPossibleMaxNeighbours].Next == null && freeNeighbours[bestNeighbour.Idx] != currentPossibleMaxNeighbours)
                    {
                        currentPossibleMaxNeighbours--;
                    }

                    neighbourVisited[currentNode.Idx, bestNeighbour.Idx] = true;
                    neighbourVisited[bestNeighbour.Idx, currentNode.Idx] = true;

                    currentNode = bestNeighbour;
                }
            }
        }
        public PathResult CalculatePath(int n)
        {
            int points = n;

            bool[,] neighbourVisited = new bool[points, points];

            for (int i = 0; i < neighbourVisited.GetLength(0); i++)
            {
                neighbourVisited[i, i] = true;
            }

            int[] freeNeighbours = Enumerable.Repeat(points - 1, points).ToArray();

            // Idx 0: number of points that have 0 available neighbours
            // Idx 1: number of points that have 1 available neighbour
            int[] pointsWithNNeighbours = new int[points];
            pointsWithNNeighbours[points - 1] = points;

            List<int> route = new List<int>((points - 1) * (points) / 2);

            Node[] pointsPerNeighbourCount = new Node[points];

            // Early init last node
            pointsPerNeighbourCount[points - 1] = new Node();

            // Create dummy node at all starts
            for (int i = 0; i < points; i++)
            {
                if (pointsPerNeighbourCount[i] == null)
                {
                    // Create dummy nodes at all start locations
                    pointsPerNeighbourCount[i] = new Node();
                }

                // Insert node at the last location since all nodes have the same amount of neighbours at the beginning
                Node newNode = new Node() { Idx = i, Next = pointsPerNeighbourCount[points - 1].Next };
                if (newNode.Next != null)
                {
                    newNode.Next.Previous = newNode;
                }

                pointsPerNeighbourCount[points - 1].Next = newNode;
                newNode.Previous = pointsPerNeighbourCount[points - 1];
            }

            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            while (freeNeighbours.Any(f => f != 0))
            {
                // FindRouteO(route, neighbourVisited, freeNeighbours);
                // FindRouteS(points, route, neighbourVisited, freeNeighbours, pointsWithNNeighbours);
                FindRouteLL(points, route, neighbourVisited, freeNeighbours, pointsPerNeighbourCount);
            }

            return new PathResult() { 
                NodeCount = n,
                Path = route
            };
        }
    }
}
