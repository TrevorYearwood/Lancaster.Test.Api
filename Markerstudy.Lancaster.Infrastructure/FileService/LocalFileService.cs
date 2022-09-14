using System;
using System.IO;
using Markerstudy.Lancaster.Application.Contracts.Infrastructure;
using Microsoft.Extensions.Logging;

namespace Markerstudy.Lancaster.Infrastructure.FileService
{
    public class LocalFileService :ILocalFileService
    {
        private readonly ILogger<LocalFileService> _logger;
        private const string _dataFolder = "Data";

        public LocalFileService(ILogger<LocalFileService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public string GetFilePath(string filename)
        {
            var envDir = Environment.CurrentDirectory;
            var path = Path.Combine(envDir, _dataFolder);
            var filePath = Path.Combine(path, filename);

            if (!File.Exists(filePath))
            {
                var errorMessage = "File Not Found";

                _logger.LogError($"{errorMessage} - {filePath}");
                throw new FileNotFoundException(errorMessage, filePath);
            }

            return filePath;
        }
    }
}
