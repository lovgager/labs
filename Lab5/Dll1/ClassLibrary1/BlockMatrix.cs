using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace ClassLibrary1
{
    public class BlockMatrix
    {
        int N; //blocks
        List<DiagMatrix> mainDiag, upDiag, downDiag;
        public BlockMatrix(int blocks = 2, int smallOrder = 1)
        {
            mainDiag = new List<DiagMatrix>();
            upDiag = new List<DiagMatrix>();
            downDiag = new List<DiagMatrix>();
            N = blocks;
            downDiag.Add(null);
            for (int i = 0; i < blocks - 1; ++i)
            {
                mainDiag.Add(new DiagMatrix(smallOrder));
                upDiag.Add(new DiagMatrix(smallOrder));
                downDiag.Add(new DiagMatrix(smallOrder));
            }
            mainDiag.Add(new DiagMatrix(smallOrder));
        }

        public void setMainDiag(List<DiagMatrix> other)
        {
            try
            {
                for (int i = 0; i < N; ++i)
                {
                    mainDiag[i] = other[i];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void setUpDiag(List<DiagMatrix> other)
        {
            try
            {
                for (int i = 0; i < N - 1; ++i)
                {
                    upDiag[i] = other[i];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void setDownDiag(List<DiagMatrix> other)
        {
            try
            {
                for (int i = 1; i < N; ++i)
                {
                    downDiag[i] = other[i];
                }
            } 
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void printBlockMatrix(StreamWriter sw)
        {
            try
            {
                sw.WriteLine("Main diag:");
                for (int i = 0; i < N; ++i)
                {
                    mainDiag[i].printMatrix(sw);
                }
                sw.WriteLine("Up diag:");
                for (int i = 0; i < N - 1; ++i)
                {
                    upDiag[i].printMatrix(sw);
                }
                sw.WriteLine("Down diag:");
                for (int i = 1; i < N; ++i)
                {
                    downDiag[i].printMatrix(sw);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<double> sum(List<double> li1, List<double> li2)
        {
            try
            {
                int size = li1.Count();
                if (size != li2.Count()) throw new Exception("Vectors addition error");
                List<double> res = new List<double>();
                for (int i = 0; i < size; ++i)
                {
                    res.Add(li1[i] + li2[i]);
                }
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<double> diff(List<double> li1, List<double> li2)
        {
            try
            {
                int size = li1.Count();
                if (size != li2.Count()) throw new Exception("Vectors subtraction error");
                List<double> res = new List<double>();
                for (int i = 0; i < size; ++i)
                {
                    res.Add(li1[i] - li2[i]);
                }
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<List<double>> operator *(BlockMatrix b, List<List<double>> v)
        {
            try
            {
                if (v.Count() != b.N) throw new Exception("Multiplication error");
                List<List<double>> res = new List<List<double>>();

                List<double> tmp1 = b.mainDiag[0] * v[0];
                List<double> tmp2 = b.upDiag[0] * v[1], tmp3;
                res.Add(sum(tmp1, tmp2));

                int N = b.N;
                for (int i = 1; i < N - 1; ++i)
                {
                    tmp1 = b.downDiag[i] * v[i - 1];
                    tmp2 = b.mainDiag[i] * v[i];
                    tmp3 = b.upDiag[i] * v[i + 1];
                    res.Add(sum(sum(tmp1, tmp2), tmp3));
                }

                tmp1 = b.downDiag[N - 1] * v[N - 2];
                tmp2 = b.mainDiag[N - 1] * v[N - 1];
                res.Add(sum(tmp1, tmp2));
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<List<double>> solve(List<List<double>> f)
        {
            try
            {
                List<DiagMatrix> alpha = new List<DiagMatrix>(N);
                List<List<double>> beta = new List<List<double>>(N + 1);
                alpha.Add(null);
                beta.Add(null);
                alpha.Add(DiagMatrix.inverse(mainDiag[0]) * upDiag[0]);
                beta.Add(DiagMatrix.inverse(mainDiag[0]) * f[0]);
                for (int i = 1; i < N - 1; ++i)
                {
                    DiagMatrix tmp = DiagMatrix.inverse(mainDiag[i] - downDiag[i] * alpha[i]);
                    alpha.Add(tmp * upDiag[i]);
                    beta.Add(tmp * diff(f[i], downDiag[i] * beta[i]));
                }
                beta.Add(DiagMatrix.inverse(mainDiag[N - 1] - downDiag[N - 1] * alpha[N - 1]) * diff(f[N - 1], downDiag[N - 1] * beta[N - 1]));

                List<List<double>> x = new List<List<double>>(N);
                for (int i = 0; i < N; ++i)
                {
                    x.Add(null);
                }
                x[N - 1] = beta[N];
                for (int i = N - 2; i >= 0; --i)
                {
                    x[i] = diff(beta[i + 1], alpha[i + 1] * x[i + 1]);
                }
                return x;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<List<double>> solveAndSave(List<List<double>> f, StreamWriter sw)
        {
            try
            {
                List<List<double>> x = solve(f);
                printBlockMatrix(sw);

                sw.WriteLine("Right part:");
                for (int i = 0; i < f.Count(); ++i)
                {
                    for (int j = 0; j < f[i].Count(); ++j)
                    {
                        sw.Write(f[i][j]);
                        sw.Write(" ");
                    }
                }
                sw.WriteLine();
                sw.WriteLine();

                sw.WriteLine("Solution: ");
                for (int i = 0; i < x.Count(); ++i)
                {
                    for (int j = 0; j < x[i].Count(); ++j)
                    {
                        sw.WriteLine(x[i][j]);
                        sw.WriteLine(" ");
                    }
                }
                sw.WriteLine();
                sw.WriteLine();

                return x;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    };
}