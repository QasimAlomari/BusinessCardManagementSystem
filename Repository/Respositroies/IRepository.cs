using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Respositroies
{
    public interface IRepository<T>
    {
        Task<IList<T>> ListData(string spName, object parameters = null);
        Task ExecCommand(string spName, object parameters = null);
        Task<T> FindExecCommand(string spName, object parameters = null);
        Task<TScalar> ExecuteScalar<TScalar>(string spName, object parameters = null);
        Task<T> GetSingleRow<T>(string spName, object parameters = null);
        Task BulkInsertAsync(DataTable dataTable, string destinationTableName);
    }
}
