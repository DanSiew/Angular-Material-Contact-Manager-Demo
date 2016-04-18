using AutoMapper;
using ContactManager.Services.Models;
using ContactManager.Services.ViewModel;

namespace ContactManager.Services.Infrastructure.Mappings
{
    public class DomainToViewModelMappingProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<ContactNote, ContactNoteModel>().ReverseMap();
            Mapper.CreateMap<UserContact, UserContactModel>().ReverseMap();
        }
    }
}
