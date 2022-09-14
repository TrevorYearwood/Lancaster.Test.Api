using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Markerstudy.Lancaster.Application.Contracts.Infrastructure;
using MediatR;

namespace Markerstudy.Lancaster.Application.Features.File.Queries.FindFiles
{
    public class FindFilesQueryHandler : IRequestHandler<FindFilesQuery, FileQueryResponse>
    {
        private readonly IMapper _mapper;
        private readonly IAsyncRepository<Domain.Models.File> _repository;

        public FindFilesQueryHandler(IMapper mapper, IAsyncRepository<Domain.Models.File> repository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<FileQueryResponse> Handle(FindFilesQuery query, CancellationToken cancellationToken)
        {
            var fileQueryResponse = new FileQueryResponse();

            var result = await _repository.ListAllAsync(f => f.Name.Equals(query.FileName, StringComparison.InvariantCultureIgnoreCase), cancellationToken);

            if (result.Count > 0)
            {
                fileQueryResponse.Success = false;
                fileQueryResponse.Message = "File has already been upload";
            }
            else
            {
                var file = new Domain.Models.File
                {
                    Name = query.FileName
                };

                var newFile = await _repository.AddAsync(file);

                fileQueryResponse.Success = true;
                fileQueryResponse.Message = $"file id - {newFile.Id}";
            }

            return fileQueryResponse;
        }
    }
}
