using IssuePilot.Models.DBModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IssuePilot.Data.Configuration
{
    public class TicketCategoriesConfiguration : IEntityTypeConfiguration<TicketCategory>
    {

        public void Configure(EntityTypeBuilder<TicketCategory> builder)
        {
            builder.ToTable("TicketCategories");
            builder.HasData(
                new
                {
                    Id = 1,
                    Name = "Bug",
                    ProjectId = 1
                },
                new
                {
                    Id = 2,
                    Name = "Frage",
                    ProjectId = 1
                },
                 new
                 {
                     Id = 3,
                     Name = "Dokumentation",
                     ProjectId = 1
                 },
                new
                {
                    Id = 4,
                    Name = "Diskussion",
                    ProjectId = 1
                },
                new
                {
                    Id = 5,
                    Name = "Feature",
                    ProjectId = 1
                },
                new
                {
                    Id = 6,
                    Name = "Bug",
                    ProjectId = 2
                },
                new
                {
                    Id = 7,
                    Name = "Frage",
                    ProjectId = 2
                },
                 new
                 {
                     Id = 8,
                     Name = "Dokumentation",
                     ProjectId = 2
                 },
                new
                {
                    Id = 9,
                    Name = "Diskussion",
                    ProjectId = 2
                },
                new
                {
                    Id = 10,
                    Name = "Feature",
                    ProjectId = 2
                });
        }
    }
}
