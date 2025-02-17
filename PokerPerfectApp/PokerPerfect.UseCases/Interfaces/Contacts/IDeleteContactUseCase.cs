namespace PokerPerfect.UseCases.Interfaces.Contacts
{
    public interface IDeleteContactUseCase
    {
        Task ExecuteAsync(int contactId);
    }
}