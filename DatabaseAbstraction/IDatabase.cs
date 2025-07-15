using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAbstraction
{
    public interface IDatabase
    {
        Task OpenAsync(string dataSource);
        Task CreateTableAsync(string table, Dictionary<string, Type> columns);
        Task AddDataAsync(string table, params object[] datas);
        Task UpdateAsync(string table, Dictionary<string, object> valuePair, string? condition = null);
        Task DeleteAsync(string table, string? condition = null);
        Task<long> GetCountAsync(string table, string? condition = null);
        Task<List<object[]>> GetRowAsync(string table, string[] readColumns, string? condition = null);
    }
}
