using AutoMapper;
using FileService.Models.Dtos;
using FileService.Models.Requests;
using FileService.Models.Responses;
using FileService.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.IO;


namespace FileService.Controllers;

[ApiController]
[Route("api/v1/files")]
public class FileController : ControllerBase
{
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;

    public FileController(IFileService fileService, IMapper mapper)
    {
        _fileService = fileService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> UploadFile([FromForm] UploadFileRequest uploadFileRequest, CancellationToken cancellationToken)
    {
        var uploadFileDto = _mapper.Map<UploadFileDto>(uploadFileRequest);
        var uniqueFileName = await _fileService.SaveAsync(uploadFileDto, cancellationToken);

        var response = new Response<UploadFileResponse>
        {
            Data = uniqueFileName
        };

        return CreatedAtAction(nameof(UploadFile), response);
    }

    [HttpGet("{fileName}")]
    public async Task<IActionResult> DownloadFile([FromRoute] string fileName, CancellationToken cancellationToken)
    {
        var fileDto = await _fileService.GetAsync(fileName, cancellationToken);

        return File(fileDto, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteFile(DeleteFileRequest deleteFileRequest, CancellationToken cancellationToken)
    {
        var deleteFileDto = _mapper.Map<DeleteFileDto>(deleteFileRequest);
        await _fileService.DeleteAsync(deleteFileDto, cancellationToken);

        return Ok();
    }
}
