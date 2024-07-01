using Microsoft.EntityFrameworkCore;

namespace FileService;

public class Context : DbContext
{
    public Context(DbContextOptions<Context> options) : base(options)
    {}
    public DbSet<FileEntity> File { get; set; }
}
