using IssuePilot.Helper;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using IssuePilot.Models.DBModels;
using System.ComponentModel.DataAnnotations;

namespace IssuePilot.Models.ViewModels
{
    public class TicketDetailsViewModel : TicketDataModel
    {
        public List<string> CategoriesOfTicket { get; set; }
        public TicketStatusId SelectedTicketStatusId { get; set; }
        public TicketStatusId CurrentTicketStatus { get; set; }
        public PaginatedList<Comment> CommentsOfTicket { get; set; }
        public List<string> SelectedAssignees { get; set; }
        public List<string> SelectedAssigneesIds { get; set; }
        [StringLength(3000, ErrorMessage = "Ein Kommentar darf nicht mehr als {2} Zeichen enthalten.")]
        public string CommentInputText { get; set; }
        public SelectList StatusList { get; set; }
        public List<SelectListItem> MemberList { get; set; }
        public string ClosedByUser { get; set; }
        public string CreatedByUser { get; set; }
        public int SelectedCommentId { get; set; }
        public List<string> ImageDataURLList { get; set; }
        public int PostCommand { get; set; }
        public bool IsUserAssigned { get; set; }
        public string CurrentUserId { get; set; }
    }
}
