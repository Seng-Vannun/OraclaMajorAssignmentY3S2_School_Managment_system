using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OraclaMajorAssignmentY3S2.Model.myclass
{
     class ScoreModel:Connection
    {
        public int Student_id { get; set; }
        public int Subject_id { get; set; }
        public int Year { get; set; }
        public int Semester { get; set; }
        public int Score { get; set; }
        public string Subject_Name { get; set; }
        public string Description { get; set; }
    }
}
