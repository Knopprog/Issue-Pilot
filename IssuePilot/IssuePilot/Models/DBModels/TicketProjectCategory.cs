namespace IssuePilot.Models.DBModels
{
    public class TicketProjectCategory
    {
        public int TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }
        public int TicketCategoryId { get; set; }
        public virtual TicketCategory TicketCategory { get; set; }
    }
}