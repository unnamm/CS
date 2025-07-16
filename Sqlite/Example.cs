using DatabaseAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sqlite
{
    public class Example
    {
        public static async Task Sample()
        {
            const string TABLE = "myTable";

            //open database
            IDatabase db = new SqliteProcess(new());
            await db.OpenAsync(@"D:\data.db");

            //create table
            Dictionary<string, Type> dic = [];
            dic.Add(GetColumns()[0], typeof(string));
            dic.Add(GetColumns()[1], typeof(int));
            dic.Add(GetColumns()[2], typeof(bool));
            await db.CreateTableAsync(TABLE, dic);

            //add row
            await db.AddDataAsync(TABLE, "name1", 1.1, true);
            await db.AddDataAsync(TABLE, "name2", 2.0, false);
            await db.AddDataAsync(TABLE, "name3", 3.3, 1);
            await db.AddDataAsync(TABLE, "name4", 4.0, 0);

            //add row, want column
            Dictionary<string, object> columnAndValue = [];
            columnAndValue.Add(GetColumns()[1], 4.4);
            await db.AddDataAsync(TABLE, columnAndValue);

            //update all row
            Dictionary<string, object> dic2 = [];
            dic2.Add("name", "this is true");
            await db.UpdateAsync(TABLE, dic2, "column3 = true");

            //get row count
            var allRowCount = await db.GetCountAsync(TABLE);
            var rowCount = await db.GetCountAsync(TABLE, "column3 = true");

            //get row datas
            var allResult = await db.GetRowAsync((TABLE), GetColumns());
            var results = await db.GetRowAsync(TABLE, GetColumns(), "column3 = true");

            //delete rows
            await db.DeleteAsync(TABLE, "value = 4");

            //delete all rows
            //await db.DeleteAsync(TABLE);
        }

        private static string[] GetColumns()
        {
            return ["name", "value", "column3"];
        }
    }
}
