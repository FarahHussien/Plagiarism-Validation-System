using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo_Project
{
    internal class Graph
    {
        public static Dictionary<string, List<Tuple<string, float>>> BuildGraph(Tuple<string, string, float, float, float>[] edges)
        {
            Dictionary<string, List<Tuple<string, float>>> graph = new Dictionary<string, List<Tuple<string, float>>>();

            foreach (var edge in edges)
            {
                // item1 = file1
                if (!graph.TryGetValue(edge.Item1, out var neighbors1))
                {
                    neighbors1 = new List<Tuple<string, float>>();
                    graph[edge.Item1] = neighbors1;
                }

                // item2 = file2
                if (!graph.TryGetValue(edge.Item2, out var neighbors2))
                {
                    neighbors2 = new List<Tuple<string, float>>();
                    graph[edge.Item2] = neighbors2;
                }
                // item3=simfile1
                neighbors1.Add(new Tuple<string, float>(edge.Item2, edge.Item3));
                // item4=simfile2
                neighbors2.Add(new Tuple<string, float>(edge.Item1, edge.Item4)); // for undirected graph
            }

            return graph;
        }
    }
}
