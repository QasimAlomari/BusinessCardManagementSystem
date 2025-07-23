using Services.Interfaces.BulkExcelUpload;
using Services.Interfaces.DIInjection;
using System.Data;
using ExcelDataReader;
using System.Text;

namespace Services.Implementations.BulkExcelUpload
{
    public class ExcelHelper : IExcelHelper, IScopedService
    {
        public DataTable ReadExcelToDataTable(Stream stream, string fileName)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            using var reader = ExcelReaderFactory.CreateReader(stream);
            var result = reader.AsDataSet(new ExcelDataSetConfiguration
            {
                ConfigureDataTable = _ => new ExcelDataTableConfiguration
                {
                    UseHeaderRow = true
                }
            });

            return result.Tables[0];
        }
    }
}
