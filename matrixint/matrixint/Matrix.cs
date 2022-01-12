using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace matrixint
{
    public class Matrix
    {
        public readonly int[][] Data;

        public Matrix(int[][] data)
        {
            Data = data;
        }

        public Matrix Mul(Matrix mx)
        {
            var m1 = Data;
            var m2 = mx.Data;

            if (m1[0].Length != m2.Length)
            {
                throw new Exception("Bad input matrices");
            }

            var resIMax = m1.Length;
            var resJMax = m2[0].Length;

            var res = new int[resIMax][];

            for (var i = 0; i < resIMax; i++)
            {
                res[i] = new int[resJMax];
                for (var j = 0; j < resJMax; j++)
                {
                    res[i][j] = CalcResValue(m1, m2, i, j);
                }
            }

            return new Matrix(res);
        }

        public override string ToString()
        {
            return Data.Select(
                    ints =>
                        ints.Select(
                            i => i.ToString()
                        ).Aggregate((acc, cur) => $"{acc}, {cur}"))
                .Aggregate((acc, cur) => $"{acc}{Environment.NewLine}{cur}") + Environment.NewLine;
        }

        private int CalcResValue(int[][] a, int[][] b, int i, int j)
        {
            
            var res = new int();
            for (var k = 0; k < b.Length; k++)
            {
                res = res+(a[i][k]*b[k][j]);
            }

            return res;
        }
        public override bool Equals(object obj)
        {
            Matrix objToCompare = obj as Matrix;
            if (objToCompare.Data.Length != this.Data.Length || objToCompare.Data[0].Length != this.Data[0].Length)
                return false;
            for (int i = 0; i < objToCompare.Data.Length; i++)
                for (int j = 0; j < objToCompare.Data[0].Length; j++)
                    if (this.Data[i][j] != objToCompare.Data[i][j])
                        return false;
            return true;
        }
    }
}
