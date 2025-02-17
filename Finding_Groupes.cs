using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.IO;

namespace Algo_Project
{

    internal class GraphComponents
    {
        private Dictionary<string, List<Tuple<string, float>>> graph;
        private List<List<string>> components;
        private HashSet<string> visited;
        private List<Tuple<double, int>> avg_simi;
        private List<List<int>> components_int;
        private List<List<Tuple<float, int>>> Line_Matches;

        public GraphComponents(Dictionary<string, List<Tuple<string, float>>> graph)
        {
            this.graph = graph;
            components = new List<List<string>>();
            visited = new HashSet<string>();
            avg_simi = new List<Tuple<double, int>>();
            components_int = new List<List<int>>();
            Line_Matches = new List<List<Tuple<float, int>>>();

        }

        // Refine each group, by finding its max spanning tree
        public void splitting_graph(List<Tuple<string, float, string, float, float, string, string>> sets, List<List<Tuple<string, float, string, float, float, string, string>>> new_sets, List<List<string>> vertex_set)
        {
            // list for each group => contains its vertices  => put in new_sets
            // make a set for each vertex => put in vertex_set
            int i = 0, j = 0;
            foreach (var group in components)
            {
                new_sets.Add(new List<Tuple<string, float, string, float, float, string, string>>());
                foreach (var items in group)
                {
                    vertex_set.Add(new List<string>());
                    vertex_set[j].Add(items);
                    j++;

                    foreach (var edge in sets)
                    {
                        if ((items == edge.Item1))
                        {
                            new_sets[i].Add(Tuple.Create(edge.Item1, edge.Item2, edge.Item3, edge.Item4, edge.Item5, edge.Item6, edge.Item7));
                            //sets.RemoveAt(0);

                        }
                    }
                }
                i++;
            }

        }
        public void kruskal(List<List<Tuple<string, float, string, float, float, string, string>>> groups, List<List<Tuple<string, float, string, float, float, string, string>>> new_sets, List<List<string>> vertex_set)
        {
            //avg_simi.Sort((x, y) => y.Item1.CompareTo(x.Item1));
            //vertex_set.Add(new List<string>());
            string path = "D:/learnning/level3/second/algorithms/Project/Algo Project [3]/Algo Project/MST.xlsx";

            using (ExcelPackage package = new ExcelPackage())
            {
                // add a new worksheet to the empty workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("MST");

                //Add the headers
                worksheet.Cells[1, 1].Value = "File 1";
                worksheet.Cells[1, 2].Value = "File 2";
                worksheet.Cells[1, 3].Value = "Line Matches";

                int i = 0, r = 0, group_index = 0, line_match_index = 0, count = 0, first = 0, second = 0;
                foreach (var items in avg_simi)//sorting by avarage similarity
                {
                    group_index = items.Item2 - 1;
                    quick_sort(groups[group_index], 0, (groups[group_index].Count - 1));//sort each group by vertex similarity

                    int index = 0;
                    new_sets.Add(new List<Tuple<string, float, string, float, float, string, string>>());
                    Line_Matches.Add(new List<Tuple<float, int>>());

                    count = groups[group_index].Count;

                    for (int ind = count - 1; ind >= 0; ind--)
                    {

                        first = find_set(vertex_set, groups[group_index][ind].Item1);
                        second = find_set(vertex_set, groups[group_index][ind].Item3);

                        if (first != second)
                        {
                            new_sets[i].Add(Tuple.Create(groups[group_index][ind].Item1, groups[group_index][ind].Item2, groups[group_index][ind].Item3, groups[group_index][ind].Item4, groups[group_index][ind].Item5, groups[group_index][ind].Item6, groups[group_index][ind].Item7));
                            Line_Matches[i].Add(Tuple.Create(groups[group_index][ind].Item5, index));
                            vertex_set[first] = vertex_set[first].Union(vertex_set[second]).ToList();

                            vertex_set.RemoveAt(second);
                            index++;

                        }

                    }

                    Line_Matches[i].Sort((x, y) => y.Item1.CompareTo(x.Item1));

                    //Console.WriteLine("______________________________________________________\n"+i+"\n");
                    foreach (var item in Line_Matches[i])
                    {
                        line_match_index = item.Item2;
                        r++;
                        //Console.WriteLine(r + "_   " + new_sets[i][line_match_index].Item1 + "/ (" + new_sets[i][line_match_index].Item2 + "%)  |  " + new_sets[i][line_match_index].Item3 + "/ (" + new_sets[i][line_match_index].Item4 + "%) | Line Matches: " + new_sets[i][line_match_index].Item5);

                        worksheet.Cells[r + 1, 1].Value = new_sets[i][line_match_index].Item6;
                        worksheet.Cells[r + 1, 2].Value = new_sets[i][line_match_index].Item7;
                        worksheet.Cells[r + 1, 3].Value = new_sets[i][line_match_index].Item5;
                    }
                    i++;
                }
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    package.SaveAs(stream);
                }
                //Console.WriteLine("______________________________________________________");
            }

        }
        public int find_set(List<List<string>> vertex_set, string vertex)
        {
            int j = 0;
            foreach (var v in vertex_set)
            {
                if (v.Contains(vertex))
                {
                    return j;
                }
                j++;
            }
            return -1;
        }
        public void quick_sort(List<Tuple<string, float, string, float, float, string, string>> solve, int strart, int end)
        {
            int mid = 0;
            if (strart < end)
            {
                mid = partition(solve, strart, end);

                quick_sort(solve, strart, mid - 1);
                quick_sort(solve, mid + 1, end);
            }
        }
        public int partition(List<Tuple<string, float, string, float, float, string, string>> solve, int start, int end)
        {
            Tuple<string, float, string, float, float, string, string> temp;

            Tuple<string, float, string, float, float, string, string> pivotValue = solve[start];

            int leftmark = start + 1;
            int rightmark = end;
            bool done = false;
            while (!done)
            {
                while ((leftmark <= rightmark) && (Math.Max(solve[leftmark].Item2, solve[leftmark].Item4) <= Math.Max(pivotValue.Item2, pivotValue.Item4)))
                {
                    if (Math.Max(solve[leftmark].Item2, solve[leftmark].Item4) == Math.Max(pivotValue.Item2, pivotValue.Item4))
                    {
                        if (pivotValue.Item5 < solve[leftmark].Item5)
                        {
                            break;
                        }
                    }

                    leftmark = leftmark + 1;

                }
                while ((Math.Max(solve[rightmark].Item2, solve[rightmark].Item4) >= Math.Max(pivotValue.Item2, pivotValue.Item4)) && (rightmark >= leftmark))
                {
                    if (Math.Max(solve[rightmark].Item2, solve[rightmark].Item4) == Math.Max(pivotValue.Item2, pivotValue.Item4))
                    {
                        if (pivotValue.Item5 > solve[rightmark].Item5)
                        {
                            break;
                        }
                    }
                    rightmark = rightmark - 1;

                }
                if (rightmark < leftmark)
                    done = true;
                else
                {
                    temp = solve[leftmark];
                    solve[leftmark] = solve[rightmark];
                    solve[rightmark] = temp;
                }
            }
            temp = solve[start];
            solve[start] = solve[rightmark];
            solve[rightmark] = temp;

            return rightmark;
        }


        // Identify the groups & Calculate the statistics
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
            PrintStatistics();
        }
        
        private double RoundToNearest(decimal value, int decimals)
        {

            decimal factor = (decimal)Math.Pow(10, decimals);
            decimal roundedValue = Math.Round(value * factor) / factor;
            return (double)roundedValue;
        }
       
        private void PrintStatistics()
        {
            int count = 1;
            foreach (var component in components)
            {
                List<string> sortedComponent = component.OrderBy(GetNumericPart).ToList();
                List<int> component_ = new List<int>();
                float denominator = 0; // Total count of edges in the component (groups)
                float nominator = 0; // Sum of weights in the component
                foreach (var vertexStr in sortedComponent)
                {
                    int vertex = int.Parse(GetNumericPart(vertexStr));
                    component_.Add(vertex);


                    string vertexKey = vertex.ToString(); // Convert the vertex to a string

                    denominator += graph[vertexKey].Count;
                    nominator += graph[vertexKey].Sum(edge => edge.Item2);
                    //component.Remove(vertexStr);

                }

                component_.Sort();
                components_int.Add(component_);
                double res = denominator > 0 ? nominator / denominator : 0;
                avg_simi.Add(Tuple.Create(res, count));

                count++;

            }

            // Sort the components by average similarity in descending order
            avg_simi.Sort((x, y) => y.Item1.CompareTo(x.Item1));

        }

        private string GetNumericPart(string str)
        {
            return new string(str.Where(char.IsDigit).ToArray());
        }

        // Refine each group, by finding its max spanning tree

    }
}