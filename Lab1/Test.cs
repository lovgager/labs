using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    enum SubjectSet { linal, matan, english, programming }
    class Test
    {
        private DateTime date;
        public SubjectSet Subject { get; set; }
        public bool Passed { get; set; }

        public DateTime Date
        {
            get
            {
                return date;
            }
            set
            {
                date = value;
            }
        }

        public string Short_date
        {
            get
            {
                return "Day: " + date.Day.ToString() + " Month: " + date.Month.ToString();
            }
        }


        public Test(SubjectSet s = 0, DateTime d = new DateTime(), bool b = false)
        {
            Subject = s;
            date = d;
            Passed = b;
        }

        public override string ToString()
        {
            return "Date: " + date.ToString() + " Subject: " + Subject + " Passed: " + Passed;
        }
    }
}
