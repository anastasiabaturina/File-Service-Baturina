using FileService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Threading;

namespace FileService;

public class Repository : IRepository
{
    private readonly DocumentContext _context;

    public Repository(DocumentContext context, IConfiguration configuration)
    {
        _context = context;
    }

    public async Task SaveAsync(Document file, CancellationToken cancellationToken)
    {
        await _context.Files.AddAsync(file, cancellationToken);
        await _context.SaveChangesAsync(); 
    }

    public async Task<Document> GetAsync(string uniqueName, CancellationToken cancellationToken)
    {
        var file = await _context.Files.FirstOrDefaultAsync(x => x.UniqueName == uniqueName, cancellationToken);

        if (file == null)
        {
            throw new FileNotFoundException();
        }

        return file;
    }

    public async Task DeleteAsync(string uniqueName, CancellationToken cancellationToken)
    {
        var file = await _context.Files.FirstOrDefaultAsync(x => x.UniqueName == uniqueName, cancellationToken);

        if (file == null)
        {
            throw new FileNotFoundException();
        }

        _context.Files.Remove(file);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteFilesByDateTimeAsync(DateTime timeInterval, CancellationToken cancellationToken)
    {
        await _context.Files.Where(f => f.UploadDateTime < timeInterval)
            .ExecuteDeleteAsync(cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
