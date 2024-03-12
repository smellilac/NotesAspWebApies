using EntityTypeConfiguration;
using Microsoft.EntityFrameworkCore;
using Notes.Application;
using Notes.Domain;

namespace Notes.WebApi.PersistanceLevel;
public class NotesDbContext : DbContext, INotesDbContext
{
    public DbSet<Note> Notes { get; set; }

    public NotesDbContext(DbContextOptions<NotesDbContext> options)
        : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new NoteConfiguration());
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=aspnet-53bc9b9d-9d6a-45d4-8429-2a2761773502;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
    }
}