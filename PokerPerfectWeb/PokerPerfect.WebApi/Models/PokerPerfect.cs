using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace PokerPerfect.WebApi.Models
{
    // Add-Migration Init
    // Update-Database Init

    public class Blind
    {
        public int BlindId { get; set; }
        public int GameId { get; set; }
        public int BlindNo { get; set; } // for sorting
        public int Amount { get; set; }
        public int Ante { get; set; }
    }
    public class Payout
    {
        public int PayoutId { get; set; } // running index
        public int GameId { get; set; }
        public int PayoutNo { get; set; } // for sorting
        public int Start { get; set; }
        public int Through { get; set; }
        public float Percent { get; set; }
        public float Amount { get; set; }
    }
    public class Table
    {
        public int TableId { get; set; } // running index 
        public int GameId { get; set; }
        public int TableNo { get; set; } // running number within game
        public string? Active { get; set; }
    }
    public class Player
    {
        public int PlayerId { get; set; } // running index 
        public int GameId { get; set; }
        public int TableId { get; set; }
        public int ContactId { get; set; }
        public string? Name { get; set; } // for lookups during game
        public string? Handle { get; set; } // for lookups during game
        public int Amount { get; set; } // amount paid in $
        public int Chips { get; set; } // current chips
        public int Rebuys { get; set; } // Rebuys left
        public int Position { get; set; }
    }
    public class Game
    {
        public int GameId { get; set; }
        public string? Name { get; set; }
        public string? Date { get; set; } // 01/01/2023 format
        public string? Start { get; set; } // 12:10:12 format
        public string? Finish { get; set; } // 12:10:12 format
        public int GameType { get; set; }
        public int BuyIn { get; set; } // $
        public int PrizePool { get; set; } // $
        public int PlayerCount { get; set; }
        public int BetTime { get; set; } // seconds
        public int BlindTime { get; set; } // minutes
        public int BreakTime { get; set; } // minutes
        public int PlayersExp { get; set; }
        public int DurationExp { get; set; } // minutes
        public int ChipSet { get; set; }
        public int ChipsStart { get; set; }
        public int BlindStart { get; set; } // starting big blind
        public int Antes { get; set; } // percent of big blind - zero means none
        public int Rebuys { get; set; }
        public int RebuyExp { get; set; }
        public int RebuyChips { get; set; }
        public int RebuyAmount { get; set; } // $
        public int AddonExp { get; set; }
        public int AddonChips { get; set; }
        public int AddonAmount { get; set; } // $
    }
    public class Contact
    {
        public int ContactId { get; set; }     
        public string? Name { get; set; }        
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Handle { get; set; }
        public string? Use { get; set; }
    }

    public class Chipset
    {
        public int ChipsetId { get; set; } // running index 
        public string? Description { get; set; }
        public string? Denominations { get; set; }
    }
    public class Chip
    {
        public int ChipId { get; set; } // running index 
        public int ChipsetId { get; set; }
        public int Denomination { get; set; }
        public int ColorBase { get; set; }
        public int ColorSpoke { get; set; }
        public int ColorDot { get; set; }
    }
}
