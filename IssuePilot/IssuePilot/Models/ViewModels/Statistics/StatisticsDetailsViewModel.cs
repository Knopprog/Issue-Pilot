using System;
using System.Collections.Generic;
using IssuePilot.Models.DBModels;

namespace IssuePilot.Models.ViewModels.Statistics
{
    public class StatisticsDetailsViewModel
    {
        public int Id { get; set; }
        public string ProjectTitle { get; set; }
        public int NumberOfTicketsCreated { get; set; }
        public List<NumbersOfTicketStatus> ListNumbersOfTicketStatus { get; set; }
        public List<NumberOfCreatedTicketsByUser> ListNumberOfCreatedTicketsByUsers { get; set; }
        public List<NumberOfTicketsAssignedToUser> ListNumberOfTicketsAssignedToUser { get; set; }
        public int NumberOfDeletedTickets { get; set; }
        public List<NumberOfTimesCategoryWasUsed> ListNumberOfTimesCategoryWasUsed { get; set; }
        public int NumberOfMembers { get; set; }
        public int NumberOfTicketsPastDeadline { get; set; }
        public TimeSpan AVGProcessingTimeOfTickets { get; set; }
    }
}
