using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSSQL
{
    public class MSSQLProcess
    {
        private readonly SqlConnection _connect;

        public MSSQLProcess(string dataSource)
        {
            _connect = new($"Data Source={dataSource}");
        }

        public Task OpenAsync(string dataSource) => _connect.OpenAsync();
    }
}
