using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using OfficeOpenXml;

namespace Algo_Project
{
    internal class ReadFiles
    {
        public static List<Tuple<string, float, string, float, float, string, string>> solved = new List<Tuple<string, float, string, float, float, string, string>>();
        public static Tuple<string, string, float, float, float>[] ReadExcelFile(string excelFilePath)
        {
            List<Tuple<string, string, float, float, float>> edgesList = new List<Tuple<string, string, float, float, float>>();

            using (ExcelPackage package = new ExcelPackage(new System.IO.FileInfo(excelFilePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Assuming data is in the first worksheet

                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    string file1Path = worksheet.Cells[row, 1].Value.ToString();
                    string file2Path = worksheet.Cells[row, 2].Value.ToString();

                    // Extracting the average similarity percentage from file1Path
                    float file1Similarity = ExtractSimilarity(file1Path);

                    // Extracting the average similarity percentage from file2Path
                    float file2Similarity = ExtractSimilarity(file2Path);

                    string name1 = file1Path;
                    string name2 = file2Path;

                    file1Path = ExtractFileName(file1Path);
                    file2Path = ExtractFileName(file2Path);

                    float linesMatched;
                    if (!float.TryParse(worksheet.Cells[row, 3].Value.ToString(), out linesMatched))
                    {
                        // Handle conversion failure gracefully
                        Console.WriteLine($"Error converting value at row {row}, column 3 to float.");
                        // You can choose to skip this row or assign a default value to linesMatched
                        continue;
                    }

                    edgesList.Add(Tuple.Create(file1Path, file2Path, file1Similarity, file2Similarity, linesMatched));
                    solved.Add(Tuple.Create(file1Path, file1Similarity, file2Path, file2Similarity, linesMatched, name1, name2));
                }
            }
            return edgesList.ToArray();
        }

        private static float ExtractSimilarity(string filePath)
        {
            string sub = "";
            int endIndex = filePath.Length - 3;

            // Traverse the file path string from the end until a non-digit character is encountered
            while (endIndex >= 0 && char.IsDigit(filePath[endIndex]))
            {
                // Append the current character to the substring
                sub = filePath[endIndex] + sub;
                endIndex--;
            }

            // Parse the extracted substring to a float value
            if (float.TryParse(sub, out float similarity))
            {
                return similarity;
            }
            else
            {
                // Handle parsing failure gracefully (return 0.0f or any default value)
                return 0.0f;
            }
        }

        // Extract name of file
        private static string ExtractFileName(string path)
        {
            string sub = "";
            int endIndex = path.Length - 7;

            while (endIndex >= 0)
            {
                if (!char.IsDigit(path[endIndex]))
                {
                    endIndex--;
                    continue;
                }
                else
                {
                    sub = path[endIndex] + sub;
                    endIndex--;
                }
            }
            return sub;
        }

    }
}