using PokerPerfect.UseCases.PluginInterfaces;
using PokerPerfect.CoreBusiness;
using PokerPerfect.UseCases.Interfaces.Payouts;

namespace PokerPerfect.UseCases.UseCases.Payouts
{
    public class ViewPayoutUseCase : IViewPayoutUseCase
    {
        private readonly IPayoutRepository payoutRepository;

        public ViewPayoutUseCase(IPayoutRepository payoutRepository)
        {
            this.payoutRepository = payoutRepository;
        }

        public async Task<Payout> ExecuteAsync(int payoutId)
        {
            return await payoutRepository.GetPayoutByIdAsync(payoutId);
        }
    }
}
