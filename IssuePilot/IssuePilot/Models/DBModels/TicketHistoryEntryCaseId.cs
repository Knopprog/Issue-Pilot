namespace IssuePilot.Models.DBModels
{
    public enum TicketHistoryEntryCaseId : int
    {
        MemberAssigned,
        MemberUnassigned,
        TicketClosed,
        TicketOpened,
        TicketCanceled,
        TicketPaused,
        TicketInProgress,
        CommentAdded,
        CommentDeleted
    }
}
