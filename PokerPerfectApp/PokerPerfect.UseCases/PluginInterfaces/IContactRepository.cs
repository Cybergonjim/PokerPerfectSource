using PokerPerfect.CoreBusiness;
using Contact = PokerPerfect.CoreBusiness.Contact;

namespace PokerPerfect.UseCases.PluginInterfaces
{
    public interface IContactRepository
    {
        Task AddContactAsync(Contact contact);
        Task DeleteContactAsync(int contactId);
        Task<Contact> GetContactByIdAsync(int contactId);
        Task<List<Contact>> GetContactsAsync(string filterText);
        Task UpdateContactAsync(int contactId, Contact contact);
    }

    public interface IGameRepository
    {
        Task AddGameAsync(Game game);
        Task DeleteGameAsync(int gameId);
        Task<Game> GetGameByIdAsync(int gameId);
        Task<List<Game>> GetGamesAsync(string filterText);
        Task UpdateGameAsync(int gameId, Game game);
    }

    public interface IPlayerRepository
    {
        Task AddPlayerAsync(Player player);
        Task DeletePlayerAsync(int playerId);
        Task RebuyPlayerAsync(int playerId);
        Task<Player> GetPlayerByIdAsync(int playerId);
        Task<List<Player>> GetPlayersAsync(string filterText);
        Task UpdatePlayerAsync(int playerId, Player player);
    }

    public interface IBlindRepository
    {
        Task AddBlindAsync(Blind blind);
        Task DeleteBlindAsync(int blindId);
        Task RebuyBlindAsync(int playerId);
        Task<Blind> GetBlindByIdAsync(int blindId);
        Task<List<Blind>> GetBlindsAsync(string filterText);
        Task UpdateBlindAsync(int blindId, Blind blind);
    }

    public interface IPayoutRepository
    {
        Task AddPayoutAsync(Payout payout);
        Task DeletePayoutAsync(int payoutId);
        Task RebuyPayoutAsync(int playerId);
        Task<Payout> GetPayoutByIdAsync(int payoutId);
        Task<List<Payout>> GetPayoutsAsync(string filterText);
        Task UpdatePayoutAsync(int payoutId, Payout payout);
    }

    public interface ITableRepository
    {
        Task AddTableAsync(Table table);
        Task DeleteTableAsync(int tableId);
        Task RebuyTableAsync(int playerId);
        Task<Table> GetTableByIdAsync(int tableId);
        Task<List<Table>> GetTablesAsync(string filterText);
        Task UpdateTableAsync(int tableId, Table table);
    }

    public interface IChipsetRepository
    {
        Task AddChipsetAsync(Chipset chipset);
        Task DeleteChipsetAsync(int chipsetId);
        Task RebuyChipsetAsync(int playerId);
        Task<Chipset> GetChipsetByIdAsync(int chipsetId);
        Task<List<Chipset>> GetChipsetsAsync(string filterText);
        Task UpdateChipsetAsync(int chipsetId, Chipset chipset);
    }

    public interface IChipRepository
    {
        Task AddChipAsync(Chip chip);
        Task DeleteChipAsync(int chipId);
        Task RebuyChipAsync(int playerId);
        Task<Chip> GetChipByIdAsync(int chipId);
        Task<List<Chip>> GetChipsAsync(string filterText);
        Task UpdateChipAsync(int chipId, Chip chip);
    }

}
