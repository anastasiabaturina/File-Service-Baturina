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

    public async Task SaveAsync(Document document, CancellationToken cancellationToken)
    {
        await _context.Files.AddAsync(document, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken); 
    }

    public async Task<Document> GetAsync(string uniqueName, CancellationToken cancellationToken)
    {
        var file = await _context.Files.FirstOrDefaultAsync(x => x.UniqueName == uniqueName, cancellationToken);

        return file;
    }

    public async Task DeleteAsync(Document document, CancellationToken cancellationToken)
    {
        _context.Files.Remove(document);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteFilesByDateTimeAsync(DateTime timeInterval, CancellationToken cancellationToken)
    {
        await _context.Files.Where(f => f.UploadDateTime < timeInterval)
            .ExecuteDeleteAsync(cancellationToken);
        await _context.SaveChangesAsync();
    }
}
