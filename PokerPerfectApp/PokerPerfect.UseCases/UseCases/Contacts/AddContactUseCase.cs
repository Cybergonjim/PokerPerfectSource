using PokerPerfect.UseCases.Interfaces.Contacts;
using PokerPerfect.UseCases.PluginInterfaces;
using Contact = PokerPerfect.CoreBusiness.Contact;

namespace PokerPerfect.UseCases.UseCases.Contacts
{
    public class AddContactUseCase : IAddContactUseCase
    {
        private readonly IContactRepository contactRepository;

        public AddContactUseCase(IContactRepository contactRepository)
        {
            this.contactRepository = contactRepository;
        }

        public async Task ExecuteAsync(Contact contact)
        {
            await contactRepository.AddContactAsync(contact);
        }
    }
}
