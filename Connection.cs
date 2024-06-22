using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace OraclaMajorAssignmentY3S2
{
    internal class Connection
    {
        public OracleConnection conn;
        public Connection()
        {
            String connectionString =
                "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)" +
                "(HOST=localhost)(PORT=1521)))\r\n" +
                "(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=xe)));" +
                "User Id=Vannun;\r\nPassword=12959240;";
            conn = new OracleConnection(connectionString);
            conn.Open();
        }
    }
}
