using ContactManager.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactManager.Services.Infrastructure.Repositories
{
    public interface IContactNoteRepository : IEntityBaseRepository<ContactNote> {  }

    public interface ILoggingRepository : IEntityBaseRepository<Error> { }

    public interface IUserContactRepository : IEntityBaseRepository<UserContact>
    {
        UserContact GetSingleByName(string name);
    }

}
