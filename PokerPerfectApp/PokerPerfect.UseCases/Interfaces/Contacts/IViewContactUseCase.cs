namespace PokerPerfect.UseCases.Interfaces.Contacts
{
    public interface IViewContactUseCase
    {
        Task<CoreBusiness.Contact> ExecuteAsync(int contactId);
    }
}