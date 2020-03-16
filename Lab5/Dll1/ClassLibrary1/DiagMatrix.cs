using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace ClassLibrary1
{
    public class DiagMatrix
    {

        int order;
        List<double> data;
        public DiagMatrix(int order_in = 1)
        {
            order = order_in;
            data = new List<double>();
            for (int i = 0; i < order; ++i)
            {
                data.Add(0.0);
            }
        }

        public DiagMatrix(int order_in, List<double> data_in)
        {
            order = order_in;
            data = data_in;
            for (int i = 0; i < order; ++i)
            {
                data.Add(data_in[i]);
            }
        }

        public DiagMatrix(DiagMatrix other)
        {
            order = other.order;
            data = other.data;
        }

        public void printMatrix(StreamWriter sw)
        {
            try
            {
                for (int i = 0; i < order; ++i)
                {
                    for (int j = 0; j < order; ++j)
                    {
                        if (i != j) sw.Write("0 ");
                        else
                        {
                            sw.Write(data[i]);
                            sw.Write(" ");
                        }
                    }
                    sw.WriteLine();
                }
                sw.WriteLine();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DiagMatrix operator +(DiagMatrix d1, DiagMatrix d2)
        {
            try
            {
                if (d1.order != d2.order) throw new Exception("Addition error: different sizes of matrices");
                int order = d1.order;
                DiagMatrix res = new DiagMatrix(order);
                for (int i = 0; i < order; ++i)
                {
                    res.data[i] = d1.data[i] + d2.data[i];
                }
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DiagMatrix operator -(DiagMatrix d1, DiagMatrix d2)
        {
            try
            {
                if (d1.order != d2.order) throw new Exception("Subtraction error: different sizes of matrices");
                int order = d1.order;
                DiagMatrix res = new DiagMatrix(order);
                for (int i = 0; i < order; ++i)
                {
                    res.data[i] = d1.data[i] - d2.data[i];
                }
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DiagMatrix operator *(DiagMatrix d1, DiagMatrix d2)
        {
            try
            {
                if (d1.order != d2.order) throw new Exception("Multiplication error: different sizes of matrices");
                int order = d1.order;
                DiagMatrix res = new DiagMatrix(order);
                for (int i = 0; i < order; ++i)
                {
                    res.data[i] = d1.data[i] * d2.data[i];
                }
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<double> operator *(DiagMatrix d, List<double> li)
        {
            try
            {
                int size = li.Count();
                if (d.order != size) throw new Exception("Multiplication error: different sizes of matrix and vector");

                List<double> res = new List<double>();
                for (int i = 0; i < size; ++i)
                {
                    res.Add(d.data[i] * li[i]);
                }
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DiagMatrix inverse(DiagMatrix arg)
        {
            try
            {
                DiagMatrix res = new DiagMatrix(arg.order);
                for (int i = 0; i < res.order; ++i)
                {
                    if (arg.data[i] == 0) throw new Exception("Inversion error: inverse matrix does not exist");
                    res.data[i] = 1.0 / arg.data[i];
                }
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}