using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;

namespace Algo_Project
{
    internal class ReadFiles
    {
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

                    /*string file1 = file1Path;
                    string id1="";
                    string id2="";
                    string file2 = file2Path;
                    int i = 10;

                    while (file1[i]!='/')
                    {
                        id1 += file1[i];
                        i++;
                    }
                    while (file2[i]!='/')
                    {
                        id2 += file2[i];
                        i++;
                    }

                    Console.WriteLine("id1: "+ id1+" id2: "+ id2);*/

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
                }
            }
            return edgesList.ToArray();
        }

        private static float ExtractSimilarity(string filePath)
        {
            int openingParenIndex = filePath.IndexOf('(');
            int closingParenIndex = filePath.IndexOf(')', openingParenIndex);

            // Extract the substring between '(' and ')' (excluding '(' and ')')
            string percentageSubstring = filePath.Substring(openingParenIndex + 1, closingParenIndex - openingParenIndex - 1);

            // Remove any leading or trailing whitespace from the extracted substring
            percentageSubstring = percentageSubstring.Trim();

            // Remove the '%' character from the substring
            percentageSubstring = percentageSubstring.TrimEnd('%');

            return float.Parse(percentageSubstring);
        }

        private static string ExtractFileName(string path)
        {
            int lastSlashIndex = path.LastIndexOf('/'); // Find the index of the last slash
            int secondLastSlashIndex = path.Substring(0, lastSlashIndex).LastIndexOf('/'); // Find the index of the second last slash
            string extractedName = "";
            if (lastSlashIndex != -1 && secondLastSlashIndex != -1)
            {
                extractedName = path.Substring(secondLastSlashIndex + 1, lastSlashIndex - secondLastSlashIndex - 1);
            }
            else
            {
                Console.WriteLine("Invalid path format.");
            }
            return extractedName;
        }
    }
}
