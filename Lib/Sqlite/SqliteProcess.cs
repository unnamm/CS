using Microsoft.Data.Sqlite;
using System.Data.Common;

namespace Lib.Sqlite
{
    /// <summary>
    /// Microsoft.Data.Sqlite (8.0.8)
    /// </summary>
    internal class SqliteProcess
    {
        private readonly SqliteConnection _connection;

        public SqliteProcess(string databaseFilePath)
        {
            _connection = new SqliteConnection($"Data Source={databaseFilePath}");
        }

        public void Init()
        {
            _connection.Open();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columns">column name, data type</param>
        public void CreateTable(string tableName, Dictionary<string, Type> columns)
        {
            var query = $"create table if not exists '{tableName}' (";
            foreach (var pair in columns)
            {
                string dataType;
                if (pair.Value == typeof(int) || pair.Value == typeof(bool))
                {
                    dataType = "INTEGER";
                }
                else if (pair.Value == typeof(double) || pair.Value == typeof(float))
                {
                    dataType = "REAL";
                }
                else if (pair.Value == typeof(string))
                {
                    dataType = "TEXT";
                }
                else //if need type, add if and typeof
                {
                    dataType = "BLOB";
                }

                query += $", '{pair.Key}' {dataType}";
            }
            query += ")";

            var command = _connection.CreateCommand();
            command.CommandText = query;
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="datas">match column</param>
        public void AddDatas(string tableName, object[] datas)
        {
            var query = $"insert into '{tableName}' values (";
            foreach (var data in datas)
            {
                query += $", '{data}'";
            }
            query += ")";

            var command = _connection.CreateCommand();
            command.CommandText = query;
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// get columnList in table
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns>all columns</returns>
        /// <exception cref="Exception"></exception>
        public string[] GetColumnList(string tableName)
        {
            var command = _connection.CreateCommand();
            command.CommandText = $"select * from '{tableName}'"; //can add where

            using var reader = command.ExecuteReader() ?? throw new Exception("empty");
            return reader.GetColumnSchema().Select(x => x.ColumnName).ToArray();
        }

        /// <summary>
        /// get all datas
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns>all row datas</returns>
        /// <exception cref="Exception"></exception>
        public object[][] GetDatas(string tableName)
        {
            var command = _connection.CreateCommand();
            command.CommandText = $"select * from '{tableName}'"; //can add where

            using var reader = command.ExecuteReader() ?? throw new Exception("empty");

            List<object[]> lists = [];

            var columnList = GetColumnList(tableName);

            while (reader.Read())
            {
                lists.Add(columnList.Select(x => reader[x]).ToArray());
            }

            return [.. lists];
        }

        public static void Example()
        {
            SqliteProcess sp = new(@"D:/data.db");
            sp.Init();

            Dictionary<string, Type> dic = [];

            dic.Add("col1", typeof(int));
            dic.Add("col2", typeof(double));
            dic.Add("col3", typeof(string));

            var tableName = "firstTable";

            sp.CreateTable(tableName, dic); //make table
            sp.AddDatas(tableName, [15, 12.2, "aaa"]); //add value
            var allData = sp.GetDatas(tableName); //get value
        }
    }
}
