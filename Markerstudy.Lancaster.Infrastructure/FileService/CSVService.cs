using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.TypeConversion;
using Markerstudy.Lancaster.Application.Contracts.Infrastructure;
using Markerstudy.Lancaster.Application.Exceptions;
using Markerstudy.Lancaster.Domain.Models;
using Microsoft.Extensions.Logging;

namespace Markerstudy.Lancaster.Infrastructure.FileService
{
    public class CSVService : ICSVService
    {
        private readonly ILogger<CSVService> _logger;
        private readonly IBlobStorageService _blobStorageService;

        public CSVService(ILogger<CSVService> logger, IBlobStorageService blobStorageService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _blobStorageService = blobStorageService ?? throw new ArgumentNullException(nameof(blobStorageService));
        }

        public async Task<IEnumerable<Valuation>> GetRecordsAsync(string filename)
        {
            var records = new List<Valuation>();

            try
            {
                Stream file = await _blobStorageService.GetFileFromBlobContainer(filename);

                if (file != null)
                {
                    var rowExceptions = new List<TypeConverterException>();

                    using var reader = new StreamReader(file);
                    using var csv = new CsvReader(reader, CultureInfo.CurrentCulture);
                    while (csv.Read())
                    {
                        try
                        {
                            var record = csv.GetRecord<Valuation>();
                            records.Add(record);
                        }
                        catch (TypeConverterException exType)
                        {
                            rowExceptions.Add(exType);
                        }
                    }

                    if (rowExceptions.Count > 0)
                        throw new RowValidationExeception(rowExceptions);
                }

                return records;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, ex.Message);
                throw;
            }
        }
    }
}