using AutoMapper;
using FileService.Models;
using FileService.Models.Dtos;
using FileService.Models.Requests;
using FileService.Models.Responses;

namespace FileService.Automapper;
public class ApiMappingProfile : Profile
{
    public ApiMappingProfile()
    {
        CreateMap<UploadFileRequest, UploadFileDto>()
            .ForMember(desc => desc.Password, opt => opt.MapFrom(src => src.Password))
            .ForMember(desc => desc.File, opt => opt.MapFrom(src => src.File));

        CreateMap<DeleteFileRequest, DeleteFileDto>()
            .ForMember(desc => desc.Password, opt => opt.MapFrom(src => src.Password))
            .ForMember(desc => desc.UniqueName, opt => opt.MapFrom(src => src.UniqueName)); 

        CreateMap<Document, UploadFileResponse>()
            .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.UniqueName));
    }
}

