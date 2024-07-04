using FileService.Models;
using Microsoft.EntityFrameworkCore;

namespace FileService;

public class Context : DbContext
{
    public Context(DbContextOptions<Context> options) : base(options)
    {}
    public DbSet<Document> Files { get; set; }
}
