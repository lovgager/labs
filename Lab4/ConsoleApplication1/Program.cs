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
            TeamList t = new TeamList();
            t.AddDefaults();
            foreach (Team team in t.Teams)
            {
                Console.WriteLine(team);
            }
            Console.WriteLine("MaxPublications: " + t.MaxPublications);
            Console.WriteLine("MaxResearcher: " + t.MaxResearcher);

            Console.WriteLine("Sorted programmers: ");
            foreach (Programmer pr in t.ExpProgrammers)
            {
                Console.WriteLine("\t" + pr);
            }

            Console.WriteLine("Grouped programmers: ");
            foreach (var group in t.GroupedProgrammers)
            {
                Console.WriteLine("Key = " + group.Key);
                foreach (Programmer p in group) Console.WriteLine("\t" + p);
            }

            Console.WriteLine();
            foreach (Person p in t.NonUnique)
            {
                Console.WriteLine(p);
            }
        }
    }
}
