using IssuePilot.Models.ViewModels;
using IssuePilot.Models.Repositorys;
using IssuePilot.Test.TestData;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Threading.Tasks;
using System.Linq;
using IssuePilot.Models.DBModels;
using IssuePilot.Helper;

namespace IssuePilot.Test
{
    public class TicketRepositoryTests : InitDbWithData
    {
        /*
         * CreateTicketAsync test cases
         */
        [Fact]
        public async Task CreateTicketAsyncTest_HasTitleBeenCreated()
        {
            using var context = InitWithDataAndContext();
            // Arrange
            TicketRepository ticketRepository = new TicketRepository(context);
            User user = context.Users.First();
            TicketCreateViewModel modelToSave = new TicketCreateViewModel() { Title = "SaveTestTicketTitle" };
            Project project = context.Projects.First();
            //Act
            var resultTicket = await ticketRepository.CreateTicketAsync(modelToSave, project, user, new List<Image>());
            //Assert
            Assert.Equal(resultTicket.Title, modelToSave.Title);
        }
        [Fact]
        public async Task CreateTicketAsyncTest_HasDescriptionBeenCreated()
        {
            using var context = InitWithDataAndContext();
            // Arrange
            TicketRepository ticketRepository = new TicketRepository(context);
            User user = context.Users.First();
            TicketCreateViewModel modelToSave = new TicketCreateViewModel() { Title = "SaveTestTicketTitle", Description = "A Test Ticket from a Test!" };
            Project project = context.Projects.Find(1);
            //Act
            var resultTicket = await ticketRepository.CreateTicketAsync(modelToSave, project, user, new List<Image>());
            //Assert
            Assert.Equal(resultTicket.Description, modelToSave.Description);
        }
        [Fact]
        public async Task CreateTicketAsyncTest_IsCurrentDateAdded()
        {
            using var context = InitWithDataAndContext();
            // Arrange
            TicketRepository ticketRepository = new TicketRepository(context);
            User user = context.Users.First();
            Project project = context.Projects.First();
            TicketCreateViewModel modelToSave = new TicketCreateViewModel() { Title = "SaveTestTicketTitle" };
            DateTime currentTime = DateTime.Now;
            //Act
            var resultTicket = await ticketRepository.CreateTicketAsync(modelToSave, project, user, new List<Image>());
            // Assert
            Assert.True(resultTicket.CreateDate > currentTime);
        }
        [Fact]
        public async Task CreateTicketAsyncTest_IsDeadlineAdded()
        {
            using var context = InitWithDataAndContext();
            // Arrange
            TicketRepository ticketRepository = new TicketRepository(context);
            User user = context.Users.First();
            Project project = context.Projects.First();
            TicketCreateViewModel modelToSave = new TicketCreateViewModel() { Title = "SaveTestTicketTitle", Deadline = DateTime.Now };
            //Act
            var resultTicket = await ticketRepository.CreateTicketAsync(modelToSave, project, user, new List<Image>());

            //Assert
            Assert.Equal(resultTicket.Deadline, modelToSave.Deadline);
        }
        [Fact]
        public async Task CreateTicketAsyncTest_IsStatusAdded()
        {
            using var context = InitWithDataAndContext();
            // Arrange
            TicketRepository ticketRepository = new TicketRepository(context);
            User user = context.Users.First();
            Project project = context.Projects.First();
            TicketCreateViewModel modelToSave = new TicketCreateViewModel() { Title = "SaveTestTicketTitle" };
            //Act
            var resultTicket = await ticketRepository.CreateTicketAsync(modelToSave, project, user, new List<Image>());
            //Assert
            Assert.Equal(TicketStatusId.Open, resultTicket.Status.Id);
        }
        [Fact]
        public async Task CreateTicketAsyncTest_IsWeightCorrect()
        {
            using var context = InitWithDataAndContext();
            // Arrange
            TicketRepository ticketRepository = new TicketRepository(context);
            User user = context.Users.First();
            Project project = context.Projects.First();
            TicketCreateViewModel modelToSave = new TicketCreateViewModel() { Title = "SaveTestTicketTitle", Weight = 1 };
            //Act
            var resultTicket = await ticketRepository.CreateTicketAsync(modelToSave, project, user, new List<Image>());
            //Assert
            Assert.Equal(resultTicket.Weight, modelToSave.Weight);
        }
        [Fact]
        public async Task CreateTicketAsyncTest_IsDescriptionNull()
        {
            using var context = InitWithDataAndContext();
            // Arrange
            TicketRepository ticketRepository = new TicketRepository(context);
            User user = context.Users.First();
            Project project = context.Projects.First();
            TicketCreateViewModel modelToSave = new TicketCreateViewModel() { Title = "SaveTestTicketTitle" };
            //Act
            var resultTicket = await ticketRepository.CreateTicketAsync(modelToSave, project, user, new List<Image>());
            //Assert
            Assert.Null(resultTicket.Description);
        }
        [Fact]
        public async Task CreateTicketAsyncTest_IsDeadlineNull()
        {
            using var context = InitWithDataAndContext();
            // Arrange
            TicketRepository ticketRepository = new TicketRepository(context);
            User user = context.Users.First();
            Project project = context.Projects.First();
            TicketCreateViewModel modelToSave = new TicketCreateViewModel() { Title = "SaveTestTicketTitle" };
            //Act
            var resultTicket = await ticketRepository.CreateTicketAsync(modelToSave, project, user, new List<Image>());
            //Assert
            Assert.Null(resultTicket.Deadline);
        }

        [Fact]
        public async Task CreateTicketAsyncTest_TicketCreatorIsSet()
        {
            using var context = InitWithDataAndContext();
            // Arrange
            TicketRepository ticketRepository = new TicketRepository(context);
            User user = context.Users.First();
            Project project = context.Projects.First();
            TicketCreateViewModel modelToSave = new TicketCreateViewModel() { Title = "SaveTestTicketTitle" };
            //Act
            var resultTicket = await ticketRepository.CreateTicketAsync(modelToSave, project, user, new List<Image>());
            // Assert
            Assert.NotNull(user.CreatedTickets.FirstOrDefault(r => r.Id == resultTicket.Id));
        }
        [Fact]
        public async Task CreateTicketAsyncTest_ProjectIsSet()
        {
            using var context = InitWithDataAndContext();
            // Arrange
            TicketRepository ticketRepository = new TicketRepository(context);
            User user = context.Users.First();
            Project project = context.Projects.First();
            TicketCreateViewModel modelToSave = new TicketCreateViewModel() { Title = "SaveTestTicketTitle" };
            //Act
            var resulticket = await ticketRepository.CreateTicketAsync(modelToSave, project, user, new List<Image>());
            //Assert
            Assert.Equal(resulticket.Project, project);
        }
        [Fact]
        public async Task CreateTicketAsyncTest_ThrowNullReferenceExceptionIfModelIsNull()
        {
            using var context = InitWithDataAndContext();
            // Arrange
            TicketRepository ticketRepository = new TicketRepository(context);
            User user = context.Users.First();
            Project project = context.Projects.First();
            //Act
            async Task actual() => await ticketRepository.CreateTicketAsync(null, project, user, new List<Image>());

            //Assert
            await Assert.ThrowsAsync<NullReferenceException>(actual);
        }
        [Fact]
        public async Task CreateTicketAsyncTest_HasImageBeenAdded()
        {
            using var context = InitWithDataAndContext();
            // Arrange
            TicketRepository ticketRepository = new TicketRepository(context);
            User user = context.Users.First();
            Project project = context.Projects.First();
            TicketCreateViewModel modelToSave = new TicketCreateViewModel() { Title = "SaveTestTicketTitle" };
            Image image = new Image() { ImageData = new byte[1] };
            // Act
            var resultTicket = await ticketRepository.CreateTicketAsync(modelToSave, project, user, new List<Image>() { image });
            // Assert
            Assert.Equal(image, resultTicket.Images.First());
        }
        /*
         * * GetTicketProjectCategoriesOfProjectAsync test cases
         */
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public async Task CreateTicketAsyncAndGetTicketProjectCategoriesOfProjectAsync_ReturnsCorrectTicketProjectCategories(int categoryIndex)
        {
            using var context = InitWithDataAndContext();
            // Arrange
            TicketRepository ticketRepository = new TicketRepository(context);
            const int PROJECT_ID = 3;
            List<TicketCategory> ticketCategoriesOfProject = context.TicketCategories.Where(t => t.Project.Id == PROJECT_ID).ToList();
            List<int> selectedTicketCategories = new List<int>();
            foreach (var t in ticketCategoriesOfProject)
            {
                selectedTicketCategories.Add(t.Id);
            }
            User user = context.Users.First();
            Project project = context.Projects.Find(PROJECT_ID);
            TicketCreateViewModel modelToSave = new TicketCreateViewModel() { Title = "SaveTestTicketTitle", SelectedTicketCategories = selectedTicketCategories };
            // Act
            var resulticket = await ticketRepository.CreateTicketAsync(modelToSave, project, user, new List<Image>());
            var ticketProjectCategories = await ticketRepository.GetTicketProjectCategoriesOfProjectAsync(ticketCategoriesOfProject);
            // Assert
            Assert.Contains(resulticket.TicketProjectCategories.ToList()[categoryIndex], ticketProjectCategories);
        }
        /*
         * * UpdateAssigneesAsync test cases
         */
        [Fact]
        public async Task UpdateAssigneesAsyncTest_AssigneeStaysAdded()
        {
            using var context = InitWithDataAndContext();
            // Arrange
            TicketRepository ticketRepository = new TicketRepository(context);
            User user = context.Users.First();
            Ticket ticket = context.Tickets.First();
            List<string> selectedAssignees = new List<string>
            {
                user.Id
            };
            // Act
            await ticketRepository.UpdateAssigneesAsync(selectedAssignees, ticket.Id, user.Id);
            // Assert
            Assert.Equal(user.Id, ticket.TicketAssignees.First().FK_UserId);
        }
        [Fact]
        public async Task UpdateAssigneesAsyncTest_AssigneeIsDeleted()
        {
            using var context = InitWithDataAndContext();
            // Arrange
            DashboardRepository dashboardRepository = new DashboardRepository(context);
            TicketRepository ticketRepository = new TicketRepository(context, dashboardRepository);
            TicketAssignee ticketAssignee = context.TicketAssignees.First();
            List<string> selectedAssignees = new List<string>();
            // Act
            await ticketRepository.UpdateAssigneesAsync(selectedAssignees, ticketAssignee.FK_TicketId, ticketAssignee.FK_UserId);
            // Assert
            Assert.Empty(ticketAssignee.Ticket.TicketAssignees);
        }
        [Fact]
        public async Task UpdateAssigneesAsyncTest_ThrowNullReferenceExceptionIfSelectedAssigneesIsNull()
        {
            using var context = InitWithDataAndContext();
            // Arrange
            DashboardRepository dashboardRepository = new DashboardRepository(context);
            TicketRepository ticketRepository = new TicketRepository(context, dashboardRepository);
            TicketAssignee ticketAssignee = context.TicketAssignees.First();
            // Act
            async Task actual() => await ticketRepository.UpdateAssigneesAsync(null, ticketAssignee.FK_TicketId, ticketAssignee.FK_UserId);
            // Assert
            await Assert.ThrowsAsync<NullReferenceException>(actual);
        }
        [Fact]
        public async Task UpdateAssigneesAsyncTest_NonProjectMemberIsNotAdded()
        {
            using var context = InitWithDataAndContext();
            // Arrange
            DashboardRepository dashboardRepository = new DashboardRepository(context);
            TicketRepository ticketRepository = new TicketRepository(context, dashboardRepository);
            User user = context.Users.ToList()[1];
            Ticket ticket = context.Tickets.First();
            List<string> selectedAssignees = new List<string>
            {
                user.Id
            };
            // Act
            await ticketRepository.UpdateAssigneesAsync(selectedAssignees, ticket.Id, user.Id);
            // Assert
            Assert.True(ticket.TicketAssignees.FirstOrDefault(t => t.FK_UserId == user.Id) == null);
        }
        [Fact]
        public async Task UpdateAssigneesAsyncTest_MemberIsAdded()
        {
            using var context = InitWithDataAndContext();
            // Arrange
            DashboardRepository dashboardRepository = new DashboardRepository(context);
            TicketRepository ticketRepository = new TicketRepository(context, dashboardRepository);
            User user = context.Users.ToList()[2];
            Ticket ticket = context.Tickets.First();
            List<string> selectedAssignees = new List<string>
            {
                user.Id
            };
            // Act
            await ticketRepository.UpdateAssigneesAsync(selectedAssignees, ticket.Id, user.Id);
            // Assert
            Assert.Equal(user.Id, ticket.TicketAssignees.First().FK_UserId);
        }
        /*
         * * AddSelfAsAssigneeAsync test cases
         */
        [Fact]
        public async Task AddSelfAsAssigneeAsyncTest_MemberIsAdded()
        {
            using var context = InitWithDataAndContext();
            // Arrange
            DashboardRepository dashboardRepository = new DashboardRepository(context);
            TicketRepository ticketRepository = new TicketRepository(context, dashboardRepository);
            User user = context.Users.ToList()[2];
            Ticket ticket = context.Tickets.ToList()[1];
            // Act
            await ticketRepository.AddSelfAsAssigneeAsync(ticket.Id, user);
            // Assert
            Assert.Equal(user.Id, ticket.TicketAssignees.First().FK_UserId);
        }
        [Fact]
        public async Task AddSelfAsAssigneeAsyncTest_ThrowNullReferenceExceptionIfTicketIsNull()
        {
            using var context = InitWithDataAndContext();
            // Arrange
            DashboardRepository dashboardRepository = new DashboardRepository(context);
            TicketRepository ticketRepository = new TicketRepository(context, dashboardRepository);
            User user = context.Users.ToList()[2];
            const int NONEXISTENT_TICKET_ID = 300;
            // Act
            async Task actual() => await ticketRepository.AddSelfAsAssigneeAsync(NONEXISTENT_TICKET_ID, user);
            // Assert
            await Assert.ThrowsAsync<NullReferenceException>(actual);
        }
        /*
         * * RemoveSelfFromAssigneesAsync test cases
         */
        [Fact]
        public async Task RemoveSelfFromAssigneesAsyncTest_AssigneeIsDeleted()
        {
            using var context = InitWithDataAndContext();
            // Arrange
            DashboardRepository dashboardRepository = new DashboardRepository(context);
            TicketRepository ticketRepository = new TicketRepository(context, dashboardRepository);
            TicketAssignee ticketAssignee = context.TicketAssignees.First();
            // Act
            await ticketRepository.RemoveSelfFromAssigneesAsync(ticketAssignee.FK_TicketId, ticketAssignee.FK_UserId);
            // Assert
            Assert.Empty(ticketAssignee.Ticket.TicketAssignees);
        }
        /*
         * * CreateTicketHistoryEntryAsync test cases
         */
        [Theory]
        [InlineData(TicketHistoryEntryCaseId.CommentAdded)]
        [InlineData(TicketHistoryEntryCaseId.CommentDeleted)]
        [InlineData(TicketHistoryEntryCaseId.MemberAssigned)]
        [InlineData(TicketHistoryEntryCaseId.MemberUnassigned)]
        [InlineData(TicketHistoryEntryCaseId.TicketCanceled)]
        [InlineData(TicketHistoryEntryCaseId.TicketClosed)]
        [InlineData(TicketHistoryEntryCaseId.TicketInProgress)]
        [InlineData(TicketHistoryEntryCaseId.TicketOpened)]
        [InlineData(TicketHistoryEntryCaseId.TicketPaused)]
        public async Task CreateTicketHistoryEntryAsyncTest_EntryIsCreatedAndHasCorrectEntryCaseId(TicketHistoryEntryCaseId ticketHistoryEntryCaseId)
        {
            using var context = InitWithDataAndContext();
            // Arrange
            TicketRepository ticketRepository = new TicketRepository(context);
            TicketAssignee ticketAssignee = context.TicketAssignees.First();
            User user = context.Users.First();
            Ticket ticket = context.Tickets.First();
            // Act
            await ticketRepository.CreateTicketHistoryEntryAsync(ticket, user, ticketHistoryEntryCaseId);
            // Assert
            Assert.True(context.TicketHistoryEntries.First().TicketHistoryEntryCaseId == ticketHistoryEntryCaseId);
        }
        [Fact]
        public async Task CreateTicketHistoryEntryAsyncTest_EntryCreatorIsSet()
        {
            using var context = InitWithDataAndContext();
            // Arrange
            TicketRepository ticketRepository = new TicketRepository(context);
            TicketAssignee ticketAssignee = context.TicketAssignees.First();
            User user = context.Users.First();
            Ticket ticket = context.Tickets.First();
            // Act
            await ticketRepository.CreateTicketHistoryEntryAsync(ticket, user, TicketHistoryEntryCaseId.CommentAdded);
            // Assert
            Assert.Equal(context.TicketHistoryEntries.First().EntryCreator, user);
        }
        [Fact]
        public async Task CreateTicketHistoryEntryAsyncTest_ReferredUserIsSet()
        {
            using var context = InitWithDataAndContext();
            // Arrange
            TicketRepository ticketRepository = new TicketRepository(context);
            TicketAssignee ticketAssignee = context.TicketAssignees.First();
            User entryCreator = context.Users.First();
            User referredUser = context.Users.Last();
            Ticket ticket = context.Tickets.First();
            // Act
            await ticketRepository.CreateTicketHistoryEntryAsync(ticket, entryCreator, TicketHistoryEntryCaseId.MemberAssigned, referredUser);
            // Assert
            Assert.Equal(context.TicketHistoryEntries.First().ReferredUser, referredUser);

        }
        [Fact]
        public async Task CreateTicketHistoryEntryAsyncTest_TicketIsSet()
        {
            using var context = InitWithDataAndContext();
            // Arrange
            TicketRepository ticketRepository = new TicketRepository(context);
            TicketAssignee ticketAssignee = context.TicketAssignees.First();
            User user = context.Users.First();
            Ticket ticket = context.Tickets.First();
            // Act
            await ticketRepository.CreateTicketHistoryEntryAsync(ticket, user, TicketHistoryEntryCaseId.TicketClosed);
            // Assert
            Assert.Equal(context.TicketHistoryEntries.First().Ticket, ticket);
        }
        /*
         * * GetCategoriesOfTicketAsync test cases
         */
        [Fact]
        public async Task CreateTicketAsyncAndGetCategoriesOfTicketAsyncTest_ReturnsCorrectCategory()
        {
            using var context = InitWithDataAndContext();
            // Arrange
            TicketRepository ticketRepository = new TicketRepository(context);
            const int PROJECT_ID = 3;
            List<TicketCategory> ticketCategoriesOfProject = context.TicketCategories.Where(t => t.Project.Id == PROJECT_ID).ToList();
            List<int> selectedTicketCategories = new List<int>();
            foreach (var t in ticketCategoriesOfProject)
            {
                selectedTicketCategories.Add(t.Id);
            }
            User user = context.Users.First();
            Project project = context.Projects.Find(PROJECT_ID);
            TicketCreateViewModel modelToSave = new TicketCreateViewModel() { Title = "SaveTestTicketTitle", SelectedTicketCategories = selectedTicketCategories };
            // Act
            var resulticket = await ticketRepository.CreateTicketAsync(modelToSave, project, user, new List<Image>());
            var ticketProjectCategories = await ticketRepository.GetCategoriesOfTicketAsync(resulticket.Id, ticketCategoriesOfProject);
            // Assert
            Assert.Equal(resulticket.TicketProjectCategories.ToList().Last().TicketCategory.Name, ticketProjectCategories.Item1.First());
        }
        /*
         * * GetAssigneesAsync test cases
         */
        [Fact]
        public async Task GetAssigneesAsyncTest_DoesNotReturnDeletedUserAsAssignee()
        {
            using var context = InitWithDataAndContext();
            // Arrange
            DashboardRepository dashboardRepository = new DashboardRepository(context);
            TicketRepository ticketRepository = new TicketRepository(context, dashboardRepository);
            User user = context.Users.First();
            Ticket ticket = context.Tickets.ToList()[2];
            var users = context.Users.ToList();
            user.IsDeleted = true;
            TicketAssignee ticketAssignee = context.TicketAssignees.First();
            // Act
            var assignees = await ticketRepository.GetAssigneesAsync(ticket.Id, users);
            // Assert
            Assert.Empty(assignees.Item1);
        }
        /*
         * * UpdateTicketStatusAsync test cases
         */
        [Theory]
        [InlineData(TicketStatusId.Canceled)]
        [InlineData(TicketStatusId.Closed)]
        [InlineData(TicketStatusId.InProgress)]
        [InlineData(TicketStatusId.Open)]
        [InlineData(TicketStatusId.Paused)]
        public async Task UpdateTicketStatusAsyncTest_CorrectlySetTicketStatusId(TicketStatusId expectedTicketStatusId)
        {
            using var context = InitWithDataAndContext();
            // Arrange
            DashboardRepository dashboardRepository = new DashboardRepository(context);
            TicketRepository ticketRepository = new TicketRepository(context, dashboardRepository);
            User user = context.Users.First();
            Ticket ticket = context.Tickets.ToList()[2];
            // Act
            await ticketRepository.UpdateTicketStatusAsync(expectedTicketStatusId, user.Id, ticket.Id);
            // Assert
            Assert.Equal(expectedTicketStatusId, ticket.TicketStatusId);
        }
        [Fact]
        public async Task UpdateTicketStatusAsyncTest_CorrectlySetClosedByUser()
        {
            using var context = InitWithDataAndContext();
            // Arrange
            DashboardRepository dashboardRepository = new DashboardRepository(context);
            TicketRepository ticketRepository = new TicketRepository(context, dashboardRepository);
            User user = context.Users.First();
            Ticket ticket = context.Tickets.ToList().First();
            // Act
            await ticketRepository.UpdateTicketStatusAsync(TicketStatusId.Closed, user.Id, ticket.Id);
            // Assert
            Assert.Equal(user, ticket.ClosedByUser);
        }
        [Fact]
        public async Task UpdateTicketStatusAsyncTest_CorrectlyRemoveClosedByUser()
        {
            using var context = InitWithDataAndContext();
            // Arrange
            DashboardRepository dashboardRepository = new DashboardRepository(context);
            TicketRepository ticketRepository = new TicketRepository(context, dashboardRepository);
            User user = context.Users.First();
            Ticket ticket = context.Tickets.ToList().First();
            // Act
            await ticketRepository.UpdateTicketStatusAsync(TicketStatusId.Open, user.Id, ticket.Id);
            // Assert
            Assert.Null(ticket.ClosedByUser);
        }
        /*
         * * DeleteTicketAsync test cases
         */
        [Fact]
        public async Task DeleteTicketAsyncTest_TicketIsDeleted()
        {
            using var context = InitWithDataAndContext();
            // Arrange
            DashboardRepository dashboardRepository = new DashboardRepository(context);
            TicketRepository ticketRepository = new TicketRepository(context, dashboardRepository);
            Ticket ticket = context.Tickets.ToList().First();
            int ticketId = ticket.Id;
            // Act
            await ticketRepository.DeleteTicketAsync(ticketId);
            // Assert
            Assert.Null(context.Tickets.Find(ticketId));
        }
        /*
         * * UpdateTicketAsync test cases
         */
        [Fact]
        public async Task UpdateTicketAsyncTest_HasTitleBeenChanged()
        {
            using var context = InitWithDataAndContext();
            // Arrange
            TicketRepository ticketRepository = new TicketRepository(context);
            Ticket ticket = context.Tickets.First();
            TicketEditViewModel modelToSave = new TicketEditViewModel() { Id = ticket.Id, Title = "TestTicketTitle" };
            //Act
            await ticketRepository.UpdateTicketAsync(modelToSave, new List<Image>());
            //Assert
            Assert.Equal(ticket.Title, modelToSave.Title);
        }
        [Fact]
        public async Task UpdateTicketAsyncTest_HasDescriptionBeenChanged()
        {
            using var context = InitWithDataAndContext();
            // Arrange
            TicketRepository ticketRepository = new TicketRepository(context);
            Ticket ticket = context.Tickets.First();
            TicketEditViewModel modelToSave = new TicketEditViewModel() { Id = ticket.Id, Description = "TestTicketDescription" };
            //Act
            await ticketRepository.UpdateTicketAsync(modelToSave, new List<Image>());
            //Assert
            Assert.Equal(ticket.Description, modelToSave.Description);
        }
        [Fact]
        public async Task UpdateTicketAsyncTest_HasDeadlineBeenChanged()
        {
            using var context = InitWithDataAndContext();
            // Arrange
            TicketRepository ticketRepository = new TicketRepository(context);
            Ticket ticket = context.Tickets.First();
            TicketEditViewModel modelToSave = new TicketEditViewModel() { Id = ticket.Id, Deadline = DateTime.MinValue };
            //Act
            await ticketRepository.UpdateTicketAsync(modelToSave, new List<Image>());
            //Assert
            Assert.Equal(ticket.Deadline, modelToSave.Deadline);
        }
        [Fact]
        public async Task UpdateTicketAsyncTest_HasWeightBeenChanged()
        {
            using var context = InitWithDataAndContext();
            // Arrange
            TicketRepository ticketRepository = new TicketRepository(context);
            Ticket ticket = context.Tickets.First();
            TicketEditViewModel modelToSave = new TicketEditViewModel() { Id = ticket.Id, Weight = 1 };
            //Act
            await ticketRepository.UpdateTicketAsync(modelToSave, new List<Image>());
            //Assert
            Assert.Equal(ticket.Weight, modelToSave.Weight);
        }
        [Fact]
        public async Task UpdateTicketAsyncTest_HasImageBeenAdded()
        {
            using var context = InitWithDataAndContext();
            // Arrange
            TicketRepository ticketRepository = new TicketRepository(context);
            Ticket ticket = context.Tickets.Find(1);
            Image image = context.Images.First();
            TicketEditViewModel modelToSave = new TicketEditViewModel() { Id = ticket.Id };
            //Act
            await ticketRepository.UpdateTicketAsync(modelToSave, new List<Image>() { });
            //Assert
            Assert.Equal(ticket.Images.First(), image);
        }
        [Fact]
        public async Task UpdateTicketAsyncTest_HasImageBeenDeleted()
        {
            using var context = InitWithDataAndContext();
            // Arrange
            TicketRepository ticketRepository = new TicketRepository(context);
            const int TICKET_ID = 1;
            Ticket ticket = context.Tickets.Find(TICKET_ID);
            SelectableImage selectableImage = new SelectableImage() { ImageId = ticket.Images.First().Id, IsSelected = true };
            TicketEditViewModel modelToSave = new TicketEditViewModel() { Id = TICKET_ID, ImageList = new List<SelectableImage>() { selectableImage } };
            //Act
            await ticketRepository.UpdateTicketAsync(modelToSave, new List<Image>());
            //Assert
            Assert.Empty(ticket.Images);
        }
        [Fact]
        public async Task UpdateTicketAsyncTest_HasCategoryBeenAdded()
        {
            using var context = InitWithDataAndContext();
            // Arrange
            TicketRepository ticketRepository = new TicketRepository(context);
            Ticket ticket = context.Tickets.First();
            const int PROJECT_ID = 1;
            List<TicketCategory> ticketCategoriesOfProject = context.TicketCategories.Where(t => t.Project.Id == PROJECT_ID).ToList();
            List<int> selectedTicketCategories = new List<int>();
            foreach (var t in ticketCategoriesOfProject)
            {
                selectedTicketCategories.Add(t.Id);
            }
            TicketEditViewModel modelToSave = new TicketEditViewModel() { Id = ticket.Id, SelectedTicketCategories = selectedTicketCategories };
            //Act
            await ticketRepository.UpdateTicketAsync(modelToSave, new List<Image>());
            //Assert
            Assert.Equal(ticket.TicketProjectCategories.First(), ticketCategoriesOfProject[0].TicketProjectCategories.First()); ;
        }
        [Fact]
        public async Task UpdateTicketAsyncTest_HasCategoryBeenRemoved()
        {
            using var context = InitWithDataAndContext();
            // Arrange
            TicketRepository ticketRepository = new TicketRepository(context);
            Ticket ticket = context.Tickets.Find(1);
            const int PROJECT_ID = 1;
            List<TicketCategory> ticketCategoriesOfProject = context.TicketCategories.Where(t => t.Project.Id == PROJECT_ID).ToList();
            List<int> selectedTicketCategories = new List<int>();
            foreach (var t in ticketCategoriesOfProject)
            {
                selectedTicketCategories.Add(t.Id);
            }
            TicketEditViewModel modelAddCategories = new TicketEditViewModel() { Id = ticket.Id, SelectedTicketCategories = selectedTicketCategories, OldTicketCategories = new List<int>() };
            TicketEditViewModel modelRemoveCategories = new TicketEditViewModel() { Id = ticket.Id, SelectedTicketCategories = new List<int>(), OldTicketCategories = selectedTicketCategories };
            // Act
            await ticketRepository.UpdateTicketAsync(modelAddCategories, new List<Image>());
            await ticketRepository.UpdateTicketAsync(modelRemoveCategories, new List<Image>());
            //Assert
            Assert.Empty(ticket.TicketProjectCategories);
        }
        /*
         * * AddCommentAsync test cases
         */
        [Fact]
        public async Task AddCommentAsyncTest_HasCorrectText()
        {
            using var context = InitWithDataAndContext();
            // Arrange
            DashboardRepository dashboardRepository = new DashboardRepository(context);
            TicketRepository ticketRepository = new TicketRepository(context, dashboardRepository);
            Ticket ticket = context.Tickets.First();
            User user = context.Users.First();
            const string TEST_COMMENT = "TestComment";
            // Act
            await ticketRepository.AddCommentAsync(TEST_COMMENT, user.Id, ticket.Id);
            // Assert
            Assert.Equal(TEST_COMMENT, context.Comments.FirstOrDefault(c => c.TicketId == ticket.Id).Text);
        }
        [Fact]
        public async Task AddCommentAsyncTest_HasCorrectCreator()
        {
            using var context = InitWithDataAndContext();
            // Arrange
            DashboardRepository dashboardRepository = new DashboardRepository(context);
            TicketRepository ticketRepository = new TicketRepository(context, dashboardRepository);
            Ticket ticket = context.Tickets.First();
            User user = context.Users.First();
            const string TEST_COMMENT = "TestComment";
            // Act
            await ticketRepository.AddCommentAsync(TEST_COMMENT, user.Id, ticket.Id);
            // Assert
            Assert.Equal(user, context.Comments.FirstOrDefault(c => c.TicketId == ticket.Id).Creator);
        }
        /*
         * * GetClosedByUserOfTicketAsync test cases
         */
        [Fact]
        public async Task GetClosedByUserOfTicketAsyncTest_ReturnsClosedByUserName()
        {
            using var context = InitWithDataAndContext();
            // Arrange
            TicketRepository ticketRepository = new TicketRepository(context);
            Ticket ticket = context.Tickets.First();
            User user = context.Users.First();
            // Act
            var userName = await ticketRepository.GetClosedByUserOfTicketAsync(ticket.Id);
            // Assert
            Assert.Equal(user.UserName, userName);
        }

        /*
         * * DeleteCommentAsync test cases
         */
        [Fact]
        public async Task DeleteCommentAsyncTest_CommentIsDeleted()
        {
            using var context = InitWithDataAndContext();
            // Arrange
            TicketRepository ticketRepository = new TicketRepository(context);
            Ticket ticket = context.Tickets.First();
            User entryCreator = context.Users.First();
            const int COMMENT_ID = 1;
            // Act
            await ticketRepository.DeleteCommentAsync(COMMENT_ID, entryCreator);
            // Assert
            Assert.Null(context.Comments.FirstOrDefault(c => c.Id == COMMENT_ID));
        }
    }
}
