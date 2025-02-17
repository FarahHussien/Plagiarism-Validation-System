using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo_Project
{

    internal class GraphComponents
    {
        private Dictionary<string, List<Tuple<string, float>>> graph;
        private List<List<string>> components;
        private HashSet<string> visited;

        public GraphComponents(Dictionary<string, List<Tuple<string, float>>> graph)
        {
            this.graph = graph;
            components = new List<List<string>>();
            visited = new HashSet<string>();
        }

        private void DFS(string vertex, List<string> component)
        {
            visited.Add(vertex);
            component.Add(vertex);

            foreach (var neighbor in graph[vertex])
            {
                if (!visited.Contains(neighbor.Item1))
                {
                    DFS(neighbor.Item1, component);
                }
            }
        }

        public void FindComponents()
        {
            visited.Clear();
            components.Clear();

            foreach (var vertex in graph.Keys)
            {
                if (!visited.Contains(vertex))
                {
                    List<string> component = new List<string>();
                    DFS(vertex, component);
                    components.Add(component);
                }
            }
        }

        public void PrintComponents()
        {
            int count = 1;
            foreach (var component in components)
            {
                Console.WriteLine($"Group {count}: " + string.Join(", ", component));
                count++;
            }
        }
    }
}


