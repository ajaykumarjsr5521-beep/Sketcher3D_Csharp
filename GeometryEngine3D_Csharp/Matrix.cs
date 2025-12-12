using System;

namespace GeometryEngine3D_Csharp
{
    public class Matrix
    {
        private readonly int _rows, _cols;
        private readonly double[,] _data;

        public Matrix(int rows = 4, int cols = 4)
        {
            _rows = rows; _cols = cols; _data = new double[rows, cols];
        }

        public double this[int r, int c]
        {
            get { return _data[r, c]; }
            set { _data[r, c] = value; }
        }

        public static Matrix GetIdentity()
        {
            Matrix m = new Matrix(4, 4);
            for (int i = 0; i < 4; i++) m[i, i] = 1.0;
            return m;
        }

        public static Matrix operator +(Matrix a, Matrix b)
        {
            if (a._rows != b._rows || a._cols != b._cols)
                throw new InvalidOperationException("Matrix add: size mismatch");

            Matrix r = new Matrix(a._rows, a._cols);
            for (int i = 0; i < a._rows; i++)
                for (int j = 0; j < a._cols; j++)
                    r[i, j] = a[i, j] + b[i, j];
            return r;
        }

        public static Matrix operator *(Matrix a, Matrix b)
        {
            if (a._cols != b._rows)
                throw new InvalidOperationException("Matrix mul: size mismatch");

            Matrix r = new Matrix(a._rows, b._cols);
            for (int i = 0; i < a._rows; i++)
            {
                for (int j = 0; j < b._cols; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < a._cols; k++) sum += a[i, k] * b[k, j];
                    r[i, j] = sum;
                }
            }
            return r;
        }

        public static Matrix GetTranslationMatrix(double tx, double ty, double tz)
        {
            Matrix m = GetIdentity();
            m[0, 3] = tx; m[1, 3] = ty; m[2, 3] = tz;
            return m;
        }

        public static Matrix GetScalingMatrix(double sx, double sy, double sz)
        {
            Matrix m = GetIdentity();
            m[0, 0] = sx; m[1, 1] = sy; m[2, 2] = sz;
            return m;
        }

        public static Matrix GetRotationXMatrix(double degrees)
        {
            Matrix m = GetIdentity();
            double r = degrees * MathConstants.PI / 180.0;
            double c = Math.Cos(r), s = Math.Sin(r);
            m[1, 1] = c; m[1, 2] = -s;
            m[2, 1] = s; m[2, 2] = c;
            return m;
        }

        public static Matrix GetRotationYMatrix(double degrees)
        {
            Matrix m = GetIdentity();
            double r = degrees * MathConstants.PI / 180.0;
            double c = Math.Cos(r), s = Math.Sin(r);
            m[0, 0] = c; m[0, 2] = s;
            m[2, 0] = -s; m[2, 2] = c;
            return m;
        }

        public static Matrix GetRotationZMatrix(double degrees)
        {
            Matrix m = GetIdentity();
            double r = degrees * MathConstants.PI / 180.0;
            double c = Math.Cos(r), s = Math.Sin(r);
            m[0, 0] = c; m[0, 1] = -s;
            m[1, 0] = s; m[1, 1] = c;
            return m;
        }
    }
}
