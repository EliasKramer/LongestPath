using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    public class PathResult
    {
        public int NodeCount { get; set; }
        public List<int> Path { get; set; }
        public int NodesVisited => Path.Count;
        public int ConnectionCount => Path.Count - 1;
    }
}
