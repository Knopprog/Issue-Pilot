using System;
using System.ComponentModel.DataAnnotations;

namespace IssuePilot.Models.ViewModels.Statistics
{
    public class CalendarEventViewModel
    {
        public int TicketsCount { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
    }
}
