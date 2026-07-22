using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class MSSQL : Database
    {
        public MSSQL(string ip, string id, string password)
            : base(new Microsoft.Data.SqlClient.SqlConnection($"Data Source={ip};USER ID={id};PASSWORD={password};TrustServerCertificate=true")) { }
    }
}
