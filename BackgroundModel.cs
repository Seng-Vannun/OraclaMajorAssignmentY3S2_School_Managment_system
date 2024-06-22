using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OraclaMajorAssignmentY3S2.Model.myclass
{
    class BackgroundModel:Connection
    {

        public int Student_id { get; set; }
        public string Father_Name { get; set; }
        public string Father_Occupation { get; set; }
        public string Mother_Name { get; set; }
        public string Mother_Occupation { get; set; }
        public string Contact_Number { get; set; }
        public string Current_Address { get; set; }


    }
}
