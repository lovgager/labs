using ClassLibrary1;
using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Diagnostics;

class Program
{
    [DllImport("../../../Debug/Dll1.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern void GlobalExportFunction(
            double[] mainDiag,
            double[] upDiag,
            double[] downDiag,
            double[] f,
            double[] solution,
            int blocks,
            int smallOrder,
            ref double time,
            string filename,
            bool save);

    public static void Test()
    {
        int blocks = 4, order = 3;

        //solve linear system using C# classes
        {
            try
            {
                BlockMatrix b = new BlockMatrix(blocks, order);
                List<double> diag1 = new List<double> { 1, 2, 3 };
                List<double> diag2 = new List<double> { 3, 4, 5 };
                List<double> diag3 = new List<double> { 2, 5, 11 };
                List<double> diag4 = new List<double> { 6, 2, 8 };

                DiagMatrix d1 = new DiagMatrix(order, diag1);
                DiagMatrix d2 = new DiagMatrix(order, diag2);
                DiagMatrix d3 = new DiagMatrix(order, diag3);
                DiagMatrix d4 = new DiagMatrix(order, diag4);
                List<DiagMatrix> mainDiag = new List<DiagMatrix> { d1, d2, d1, d2 };
                List<DiagMatrix> upDiag = new List<DiagMatrix> { d3, d4, d3 };
                List<DiagMatrix> downDiag = new List<DiagMatrix> { null, d4, d3, d4 };

                b.setMainDiag(mainDiag);
                b.setUpDiag(upDiag);
                b.setDownDiag(downDiag);

                List<double> f1 = new List<double> { 5, 6, 7 };
                List<List<double>> f = new List<List<double>> { f1, f1, f1, f1 };

                StreamWriter sw = new StreamWriter("outSharp.txt");
                List<List<double>> solution = b.solveAndSave(f, sw);

                sw.WriteLine("BlockMatrix * solution: \n");
                List<List<double>> f_new = b * solution;
                foreach (List<double> list in f_new)
                {
                    foreach (double elem in list)
                    {
                        sw.Write(elem + " ");
                    }
                }
                sw.WriteLine();
                sw.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка в тесте на C#: " + ex);
            }
        }

        //using C++ function from DLL
        {
            try
            {
                double[] mainDiag = new double[] { 1, 2, 3, 3, 4, 5, 1, 2, 3, 3, 4, 5 };
                double[] upDiag = new double[] { 2, 5, 11, 6, 2, 8, 2, 5, 11 };
                double[] downDiag = new double[] { 6, 2, 8, 2, 5, 11, 6, 2, 8 };
                double[] f = new double[] { 5, 6, 7, 5, 6, 7, 5, 6, 7, 5, 6, 7 };
                double[] solution = new double[12];
                double time = 0;
                GlobalExportFunction(mainDiag, upDiag, downDiag, f, solution, blocks, order, ref time, "OutPlus.txt", true);
            } 
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка в тесте на C++: " + ex);
            }
        }
    }

    public static void GenerateAndSolve(int blocks, int smallOrder, ref double msSharp, ref double msPlus)
    {
        try
        {
            {
                BlockMatrix b = new BlockMatrix(blocks, smallOrder);
                List<DiagMatrix> mainDiag = new List<DiagMatrix>();
                List<DiagMatrix> upDiag = new List<DiagMatrix>();
                List<DiagMatrix> downDiag = new List<DiagMatrix>();
                downDiag.Add(null);
                List<List<double>> f = new List<List<double>>();

                for (int i = 0; i < blocks; ++i)
                {
                    List<double> diag = new List<double>();
                    for (int j = 0; j < smallOrder; ++j)
                        diag.Add(j + 1);
                    DiagMatrix d = new DiagMatrix(smallOrder, diag);
                    mainDiag.Add(d);

                    diag = new List<double>();
                    for (int j = 0; j < smallOrder; ++j)
                        diag.Add(j * 0.5 + i + 1);
                    d = new DiagMatrix(smallOrder, diag);
                    upDiag.Add(d);

                    diag = new List<double>();
                    for (int j = 0; j < smallOrder; ++j)
                        diag.Add(-j * i * 0.5);
                    d = new DiagMatrix(smallOrder, diag);
                    downDiag.Add(d);

                    diag = new List<double>();
                    for (int j = 0; j < smallOrder; ++j)
                    {
                        double value = -i * 1.2 + j * 4 - 10;
                        if (value == 0) diag.Add(1);
                        else diag.Add(value);
                    }
                    f.Add(diag);
                }

                b.setMainDiag(mainDiag);
                b.setUpDiag(upDiag);
                b.setDownDiag(downDiag);

                Stopwatch sw = new Stopwatch();
                sw.Start();
                b.solve(f);
                sw.Stop();
                msSharp = sw.Elapsed.TotalMilliseconds;
            }

            {
                int totalOrder = blocks * smallOrder;
                double[] mainDiag = new double[totalOrder];
                double[] upDiag = new double[totalOrder];
                double[] downDiag = new double[totalOrder] ;
                double[] f = new double[totalOrder];
                double[] solution = new double[totalOrder];
                double time = 0.0;

                int k = 0;
                for (int i = 0; i < blocks; ++i)
                {
                    for (int j = 0; j < smallOrder; ++j)
                    {
                        mainDiag[k] = j + 1;
                        upDiag[k] = j * 0.5 + i + 1;
                        downDiag[k] = -j * i * 0.5;
                        double value = -i * 1.2 + j * 4 - 10;
                        if (value == 0) f[k] = 1.0;
                        else f[k] = value;
                        k++;
                    }
                }
                GlobalExportFunction(mainDiag, upDiag, downDiag, f, solution, blocks, smallOrder, ref time, null, false);
                msPlus = (double)time;
            }
            
        }
        catch (Exception)
        {
            Console.WriteLine("Ошибка в вычислениях");
        }
    }

    static void Main(string[] args)
    {
        try
        {
            Test();
            TestTime tt = new TestTime();
            TestTime.Load("result", ref tt);

            Console.WriteLine("Конец ввода - q");
            while (true)
            {
                Console.Write("Порядок блочной матрицы: ");
                try
                {
                    string input = Console.ReadLine();
                    if (input == "q") break;
                    int blocks = Convert.ToInt32(input);
                    if (blocks <= 0) throw new Exception();

                    Console.Write("Порядок матриц-элементов: ");
                    input = Console.ReadLine();
                    if (input == "q") break;
                    int smallOrder = Convert.ToInt32(input);
                    if (smallOrder <= 0) throw new Exception();

                    double msSharp = 0.0, msPlus = 0.0;
                    GenerateAndSolve(blocks, smallOrder, ref msSharp, ref msPlus);
                    string s = "Порядок блочной матрицы: " + blocks.ToString() +
                            "\nПорядок матриц-элементов: " + smallOrder.ToString() +
                            "\nВремя на C#: " + msSharp.ToString() +
                            "\nВремя на C++: " + msPlus.ToString() +
                            "\nОтношение: " + (msSharp / msPlus).ToString() + "\n";
                    tt.Add(s);
                }
                catch (Exception)
                {
                    Console.WriteLine("Неправильный ввод\n");
                }
            }

            Console.WriteLine("==================================");
            Console.WriteLine(tt);
            TestTime.Save("result", tt);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}
