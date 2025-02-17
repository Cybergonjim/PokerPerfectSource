namespace PokerPerfect.UseCases.Interfaces.Payouts
{
    public interface IViewPayoutUseCase
    {
        Task<CoreBusiness.Payout> ExecuteAsync(int payoutId);
    }
}