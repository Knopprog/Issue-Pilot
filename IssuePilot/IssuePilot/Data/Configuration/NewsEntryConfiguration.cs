using IssuePilot.Models.DBModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssuePilot.Data.Configuration
{
    public class NewsEntryConfiguration : IEntityTypeConfiguration<NewsEntry>
    {
        public void Configure(EntityTypeBuilder<NewsEntry> builder)
        {
            builder.ToTable("NewsEntries");
            builder.HasData(
                new
                {
                    Id = 1,
                    CreateDate = DateTime.Now,
                    Seen = false,
                    OwnerId = "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1",
                    ReferredUserId = "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1",
                    NewsEntryCaseId = NewsEntryCaseId.NewProject,
                    ProjectId = 1,
                    RouteId = 1,
                    ActionId = 1,
                    ControllerId = 1
                },
                new
                {
                    Id = 2,
                    CreateDate = DateTime.Now,
                    Seen = false,
                    OwnerId = "B22698B8-42A2-4115-9631-1C2D1E2AC5C5",
                    ReferredUserId = "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1",
                    NewsEntryCaseId = NewsEntryCaseId.AddedToProject,
                    ProjectId = 1,
                    RouteId = 1,
                    ActionId = 4,
                    ControllerId = 2
                }, new
                {
                    Id = 3,
                    CreateDate = DateTime.Now,
                    Seen = false,
                    OwnerId = "e28b8357-2955-4f3c-9c3d-1ec6ab1f4335",
                    ReferredUserId = "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1",
                    NewsEntryCaseId = NewsEntryCaseId.AddedToProject,
                    ProjectId = 1,
                    RouteId = 1,
                    ActionId = 4,
                    ControllerId = 2
                },
                new
                {
                    Id = 4,
                    CreateDate = DateTime.Now,
                    Seen = false,
                    OwnerId = "e28b8357-2955-4f3c-9c3d-1ec6ab1f4335",
                    ReferredUserId = "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1",
                    NewsEntryCaseId = NewsEntryCaseId.NewTicket,
                    TicketId = 1,
                    ProjectId = 1,
                    RouteId = 1,
                    ActionId = 2,
                    ControllerId = 1,
                    StatusId = TicketStatusId.Open
                },
                new
                {
                    Id = 5,
                    CreateDate = DateTime.Now,
                    Seen = false,
                    OwnerId = "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1",
                    ReferredUserId = "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1",
                    NewsEntryCaseId = NewsEntryCaseId.NewTicket,
                    TicketId = 1,
                    ProjectId = 1,
                    RouteId = 1,
                    ActionId = 2,
                    ControllerId = 1,
                    StatusId = TicketStatusId.Open
                },
                new
                {
                    Id = 6,
                    CreateDate = DateTime.Now,
                    Seen = false,
                    OwnerId = "B22698B8-42A2-4115-9631-1C2D1E2AC5C5",
                    ReferredUserId = "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1",
                    NewsEntryCaseId = NewsEntryCaseId.NewTicket,
                    TicketId = 1,
                    ProjectId = 1,
                    RouteId = 1,
                    ActionId = 2,
                    ControllerId = 1,
                    StatusId = TicketStatusId.Open
                },
                new
                {
                    Id = 7,
                    CreateDate = DateTime.Now,
                    Seen = false,
                    OwnerId = "B22698B8-42A2-4115-9631-1C2D1E2AC5C5",
                    ReferredUserId = "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1",
                    NewsEntryCaseId = NewsEntryCaseId.NewStatus,
                    TicketId = 2,
                    ProjectId = 1,
                    RouteId = 2,
                    ActionId = 2,
                    ControllerId = 1,
                    StatusId = TicketStatusId.InProgress
                },
                new
                {
                    Id = 8,
                    CreateDate = DateTime.Now,
                    Seen = false,
                    OwnerId = "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1",
                    ReferredUserId = "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1",
                    NewsEntryCaseId = NewsEntryCaseId.NewStatus,
                    TicketId = 2,
                    ProjectId = 1,
                    RouteId = 2,
                    ActionId = 2,
                    ControllerId = 1,
                    StatusId = TicketStatusId.InProgress
                },
                new
                {
                    Id = 9,
                    CreateDate = DateTime.Now,
                    Seen = false,
                    OwnerId = "e28b8357-2955-4f3c-9c3d-1ec6ab1f4335",
                    ReferredUserId = "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1",
                    NewsEntryCaseId = NewsEntryCaseId.NewStatus,
                    TicketId = 2,
                    ProjectId = 1,
                    RouteId = 2,
                    ActionId = 2,
                    ControllerId = 1,
                    StatusId = TicketStatusId.InProgress
                },
                new
                {
                    Id = 10,
                    CreateDate = DateTime.Now,
                    Seen = false,
                    OwnerId = "e28b8357-2955-4f3c-9c3d-1ec6ab1f4335",
                    ReferredUserId = "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1",
                    NewsEntryCaseId = NewsEntryCaseId.NewTicket,
                    TicketId = 2,
                    ProjectId = 1,
                    RouteId = 1,
                    ActionId = 2,
                    ControllerId = 1,
                    StatusId = TicketStatusId.Open
                },
                new
                {
                    Id = 11,
                    CreateDate = DateTime.Now,
                    Seen = false,
                    OwnerId = "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1",
                    ReferredUserId = "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1",
                    NewsEntryCaseId = NewsEntryCaseId.NewTicket,
                    TicketId = 2,
                    ProjectId = 1,
                    RouteId = 1,
                    ActionId = 2,
                    ControllerId = 1,
                    StatusId = TicketStatusId.Open
                },
                new
                {
                    Id = 12,
                    CreateDate = DateTime.Now,
                    Seen = false,
                    OwnerId = "B22698B8-42A2-4115-9631-1C2D1E2AC5C5",
                    ReferredUserId = "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1",
                    NewsEntryCaseId = NewsEntryCaseId.NewTicket,
                    TicketId = 2,
                    ProjectId = 1,
                    RouteId = 1,
                    ActionId = 2,
                    ControllerId = 1,
                    StatusId = TicketStatusId.Open
                }, new
                {
                    Id = 13,
                    CreateDate = DateTime.Now,
                    Seen = false,
                    OwnerId = "e28b8357-2955-4f3c-9c3d-1ec6ab1f4335",
                    ReferredUserId = "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1",
                    NewsEntryCaseId = NewsEntryCaseId.NewTicket,
                    TicketId = 3,
                    ProjectId = 1,
                    RouteId = 1,
                    ActionId = 2,
                    ControllerId = 1,
                    StatusId = TicketStatusId.Open
                },
                new
                {
                    Id = 14,
                    CreateDate = DateTime.Now,
                    Seen = false,
                    OwnerId = "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1",
                    ReferredUserId = "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1",
                    NewsEntryCaseId = NewsEntryCaseId.NewTicket,
                    TicketId = 3,
                    ProjectId = 1,
                    RouteId = 1,
                    ActionId = 2,
                    ControllerId = 1,
                    StatusId = TicketStatusId.Open
                },
                new
                {
                    Id = 15,
                    CreateDate = DateTime.Now,
                    Seen = false,
                    OwnerId = "B22698B8-42A2-4115-9631-1C2D1E2AC5C5",
                    ReferredUserId = "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1",
                    NewsEntryCaseId = NewsEntryCaseId.NewTicket,
                    TicketId = 3,
                    ProjectId = 1,
                    RouteId = 1,
                    ActionId = 2,
                    ControllerId = 1,
                    StatusId = TicketStatusId.Open
                },new
                {
                    Id = 16,
                    CreateDate = DateTime.Now,
                    Seen = false,
                    OwnerId = "e28b8357-2955-4f3c-9c3d-1ec6ab1f4335",
                    ReferredUserId = "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1",
                    NewsEntryCaseId = NewsEntryCaseId.NewTicket,
                    TicketId = 4,
                    ProjectId = 1,
                    RouteId = 1,
                    ActionId = 2,
                    ControllerId = 1,
                    StatusId = TicketStatusId.Open
                },
                new
                {
                    Id = 17,
                    CreateDate = DateTime.Now,
                    Seen = false,
                    OwnerId = "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1",
                    ReferredUserId = "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1",
                    NewsEntryCaseId = NewsEntryCaseId.NewTicket,
                    TicketId = 4,
                    ProjectId = 1,
                    RouteId = 1,
                    ActionId = 2,
                    ControllerId = 1,
                    StatusId = TicketStatusId.Open
                },
                new
                {
                    Id = 18,
                    CreateDate = DateTime.Now,
                    Seen = false,
                    OwnerId = "B22698B8-42A2-4115-9631-1C2D1E2AC5C5",
                    ReferredUserId = "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1",
                    NewsEntryCaseId = NewsEntryCaseId.NewTicket,
                    TicketId = 4,
                    ProjectId = 1,
                    RouteId = 1,
                    ActionId = 2,
                    ControllerId = 1,
                    StatusId = TicketStatusId.Open
                }, new
                {
                    Id = 19,
                    CreateDate = DateTime.Now,
                    Seen = false,
                    OwnerId = "e28b8357-2955-4f3c-9c3d-1ec6ab1f4335",
                    ReferredUserId = "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1",
                    NewsEntryCaseId = NewsEntryCaseId.NewTicket,
                    TicketId = 5,
                    ProjectId = 1,
                    RouteId = 1,
                    ActionId = 2,
                    ControllerId = 1,
                    StatusId = TicketStatusId.Open
                },
                new
                {
                    Id = 20,
                    CreateDate = DateTime.Now,
                    Seen = false,
                    OwnerId = "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1",
                    ReferredUserId = "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1",
                    NewsEntryCaseId = NewsEntryCaseId.NewTicket,
                    TicketId = 5,
                    ProjectId = 1,
                    RouteId = 1,
                    ActionId = 2,
                    ControllerId = 1,
                    StatusId = TicketStatusId.Open
                },
                new
                {
                    Id = 21,
                    CreateDate = DateTime.Now,
                    Seen = false,
                    OwnerId = "B22698B8-42A2-4115-9631-1C2D1E2AC5C5",
                    ReferredUserId = "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1",
                    NewsEntryCaseId = NewsEntryCaseId.NewTicket,
                    TicketId = 5,
                    ProjectId = 1,
                    RouteId = 1,
                    ActionId = 2,
                    ControllerId = 1,
                    StatusId = TicketStatusId.Open
                }
                );
        }
    }
}
