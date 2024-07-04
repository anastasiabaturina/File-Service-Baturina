using AutoMapper;
using FileService.Models.Dto_s;
using FileService.Models.Request;
using FileService.Models.UploadFileDto;

namespace FileService.Automapper;
public class MapFile : Profile
{
    public MapFile()
    {
        CreateMap<UploadFileRequest, UploadFileDto>();
        CreateMap<DeleteFileRequest, DeleteFileDto>();
    }
}

