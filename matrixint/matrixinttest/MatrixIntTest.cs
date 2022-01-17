using Microsoft.VisualStudio.TestTools.UnitTesting;
using matrixint;
using System;
using System.Linq;
using System.IO;

namespace matrixinttest
{

    [TestClass]
    public class MatrixIntTest
    {
        private static readonly string FilesPath = $"{Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName}\\test_data";
        private static readonly string FilesPathRes = $"{Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName}\\test_res";

        [TestMethod]
        public void TestMult()
        {

            var rw = new ReaderWriter<int>();
            var m1 = rw.ReadFile($"{FilesPath}\\matrix1.txt");
            var m2 = rw.ReadFile($"{FilesPath}\\matrix2.txt");
            var fact= rw.ReadFile($"{FilesPathRes}\\TestAct.txt");
            var tmp1 = m1.ToArray();
            var tmp2 = m2.ToArray();
            var tmpact = fact.ToArray();

            var mm1 = new Matrix(tmp1);
            var mm2 = new Matrix(tmp2);
            var mact = new Matrix(tmpact);

            var mm3 = mm1.Mul(mm2);

            var tmp3 = mm3.Data.ToArray();
            Assert.IsTrue(mm3.Equals(mact));
        }

        [TestMethod]
        public void TestMultExc()
        {
            
            var rw = new ReaderWriter<int>();
            var m1 = rw.ReadFile($"{FilesPath}\\matrix1.txt");
            var m2 = rw.ReadFile($"{FilesPath}\\matrix3.txt");

            var tmp1 = m1.ToArray();
            var tmp2 = m2.ToArray();

            var mm1 = new Matrix(tmp1);
            var mm2 = new Matrix(tmp2);

            Assert.ThrowsException<Exception>(delegate {Matrix mm3 = mm1.Mul(mm2); });
        }
    }
}
