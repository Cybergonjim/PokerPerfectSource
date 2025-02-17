using PokerPerfect.UseCases.Interfaces.Contacts;
using PokerPerfect.UseCases.PluginInterfaces;
using Contact = PokerPerfect.CoreBusiness.Contact;

namespace PokerPerfect.UseCases.UseCases.Contacts
{
    public class ViewContactUseCase : IViewContactUseCase
    {
        private readonly IContactRepository contactRepository;

        public ViewContactUseCase(IContactRepository contactRepository)
        {
            this.contactRepository = contactRepository;
        }

        public async Task<Contact> ExecuteAsync(int contactId)
        {
            return await contactRepository.GetContactByIdAsync(contactId);
        }
    }
}
