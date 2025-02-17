using PokerPerfect.UseCases.PluginInterfaces;
using PokerPerfect.CoreBusiness;
using PokerPerfect.UseCases.Interfaces.Payouts;

namespace PokerPerfect.UseCases.UseCases.Payouts
{
    public class AddPayoutUseCase : IAddPayoutUseCase
    {
        private readonly IPayoutRepository payoutRepository;

        public AddPayoutUseCase(IPayoutRepository payoutRepository)
        {
            this.payoutRepository = payoutRepository;
        }

        public async Task ExecuteAsync(Payout payout)
        {
            await payoutRepository.AddPayoutAsync(payout);
        }
    }
}
