using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABCDEFG
{
    public class Node
    {
        public int Idx { get; set; } = -1;
        public Node Next { get; set; } = null;
        public Node Previous { get; set; } = null;
    }

    public static class Test
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
    }
}
