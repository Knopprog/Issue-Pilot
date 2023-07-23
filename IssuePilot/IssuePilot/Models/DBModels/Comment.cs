using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IssuePilot.Models.DBModels
{
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }
        [Required]
        [MaxLength(3001)]
        public string Text { set; get; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy, HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime CreateDate { set; get; }
        public virtual User Creator { set; get; }
        public int TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }
        public virtual ICollection<NewsEntry> NewsEntries { get; set; } = new List<NewsEntry>();
    }
}