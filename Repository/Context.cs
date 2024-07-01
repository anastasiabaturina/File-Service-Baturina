using Microsoft.EntityFrameworkCore;

namespace File_Service
{ 
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {}
        public DbSet<FileEntity> File { get; set; }
    }
}
