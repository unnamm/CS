using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class SQLite : Database
    {
        public SQLite(string path) : base(new Microsoft.Data.Sqlite.SqliteConnection($"Data Source={path}")) { }
    }
}
