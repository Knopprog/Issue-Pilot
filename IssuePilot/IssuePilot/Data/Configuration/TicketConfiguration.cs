using IssuePilot.Models.DBModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace IssuePilot.Data.Configuration
{
    public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.ToTable("Tickets");
            builder.HasData(
                new
                {
                    Id = 1,
                    CreateDate = DateTime.Now,
                    Title = "TestTicket",
                    Description = "Ein vom System generiertes Ticket zum Testen.",
                    TicketCreatorId = "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1",
                    TicketStatusId = TicketStatusId.Open,
                    Weight = 1,
                    ProjectId = 1,
                    Deadline = DateTime.Now.AddYears(1)
                },
                new
                {
                    Id = 2,
                    CreateDate = DateTime.Now,
                    Title = "TestTicket 2",
                    Description = "Ein vom System generiertes Ticket zum Testen. Ein vom System generiertes Ticket zum Testen. Ein vom System generiertes Ticket zum Testen. Ein vom System generiertes Ticket zum Testen. Ein vom System generiertes Ticket zum Testen. Ein vom System generiertes Ticket zum Testen. Ein vom System generiertes Ticket zum Testen. Ein vom System generiertes Ticket zum Testen. Ein vom System generiertes Ticket zum Testen. Ein vom System generiertes Ticket zum Testen. Ein vom System generiertes Ticket zum Testen.",
                    TicketCreatorId = "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1",
                    TicketStatusId = TicketStatusId.Paused,
                    Weight = 2,
                    ProjectId = 1,
                },
                new
                {
                    Id = 3,
                    CreateDate = DateTime.Now,
                    Title = "TestTicket 3",
                    Description = "Ein vom System generiertes Ticket zum Testen.",
                    TicketCreatorId = "B22698B8-42A2-4115-9631-1C2D1E2AC5C5",
                    TicketStatusId = TicketStatusId.InProgress,
                    Weight = 3,
                    ProjectId = 1,
                    Deadline = DateTime.Now
                },
                new
                {
                    Id = 4,
                    CreateDate = DateTime.Now,
                    Title = "TestTicket 4",
                    Description = "Ein vom System generiertes Ticket zum Testen.",
                    TicketCreatorId = "B22698B8-42A2-4115-9631-1C2D1E2AC5C5",
                    TicketStatusId = TicketStatusId.Canceled,
                    Weight = 2,
                    ProjectId = 1
                },
                new
                {
                    Id = 5,
                    CreateDate = DateTime.Now,
                    Title = "TestTicket 5",
                    Description = "Ein vom System generiertes Ticket zum Testen.",
                    TicketCreatorId = "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1",
                    TicketStatusId = TicketStatusId.Closed,
                    Weight = 0,
                    ProjectId = 1,
                    CloseDate = DateTime.Now,
                    Deadline = DateTime.Now.AddDays(2),
                    ClosedByUserId = "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1"
                },
                new
                {
                    Id = 6,
                    CreateDate = DateTime.Now,
                    Title = "TestTicket 6",
                    Description = "Ein vom System generiertes Ticket zum Testen.",
                    TicketCreatorId = "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1",
                    TicketStatusId = TicketStatusId.Open,
                    Weight = 1,
                    ProjectId = 1
                },
                new
                {
                    Id = 7,
                    CreateDate = DateTime.Now,
                    Title = "TestTicket 7",
                    Description = "Ein vom System generiertes Ticket zum Testen.",
                    TicketCreatorId = "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1",
                    TicketStatusId = TicketStatusId.Open,
                    Weight = 1,
                    ProjectId = 1
                },
                new
                {
                    Id = 8,
                    CreateDate = DateTime.Now,
                    Title = "TestTicket 8",
                    Description = "Ein vom System generiertes Ticket zum Testen.",
                    TicketCreatorId = "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1",
                    TicketStatusId = TicketStatusId.Open,
                    Weight = 1,
                    ProjectId = 1
                },
                new
                {
                    Id = 9,
                    CreateDate = DateTime.Now,
                    Title = "TestTicket 9",
                    Description = "Ein vom System generiertes Ticket zum Testen.",
                    TicketCreatorId = "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1",
                    TicketStatusId = TicketStatusId.Open,
                    Weight = 1,
                    ProjectId = 1
                },
                new
                {
                    Id = 10,
                    CreateDate = DateTime.Now,
                    Title = "TestTicket 10",
                    Description = "Ein vom System generiertes Ticket zum Testen.",
                    TicketCreatorId = "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1",
                    TicketStatusId = TicketStatusId.Open,
                    Weight = 1,
                    ProjectId = 1
                },
                new
                {
                    Id = 11,
                    CreateDate = DateTime.Now,
                    Title = "TestTicket 11",
                    Description = "Ein vom System generiertes Ticket zum Testen.",
                    TicketCreatorId = "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1",
                    TicketStatusId = TicketStatusId.Open,
                    Weight = 1,
                    ProjectId = 1
                },
                new
                {
                    Id = 12,
                    CreateDate = DateTime.Now,
                    Title = "TestTicket 12",
                    Description = "Ein vom System generiertes Ticket zum Testen.",
                    TicketCreatorId = "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1",
                    TicketStatusId = TicketStatusId.Open,
                    Weight = 1,
                    ProjectId = 1
                },
                new
                {
                    Id = 13,
                    CreateDate = DateTime.Now,
                    Title = "TestTicket 13",
                    Description = "Ein vom System generiertes Ticket zum Testen.",
                    TicketCreatorId = "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1",
                    TicketStatusId = TicketStatusId.Open,
                    Weight = 1,
                    ProjectId = 1
                });
        }
    }
}
