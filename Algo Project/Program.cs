using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;


namespace Algo_Project
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("hi");

            // Path to the Excel file

            string excelFilePath = "C:\\Users\\Farah Hussein\\OneDrive - Faculty of Computer and Information Sciences (Ain Shams University)\\Desktop\\Algo Project\\Algo Project\\input_files\\2-Input.xlsx";

            // Read data from Excel and store it in an array of tuples
            Tuple<string, string, float, float, float>[] edges = ReadFiles.ReadExcelFile(excelFilePath);

            // Print the data from the array of tuples
            foreach (var edge in edges)
            {
                Console.WriteLine($"File 1: {edge.Item1}, File 2: {edge.Item2},F1Similarity:{edge.Item3},F2Similarity:{edge.Item4}, Lines Matched: {edge.Item5}");
            }

            Dictionary<string, List<Tuple<string, float>>> graph = Graph.BuildGraph(edges);

            // Print the graph
            Console.WriteLine("\nGraph:");
            foreach (var node in graph)
            {
                Console.Write($"vertex:{node.Key}: ");
                foreach (var neighbor in node.Value)
                {
                    Console.Write($"({neighbor.Item1}, {neighbor.Item2}), ");
                    Console.WriteLine("\n");
                }
                Console.WriteLine();
            }



            GraphComponents gc = new GraphComponents(graph);
            gc.FindComponents();
            gc.PrintComponents();
        }
    }
}
