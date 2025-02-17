namespace PokerPerfect.UseCases.Interfaces.Payouts
{
    public interface IDeletePayoutUseCase
    {
        Task ExecuteAsync(int payoutId);
    }
}