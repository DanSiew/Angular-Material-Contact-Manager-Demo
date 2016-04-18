using System;

namespace ContactManager.Services.Models
{
    public class ContactNote : IEntityBase
    {
        public int Id { get; set; }
        public string NoteText { get; set; }
        public DateTime NoteDate { get; set; }
        public int UserContactId { get; set; }

        public virtual UserContact UserContact { get; set; }

    }
}
