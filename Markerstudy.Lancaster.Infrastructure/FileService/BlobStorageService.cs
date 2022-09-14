using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Markerstudy.Lancaster.Application.Contracts.Infrastructure;
using Markerstudy.Lancaster.Application.Features.File.Commands.CreateFile;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Markerstudy.Lancaster.Infrastructure.FileService
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly ILogger<BlobStorageService> _logger;
        private readonly IConfiguration _configuration;

        private readonly BlobContainerClient _containerClient;

        public BlobStorageService(ILogger<BlobStorageService> logger, IConfiguration configuration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _containerClient = new BlobContainerClient(
                                        _configuration["BlobConnection"],
                                        _configuration["BlobContainerName"]);
        }

        public async Task<bool> IsFileInBlobContainer(string filename)
        {
            try
            {
                bool isFileFound = false;

                await foreach (BlobItem blobItem in _containerClient.GetBlobsAsync())
                {
                    if (blobItem.Name == filename)
                        isFileFound = true;
                }

                return isFileFound;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<Stream?> GetFileFromBlobContainer(string filename)
        {
            try
            {
                var blobClient = _containerClient.GetBlobClient(filename);

                if (await blobClient.ExistsAsync())
                {
                    BlobDownloadInfo downloadedFile = await blobClient.DownloadAsync();

                    return downloadedFile.Content;
                }

                throw new FileNotFoundException("File not found", filename);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task UploadFileToBlobContainer(CreateFileCommand createFileCommand)
        {
            try
            {
                BlobClient blobClient = _containerClient.GetBlobClient(createFileCommand.Filename);

                using (var stream = new MemoryStream(createFileCommand.FileData))
                    await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = "text/csv" });
                //await blob.UploadFromStreamAsync(stream);

                // set the type after the upload, otherwise will get an error that blob does not exist
                //await blobClient.SetHttpHeadersAsync(new BlobHttpHeaders { ContentType = "text/csv" });

                //uploadFileStream.Close();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
