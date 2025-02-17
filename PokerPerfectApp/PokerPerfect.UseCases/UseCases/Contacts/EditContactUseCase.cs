using PokerPerfect.UseCases.Interfaces.Contacts;
using PokerPerfect.UseCases.PluginInterfaces;
using Contact = PokerPerfect.CoreBusiness.Contact;

namespace PokerPerfect.UseCases.UseCases.Contacts
{
    public class EditContactUseCase : IEditContactUseCase
    {
        private readonly IContactRepository contactRepository;

        public EditContactUseCase(IContactRepository contactRepository)
        {
            this.contactRepository = contactRepository;
        }

        public async Task ExecuteAsync(int contactId, Contact contact)
        {
            await contactRepository.UpdateContactAsync(contactId, contact);
        }
    }
}
