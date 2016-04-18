using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactManager.Services.Models
{
    public class UserContact : IEntityBase
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Avatar { get; set; }
        public string Email { get; set; }
        public string AlternativeEmail { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string BioDetails { get; set; }

        public virtual ICollection<ContactNote> ContactNotes { get; set; }

    }
}
