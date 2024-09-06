using System.Diagnostics.CodeAnalysis;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using StarterKit.Maui.Core.Infrastructure.Platform;
using StarterKit.Maui.Features.Post.Domain.Models;

namespace StarterKit.Maui.App.Database;

[ExcludeFromCodeCoverage]
public class AppDbContext : DbContext
{
    private readonly IPathProvider _pathProvider;

    public AppDbContext(IPathProvider pathProvider)
    {
        _pathProvider = pathProvider;

        Batteries_V2.Init();

        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string databasePath = _pathProvider.DatabasePath;
        SqliteConnectionStringBuilder connection = new SqliteConnectionStringBuilder
        {
            DataSource = databasePath,
            Mode = SqliteOpenMode.ReadWriteCreate
        };

        optionsBuilder.UseSqlite(new SqliteConnection(connection.ToString()));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new PostEntityConfiguration());
    }
}