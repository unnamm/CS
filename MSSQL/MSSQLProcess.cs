using DatabaseAbstraction;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSSQL
{
    internal class MSSqlProcess : Database<SqlDbType>
    {
        public MSSqlProcess(MSSqlConnect con) : base(con) { }

        protected override SqlDbType CheckType(Type type)
        {
            if (type == typeof(long))
            {
                return SqlDbType.BigInt;
            }
            if (type == typeof(byte))
            {
                return SqlDbType.TinyInt;
            }
            if (type == typeof(bool))
            {
                return SqlDbType.Bit;
            }
            if (type == typeof(string))
            {
                return SqlDbType.NVarChar;
            }
            if (type == typeof(DateTime))
            {
                return SqlDbType.DateTime;
            }
            if (type == typeof(decimal))
            {
                return SqlDbType.Decimal;
            }
            if (type == typeof(double))
            {
                return SqlDbType.Float;
            }
            if (type == typeof(int))
            {
                return SqlDbType.Int;
            }
            if (type == typeof(float))
            {
                return SqlDbType.Real;
            }
            if (type == typeof(short))
            {
                return SqlDbType.SmallInt;
            }
            if (type == typeof(byte[]))
            {
                return SqlDbType.VarBinary;
            }

            throw new NotImplementedException($"type={type}");
        }

        protected override object Format(object data)
        {
            return data;
        }
    }
}
