using IssuePilot.Data;
using IssuePilot.Models.DBModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IssuePilot.Test.TestData
{
    public class InitDbWithData
    {
        public ApplicationDbContext InitWithDataAndContext()
        {
            DbContextOptionsBuilder<ApplicationDbContext> builder = new DbContextOptionsBuilder<ApplicationDbContext>()
               .EnableSensitiveDataLogging()
               .UseInMemoryDatabase(Guid.NewGuid().ToString());


            ApplicationDbContext context = new ApplicationDbContext(builder.Options);

            //--------------
            // Seed data
            //--------------

            // User
            var usersToSeed = ListOfUsers();
            foreach (var user in usersToSeed)
            {
                context.Users.Add(user);
            }

            // IdentityRole
            var rolesToSeed = ListOfIdentityRolesWithId();
            foreach (var roles in rolesToSeed)
            {
                context.Roles.Add(roles);
            }

            // ProjectRole
            var projectRolesToSeed = ListOfProjectRoles();
            foreach (var roles in projectRolesToSeed)
            {
                context.ProjectRoles.Add(roles);
            }

            // Projects
            var projectToSeed = ListOfProjects();
            foreach (var project in projectToSeed)
            {
                context.Projects.Add(project);
            }

            // ProjectMemberEntries
            var projectMember = ListOfProjectMemberEntries();
            foreach (var member in projectMember)
            {
                context.ProjectMemberEntries.Add(member);
            }
            context.SaveChanges();

            // TicketCategories
            var ticketCategories = ListOfTicketCategories();
            ticketCategories[0].Project = context.Projects.Find(3);
            ticketCategories[1].Project = context.Projects.Find(3);
            ticketCategories[2].Project = context.Projects.Find(1);
            ticketCategories[3].Project = context.Projects.Find(1);
            foreach (var category in ticketCategories)
            {
                context.TicketCategories.Add(category);
            }

            // TicketStatuses
            var statuses = ListOfStatuses();
            foreach (var status in statuses)
            {
                context.TicketStatuses.Add(status);
            }

            // Tickets
            var tickets = ListOfTickets();
            tickets[2].ClosedByUser = context.Users.Find("2301D884-221A-4E7D-B509-0113DCC043E0"); // Admin
            tickets[1].ClosedByUser = context.Users.Find("2301D884-221A-4E7D-B509-0113DCC043E0"); // Admin
            foreach (var ticket in tickets)
            {
                ticket.TicketCreator = context.Users.Find("2301D884-221A-4E7D-B509-0113DCC043E0"); // Admin
                ticket.Project = context.Projects.Find(1);
                context.Tickets.Add(ticket);
            }

            // TicketAssignees
            var ticketAssignees = ListOfTicketAssignees();
            foreach (var ticketAssignee in ticketAssignees)
            {
                context.TicketAssignees.Add(ticketAssignee);
            }

            // NewsEntries
            var newsEntries = ListOfNewsEntries();
            foreach (var entry in newsEntries)
            {
                entry.Owner = context.Users.Find("2301D884-221A-4E7D-B509-0113DCC043E1"); // Manager
                entry.Project = context.Projects.Find(1);
                entry.ReferredUser = context.Users.Find("2301D884-221A-4E7D-B509-0113DCC043E0"); // Admin
                context.NewsEntries.Add(entry);
            }
            newsEntries[^1].Ticket = context.Tickets.Find(1);
            newsEntries[^1].RouteId = context.Tickets.Find(1).Id;
            newsEntries[^1].Status = context.TicketStatuses.Find(TicketStatusId.Open);

            // Comments
            var comments = ListOfComments();
            comments[0].Creator = context.Users.Find("2301D884-221A-4E7D-B509-0113DCC043E0"); // Admin
            context.Comments.Add(comments[0]);

            // TicketHistoryEntries
            var ticketHistoryEntryCases = ListOfTicketHistoryEntryCases();
            foreach (var ticketHistoryEntryCase in ticketHistoryEntryCases)
            {
                context.TicketHistoryEntryCases.Add(ticketHistoryEntryCase);
            }

            // Images
            var images = ListOfImages();
            foreach(var image in images)
            {
                image.Ticket = context.Tickets.Find(1);
                context.Images.Add(image);
            }

            context.SaveChanges();
            context.Database.EnsureCreated();
            return context;
        }

        //--------------
        // Data to seed
        //--------------

        public List<User> ListOfUsers()
        {
            return new List<User>
            {
               new User
                {
                    Id = "2301D884-221A-4E7D-B509-0113DCC043E0",
                    Firstname = "Ad",
                    Surname = "min",
                    UserName = "Admin",
                    NormalizedUserName = "ADMIN",
                    Email = "Admin@Admin.com",
                    NormalizedEmail = "ADMIN@ADMIN.COM",
                    TwoFactorEnabled = true
                },
               new User
                {
                    Id = "2301D884-221A-4E7D-B509-0113DCC043E1",
                    Firstname = "Man",
                    Surname = "ager",
                    UserName = "Manager",
                    NormalizedUserName = "MANAGER",
                    Email = "Manager@Admin.com",
                    NormalizedEmail = "MANAGER@ADMIN.COM",
                },
               new User
                {
                    Id = "2301D884-221A-4E7D-B509-0113DCC043E2",
                    Firstname = "Ben",
                    Surname = "utzer",
                    UserName = "Benutzer",
                    NormalizedUserName = "BENUTZER",
                    Email = "benutzer@Admin.com",
                    NormalizedEmail = "BENUTZER@ADMIN.COM"
                },
                new User
                {
                    Id = "2301D884-221A-4E7D-B509-0113DCC043E3",
                    Firstname = "empty",
                    Surname = "empty",
                    UserName = "empty",
                    NormalizedUserName = "EMPTY",
                    Email = "empty@Admin.com",
                    NormalizedEmail = "EMPTY@ADMIN.COM"
                }

            };
        }

        public List<IdentityRole> ListOfIdentityRolesWithId()
        {
            return new List<IdentityRole>
                {
                    new IdentityRole
                    {
                        Id = "dc78daf-e0e7-4da1-873d-7b2ef7cd8860",
                        Name = "Admin",
                        NormalizedName="ADMIN"
                    },
                    new IdentityRole
                    {
                        Id = "dc78daf-e0e7-4da1-873d-7b2ef7cd8861",
                        Name = "Projektmanager",
                        NormalizedName = "PROJEKTMANAGER"
                    },
                    new IdentityRole
                    {
                        Id = "dc78daf-e0e7-4da1-873d-7b2ef7cd8862",
                        Name = "Benutzer",
                        NormalizedName = "BENUTZER"
                    }
                };
        }

        public List<ProjectRole> ListOfProjectRoles()
        {
            return new List<ProjectRole>
                {
                    new ProjectRole
                    {
                        Id = ProjectRoleId.Owner,
                        Title = "Owner"
                    },
                    new ProjectRole
                    {
                        Id = ProjectRoleId.Member,
                        Title = "Member"
                    }
                };
        }

        public List<Project> ListOfProjects()
        {
            return new List<Project>
                {
                    new Project
                    {
                        Id = 1,
                        Title = "TestProjekt1",
                        CreateDate = DateTime.Now,
                        DeletedTicketsCount = 1,
                        Description = "Erste Beschreibung!"
                    },
                    new Project
                    {
                        Id = 2,
                        Title = "TestProjekt2",
                        CreateDate = DateTime.Now,
                        DeletedTicketsCount = 0,
                        Description = "Zweite Beschreibung!"
                    },
                     new Project
                    {
                        Id = 3,
                        Title = "TestProjekt3",
                        CreateDate = DateTime.Now,
                        DeletedTicketsCount = 0,
                        Description = "Dritte Beschreibung!"
                    }
                };
        }

        public List<ProjectMemberEntry> ListOfProjectMemberEntries()
        {
            return new List<ProjectMemberEntry>
                {
                    new ProjectMemberEntry
                    {
                         FK_UserId = ListOfUsers()[1].Id,
                         FK_ProjectId = ListOfProjects()[1].Id
                    },
                    new ProjectMemberEntry
                    {
                         FK_UserId = ListOfUsers()[2].Id,
                         FK_ProjectId = ListOfProjects()[1].Id
                    },
                    new ProjectMemberEntry
                    {
                         FK_UserId = ListOfUsers()[0].Id,
                         FK_ProjectId = ListOfProjects()[1].Id
                    },
                    new ProjectMemberEntry
                    {
                         FK_UserId = ListOfUsers()[2].Id,
                         FK_ProjectId = 1
                    },
                    new ProjectMemberEntry
                    {
                         FK_UserId = ListOfUsers()[0].Id,
                         FK_ProjectId = 1
                    }
                };
        }

        public List<TicketCategory> ListOfTicketCategories()
        {
            return new List<TicketCategory>
                {
                    new TicketCategory
                    {
                        Id = 1,
                        Name = "TestCategory"
                    },
                    new TicketCategory
                    {
                        Id = 2,
                        Name = "Category2"
                    },
                    new TicketCategory
                    {
                        Id = 3,
                        Name = "TestCategory"
                    },
                    new TicketCategory
                    {
                        Id = 4,
                        Name = "Category2"
                    }
                };
        }
        public List<TicketAssignee> ListOfTicketAssignees()
        {
            return new List<TicketAssignee>
            {
                new TicketAssignee
                {
                    FK_TicketId = 3,
                    FK_UserId = "2301D884-221A-4E7D-B509-0113DCC043E0"
                }
            };
        }

        public List<NewsEntry> ListOfNewsEntries()
        {
            return new List<NewsEntry>
                {
                    new NewsEntry
                    {
                        Id = 1,
                        CreateDate = DateTime.Now,
                        NewsEntryCaseId = NewsEntryCaseId.AddedToProject,
                        Seen = false,
                        ActionId = 4,
                        ControllerId = 2,
                        RouteId = 1
                    },
                    new NewsEntry
                    {
                        Id = 2,
                        CreateDate = DateTime.Now,
                        NewsEntryCaseId = NewsEntryCaseId.NewProject,
                        Seen = true,
                        ActionId = 2,
                        ControllerId = 1
                    }
                };
        }
        public List<TicketStatus> ListOfStatuses()
        {
            return new List<TicketStatus>
            {
                new TicketStatus
                {
                    Id = TicketStatusId.Open,
                    Name = "Open"
                },
                new TicketStatus
                {
                    Id = TicketStatusId.Closed,
                    Name = "Closed"
                },
                new TicketStatus
                {
                    Id = TicketStatusId.Canceled,
                    Name = "Canceled"
                },
                new TicketStatus
                {
                    Id = TicketStatusId.Paused,
                    Name = "Paused"
                },
                new TicketStatus
                {
                    Id = TicketStatusId.InProgress,
                    Name = "InProgress"
                }
            };
        }
        public List<Ticket> ListOfTickets()
        {
            return new List<Ticket>
            {
                new Ticket
                {
                    Id = 1,
                    CreateDate = DateTime.Now,
                    Title = "TestTicket",
                    Description = "Ein vom System generiertes Ticket zum Testen.",
                    Weight = 1,
                    TicketStatusId = TicketStatusId.Open,
                    Deadline = DateTime.MinValue
                },
                new Ticket
                {
                    Id = 2,
                    CreateDate = DateTime.Now,
                    Title = "TestTicket2",
                    Description = "Ein vom System generiertes Ticket zum Testen.",
                    Weight = 2,
                    CloseDate = DateTime.Now.AddSeconds(1),
                    TicketStatusId = TicketStatusId.Closed,
                    Deadline = DateTime.Now.AddDays(1)
                },
                new Ticket
                {
                    Id = 3,
                    CreateDate = DateTime.Now,
                    Title = "TestTicket3",
                    Description = "Ein vom System generiertes Ticket zum Testen.",
                    Weight = 2,
                    CloseDate = DateTime.Now.AddSeconds(3),
                    TicketStatusId = TicketStatusId.Closed
                }
            };
        }
        public List<Comment> ListOfComments()
        {
            return new List<Comment>
            {
                new Comment
                {
                    Id = 1,
                    Text = "Testkommentar",
                    TicketId = 1
                }
            };
        }

        public List<TicketHistoryEntryCase> ListOfTicketHistoryEntryCases()
        {
            return new List<TicketHistoryEntryCase>
            {
                new TicketHistoryEntryCase
                {
                    EntryCaseId = TicketHistoryEntryCaseId.CommentAdded,
                    Name = "CommentAdded"
                },
                new TicketHistoryEntryCase
                {
                    EntryCaseId = TicketHistoryEntryCaseId.CommentDeleted,
                    Name = "CommentDeleted"
                },
                new TicketHistoryEntryCase
                {
                    EntryCaseId = TicketHistoryEntryCaseId.MemberAssigned,
                    Name = "MemberAssigned"
                },
                new TicketHistoryEntryCase
                {
                    EntryCaseId = TicketHistoryEntryCaseId.MemberUnassigned,
                    Name = "MemberUnassigned"
                },
                new TicketHistoryEntryCase
                {
                    EntryCaseId = TicketHistoryEntryCaseId.TicketCanceled,
                    Name = "TicketCanceled"
                },
                new TicketHistoryEntryCase
                {
                    EntryCaseId = TicketHistoryEntryCaseId.TicketClosed,
                    Name = "TicketClosed"
                },
                new TicketHistoryEntryCase
                {
                    EntryCaseId = TicketHistoryEntryCaseId.TicketInProgress,
                    Name = "TicketInProgress"
                },
                new TicketHistoryEntryCase
                {
                    EntryCaseId = TicketHistoryEntryCaseId.TicketOpened,
                    Name = "TicketOpened"
                },
                new TicketHistoryEntryCase
                {
                    EntryCaseId = TicketHistoryEntryCaseId.TicketPaused,
                    Name = "TicketPaused"
                }
            };
        }
        public List<Image> ListOfImages()
        {
            return new List<Image>
            {
                new Image
                {
                    Id = 1,
                    ImageData = new byte[1]
                }
            };
        }


    }
}
