using AutoMapper;
using Markerstudy.Lancaster.Application.Features.File.Commands.CreateFile;
using Markerstudy.Lancaster.Application.Features.Files;
using Markerstudy.Lancaster.Application.Features.Valuation;
using Markerstudy.Lancaster.Domain.Models;

namespace Markerstudy.Lancaster.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<File, FileResponse>().ReverseMap();
            CreateMap<File, CreateFileCommand>().ReverseMap();
            CreateMap<Valuation, ValuationResponse>().ReverseMap();
        }
    }
}
