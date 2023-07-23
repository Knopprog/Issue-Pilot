using IssuePilot.Models.Repositorys;
using IssuePilot.Models.ViewModels.Statistics;
using IssuePilot.Test.TestData;
using System;
using System.Threading.Tasks;
using Xunit;

namespace IssuePilot.Test
{
    public class StatisticsRepositoryTests : InitDbWithData
    {
        /*
         * GetProjectStatisticDataAsync test cases
         */
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GetProjectStatisticDataAsyncTest_MembersHaveNoTicketsAsync(int memberIndex)
        {
            using var context = InitWithDataAndContext();
            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);
            ProjectRepository projectRepository = new ProjectRepository(context);
            StatisticsRepository statisticsRepository = new StatisticsRepository(context, projectRoleRepository, projectRepository);
            const int PROJECT_ID = 2;
            const int EXPECTED_NUM_OF_TICKETS = 0;
            // Act
            StatisticsDetailsViewModel statisticsDetailsViewModel = await statisticsRepository.GetProjectStatisticsDataAsync(PROJECT_ID);
            // Assert
            Assert.Equal(EXPECTED_NUM_OF_TICKETS, statisticsDetailsViewModel.ListNumberOfCreatedTicketsByUsers[memberIndex].NumberOfTickets);

        }
        [Fact]
        public async Task GetProjectStatisticDataAsyncTest_CorrectAverageProcessingTime()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);
            ProjectRepository projectRepository = new ProjectRepository(context);
            StatisticsRepository statisticsRepository = new StatisticsRepository(context, projectRoleRepository, projectRepository);
            const int PROJECT_ID = 1;
            const int EXPECTED_ROUNDED_SECONDS = 2;
            // Act
            var statisticsDetailsViewModel = await statisticsRepository.GetProjectStatisticsDataAsync(PROJECT_ID);
            // Assert
            Assert.Equal(EXPECTED_ROUNDED_SECONDS, Math.Round( statisticsDetailsViewModel.AVGProcessingTimeOfTickets.TotalSeconds));
        }

        [Fact]
        public async Task GetProjectStatisticDataAsyncTest_ReturnsCategoryUsage()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);
            ProjectRepository projectRepository = new ProjectRepository(context);
            StatisticsRepository statisticsRepository = new StatisticsRepository(context, projectRoleRepository, projectRepository);
            const int PROJECT_ID = 2;
            // Act
            var statisticsDetailsViewModel = await statisticsRepository.GetProjectStatisticsDataAsync(PROJECT_ID);
            // Assert
            Assert.Empty(statisticsDetailsViewModel.ListNumberOfTimesCategoryWasUsed);
        }

        [Theory]
        [InlineData(0,0)]
        [InlineData(0,1)]
        [InlineData(0,2)]
        [InlineData(2,3)]
        [InlineData(1,4)]
        public async Task GetProjectStatisticDataAsyncTest_ReturnsCorrectTicketStatusUsage(int expectedStatusAmount, int listIndex)
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);
            ProjectRepository projectRepository = new ProjectRepository(context);
            StatisticsRepository statisticsRepository = new StatisticsRepository(context, projectRoleRepository, projectRepository);
            const int PROJECT_ID = 1;
            // Act
            var statisticsDetailsViewModel = await statisticsRepository.GetProjectStatisticsDataAsync(PROJECT_ID);
            // Assert
            Assert.Equal(expectedStatusAmount, statisticsDetailsViewModel.ListNumbersOfTicketStatus[listIndex].NumberOfTicketsWithStatus); 
        }

        

        [Fact]
        public async Task GetProjectStatisticDataAsyncTest_ReturnsCorrectNumberOfDeletedTickets()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);
            ProjectRepository projectRepository = new ProjectRepository(context);
            StatisticsRepository statisticsRepository = new StatisticsRepository(context, projectRoleRepository, projectRepository);
            const int PROJECT_ID = 1;
            const int EXPECTED_COUNT = 1;
            // Act
            var statisticsDetailsViewModel = await statisticsRepository.GetProjectStatisticsDataAsync(PROJECT_ID);
            // Assert
            Assert.Equal(EXPECTED_COUNT, statisticsDetailsViewModel.NumberOfDeletedTickets);
        }

        [Fact]
        public async Task GetProjectStatisticDataAsyncTest_ReturnsCorrectNumberOfMembers()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);
            ProjectRepository projectRepository = new ProjectRepository(context);
            StatisticsRepository statisticsRepository = new StatisticsRepository(context, projectRoleRepository, projectRepository);
            const int PROJECT_ID = 1;
            const int EXPECTED_COUNT = 2;
            // Act
            var statisticsDetailsViewModel = await statisticsRepository.GetProjectStatisticsDataAsync(PROJECT_ID);
            // Assert
            Assert.Equal(EXPECTED_COUNT, statisticsDetailsViewModel.NumberOfMembers);
        }

        [Fact]
        public async Task GetProjectStatisticDataAsyncTest_ReturnsCorrectNumberOfTicketsCreated()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);
            ProjectRepository projectRepository = new ProjectRepository(context);
            StatisticsRepository statisticsRepository = new StatisticsRepository(context, projectRoleRepository, projectRepository);
            const int PROJECT_ID = 1;
            const int EXPECTED_COUNT = 3;
            // Act
            var statisticsDetailsViewModel = await statisticsRepository.GetProjectStatisticsDataAsync(PROJECT_ID);
            // Assert
            Assert.Equal(EXPECTED_COUNT, statisticsDetailsViewModel.NumberOfTicketsCreated);
        }
        [Fact]
        public async Task GetProjectStatisticDataAsyncTest_ReturnsCorrectNumberOfTicketsPastDeadlines()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);
            ProjectRepository projectRepository = new ProjectRepository(context);
            StatisticsRepository statisticsRepository = new StatisticsRepository(context, projectRoleRepository, projectRepository);
            const int PROJECT_ID = 1;
            const int EXPECTED_COUNT = 1;
            // Act
            var statisticsDetailsViewModel = await statisticsRepository.GetProjectStatisticsDataAsync(PROJECT_ID);
            // Assert
            Assert.Equal(EXPECTED_COUNT, statisticsDetailsViewModel.NumberOfTicketsPastDeadline);
        }
        [Theory]
        [InlineData(3,0)]
        [InlineData(0,1)]
        public async Task GetProjectStatisticDataAsyncTest_ReturnsCorrectNumberOfCreatedTicketsByUser(int expectedCount, int index)
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);
            ProjectRepository projectRepository = new ProjectRepository(context);
            StatisticsRepository statisticsRepository = new StatisticsRepository(context, projectRoleRepository, projectRepository);
            const int PROJECT_ID = 1;
            // Act
            var viewModelData = await statisticsRepository.GetProjectStatisticsDataAsync(PROJECT_ID);
            // Assert
            Assert.Equal(expectedCount, viewModelData.ListNumberOfCreatedTicketsByUsers[index].NumberOfTickets);
        }

        [Theory]
        [InlineData(0,0)]
        [InlineData(1,1)]
        public async Task GetProjectStatisticDataAsyncTest_ReturnsCorrectNumberOfTicketsAssignedToUser(int expectedCount, int index)
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);
            ProjectRepository projectRepository = new ProjectRepository(context);
            StatisticsRepository statisticsRepository = new StatisticsRepository(context, projectRoleRepository, projectRepository);
            const int PROJECT_ID = 1;
            // Act
            var viewModelData = await statisticsRepository.GetProjectStatisticsDataAsync(PROJECT_ID); 
            // Assert
            Assert.Equal(expectedCount, viewModelData.ListNumberOfTicketsAssignedToUser[index].NumberOfTickets);
        }

        [Fact]
        public async Task GetProjectStatisticDataAsyncTest_ReturnsCorrectNumberOfTimesCategoryWasUsed()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);
            ProjectRepository projectRepository = new ProjectRepository(context);
            StatisticsRepository statisticsRepository = new StatisticsRepository(context, projectRoleRepository, projectRepository);
            const int PROJECT_ID = 2;
            // Act
            var viewModelData = await statisticsRepository.GetProjectStatisticsDataAsync(PROJECT_ID);
            // Assert
            Assert.Empty(viewModelData.ListNumberOfTimesCategoryWasUsed);
        }

        /*
         * GetCalendarDataAsync test cases
         */
        [Fact]
        public async Task GetCalendarDataAsyncTest_ReturnsCreatedDate()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);
            ProjectRepository projectRepository = new ProjectRepository(context);
            StatisticsRepository statisticsRepository = new StatisticsRepository(context, projectRoleRepository, projectRepository);
            const int PROJECT_ID = 1;
            const int EXPECTED_COUNT = 3;
            // Act
            var statisticsCalendarViewModel = await statisticsRepository.GetCalendarDataAsync(PROJECT_ID);
            // Assert
            Assert.Equal(EXPECTED_COUNT, statisticsCalendarViewModel.ListOfTicketCreatedDate[0].TicketsCount);
        }

        [Fact]
        public async Task GetCalendarDataAsyncTest_ReturnsClosedDate()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);
            ProjectRepository projectRepository = new ProjectRepository(context);
            StatisticsRepository statisticsRepository = new StatisticsRepository(context, projectRoleRepository, projectRepository);
            const int PROJECT_ID = 1;
            const int EXPECTED_COUNT = 2;
            // Act
            var statisticsCalendarViewModel = await statisticsRepository.GetCalendarDataAsync(PROJECT_ID);
            // Assert
            Assert.Equal(EXPECTED_COUNT, statisticsCalendarViewModel.ListOfTicketClosedDate[0].TicketsCount);
        }

        [Fact]
        public async Task GetCalendarDataAsyncTest_ReturnsDeadline()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);
            ProjectRepository projectRepository = new ProjectRepository(context);
            StatisticsRepository statisticsRepository = new StatisticsRepository(context, projectRoleRepository, projectRepository);
            const int PROJECT_ID = 1;
            const int EXPECTED_COUNT = 1;
            // Act
            var statisticsCalendarViewModel = await statisticsRepository.GetCalendarDataAsync(PROJECT_ID);
            // Assert
            Assert.Equal(EXPECTED_COUNT, statisticsCalendarViewModel.ListOfTicketDeadlines[0].TicketsCount);
        }
    }
}
