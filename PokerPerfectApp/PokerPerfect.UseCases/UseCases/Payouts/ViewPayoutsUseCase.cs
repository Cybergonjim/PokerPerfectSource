using PokerPerfect.UseCases.PluginInterfaces;
using PokerPerfect.CoreBusiness;
using PokerPerfect.UseCases.Interfaces.Payouts;

namespace PokerPerfect.UseCases.UseCases.Payouts
{
    // All the code in this file is included in all platforms.
    public class ViewPayoutsUseCase : IViewPayoutsUseCase
    {
        private readonly IPayoutRepository payoutRepository;

        public ViewPayoutsUseCase(IPayoutRepository payoutRepository)
        {
            this.payoutRepository = payoutRepository;
        }

        public async Task<List<Payout>> ExecuteAsync(string filterText)
        {
            return await payoutRepository.GetPayoutsAsync(filterText);
        }

    }
}