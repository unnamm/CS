using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Sqlite
{
    public interface ISqlite : IDisposable
    {
        Task OpenAsync();

        /// <summary>
        /// create table if not exists
        /// </summary>
        /// <param name="table"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        Task CreateTableAsync(string table, Dictionary<string, Type> columns);

        /// <summary>
        /// add new row
        /// </summary>
        /// <param name="table"></param>
        /// <param name="datas"></param>
        /// <returns></returns>
        Task AddDatasAsync(string table, object[] datas);

        /// <summary>
        /// get row count
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        Task<int> GetCountAsync(string table);

        /// <summary>
        /// get row count where condition
        /// </summary>
        /// <param name="table"></param>
        /// <param name="column"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<int> GetCountAsync(string table, string column, object condition);
        Task UpdateAsync(string table, string conditionColumn, object conditionValue, string updateColumn, object updateValue);

        /// <summary>
        /// get all row where condition
        /// </summary>
        /// <param name="table"></param>
        /// <param name="column"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<List<object[]>> GetRowsAsync(string table, string column, object condition);
        Task ClearTableAsync(string table);
        Task DeleteAsync(string table, string column, object condition);

        public static string[] GetColumns()
        {
            return ["name", "value", "column3"];
        }

        public static async void Example()
        {
            using ISqlite sp = new SqliteProcess(@"D:/data.db");
            await sp.OpenAsync();

            var columns = ISqlite.GetColumns();
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
