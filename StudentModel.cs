using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace OraclaMajorAssignmentY3S2.Model.myclass
{
    class StudentModel:Connection
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string ContactNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public byte[] ProfilePicture { get; set; }
        public int Status { get; set; }
    }
}
