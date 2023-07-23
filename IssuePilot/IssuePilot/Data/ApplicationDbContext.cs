using IssuePilot.Data.Configuration;
using IssuePilot.Models.DBModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace IssuePilot.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<NewsEntry> NewsEntries { get; set; }
        public DbSet<NewsEntryCase> NewsEntryCases { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TicketCategory> TicketCategories { get; set; }
        public DbSet<ProjectMemberEntry> ProjectMemberEntries { get; set; }
        public DbSet<ProjectRole> ProjectRoles { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketHistoryEntry> TicketHistoryEntries { get; set; }
        public DbSet<TicketHistoryEntryCase> TicketHistoryEntryCases { get; set; }
        public DbSet<TicketStatus> TicketStatuses { get; set; }
        public override DbSet<User> Users { get; set; }
        public DbSet<TicketAssignee> TicketAssignees { get; set; }
        public DbSet<TicketProjectCategory> TicketProjectCategories { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.UseIdentityColumns();
            builder.ApplyConfiguration(new AspNetRoleConfiguration());
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new UsersWithRolesConfig());
            builder.ApplyConfiguration(new ProjectConfiguration());
            builder.ApplyConfiguration(new ProjectMemberEntriesConfiguration());
            builder.ApplyConfiguration(new TicketConfiguration());
            builder.ApplyConfiguration(new TicketCategoriesConfiguration());
            builder.ApplyConfiguration(new TicketProjectCategoryConfiguration());
            builder.ApplyConfiguration(new NewsEntryConfiguration());

            builder.Entity<User>().Property(x => x.Email).HasMaxLength(254);

            // *** Association tables ***

            builder.Entity<Project>().HasMany(c => c.ProjectMemberEntries).WithOne(e => e.Project);
            builder.Entity<User>().HasMany(c => c.ProjectMemberEntries).WithOne(e => e.User);
            builder.Entity<ProjectMemberEntry>().HasKey(c => new { c.FK_ProjectId, c.FK_UserId });

            builder.Entity<TicketProjectCategory>()
                .HasKey(tpc => new { tpc.TicketId, tpc.TicketCategoryId });
            builder.Entity<TicketProjectCategory>()
                .HasOne(e => e.Ticket)
                .WithMany(t => t.TicketProjectCategories)
                .HasForeignKey(t => t.TicketId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<TicketProjectCategory>()
                .HasOne(e => e.TicketCategory)
                .WithMany(t => t.TicketProjectCategories)
                .HasForeignKey(e => e.TicketCategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Comment>()
                .HasMany(c => c.NewsEntries)
                .WithOne(n => n.Comment)
                .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<Ticket>().HasMany(c => c.TicketAssignees).WithOne(e => e.Ticket);
            builder.Entity<User>().HasMany(c => c.TicketAssignees).WithOne(e => e.User);
            builder.Entity<TicketAssignee>().HasKey(c => new { c.FK_TicketId, c.FK_UserId });

            builder.Entity<Project>().HasOne(c => c.Creator).WithMany(c => c.Projects);

            builder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.UserName).IsUnique();
            });

            builder.Entity<Project>(entity =>
            {
                entity.HasIndex(e => e.Title).IsUnique();
            });

            // Enum Tables

            builder.Entity<TicketHistoryEntry>().Property(e => e.TicketHistoryEntryCaseId).HasConversion<int>();
            builder.Entity<TicketHistoryEntryCase>().Property(e => e.EntryCaseId).HasConversion<int>();
            builder.Entity<TicketHistoryEntryCase>().HasData(
                    Enum.GetValues(typeof(TicketHistoryEntryCaseId))
                        .Cast<TicketHistoryEntryCaseId>()
                        .Select(e => new TicketHistoryEntryCase()
                        {
                            EntryCaseId = e,
                            Name = e.ToString()
                        })
                );

            builder.Entity<NewsEntry>().Property(e => e.NewsEntryCaseId).HasConversion<int>();
            builder.Entity<NewsEntryCase>().Property(e => e.EntryCaseId).HasConversion<int>();
            builder.Entity<NewsEntryCase>().HasData(
                    Enum.GetValues(typeof(NewsEntryCaseId))
                        .Cast<NewsEntryCaseId>()
                        .Select(e => new NewsEntryCase()
                        {
                            EntryCaseId = e,
                            Name = e.ToString()
                        })
                );

            builder.Entity<Ticket>().Property(e => e.TicketStatusId).HasConversion<int>();
            builder.Entity<TicketStatus>().Property(e => e.Id).HasConversion<int>();
            builder.Entity<TicketStatus>().HasData(
                    Enum.GetValues(typeof(TicketStatusId))
                        .Cast<TicketStatusId>()
                        .Select(e => new TicketStatus()
                        {
                            Id = e,
                            Name = e.ToString()
                        })
                );

            builder.Entity<ProjectMemberEntry>().Property(e => e.ProjectRoleId).HasConversion<int>();
            builder.Entity<ProjectRole>().Property(e => e.Id).HasConversion<int>();
            builder.Entity<ProjectRole>().HasData(
                    Enum.GetValues(typeof(ProjectRoleId))
                        .Cast<ProjectRoleId>()
                        .Select(e => new ProjectRole()
                        {
                            Id = e,
                            Title = e.ToString()
                        })
                );
        }
    }
}
