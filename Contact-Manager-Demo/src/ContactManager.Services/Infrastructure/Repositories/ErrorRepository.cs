using ContactManager.Infrastructure.Repositories;
using ContactManager.Services.Models;

namespace ContactManager.Services.Infrastructure.Repositories
{
    public class LoggingRepository : EntityBaseRepository<Error>, ILoggingRepository
    {
        public LoggingRepository(ContactManagerContext context)
            : base(context)
        { }

        public override void Commit()
        {
            try
            {
                base.Commit();
            }
            catch { }
        }
    }
}
