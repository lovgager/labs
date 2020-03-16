using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Person person1 = new Person("A", "B", new DateTime(1999, 10, 10));
            Person person2 = new Person("A", "B", new DateTime(1999, 10, 10));
            Person person3 = null;
            Console.WriteLine(person1 == person2);
            Console.WriteLine(person3 == person1);
            Console.WriteLine(person3 == null);
            Console.WriteLine();

            Researcher r1 = new Researcher("A", "B", new DateTime(1990, 5, 5), "Math", 10);
            Researcher r2 = (Researcher) r1.DeepCopy();
            r2.FirstName = "C";
            r2.Subject = "Physics";
            r2.Publications = 5;
            Console.WriteLine(r1);
            Console.WriteLine(r2);
        }
    }
}
