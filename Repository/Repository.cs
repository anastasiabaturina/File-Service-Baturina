using Microsoft.EntityFrameworkCore;

namespace FileService;

public class Repository : IRepository
{
    private readonly Context _context;

    public Repository(Context context)
    {
        _context = context;
    }

    public async Task SaveFile(File file)
    {
        await _context.File.AddAsync(file);
        await _context.SaveChangesAsync(); 
    }

    public async Task<string> GetHashPassword(string uniqueName)
    {
        var file = await _context.File.FirstOrDefaultAsync(x => x.UniqueName == uniqueName);

        if (file == null)
        {
            throw new FileNotFoundException();
        }

        return file.Password;
    }

    public async Task DeleteFile(string uniqueName)
    {
        var file = await _context.File.FirstOrDefaultAsync(x => x.UniqueName == uniqueName);

        if (file == null)
        {
            throw new FileNotFoundException();
        }

        _context.File.Remove(file);
        await _context.SaveChangesAsync();
    }

    public async Task DeletionByDate()
    {
        _context.File.RemoveRange(_context.File.Where(f => f.UploadDateTime < DateTime.UtcNow.AddDays(-1)));
        await _context.SaveChangesAsync();
    }
}
