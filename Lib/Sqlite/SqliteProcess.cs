using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace Lib.Sqlite
{
    /// <summary>
    /// Microsoft.Data.Sqlite
    /// </summary>
    internal class SqliteProcess : ISqlite
    {
        private SqliteConnection _connect;

        public Task OpenAsync(string databaseFilePath)
        {
            _connect = new($"Data Source={databaseFilePath}");
            return _connect.OpenAsync();
        }

        public Task CreateTableAsync(SqliteTable table, Dictionary<Table1Column, Type> columns)
        {
            var columnList = columns.Select(pair => $"'{pair.Key}' {TypeCheck(pair.Value)}");
            return RunQueryAsync($"create table if not exists '{table}' ({string.Join(", ", columnList)})");
        }

        public Task AddDatasAsync(SqliteTable table, object[] datas)
        {
            datas = ChangeFormat(datas);

            string[] dataList = datas.Select(x =>
            {
                if (TypeCheck(x) == SqliteType.Text)
                {
                    return $"'{x}'";
                }
                return x.ToString();
            }).ToArray()!;

            var query = $"insert into '{table}' values ({string.Join(',', dataList)})";
            return RunQueryAsync(query);
        }

        public async Task<int> GetCountAsync(SqliteTable table)
        {
            var command = _connect.CreateCommand();
            command.CommandText = $"select count(*) from '{table}'";
            return (int)(long)(await command.ExecuteScalarAsync())!;
        }

        public async Task<int> GetCountAsync(SqliteTable table, Table1Column column, object condition)
        {
            if (TypeCheck(condition) == SqliteType.Text)
            {
                condition = $"'{condition}'";
            }

            var command = _connect.CreateCommand();
            command.CommandText = $"select count(*) from '{table}' where {column} = {condition}";
            return (int)(long)(await command.ExecuteScalarAsync())!;
        }

        public Task UpdateAsync(SqliteTable table, Table1Column conditionColumn, object conditionValue, Table1Column updateColumn, object updateValue)
        {
            if (TypeCheck(conditionValue) == SqliteType.Text)
            {
                conditionValue = $"'{conditionValue}'";
            }

            if (TypeCheck(updateValue) == SqliteType.Text)
            {
                updateValue = $"'{updateValue}'";
            }

            var query = $"update '{table}' set '{updateColumn}' = {updateValue} where {conditionColumn} = {conditionValue}";
            return RunQueryAsync(query);
        }

        public async Task<List<object[]>> GetRowsAsync(SqliteTable table, Table1Column column, object condition)
        {
            if (TypeCheck(condition) == SqliteType.Text)
            {
                condition = $"'{condition}'";
            }

            var command = _connect.CreateCommand();
            command.CommandText = $"select * from '{table}' where {column} = {condition}";

            var read = await command.ExecuteReaderAsync();

            List<object[]> list = [];
            var columns = Enum.GetValues<Table1Column>();
            while (read.Read())
            {
                list.Add(columns.Select(x => read[x.ToString()]).ToArray());
            }

            return list;
        }

        public Task ClearTableAsync(SqliteTable table) => RunQueryAsync($"delete from '{table}'");

        public Task DeleteAsync(SqliteTable table, Table1Column column, object condition)
        {
            if (TypeCheck(condition) == SqliteType.Text)
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

        private static SqliteType TypeCheck<T>(T data) => TypeCheck(data!.GetType());

        private static SqliteType TypeCheck(Type type)
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

    }
}
