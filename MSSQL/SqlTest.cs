using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSSQL
{
    public class SqlTest
    {
        private readonly SqlConnection _connect;

        public SqlTest(string dataSource)
        {
            _connect = new($"Data Source={dataSource}");
        }

        public Task OpenAsync() => _connect.OpenAsync();

        public Task CreateTableAsync(string table, Dictionary<string, Type> columns)
        {
            var columnList = columns.Select(pair => $"'{pair.Key}' {TypeCheck(pair.Value)}");
            return RunQueryAsync($"create table if not exists '{table}' ({string.Join(", ", columnList)})");
        }

        public Task AddDatasAsync(string table, object[] datas)
        {
            datas = ChangeFormat(datas);

            string[] dataList = datas.Select(x =>
            {
                if (TypeCheck(x) == SqlDbType.Text)
                {
                    return $"'{x}'";
                }
                return x.ToString();
            }).ToArray()!;

            var query = $"insert into '{table}' values ({string.Join(',', dataList)})";
            return RunQueryAsync(query);
        }

        public async Task<int> GetCountAsync(string table)
        {
            var command = _connect.CreateCommand();
            command.CommandText = $"select count(*) from '{table}'";
            return (int)(long)(await command.ExecuteScalarAsync())!;
        }

        public async Task<int> GetCountAsync(string table, string column, object condition)
        {
            if (TypeCheck(condition) == SqlDbType.Text)
            {
                condition = $"'{condition}'";
            }

            var command = _connect.CreateCommand();
            command.CommandText = $"select count(*) from '{table}' where {column} = {condition}";
            return (int)(long)(await command.ExecuteScalarAsync())!;
        }

        public Task UpdateAsync(string table, string conditionColumn, object conditionValue, string updateColumn, object updateValue)
        {
            if (TypeCheck(conditionValue) == SqlDbType.Text)
            {
                conditionValue = $"'{conditionValue}'";
            }

            if (TypeCheck(updateValue) == SqlDbType.Text)
            {
                updateValue = $"'{updateValue}'";
            }

            var query = $"update '{table}' set '{updateColumn}' = {updateValue} where {conditionColumn} = {conditionValue}";
            return RunQueryAsync(query);
        }

        public async Task<List<object[]>> GetRowsAsync(string table, string column, object condition)
        {
            if (TypeCheck(condition) == SqlDbType.Text)
            {
                condition = $"'{condition}'";
            }

            var command = _connect.CreateCommand();
            command.CommandText = $"select * from '{table}' where {column} = {condition}";

            var read = await command.ExecuteReaderAsync();

            string[] columns = ["col1", "col2", "col3"];

            List<object[]> list = [];
            while (read.Read())
            {
                list.Add(columns.Select(x => read[x.ToString()]).ToArray());
            }

            return list;
        }

        public Task ClearTableAsync(string table) => RunQueryAsync($"delete from '{table}'");

        public Task DeleteAsync(string table, string column, object condition)
        {
            if (TypeCheck(condition) == SqlDbType.Text)
            {
                condition = $"'{condition}'";
            }

            var query = $"delete from '{table}' where {column} = {condition}";
            return RunQueryAsync(query);
        }

        private Task<int> RunQueryAsync(string query)
        {
            var command = _connect.CreateCommand();
            command.CommandText = query;
            return command.ExecuteNonQueryAsync();
        }

        private object[] ChangeFormat(params object[] datas)
        {
            for (int i = 0; i < datas.Length; i++)
            {
                if (datas[i] is bool b)
                {
                    datas[i] = b ? 1 : 0; //bool is 0 or 1
                }
                else if (datas[i] is DateTime dt)
                {
                    datas[i] = $"{dt:yyyy-MM-dd HH:mm:ss}"; //time is string
                }
            }
            return datas;
        }

        private static SqlDbType TypeCheck<T>(T data) => TypeCheck(data!.GetType());

        private static SqlDbType TypeCheck(Type type)
        {
            SqlDbType? returnType = null;

            if (type == typeof(bool) ||
                type == typeof(byte) ||
                type == typeof(short) || type == typeof(ushort) ||
                type == typeof(int) || type == typeof(uint) ||
                type == typeof(long) || type == typeof(ulong))
            {
                returnType = SqlDbType.Int;
            }
            else if (type == typeof(double) || type == typeof(float))
            {
                returnType = SqlDbType.Real;
            }
            else if (type == typeof(char) || type == typeof(decimal) ||
                type == typeof(string) || type == typeof(Guid) ||
                type == typeof(DateTime) || type == typeof(DateOnly) || type == typeof(DateTimeOffset) ||
                type == typeof(TimeOnly) || type == typeof(TimeSpan))
            {
                returnType = SqlDbType.Text;
            }

            if (returnType == null)
            {
                throw new Exception("this is a data type not handled by sqlite\n" + type);
            }

            return returnType.Value;
        }

        public void Dispose()
        {
            _connect.Dispose();
        }

        public static async void Example()
        {
            SqlTest sp = new SqlTest(@"D:/data.db");
            await sp.OpenAsync();

            string[] columns = ["c1", "c2", "c3"];
            Dictionary<string, Type> dic = [];
            dic.Add(columns[0], typeof(string));
            dic.Add(columns[1], typeof(int));
            dic.Add(columns[2], typeof(bool));
            await sp.CreateTableAsync("table1", dic);

            await sp.AddDatasAsync("table1", ["name1", 1, true]);
            await sp.AddDatasAsync("table1", ["name2", 2, true]);
            await sp.AddDatasAsync("table1", ["name3", 3, false]);

            var c = await sp.GetCountAsync("table1");
        }
    }
}
