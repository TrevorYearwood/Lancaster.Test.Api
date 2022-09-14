using System;
using System.IO;
using MediatR;

namespace Markerstudy.Lancaster.Application.Features.File.Commands.CreateFile
{
    public class CreateFileCommand : IRequest<Guid>
    {
        public string Filename { get; set; }

        public byte[] FileData { get; set; }

        public Stream FileStream { get; set; }
    }
}
