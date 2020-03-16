using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Test t = new Test(SubjectSet.matan, new DateTime(2019, 12, 20, 10, 0, 0), false);
            Console.WriteLine(t);
            Console.WriteLine(t.Short_date);

            Test[] test_arr = new Test[] {new Test(SubjectSet.linal, new DateTime(2019, 12, 21, 9, 0, 0), true),
                                          new Test(SubjectSet.english, new DateTime(2019, 12, 23, 11, 30, 0), true),
                                          new Test(SubjectSet.programming, new DateTime(2019, 12, 26, 12, 15, 0), false),
                                          new Test(SubjectSet.linal, new DateTime(2019, 12, 26, 12, 15, 0), true),
                                          new Test(SubjectSet.programming, new DateTime(2019, 12, 27, 15, 15, 0), true),
                                          new Test(SubjectSet.matan, new DateTime(2019, 12, 28, 12, 15, 0), false),
                                          new Test(SubjectSet.linal, new DateTime(2019, 12, 25, 14, 35, 0), true)};
            for (int i = 0; i < test_arr.Length; ++i)
            {
                Console.WriteLine(test_arr[i]);
            }
            Console.WriteLine("...................");

            Test[][] test_arr2 = new Test[4][];
            test_arr2[0] = new Test[] { test_arr[0], test_arr[3], test_arr[6] };
            test_arr2[1] = new Test[] { test_arr[5] };
            test_arr2[2] = new Test[] { test_arr[1] };
            test_arr2[3] = new Test[] { test_arr[2], test_arr[4] };
            for (int i = 0; i < test_arr2.Length; ++i)
            {
                for (int j = 0; j < test_arr2[i].Length; ++j)
                {
                    Console.WriteLine(test_arr2[i][j]);
                }
                Console.WriteLine();
            }
        }
    }
}
