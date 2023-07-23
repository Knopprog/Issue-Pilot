using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IssuePilot.Models.DBModels
{
    public class NewsEntryCase
    {
        [Key]
        public NewsEntryCaseId EntryCaseId { get; set; }
        public string Name { get; set; }
        public virtual List<NewsEntry> NewsEntries { get; set; } = new List<NewsEntry>();
    }
}
