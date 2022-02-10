using System;
using System.IO;
using System.Linq;
namespace Matrix
{
    internal class Program
    {
        private static readonly string FilesPath = $"{Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName}\\test_data";

        public static void Main(string[] args)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            Console.WriteLine("state the names of matrix files: ");
            string path1 = Console.ReadLine();
            string path2 = Console.ReadLine();

            var rw = new ReaderWriter<double>();
            var m1 = rw.ReadFile($"{FilesPath}\\{path1}");
            var m2 = rw.ReadFile($"{FilesPath}\\{path2}");

            var tmp1 = m1.ToArray();
            var tmp2 = m2.ToArray();

            var mm1 = new Matrix<double>(tmp1);
            var mm2 = new Matrix<double>(tmp2);

            Console.WriteLine("First matrix:");
            Console.WriteLine(mm1);
            Console.WriteLine("Second matrix:");
            Console.WriteLine(mm2);

            var mm3 = mm1.Mul(mm2);

            Console.WriteLine("Result matrix:");
            Console.WriteLine(mm3);

            rw.WriteFile(mm3.Data, $"{FilesPath}\\resMatrix.txt");
        }
    }
}