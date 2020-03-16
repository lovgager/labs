using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Researcher : Person, IDeepCopy, IComparable<Researcher>
    {
        public string Subject { get; set; }
        public int Publications { get; set; }

        public Researcher(string firstname = "", string lastname = "", DateTime birthday = new DateTime(), string subject = "", int publications = 0)
            : base(firstname, lastname, birthday)
        {
            Subject = subject;
            Publications = publications;
        }

        public override string ToString()
        {
            return base.ToString() + " " + Subject + " " + Publications + " - researcher";
        }

        public override object DeepCopy()
        {
            Researcher r = new Researcher(this.FirstName, this.LastName, 
                    this.Birthday, this.Subject, this.Publications);
            return r;
        }

        public int CompareTo(Researcher other)
        {
            return this.Publications.CompareTo(other.Publications);
        }
    }
}
