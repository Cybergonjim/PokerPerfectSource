namespace PokerPerfect.UseCases.Interfaces.Payouts
{
    public interface IViewPayoutsUseCase
    {
        Task<List<CoreBusiness.Payout>> ExecuteAsync(string filterText);
    }
}