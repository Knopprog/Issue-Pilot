using IssuePilot.Models.DBModels;
using System;
using System.ComponentModel.DataAnnotations;

namespace IssuePilot.Models.ViewModels
{
    public class DashboardIndexViewModel
    {
        public int Id { set; get; }
        public NewsEntryCaseId EntryCaseId { set; get; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy - HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime CreateDate { set; get; }
        public string NewsText { set; get; }
        public bool Seen { set; get; }
        public string Action { set; get; }
        public int? RouteId { set; get; }
        public string Controller { get; set; }
        public TicketStatusId StatusId { get; set; }
    }
}
