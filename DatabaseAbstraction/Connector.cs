using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAbstraction
{
    public abstract class Connector : IConnector
    {
        protected abstract DbCommand MakeCommand();

        public abstract Task OpenAsync(string dataSource);

        public async Task WriteAsync(string query)
        {
            var command = MakeCommand();
            command.CommandText = query;
            try
            {
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<long> ReadCountAsync(string query)
        {
            var command = MakeCommand();
            command.CommandText = query;
            try
            {
                return (long)(await command.ExecuteScalarAsync())!;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<List<object[]>> ReadRowsAsync(string query, params string[] readColumns)
        {
            var command = MakeCommand();
            command.CommandText = query;

            IDataReader row;

            try
            {
                row = await command.ExecuteReaderAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

            List<object[]> list = [];
            while (row.Read())
            {
                list.Add(readColumns.Select(x => row[x]).ToArray());
            }

            return list;
        }
    }
}
