using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Team : IDeepCopy
    {
        public string TeamName { get; set; }
        public List<Person> Members { get; set; }

        public Team(string TeamName)
        {
            this.TeamName = TeamName;
            Members = new List<Person>();
        }

        public void AddPerson(params Person[] persons)
        {
            foreach (Person new_p in persons)
            {
                bool is_in = false;
                foreach (Person p in Members)
                {
                    if (p == new_p)
                    {
                        is_in = true;
                        break;
                    }
                }
                if (!is_in)
                {
                    Members.Add(new_p);
                }
            }
        }

        public void AddDefaults()
        {
            Person[] persons = new Person[6];
            persons[0] = new Person("Alice", "Smith", new DateTime(1990, 10, 10));
            persons[1] = new Programmer("Bob", "Lee", new DateTime(1985, 5, 12), 5, "math");
            persons[2] = new Researcher("Charles", "Jackson", new DateTime(1987, 7, 25), "physics", 0);
            persons[3] = new Person("Dmitry", "Ivanov", new DateTime(1993, 2, 28));
            persons[4] = new Researcher("Ethan", "Carter", new DateTime(1991, 11, 5), "biology", 7);
            persons[5] = new Programmer("Fiona", "Finch", new DateTime(1995, 5, 13), 3, "math");
            this.AddPerson(persons);
        }

        public static bool IsProgrammer(Person ps)
        {
            return ps is Programmer;
        }

        public override string ToString()
        {
            string s = this.TeamName + '\n';
            foreach (Person p in Members)
            {
                s += p.ToString() + '\n';
            }
            s += '\n';
            return s;
        }

        public IEnumerable<Person> Subset (Predicate<Person> Filter)
        {
            foreach (Person p in Members)
            {
                if (Filter(p))
                {
                    yield return p;
                }
            }
        }

        public object DeepCopy()
        {
            Team t = new Team(this.TeamName);
            foreach (Person p in Members)
            {
                t.AddPerson((Person)p.DeepCopy());
            }
            return t;
        }
    }
}
