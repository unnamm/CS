using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAbstraction
{
    public interface IConnector
    {
        Task OpenAsync(string dataSource);
        Task WriteAsync(string query);
        Task<int> ReadCountAsync(string query);
        Task<List<object[]>> ReadRowsAsync(string query, params string[] readColumns);
    }
}
