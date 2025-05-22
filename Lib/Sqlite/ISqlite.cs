using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Sqlite
{
    internal interface ISqlite
    {
        Task OpenAsync(string databaseFilePath);
        Task CreateTableAsync(SqliteTable table, Dictionary<Table1Column, Type> columns);
        Task AddDatasAsync(SqliteTable table, object[] datas);
        Task<int> GetCountAsync(SqliteTable table);
        Task<int> GetCountAsync(SqliteTable table, Table1Column column, object condition);
        Task UpdateAsync(SqliteTable table, Table1Column conditionColumn, object conditionValue, Table1Column updateColumn, object updateValue);
        Task<List<object[]>> GetRowsAsync(SqliteTable table, Table1Column column, object condition);
        Task ClearTableAsync(SqliteTable table);
        Task DeleteAsync(SqliteTable table, Table1Column column, object condition);

        public static async void Example()
        {
            ISqlite sp = new SqliteProcess();
            await sp.OpenAsync(@"D:/data.db");

            Dictionary<Table1Column, Type> dic = [];
            dic.Add(Table1Column.Name, typeof(string));
            dic.Add(Table1Column.Value, typeof(int));
            dic.Add(Table1Column.Column3, typeof(bool));
            await sp.CreateTableAsync(SqliteTable.Table1, dic);

            //await sp.AddDatasAsync(SqliteTable.Table1, ["name1", 1, true]);
            //await sp.AddDatasAsync(SqliteTable.Table1, ["name2", 2, true]);
            //await sp.AddDatasAsync(SqliteTable.Table1, ["name3", 3, false]);

            //var c = await sp.GetCountAsync(SqliteTable.Table1);
            //var c = await sp.GetCountAsync(SqliteTable.Table1, SqliteColumn.Column3, true);

            //await sp.UpdateAsync(SqliteTable.Table1, SqliteColumn.Name, "name1", SqliteColumn.Value, 100);
            //var rows = await sp.GetRows(SqliteTable.Table1, Table1Column.Column3, true);

            //await sp.DeleteAsync(SqliteTable.Table1, Table1Column.Name, "name1");
        }
    }
}
