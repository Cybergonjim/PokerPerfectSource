using PokerPerfect.UseCases.PluginInterfaces;
using PokerPerfect.CoreBusiness;
using PokerPerfect.UseCases.Interfaces.Payouts;

namespace PokerPerfect.UseCases.UseCases.Payouts
{
    public class EditPayoutUseCase : IEditPayoutUseCase
    {
        private readonly IPayoutRepository payoutRepository;

        public EditPayoutUseCase(IPayoutRepository payoutRepository)
        {
            this.payoutRepository = payoutRepository;
        }

        public async Task ExecuteAsync(int payoutId, Payout payout)
        {
            await payoutRepository.UpdatePayoutAsync(payoutId, payout);
        }
    }
}
