using System.Data;
using System.Data.Common;

namespace Database
{
    public abstract class Database
    {
        private readonly DbConnection _connect;

        public Database(DbConnection connect) => _connect = connect;

        public bool IsConnected => _connect.State == ConnectionState.Open;
        public Task ConnectAsync(CancellationToken token = default)
        {
            if (string.IsNullOrWhiteSpace(_connect.DataSource))
                throw new Exception("path is empty");

            if (File.Exists(_connect.DataSource))
                Console.WriteLine($"open file: {_connect.DataSource}");
            else
                Console.WriteLine($"create new file: {_connect.DataSource}");

            return _connect.OpenAsync(token);
        }

        public Task CloseAsync() => _connect.CloseAsync();
        public void Dispose() => _connect.Dispose();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="token"></param>
        /// <returns>row num affected by query</returns>
        public Task<int> NonQueryAsync(string query, CancellationToken token = default)
        {
            using var cmd = _connect.CreateCommand();
            cmd.CommandText = query;
            return cmd.ExecuteNonQueryAsync(token);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="token"></param>
        /// <returns>read all data</returns>
        public async Task<List<object[]>> ReaderAsync(string query, CancellationToken token = default)
        {
            using var cmd = _connect.CreateCommand();
            cmd.CommandText = query;

            var list = new List<object[]>();
            using var reader = await cmd.ExecuteReaderAsync(token);
            while (await reader.ReadAsync(token))
            {
                var rows = new object[reader.FieldCount];
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    rows[i] = reader[i];
                }
                list.Add(rows);
            }
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="token"></param>
        /// <returns>first data</returns>
        public async Task<object?> ScalarAsync(string query, CancellationToken token = default)
        {
            using var cmd = _connect.CreateCommand();
            cmd.CommandText = query;
            var result = await cmd.ExecuteScalarAsync(token);
            return result;
        }
    }
}
