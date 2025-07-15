using DatabaseAbstraction;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSSQL
{
    internal class MSSqlConnect : Connector
    {
        private SqlConnection _connect;

        public override Task OpenAsync(string dataSource)
        {
            _connect = new($"Data Source={dataSource}");
            return _connect.OpenAsync();
        }

        protected override DbCommand MakeCommand()
        {
            return _connect.CreateCommand();
        }
    }
}
