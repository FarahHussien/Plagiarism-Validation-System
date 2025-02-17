using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;

namespace Algo_Project
{
    class Program
    {
        static Dictionary<string, List<Tuple<string, float>>> graph = new Dictionary<string, List<Tuple<string, float>>>(); // Declare graph outside of Main
        static List<Tuple<string, float, string, float, float, string, string>> sets = ReadFiles.solved;//mst
        static List<List<Tuple<string, float, string, float, float, string, string>>> new_sets = new List<List<Tuple<string, float, string, float, float, string, string>>>();
        static List<List<Tuple<string, float, string, float, float, string, string>>> final_sets = new List<List<Tuple<string, float, string, float, float, string, string>>>();
        static List<List<string>> vertex_set = new List<List<string>>();
        static void Main(string[] args)
        {
            // Paths of excel files inputs
            //string sample1 = "D:\\Test Cases\\Sample\\1-Input.xlsx";
            //string sample2 = "D:\\Test Cases\\Sample\\2-Input.xlsx";

            
            string easy_1 = "D:/learnning/level3/second/algorithms/Project/Test Cases/Complete/Easy/1-Input.xlsx";
            string easy_2 = "D:/learnning/level3/second/algorithms/Project/Test Cases/Complete/Easy/1-Input.xlsx";
            
            string med_1 = "D:/learnning/level3/second/algorithms/Project/Test Cases/Complete/Medium/1-Input.xlsx";
            string med_2 = "D:/learnning/level3/second/algorithms/Project/Test Cases/Complete/Medium/1-Input.xlsx";
            
            string hard_1 = "D:/learnning/level3/second/algorithms/Project/Test Cases/Complete/Hard/1-Input.xlsx";
            string hard_2 = "D:/learnning/level3/second/algorithms/Project/Test Cases/Complete/Hard/2-Input.xlsx";

            Tuple<string, string, float, float, float>[] edges = null;

            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("===== Main Menu =====");
                Console.WriteLine("1. Easy - Input 1");
                Console.WriteLine("2. Easy - Input 2");
                Console.WriteLine("3. Medium - Input 1");
                Console.WriteLine("4. Medium - Input 2");
                Console.WriteLine("5. Hard - Input 1");
                Console.WriteLine("6. Hard - Input 2");
                Console.WriteLine("7. Exit");
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        // Read data from Excel and store it in an array of tuples
                        edges = ReadFiles.ReadExcelFile(easy_1);
                        graph = Graph.BuildGraph(edges);
                        PrintStatistics();
                        break;

                    case "2":
                        edges = ReadFiles.ReadExcelFile(easy_2);
                        graph = Graph.BuildGraph(edges);
                        PrintStatistics();
                        break;

                    case "3":
                        edges = ReadFiles.ReadExcelFile(med_1);
                        graph = Graph.BuildGraph(edges);
                        PrintStatistics();
                        break;

                    case "4":
                        edges = ReadFiles.ReadExcelFile(med_2);
                        graph = Graph.BuildGraph(edges);
                        PrintStatistics();
                        break;

                    case "5":
                        edges = ReadFiles.ReadExcelFile(hard_1);
                        graph = Graph.BuildGraph(edges);
                        PrintStatistics();
                        break;

                    case "6":
                        edges = ReadFiles.ReadExcelFile(hard_2);
                        graph = Graph.BuildGraph(edges);
                        PrintStatistics();
                        break;

                    case "7":
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("Invalid choice, please try again.");
                        break;
                }

                if (!exit)
                {
                    Console.WriteLine("\nPress any key to return to the menu...");
                    Console.ReadKey();
                }
            }

        }

        static void PrintStatistics()
        {
            GraphComponents gc = new GraphComponents(graph);
            var watch = System.Diagnostics.Stopwatch.StartNew();
            gc.FindComponents();
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("Time stats (MS): "+elapsedMs);

            gc.splitting_graph(sets, new_sets, vertex_set);
            var watch_mst = System.Diagnostics.Stopwatch.StartNew();
            gc.kruskal(new_sets, final_sets, vertex_set);
            watch_mst.Stop();
            elapsedMs = watch_mst.ElapsedMilliseconds;
            Console.WriteLine("Kruskal time(MS): " + elapsedMs);
        }
    }
}
