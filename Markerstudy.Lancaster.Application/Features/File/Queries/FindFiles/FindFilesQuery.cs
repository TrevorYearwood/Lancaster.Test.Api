using MediatR;

namespace Markerstudy.Lancaster.Application.Features.File.Queries.FindFiles
{
    public class FindFilesQuery : IRequest<FileQueryResponse>
    {
        public string FileName { get; set; }
    }
}