using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ABCDEFG;

public class Vrp
{
    public static void FindRoute(int points, List<int> route, bool[,] neighbourVisited, (int index, int value)[] freeNeighbours)
    {
        Array.Sort(freeNeighbours, (n1, n2) => n2.value - n1.value);
        int currrentFreeNeighboursIndex = 0;

        while (true)
        {
            int currentNodeIndex = freeNeighbours[currrentFreeNeighboursIndex].index;
            route.Add(currentNodeIndex);

            int bestNeighbourFreeNeighboursIndex = -1;

            for (int i = 0; i < freeNeighbours.Length; i++)
            {
                if (!neighbourVisited[currentNodeIndex, freeNeighbours[i].index])
                {
                    bestNeighbourFreeNeighboursIndex = i;
                    break;
                }
            }

            if (bestNeighbourFreeNeighboursIndex == -1)
            {
                break;
            }
            else
            {
                int bestNodeIndex = freeNeighbours[bestNeighbourFreeNeighboursIndex].index;

                neighbourVisited[currentNodeIndex, bestNodeIndex] = true;
                neighbourVisited[bestNodeIndex, currentNodeIndex] = true;


                if (currrentFreeNeighboursIndex > bestNeighbourFreeNeighboursIndex)
                {
                    freeNeighbours[currrentFreeNeighboursIndex].value--;
                    ShiftDown(ref currrentFreeNeighboursIndex, ref freeNeighbours);

                    freeNeighbours[bestNeighbourFreeNeighboursIndex].value--;
                    ShiftDown(ref bestNeighbourFreeNeighboursIndex, ref freeNeighbours);
                } 
                else
                {
                    freeNeighbours[bestNeighbourFreeNeighboursIndex].value--;
                    ShiftDown(ref bestNeighbourFreeNeighboursIndex, ref freeNeighbours);

                    freeNeighbours[currrentFreeNeighboursIndex].value--;
                    ShiftDown(ref currrentFreeNeighboursIndex, ref freeNeighbours);
                }

                currrentFreeNeighboursIndex = bestNeighbourFreeNeighboursIndex;
            }
        }
    }

    public static void ShiftDown(ref int index, ref (int index, int value)[] array)
    {
        while (index + 1 < array.Length && array[index].value < array[index + 1].value)
        {
            (int, int) tmp = array[index];
            array[index] = array[index + 1];
            array[index + 1] = tmp;
            index++;
        }
    }

    public static void FindRouteO(List<int> route, bool[,] neighbourVisited, int[] freeNeighbours)
    {
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
    }

    public static void FindRouteS(int points, List<int> route, bool[,] neighbourVisited, int[] freeNeighbours, int[] pointsWithNNeighbours)
    {
        int currentNode = Array.IndexOf(freeNeighbours, freeNeighbours.Max());
        int currentPossibleMaxNeighbours = points - 1;

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

                    if (maxFreeNeighbours == currentPossibleMaxNeighbours)
                    {
                        break;
                    }
                }
            }

            if (bestNeighbourIndex == -1)
            {
                break;
            }
            else
            {
                pointsWithNNeighbours[freeNeighbours[currentNode]]--;
                pointsWithNNeighbours[freeNeighbours[bestNeighbourIndex]]--;

                freeNeighbours[currentNode]--;
                freeNeighbours[bestNeighbourIndex]--;

                pointsWithNNeighbours[freeNeighbours[currentNode]]++;
                pointsWithNNeighbours[freeNeighbours[bestNeighbourIndex]]++;

                while (pointsWithNNeighbours[currentPossibleMaxNeighbours] == 0)
                {
                    currentPossibleMaxNeighbours--;
                }

                neighbourVisited[currentNode, bestNeighbourIndex] = true;
                neighbourVisited[bestNeighbourIndex, currentNode] = true;

                currentNode = bestNeighbourIndex;
            }
        }
    }

    public static void Main(String[] args)
    {
        // int points = 10000;

        double sum = 0;
        int cnt = 0;

        for (int points = 2; points < 100; points++)
        {
        // false => neighbour is unvisited; true => the neighbour is visited
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

        Stopwatch sw = new Stopwatch();
        sw.Start();
        while (freeNeighbours.Any(f => f != 0))
        {
            // FindRouteO(route, neighbourVisited, freeNeighbours);
            // FindRouteS(points, route, neighbourVisited, freeNeighbours, pointsWithNNeighbours);
            Test.FindRouteLL(points, route, neighbourVisited, freeNeighbours, pointsPerNeighbourCount);
        }
        sw.Stop();
            if ((((points - 1) * (points)) / 2) / ((double)route.Count - 1) >= 1)
            {
                Console.WriteLine($"Points: {points}");
                Console.WriteLine($"Time: {sw.ElapsedMilliseconds}ms");
                Console.WriteLine($"Route Length: {route.Count}");
                //Console.WriteLine(route.Select(x => x.ToString()).Aggregate((n1, n2) => $"{n1}->{n2}"));
                Console.WriteLine($"That is {((((points - 1) * (points)) / 2) / ((double)route.Count - 1)) * 100:f2}% of the optimum");
            }
            sum += (((points - 1) * (points))  / 2 ) / ((double)route.Count - 1);
            cnt++;
        }

        Console.WriteLine($"Avg: {sum / cnt * 100}%");
    }
}
/*int points = 2000;

// false => neighbour is unvisited; true => the neighbour is visited
bool[,] neighbourVisited = new bool[points, points];

for (int i = 0; i < neighbourVisited.GetLength(0); i++)
{
    neighbourVisited[i, i] = true;
}

// false => neighbour is unvisited; true => the neighbour is visited
bool[,] newNeighbourVisited = new bool[points, points];

for (int i = 0; i < newNeighbourVisited.GetLength(0); i++)
{
    newNeighbourVisited[i, i] = true;
}

(int index, int value)[] freeNeighbours = new (int index, int value)[points];
for (int i = 0; i < points; i++) 
{
    freeNeighbours[i] = (i, points - 1);
}

int[] oldFreeNeighbours = Enumerable.Repeat(points - 1, points).ToArray();

List<int> route = new List<int>((points - 1) * (points) / 2);
List<int> newRoute = new List<int>((points - 1) * (points) / 2);

Stopwatch sw = new Stopwatch();
sw.Start();
while (oldFreeNeighbours.Any(f => f != 0))
{
    FindRouteO(route, neighbourVisited, oldFreeNeighbours);
}
sw.Stop();
Console.WriteLine($"Old: {sw.ElapsedMilliseconds}ms");
Console.WriteLine($"Route Length: {route.Count}");
sw.Restart();
while (freeNeighbours.Any(f => f.value != 0))
{
    FindRoute(points, newRoute, newNeighbourVisited, freeNeighbours);
}
sw.Stop();
Console.WriteLine($"New: {sw.ElapsedMilliseconds}ms");
Console.WriteLine($"Route Length: {newRoute.Count}");

//Console.WriteLine(route.Select(x => x.ToString()).Aggregate((n1, n2) => $"{n1}->{n2}"));
//Console.WriteLine(freeNeighbours.Where(n => n.value != 0).Count());*/