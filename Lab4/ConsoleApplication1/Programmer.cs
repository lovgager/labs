using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Programmer : Person, IDeepCopy
    {
        public double Exp { get; set; }
        public string Thema { get; set; }

        public Programmer(string firstName = "", string lastName = "", 
                DateTime birthday = new DateTime(), double exp = 0.0, string thema = ""):
                base(firstName, lastName, birthday)
        {
            this.Exp = exp;
            this.Thema = thema;
        }

        public override string ToString()
        {
            return base.ToString() + " " + Exp + " " + Thema + " - programmer";
        }

        public override object DeepCopy()
        {
            return new Programmer(this.FirstName, this.LastName, 
                    this.Birthday, this.Exp, this.Thema);
        }
    }
}
