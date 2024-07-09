using AutoMapper;
using FileService.Models;
using FileService.Models.Dtos;
using FileService.Models.Request;
using FileService.Models.Response;
using FileService.Models.UploadFileDto;

namespace FileService.Automapper;
public class MapFile : Profile
{
    public MapFile()
    {
        CreateMap<UploadFileRequest, UploadFileDto>();
        CreateMap<DeleteFileRequest, DeleteFileDto>();
        CreateMap<Document, UploadFileResponse>()
            .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.UniqueName));
    }
}

