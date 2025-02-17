using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;
using System.ComponentModel.DataAnnotations;

namespace PokerPerfect.CoreBusiness
{
	// All the code in this file is included in all platforms.
	public class Blind
	{
		[Required]
		[PrimaryKey, AutoIncrement]
		public int BlindId { get; set; }
		public int GameId { get; set; }
		public int BlindNo { get; set; } // for sorting
		public int Amount { get; set; }
    public int SmallBlind => Amount / 2;
    public int Ante { get; set; }
	}

	public class Payout
	{
		[Required]
		[PrimaryKey, AutoIncrement]
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
		[Required]
		[PrimaryKey, AutoIncrement]
		public int TableId { get; set; } // running index 
		public int GameId { get; set; }
		public int TableNo { get; set; } // running number within game
		public string Active { get; set; }

		public bool IsChecked
		{
			get => Active == "T";
			set => Active = value == true ? "T" : "F";
		}
	}

	public class Player : ObservableObject
	{
		private int rebuys;
		private int amount;

		[Required]
		[PrimaryKey, AutoIncrement]
		public int PlayerId { get; set; } // running index 
		public int GameId { get; set; }
		public int TableId { get; set; }
		public int ContactId { get; set; }
		public string Name { get; set; } // for lookups during game
		public string Handle { get; set; } // for lookups during game
		public int Amount
		{
			get => amount;
			set => SetProperty(ref amount, value);
		}
		public int Rebuys
		{
			get => rebuys;
			set => SetProperty(ref rebuys, value);
		}
		public int Position { get; set; }
	}

	public class Game
	{
		[Required]
		[PrimaryKey, AutoIncrement]
		public int GameId { get; set; }
		public string Name { get; set; }
		public string Date { get; set; } // 01/01/2023 format
		public string Start { get; set; } // 12:10:12 format
		public DateTime DateD
		{
			get => Date == null ? DateTime.Now : DateTime.Parse(Date).Date;
			set => Date = value.ToString();
		}
		public TimeSpan StartTimeSpan
		{
			get => Start == null ? TimeSpan.Zero : DateTime.Parse(Start).TimeOfDay;
			set => Start = value.ToString();
		}
		public TimeSpan FinishTimeSpan
		{
			get => Finish == null ? TimeSpan.Zero : DateTime.Parse(Finish).TimeOfDay;
			set => Finish = value.ToString();
		}
		public string Finish { get; set; }
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

		//public string RebuyAmount_ 
		//{
		//    get { return RebuyAmount.ToString(); }
		//    set 
		//    {
		//        int i = int.TryParse(Regex.Replace(value, "[^.0-9]", ""), out i) ? i : 0;
		//        RebuyAmount = i;
		//    }
		//}
	}

	public class Contact
	{
		[Required]
		[PrimaryKey, AutoIncrement]
		public int ContactId { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public string Email { get; set; }
		public string Phone { get; set; }
		public string Address { get; set; }
		public string Handle { get; set; }
		public string Use { get; set; }

		public bool IsChecked
		{
			get => Use == "T";
			set => Use = value ? "T" : "F";
		}
	}

	public class Chipset
	{
		string denominations;

		[Required]
		[PrimaryKey, AutoIncrement]
		public int ChipsetId { get; set; } // running index 
		public string Description { get; set; }
		public string Denominations
		{
			get => denominations;
			set => denominations = value;
		}
	}

	public class Chip
	{
		private enum ColorCode
		{
			Red = 0,
			Green = 1,
			Blue = 2
		}

		static private byte GetByteFromColor(Color color, ColorCode colorCode)
		{
			color.ToRgb(out byte r, out byte g, out byte b);

			switch (colorCode)
			{
				case ColorCode.Red:
					return r;
				case ColorCode.Green:
					return g;
				case ColorCode.Blue:
					return b;
				default:
					return 0;
			}
		}


		static private Color GetColorFromByte(Color color, byte value, ColorCode colorCode)
		{
			color.ToRgb(out byte r, out byte g, out byte b);

			switch (colorCode)
			{
				case ColorCode.Red:
					return Color.FromRgb(value, g, b);
				case ColorCode.Green:
					return Color.FromRgb(r, value, b);
				case ColorCode.Blue:
					return Color.FromRgb(r, g, value);
				default:
					return Color.FromRgb(r, g, b);
			}
		}

		[Required]
		[PrimaryKey, AutoIncrement]
		public int ChipId { get; set; } // running index 
		public int ChipsetId { get; set; }
		public int Denomination { get; set; }
		public int ColorBase { get; set; }
		public int ColorSpoke { get; set; }
		public int ColorDot { get; set; }

		public byte ColorBaseR
		{
			get => GetByteFromColor(ColorBaseRGB, ColorCode.Red);
			set => GetColorFromByte(ColorBaseRGB, value, ColorCode.Red);
		}
		public byte ColorBaseG
		{
			get => GetByteFromColor(ColorBaseRGB, ColorCode.Green);
			set => GetColorFromByte(ColorBaseRGB, value, ColorCode.Green);
		}
		public byte ColorBaseB
		{
			get => GetByteFromColor(ColorBaseRGB, ColorCode.Blue);
			set => GetColorFromByte(ColorBaseRGB, value, ColorCode.Blue);
		}
		public byte ColorSpokeR
		{
			get => GetByteFromColor(ColorSpokeRGB, ColorCode.Red);
			set => GetColorFromByte(ColorSpokeRGB, value, ColorCode.Red);
		}
		public byte ColorSpokeG
		{
			get => GetByteFromColor(ColorSpokeRGB, ColorCode.Green);
			set => GetColorFromByte(ColorSpokeRGB, value, ColorCode.Green);
		}
		public byte ColorSpokeB
		{
			get => GetByteFromColor(ColorSpokeRGB, ColorCode.Blue);
			set => GetColorFromByte(ColorSpokeRGB, value, ColorCode.Blue);
		}
		public byte ColorDotR
		{
			get => GetByteFromColor(ColorDotRGB, ColorCode.Red);
			set => GetColorFromByte(ColorDotRGB, value, ColorCode.Red);
		}
		public byte ColorDotG
		{
			get => GetByteFromColor(ColorDotRGB, ColorCode.Green);
			set => GetColorFromByte(ColorDotRGB, value, ColorCode.Green);
		}
		public byte ColorDotB
		{
			get => GetByteFromColor(ColorDotRGB, ColorCode.Blue);
			set => GetColorFromByte(ColorDotRGB, value, ColorCode.Blue);
		}

		public Color ColorBaseRGB
		{
			get => Color.FromUint((uint)ColorBase | 0xff000000);
			set => ColorBase = (int)(value.ToUint() & 0x00ffffff);
		}
		public Color ColorSpokeRGB
		{
			get => Color.FromUint((uint)ColorSpoke | 0xff000000);
			set => ColorSpoke = (int)(value.ToUint() & 0x00ffffff);
		}
		public Color ColorDotRGB
		{
			get => Color.FromUint((uint)ColorDot | 0xff000000);
			set => ColorDot = (int)(value.ToUint() & 0x00ffffff);
		}
	}
}