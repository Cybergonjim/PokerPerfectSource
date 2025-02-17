using PokerPerfect.UseCases.Interfaces.Contacts;
using PokerPerfect.UseCases.PluginInterfaces;
using Contact = PokerPerfect.CoreBusiness.Contact;

namespace PokerPerfect.UseCases.UseCases.Contacts
{
    // All the code in this file is included in all platforms.
    public class ViewContactsUseCase : IViewContactsUseCase
    {
        private readonly IContactRepository contactRepository;

        public ViewContactsUseCase(IContactRepository contactRepository)
        {
            this.contactRepository = contactRepository;
        }

        public async Task<List<Contact>> ExecuteAsync(string filterText)
        {
            return await contactRepository.GetContactsAsync(filterText);
        }

    }
}