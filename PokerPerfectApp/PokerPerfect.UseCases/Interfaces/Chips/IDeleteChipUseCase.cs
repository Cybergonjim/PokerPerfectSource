namespace PokerPerfect.UseCases.Interfaces.Chips
{
    public interface IDeleteChipUseCase
    {
        Task ExecuteAsync(int chipId);
    }
}