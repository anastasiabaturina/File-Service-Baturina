using FileService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FileService;

public class Repository : IRepository
{
    private readonly DocumentContext _context;

    public Repository(DocumentContext context, IConfiguration configuration)
    {
        _context = context;
    }

    public async Task SaveAsync(Document file)
    {
        await _context.Files.AddAsync(file);
        await _context.SaveChangesAsync(); 
    }

    public async Task<Document> GetAsync(string uniqueName)
    {
        var file = await _context.Files.FirstOrDefaultAsync(x => x.UniqueName == uniqueName);

        if (file == null)
        {
            throw new FileNotFoundException();
        }

        return file;
    }

    public async Task DeleteAsync(string uniqueName)
    {
        var file = await _context.Files.FirstOrDefaultAsync(x => x.UniqueName == uniqueName);

        if (file == null)
        {
            throw new FileNotFoundException();
        }

        _context.Files.Remove(file);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Document>> GetFilesByDateTimeAsync(DateTime timeInterval)
    {
        return await _context.Files.Where(f => f.UploadDateTime < timeInterval).ToListAsync();
    }

    public async Task RemoveListAsync(List<Document> files)
    {
        _context.Files.RemoveRange(files);
        await _context.SaveChangesAsync();
    }
}
