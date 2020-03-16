using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class TeamList
    {
        public List<Team> Teams { get; set; }

        public TeamList()
        {
            Teams = new List<Team>();
        }

        public void AddDefaults()
        {
            Teams = new List<Team>();
            Team team1 = new Team("team1");
            team1.AddDefaults();
            Team team2 = new Team("team2");
            team2.AddDefaults2();
            Teams.Add(team1);
            Teams.Add(team2);
        }

        public override string ToString()
        {
            string s = "";
            foreach (Team t in Teams)
            {
                s += t.ToString() + "\n";
            }
            return s;
        }

        public Researcher MaxResearcher
        {
            get
            {
                if (Teams.Count() == 0)
                    return null;

                var queryResearchers = from t in Teams
                                       from member in t.Members
                                       where member is Researcher
                                       select (Researcher)member;
                var queryMax = queryResearchers.Max();
                return queryMax;
            }
        }

        public int MaxPublications
        {
            get
            {
                if (MaxResearcher == null)
                {
                    return -1;
                }
                return MaxResearcher.Publications;
            }
        }

        public IEnumerable<Programmer> ExpProgrammers
        {
            get
            {
                var queryExp = from t in Teams
                               from member in t.Members
                               where member is Programmer
                               orderby ((Programmer)member).Exp
                               select (Programmer)member;
                return queryExp;
            }
        }

        public IEnumerable<IGrouping<double, Programmer>> GroupedProgrammers
        {
            get
            {
                var queryGroup = from t in Teams
                                 from member in t.Members
                                 where member is Programmer
                                 group ((Programmer)member) by ((Programmer)member).Exp;
                return queryGroup;
            }
        }

        public IEnumerable<Person> NonUnique
        {
            get
            {
                var queryNonUnique = from t in Teams
                                     from member in t.Members
                                     group member by new
                                     {
                                         key1 = member.FirstName,
                                         key2 = member.LastName,
                                         key3 = member.Birthday
                                     } into memberGroup
                                     where memberGroup.Count() > 1
                                     select memberGroup.ElementAt(0);
                return queryNonUnique;
            }
        }
    }
}
