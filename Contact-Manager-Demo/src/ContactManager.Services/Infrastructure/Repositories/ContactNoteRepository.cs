using ContactManager.Infrastructure.Repositories;
using ContactManager.Services.Models;

namespace ContactManager.Services.Infrastructure.Repositories
{
    public class ContactNoteRepository : EntityBaseRepository<ContactNote>, IContactNoteRepository
    {
        public ContactNoteRepository(ContactManagerContext context)
            : base(context)
        { }
    }
}
