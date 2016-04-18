using AutoMapper;

namespace ContactManager.Services.Infrastructure.Mappings
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(m =>
            {
                m.AddProfile<DomainToViewModelMappingProfile>();
            });
        }
    }
}
