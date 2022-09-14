using System.IO;
using System.Threading.Tasks;
using Markerstudy.Lancaster.Application.Features.File.Commands.CreateFile;

namespace Markerstudy.Lancaster.Application.Contracts.Infrastructure
{
    public interface IBlobStorageService
    {
        Task<bool> IsFileInBlobContainer(string filename);
        Task<Stream> GetFileFromBlobContainer(string filename);
        Task UploadFileToBlobContainer(CreateFileCommand createFileCommand);
    }
}