using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactManager.Services.ViewModel
{
    public class ContactNoteModel
    {
        public int Id { get; set; }
        public string NoteText { get; set; }
        public DateTime NoteDate { get; set; }

        public virtual UserContactModel UserContact { get; set; }
    }
}
