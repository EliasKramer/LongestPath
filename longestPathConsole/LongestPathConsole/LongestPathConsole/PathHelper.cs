using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets
{
    public static class PathHelper
    {
        public static int OptimalConnectionCount(int n)
        {
            return ((n - 1) * n) / 2;
        }
        public class Connection : IEquatable<Connection>
        {
            public int Start { get; set; }
            public int End { get; set; }

            public bool Equals(Connection other)
            {
                return
                    (Start == other.Start && End == other.End) ||
                    (Start == other.End && End == other.Start);
            }

            public override string ToString()
            {
                return "(" + Start + ", " + End + ")";
            }

        }
        public static void PathErrorCheck(List<int> path, int n)
        {
            var list = new List<Connection>();

            for (int i = 1; i < path.Count; i++)
            {
                Connection item = new Connection()
                {
                    Start = path[i - 1],
                    End = path[i]
                };

                if (list.Any((x) => x.Equals(item)))
                {
                   // Debug.LogError($"{n} connection {path[i]} is duplicate");
                }
                if(item.Start == item.End)
                {
                  //  Debug.LogError($"{n} identity path err");
                }

                list.Add(item);
            }

        }
    }
}
