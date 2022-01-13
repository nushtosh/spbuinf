using System;
using System.Linq;
using System.IO;

namespace matrixint
{
    internal class Program
    {
        private static readonly string FilesPath = $"{Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName}\\test_data";
        private static readonly string FilesPathRes = $"{Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName}\\test_res";

        public static void Main(string[] args)
        {
            Console.WriteLine("state the names of matrix files: ");
            string path1=Console.ReadLine();
            string path2 = Console.ReadLine();

            var rw = new ReaderWriter<int>();
            var m1 = rw.ReadFile($"{FilesPath}\\{path1}");
            var m2 = rw.ReadFile($"{FilesPath}\\{path2}");

            var tmp1 = m1.ToArray();
            var tmp2 = m2.ToArray();

            var mm1 = new Matrix(tmp1);
            var mm2 = new Matrix(tmp2);
            Console.WriteLine("first matrix");
            Console.WriteLine(mm1);
            Console.WriteLine("second matrix");
            Console.WriteLine(mm2);
            var mm3 = mm1.Mul(mm2);
            Console.WriteLine("result");
            Console.WriteLine(mm3);
            var tmp3 = mm3.Data.ToArray();
            rw.WriteFile(tmp3, $"{FilesPathRes}\\resMatrix.txt");

        }
    }
}
