using Domain.Common;
using Domain.ViewModel.BusinessCardViewModel;
using Microsoft.AspNetCore.Http;
using Repository.Respositroies;
using Services.Interfaces.BulkExcelUpload;
using Services.Interfaces.DIInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.Common.CommonClass;

namespace Services.Services.BulkExcelUploadServices
{
    public class BulkExcelUpload : IBulkExcelUpload<BusinessCardExcelModelCreate>, IScopedService
    {
        private readonly IRepository<BusinessCardExcelModelCreate> _repository;
        private readonly BusinessCardValidator _validator;
        private const int BatchSize = 5000;
        private string _createId = string.Empty;

        private readonly Dictionary<string, string> _excelColumnToPropertyMap = new()
        {
            { "Card Name", "BusinessCardName" },
            { "Card Title", "BusinessCardTitle" },
            { "Card Phone", "BusinessCardPhone" },
            { "Card Email", "BusinessCardEmail" },
            { "Card Company", "BusinessCardCompany" },
            { "Card Website", "BusinessCardWebsite" },
            { "Card Address", "BusinessCardAddress" },
            { "Card Notes", "BusinessCardNotes" }
        };
        public BulkExcelUpload(IRepository<BusinessCardExcelModelCreate> repository)
        {
            _repository = repository;
            _validator = new BusinessCardValidator();
        }
        public async Task BulkInsertAsync(IEnumerable<BusinessCardExcelModelCreate> data, string createId)
        {
            _createId = createId;

            var batch = new List<BusinessCardExcelModelCreate>(BatchSize);
            var validationErrors = new List<string>();
            int rowIndex = 1;

            foreach (var item in data)
            {
                item.CreateId = _createId;

                var errors = _validator.Validate(item);
                if (errors.Count > 0)
                {
                    foreach (var err in errors)
                        validationErrors.Add($"Row {rowIndex}: {err}");
                }
                else
                {
                    batch.Add(item);
                    if (batch.Count >= BatchSize)
                    {
                        await BulkInsertBatchAsync(batch);
                        batch.Clear();
                    }
                }
                rowIndex++;
            }

            if (batch.Count > 0)
            {
                await BulkInsertBatchAsync(batch);
            }

            if (validationErrors.Any())
            {
                throw new Exception($"Validation failed for some rows: " +
                    $"{string.Join("; ", validationErrors)}");
            }

            await _repository.ExecCommand("Sp_Process_Staging_Business_Cards_Insert", new { });
        }
        public async Task<List<BusinessCardExcelModelCreate>> ConvertDataTableToListAsync(DataTable dataTable, string createId)
        {
            foreach (var col in _excelColumnToPropertyMap.Keys)
            {
                if (!dataTable.Columns.Contains(col))
                    throw new Exception($"Missing required Excel column: {col}");
            }

            var list = new List<BusinessCardExcelModelCreate>();

            foreach (DataRow row in dataTable.Rows)
            {
                var card = new BusinessCardExcelModelCreate
                {
                    BusinessCardName = row["Card Name"]?.ToString(),
                    BusinessCardTitle = row["Card Title"]?.ToString(),
                    BusinessCardPhone = row["Card Phone"]?.ToString(),
                    BusinessCardEmail = row["Card Email"]?.ToString(),
                    BusinessCardCompany = row["Card Company"]?.ToString(),
                    BusinessCardWebsite = row["Card Website"]?.ToString(),
                    BusinessCardAddress = row["Card Address"]?.ToString(),
                    BusinessCardNotes = row["Card Notes"]?.ToString(),
                    CreateId = createId
                };
                list.Add(card);
            }
            return await Task.FromResult(list);
        }
        public async Task<List<string>> ValidateDataAsync(DataTable dataTable, string createId)
        {
            var errors = new List<string>();

            if (!dataTable.Columns.Contains("CreateId"))
                dataTable.Columns.Add("CreateId", typeof(string));

            foreach (DataRow row in dataTable.Rows)
                row["CreateId"] = createId;

            var cards = await ConvertDataTableToListAsync(dataTable, createId);

            for (int i = 0; i < cards.Count; i++)
            {
                var validationResults = _validator.Validate(cards[i]);
                if (validationResults.Count > 0)
                {
                    foreach (var error in validationResults)
                    {
                        errors.Add($"Row {i + 2}: {error}");
                    }
                }
            }

            return errors;
        }
        private async Task BulkInsertBatchAsync(List<BusinessCardExcelModelCreate> batch)
        {
            var dt = ToDataTable(batch);
            await _repository.BulkInsertAsync(dt, "StagingBusinessCard");
        }
        private DataTable ToDataTable(IEnumerable<BusinessCardExcelModelCreate> data)
        {
            var dt = new DataTable();
            dt.Columns.Add("BusinessCardName", typeof(string));
            dt.Columns.Add("BusinessCardTitle", typeof(string));
            dt.Columns.Add("BusinessCardPhone", typeof(string));
            dt.Columns.Add("BusinessCardEmail", typeof(string));
            dt.Columns.Add("BusinessCardCompany", typeof(string));
            dt.Columns.Add("BusinessCardWebsite", typeof(string));
            dt.Columns.Add("BusinessCardAddress", typeof(string));
            dt.Columns.Add("BusinessCardNotes", typeof(string));
            dt.Columns.Add("CreateId", typeof(string));

            foreach (var card in data)
            {
                dt.Rows.Add(
                    card.BusinessCardName ?? string.Empty,
                    card.BusinessCardTitle ?? string.Empty,
                    card.BusinessCardPhone ?? string.Empty,
                    card.BusinessCardEmail ?? string.Empty,
                    card.BusinessCardCompany ?? string.Empty,
                    card.BusinessCardWebsite ?? string.Empty,
                    card.BusinessCardAddress ?? string.Empty,
                    card.BusinessCardNotes ?? string.Empty,
                    card.CreateId ?? string.Empty);
            }

            return dt;
        }
        public HeaderValidationResult ValidateHeadersAsync(DataTable dataTable)
        {
            var missingHeaders = new List<string>();

            foreach (var col in _excelColumnToPropertyMap.Keys)
            {
                if (!dataTable.Columns.Contains(col))
                    missingHeaders.Add(col);
            }

            if (missingHeaders.Any())
            {
                return new HeaderValidationResult
                {
                    IsValid = false,
                    MissingHeaders = missingHeaders,
                    Message = $"Missing required Excel columns: {string.Join(", ", missingHeaders)}"
                };
            }

            return new HeaderValidationResult { IsValid = true };
        }

    }
}
