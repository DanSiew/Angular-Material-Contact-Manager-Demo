using ContactManager.Infrastructure.Repositories;
using ContactManager.Services.Models;
using System;

namespace ContactManager.Services.Infrastructure.Repositories
{
    public class UserContactRepository : EntityBaseRepository<UserContact>, IUserContactRepository
    {
        public UserContactRepository(ContactManagerContext context)
            : base(context)
        { }

        public UserContact GetSingleByName(string name)
        {
            throw new NotImplementedException();
        }
    }
}
