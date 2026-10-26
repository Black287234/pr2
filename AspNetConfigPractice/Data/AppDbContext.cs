using AspNetConfigPractice.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetConfigPractice.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Student> Students => Set<Student>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Начальные данные: появятся в базе после dotnet ef database update.
        modelBuilder.Entity<Student>().HasData(
            new Student { Id = 1, Name = "Анна", Course = 2 },
            new Student { Id = 2, Name = "Илья", Course = 3 },
            new Student { Id = 3, Name = "Сергей", Course = 1 });
    }
}
