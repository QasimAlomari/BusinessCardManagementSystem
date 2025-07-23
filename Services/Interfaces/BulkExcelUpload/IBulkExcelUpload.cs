using Domain.Common;
using Services.Interfaces.DIInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.Common.CommonClass;

namespace Services.Interfaces.BulkExcelUpload
{
    public interface IBulkExcelUpload<T> where T : class
    {
        Task BulkInsertAsync(IEnumerable<T> data, string createId);
        Task<List<T>> ConvertDataTableToListAsync(DataTable dataTable, string createId);
        Task<List<string>> ValidateDataAsync(DataTable dataTable, string createId);
        HeaderValidationResult ValidateHeadersAsync(DataTable dataTable);
    }
}
