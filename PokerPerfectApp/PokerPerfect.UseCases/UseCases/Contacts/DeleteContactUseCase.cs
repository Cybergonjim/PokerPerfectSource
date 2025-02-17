using PokerPerfect.UseCases.Interfaces.Contacts;
using PokerPerfect.UseCases.PluginInterfaces;

namespace PokerPerfect.UseCases.UseCases.Contacts
{
    public class DeleteContactUseCase : IDeleteContactUseCase
    {
        private readonly IContactRepository contactRepository;

        public DeleteContactUseCase(IContactRepository contactRepository)
        {
            this.contactRepository = contactRepository;
        }

        public async Task ExecuteAsync(int contactId)
        {
            await contactRepository.DeleteContactAsync(contactId);
        }
    }
}
