using FlowDesk.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlowDesk.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<TaskItem> Tasks => Set<TaskItem>();
    public DbSet<ProjectMember> ProjectMembers => Set<ProjectMember>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // User
modelBuilder.Entity<User>(e =>
{
    e.HasKey(u => u.Id);
    e.Property(u => u.Name).IsRequired().HasMaxLength(100);
    e.Property(u => u.Email).IsRequired().HasMaxLength(200);
    e.Property(u => u.EmployeeId).IsRequired().HasMaxLength(20);
    e.Property(u => u.Role).HasConversion<string>();

    e.HasIndex(u => u.Email).IsUnique();
    e.HasIndex(u => u.EmployeeId).IsUnique();  // ← no duplicates
});

        // Project
        modelBuilder.Entity<Project>(e =>
        {
            e.HasKey(p => p.Id);
            e.Property(p => p.Name).IsRequired().HasMaxLength(200);
            e.HasOne(p => p.CreatedBy)
             .WithMany(u => u.CreatedProjects)
             .HasForeignKey(p => p.CreatedByUserId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // TaskItem
        modelBuilder.Entity<TaskItem>(e =>
        {
            e.HasKey(t => t.Id);
            e.Property(t => t.Title).IsRequired().HasMaxLength(300);
            e.Property(t => t.Priority).HasConversion<string>();  // store as string in DB
            e.Property(t => t.Status).HasConversion<string>();    // store as string in DB

            e.HasOne(t => t.Project)
             .WithMany(p => p.Tasks)
             .HasForeignKey(t => t.ProjectId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(t => t.AssignedTo)
             .WithMany(u => u.AssignedTasks)
             .HasForeignKey(t => t.AssignedToUserId)
             .OnDelete(DeleteBehavior.SetNull);
        });

        // ProjectMember
        modelBuilder.Entity<ProjectMember>(e =>
{
    e.HasKey(pm => pm.Id);
    e.Property(pm => pm.Role).HasConversion<string>();

    e.HasIndex(pm => new { pm.ProjectId, pm.UserId }).IsUnique();

    e.HasOne(pm => pm.Project)
        .WithMany(p => p.ProjectMembers)
        .HasForeignKey(pm => pm.ProjectId)
        .OnDelete(DeleteBehavior.Cascade);

    e.HasOne(pm => pm.User)
        .WithMany(u => u.ProjectMemberships)
        .HasForeignKey(pm => pm.UserId)
        .OnDelete(DeleteBehavior.Cascade);
});
    }
}