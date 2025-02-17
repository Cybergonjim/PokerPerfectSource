namespace PokerPerfect.UseCases.Interfaces.Payouts
{
    public interface IAddPayoutUseCase
    {
        Task ExecuteAsync(CoreBusiness.Payout payout);
    }
}