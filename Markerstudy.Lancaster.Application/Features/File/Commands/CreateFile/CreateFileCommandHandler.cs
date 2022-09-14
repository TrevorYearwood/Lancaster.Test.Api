using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Markerstudy.Lancaster.Application.Contracts.Infrastructure;
using MediatR;

namespace Markerstudy.Lancaster.Application.Features.File.Commands.CreateFile
{
    public class CreateFileCommandHandler : IRequestHandler<CreateFileCommand, Guid>
    {
        private readonly IMapper _mapper;
        private readonly IAsyncRepository<Domain.Models.File> _fileRepository;
        private readonly IBlobStorageService _blobStorageService;

        public CreateFileCommandHandler(IMapper mapper, IAsyncRepository<Domain.Models.File> fileRepository, IBlobStorageService blobService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _fileRepository = fileRepository ?? throw new ArgumentNullException(nameof(fileRepository));
            _blobStorageService = blobService ?? throw new ArgumentNullException(nameof(blobService));
        }

        public async Task<Guid> Handle(CreateFileCommand command, CancellationToken cancellationToken)
        {
            var validator = new CreateFileCommandValidator();
            var validatorResult = await validator.ValidateAsync(command);

            if (validatorResult.Errors.Count > 0)
                throw new ArgumentNullException();

            await _blobStorageService.UploadFileToBlobContainer(command);

            //if (isFileInBlobStorage)
            //{
            //    throw new Exception("File has already been completed");
            //}

            //update to blob storage

            var file = _mapper.Map<Domain.Models.File>(command);

            file = await _fileRepository.AddAsync(file);

            return file.Id;
        }
    }
}
