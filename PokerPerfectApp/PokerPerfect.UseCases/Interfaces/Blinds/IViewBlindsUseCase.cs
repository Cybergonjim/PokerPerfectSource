namespace PokerPerfect.UseCases.Interfaces.Blinds
{
    public interface IViewBlindsUseCase
    {
        Task<List<CoreBusiness.Blind>> ExecuteAsync(string filterText);
    }
}