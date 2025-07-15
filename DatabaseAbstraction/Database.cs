using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAbstraction
{
    /// <summary>
    /// </summary>
    /// <typeparam name="T">database data type</typeparam>
    public abstract class Database<T> : IDatabase where T : Enum
    {
        private IConnector _connect;

        public Database(IConnector connect)
        {
            _connect = connect;
        }

        public Task OpenAsync(string dataSource)
        {
            return _connect.OpenAsync(dataSource);
        }

        public Task CreateTableAsync(string table, Dictionary<string, Type> columns)
        {
            var pair = columns.Select(x => $"'{x.Key}' {CheckType(x.Value)}");
            var querys = string.Join(", ", pair);

            var query = $"CREATE TABLE IF NOT EXISTS '{table}' ({querys})";

            return _connect.WriteAsync(query);
        }

        public Task AddDataAsync(string table, params object[] datas)
        {
            for (int i = 0; i < datas.Length; i++)
            {
                datas[i] = Format(datas[i]);
            }

            var query = $"INSERT INTO '{table}' VALUES ({string.Join(',', datas)})";

            return _connect.WriteAsync(query);
        }

        public Task UpdateAsync(string table, Dictionary<string, object> valuePair, string? condition = null)
        {
            foreach (var key in valuePair.Keys)
            {
                valuePair[key] = Format(valuePair[key]);
            }

            var pair = valuePair.Select(x => $"'{x.Key}' = {x.Value}");
            var querys = string.Join(", ", pair);

            var query = $"UPDATE '{table}' SET {querys} {GetCondition(condition)}";

            return _connect.WriteAsync(query);
        }

        public Task DeleteAsync(string table, string? condition = null)
        {
            var query = $"DELETE FROM '{table}' {GetCondition(condition)}";

            return _connect.WriteAsync(query);
        }

        public Task<long> GetCountAsync(string table, string? condition = null)
        {
            var query = $"SELECT COUNT(*) FROM '{table}' {GetCondition(condition)}";

            return _connect.ReadCountAsync(query);
        }

        public Task<List<object[]>> GetRowAsync(string table, string[] readColumns, string? condition = null)
        {
            var query = $"SELECT * FROM '{table}' {GetCondition(condition)}";

            return _connect.ReadRowsAsync(query, readColumns);
        }

        private string GetCondition(string? condition)
        {
            return condition == null ? string.Empty : $"WHERE {condition}";
        }

        /// <summary>
        /// C# type -> DB type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected abstract T CheckType(Type type);

        /// <summary>
        /// C# value -> DB value
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected abstract object Format(object data);
    }
}
