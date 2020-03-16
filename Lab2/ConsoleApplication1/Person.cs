using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Person: IDeepCopy
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }

        public Person(string firstname = "", string lastname = "", DateTime birthday = new DateTime())
        {
            FirstName = firstname;
            LastName = lastname;
            Birthday = birthday;
        }

        public override string ToString()
        {
            return FirstName + " " + LastName + " " + Birthday.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Person))
            {
                return false;
            }
            Person per = (Person) obj;
            return (per.ToString() == this.ToString());
        }

        public static bool operator== (Person p1, Person p2)
        {
            if ((object) p1 == null && (object) p2 == null) return true;
            if ((object) p1 == null || (object) p2 == null) return false;
            return p1.Equals(p2);
        }
        public static bool operator !=(Person p1, Person p2)
        {
            return !(p1 == p2);
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public virtual object DeepCopy()
        {
            Person p = new Person(this.FirstName, this.LastName, this.Birthday);
            return p;
        }
    }
}
