namespace IssuePilot.Models.DBModels
{
    public enum NewsEntryCaseId : int
    {
        NewProject,
        NewTicket,
        NewComment,
        NewStatus,
        AssignedToTicket,
        UnassignedFromTicket,
        AddedToProject,
        RemovedFromProject
    }
}