using IssuePilot.Models.DBModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IssuePilot.Data.Configuration
{
    public class TicketProjectCategoryConfiguration : IEntityTypeConfiguration<TicketProjectCategory>
    {
        public void Configure(EntityTypeBuilder<TicketProjectCategory> builder)
        {
            builder.ToTable("TicketProjectCategories");
            builder.HasData(
                new
                {
                    TicketId = 1,
                    TicketCategoryId = 2
                },
                new
                {
                    TicketId = 2,
                    TicketCategoryId = 2
                },
                new
                {
                    TicketId = 2,
                    TicketCategoryId = 1
                },
                new
                {
                    TicketId = 3,
                    TicketCategoryId = 2
                },
                new
                {
                    TicketId = 4,
                    TicketCategoryId = 3
                },
                new
                {
                    TicketId = 5,
                    TicketCategoryId = 4
                });
        }
    }
}