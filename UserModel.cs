using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
namespace OraclaMajorAssignmentY3S2.Model.myclass
{
    class UserModel : Connection
    {
        public string User_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
}
