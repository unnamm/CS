using DatabaseAbstraction;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sqlite
{
    public class SqliteConnect : Connector
    {
        private SqliteConnection _connect;

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
