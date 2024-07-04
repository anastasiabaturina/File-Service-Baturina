using FileService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FileService;

public class Repository : IRepository
{
    private readonly DocumentContext _context;
    private readonly int _timeInterval;

    public Repository(DocumentContext context, IConfiguration configuration)
    {
        _context = context;
        _timeInterval = configuration.GetValue<int>("Time:Hour");
    }

    public async Task Save(Document file)
    {
        await _context.Files.AddAsync(file);
        await _context.SaveChangesAsync(); 
    }

    public async Task<string> GetHashPassword(string uniqueName)
    {
        var file = await _context.Files.FirstOrDefaultAsync(x => x.UniqueName == uniqueName);

        if (file == null)
        {
            throw new FileNotFoundException();
        }

        return file.Password;
    }

    public async Task Delete(string uniqueName)
    {
        var file = await _context.Files.FirstOrDefaultAsync(x => x.UniqueName == uniqueName);

        if (file == null)
        {
            throw new FileNotFoundException();
        }

        _context.Files.Remove(file);
        await _context.SaveChangesAsync();
    }

    public async Task DeletionByDate()
    {
        _context.Files.RemoveRange(_context.Files.Where(f => f.UploadDateTime < DateTime.UtcNow.AddHours(-_timeInterval)));
        await _context.SaveChangesAsync();
    }
}
