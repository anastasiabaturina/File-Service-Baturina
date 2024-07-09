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
        CreateMap<UploadFileRequest, UploadFileDto>();
        CreateMap<DeleteFileRequest, DeleteFileDto>();
        CreateMap<Document, UploadFileResponse>()
            .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.UniqueName));
    }
}

