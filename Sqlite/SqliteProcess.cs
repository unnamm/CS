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
    public class SqliteProcess : Database<SqliteType>
    {
        public SqliteProcess(SqliteConnect connect) : base(connect) { }

        protected override SqliteType CheckType(Type type)
        {
            SqliteType? returnType = null;

            if (type == typeof(bool) ||
                type == typeof(byte) ||
                type == typeof(short) || type == typeof(ushort) ||
                type == typeof(int) || type == typeof(uint) ||
                type == typeof(long) || type == typeof(ulong))
            {
                returnType = SqliteType.Integer;
            }
            else if (type == typeof(double) || type == typeof(float))
            {
                returnType = SqliteType.Real;
            }
            else if (type == typeof(char) || type == typeof(decimal) ||
                type == typeof(string) || type == typeof(Guid) ||
                type == typeof(DateTime) || type == typeof(DateOnly) || type == typeof(DateTimeOffset) ||
                type == typeof(TimeOnly) || type == typeof(TimeSpan))
            {
                returnType = SqliteType.Text;
            }
            else if (type == typeof(byte[]))
            {
                returnType = SqliteType.Blob;
            }

            if (returnType == null)
            {
                throw new Exception("this is a data type not handled by sqlite\n" + type);
            }

            return returnType.Value;
        }

        protected override object Format(object data)
        {
            if (data is bool b)
            {
                data = b ? 1 : 0;
            }
            else if (data is DateTime dt)
            {
                data = $"{dt:yyyy-MM-dd HH:mm:ss}";
            }

            if (CheckType(data.GetType()) == SqliteType.Text)
            {
                return $"'{data}'";
            }

            return data;
        }
    }
}
