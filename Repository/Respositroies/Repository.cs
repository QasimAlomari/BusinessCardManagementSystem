    using Repository.Constants;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Repository.Respositroies;
using Microsoft.Data.SqlClient;

namespace Repository.Repositories
{
    public class Repository<T> : IRepository<T>
    {
        private readonly string _connectionString;
        public Repository()
        {
            _connectionString = CommonConstants.ConnectionString;

            if (string.IsNullOrWhiteSpace(_connectionString))
                throw new InvalidOperationException("Connection string is not configured.");
        }
        public async Task<IList<T>> ListData(string spName, object parameters = null)
        {
            using IDbConnection connection = new SqlConnection(_connectionString);
            if (connection.State == ConnectionState.Closed) connection.Open();

            var result = await connection.QueryAsync<T>(spName, parameters, commandType: CommandType.StoredProcedure);
            return result.ToList();
        }
        public async Task ExecCommand(string spName, object parameters = null)
        {
            using IDbConnection connection = new SqlConnection(_connectionString);
            if (connection.State == ConnectionState.Closed) connection.Open();

            await connection.ExecuteAsync(spName, parameters, commandType: CommandType.StoredProcedure);
        }
        public async Task<T> FindExecCommand(string spName, object parameters = null)
        {
            using IDbConnection connection = new SqlConnection(_connectionString);
            if (connection.State == ConnectionState.Closed) connection.Open();

            return await connection.QuerySingleOrDefaultAsync<T>(spName, parameters, commandType: CommandType.StoredProcedure);
        }
        public async Task<TScalar> ExecuteScalar<TScalar>(string spName, object parameters = null)
        {
            using IDbConnection connection = new SqlConnection(_connectionString);
            if (connection.State == ConnectionState.Closed) connection.Open();

            return await connection.ExecuteScalarAsync<TScalar>(spName, parameters, commandType: CommandType.StoredProcedure);
        }
        public async Task<T> GetSingleRow<T>(string spName, object parameters = null)
        {
            using IDbConnection connection = new SqlConnection(_connectionString);
            if (connection.State == ConnectionState.Closed) connection.Open();

            return await connection.QuerySingleOrDefaultAsync<T>(spName, parameters, commandType: CommandType.StoredProcedure);
        }
        public async Task BulkInsertAsync(DataTable dataTable, string destinationTableName)
        {
            if (dataTable == null || dataTable.Rows.Count == 0)
                throw new ArgumentException("The DataTable is null or contains no rows.", nameof(dataTable));

            await using var connection = new SqlConnection(_connectionString);
            if (connection.State != ConnectionState.Open)
                await connection.OpenAsync();

            using var bulkCopy = new SqlBulkCopy(connection)
            {
                DestinationTableName = destinationTableName,
                BatchSize = 5000,
                BulkCopyTimeout = 120, 
                NotifyAfter = 10000
            };

            foreach (DataColumn column in dataTable.Columns)
            {
                bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
            }

            await bulkCopy.WriteToServerAsync(dataTable);
        }
    }
}
