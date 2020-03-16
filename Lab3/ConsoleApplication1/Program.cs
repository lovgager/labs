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
            Team t1 = new Team("CMC");
            t1.AddDefaults();
            Console.WriteLine(t1);

            Team t2 = (Team) t1.DeepCopy();
            t2.TeamName = "Phys";
            t2.Members[1].FirstName = "Bruce";
            t2.Members[4].LastName = "Fox";
            Console.WriteLine(t1);
            Console.WriteLine(t2);

            foreach (Person p in t1.Subset(Team.IsProgrammer))
            {
                Console.WriteLine(p);
            }
            Console.WriteLine();

            foreach (Person p in t1.Subset(HasPublication))
            {
                Console.WriteLine(p);
            }
        }

        public static bool HasPublication(Person ps)
        {
            return (ps is Researcher) && (((Researcher)ps).Publications > 0);
        }
    }
}
