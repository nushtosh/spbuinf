using Microsoft.VisualStudio.TestTools.UnitTesting;
using matrixint;
using System;
using System.Linq;

namespace matrixinttest
{

    [TestClass]
    public class UnitTest1
    {
        private const string FilesPath = "C:\\Users\\olya\\source\\repos\\matrixint\\matrixint\\test_data";
        private const string FilesPathRes = "C:\\Users\\olya\\source\\repos\\matrixint\\matrixint\\test_res";
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
            rw.WriteFile(tmp3, $"{FilesPathRes}\\resMatrix.txt");
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
