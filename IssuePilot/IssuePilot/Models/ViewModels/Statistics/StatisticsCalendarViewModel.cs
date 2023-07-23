using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssuePilot.Models.ViewModels.Statistics
{
    public class StatisticsCalendarViewModel
    {
        public int Id { get; set; }
        public string ProjectTitle { get; set; }
        public List<CalendarEventViewModel> ListOfTicketCreatedDate { get; set; }
        public List<CalendarEventViewModel> ListOfTicketClosedDate { get; set; }
        public List<CalendarEventViewModel> ListOfTicketDeadlines { get; set; }
    }
}
