using FileService.Models;
using Microsoft.EntityFrameworkCore;

namespace FileService;

public class FileRepository : IFileRepository
{
    private readonly DocumentContext _context;

    public FileRepository(DocumentContext context, IConfiguration configuration)
    {
        _context = context;
    }

    public async Task SaveAsync(Document file, CancellationToken cancellationToken)
    {
        await _context.Files.AddAsync(file, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken); 
    }

    public async Task<Document> GetAsync(string uniqueName, CancellationToken cancellationToken)
    {
        var file = await _context.Files.FirstOrDefaultAsync(x => x.UniqueName == uniqueName, cancellationToken);

        return file;
    }

    public async Task DeleteAsync(string uniqueName, CancellationToken cancellationToken)
    {
        var file = await _context.Files.FirstOrDefaultAsync(x => x.UniqueName == uniqueName, cancellationToken);

        _context.Files.Remove(file);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteFilesByDateTimeAsync(DateTime timeInterval, CancellationToken cancellationToken)
    {
        await _context.Files.Where(f => f.UploadDateTime < timeInterval)
            .ExecuteDeleteAsync(cancellationToken);
        await _context.SaveChangesAsync();
    }
}
