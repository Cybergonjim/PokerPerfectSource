using System;
using System.Collections.Generic;

public static class Data
{
  public static Dictionary<string, object> data = new Dictionary<string, object>();
}

[Serializable]
public class Blinds
{
  public List<Blind> blinds;
}

[Serializable]
public class Blind
{
  public int blindId;
  public int gameId;
  public int blindNo;
  public int amount;
  public int ante;
}

[Serializable]
public class Contacts
{
  public List<Contact> contacts;
}

[Serializable]
public class Contact
{
  public int contactId;
  public string name;
  public string email;
  public string phone;
  public string address;
  public string handle;
  public string use;
}

[Serializable]
public class Payouts
{
  public List<Payout> payouts;
}

[Serializable]
public class Payout
{
  public int payoutId;
  public int gameId;
  public int payoutNo;
  public int start;
  public int through;
  public float percent;
  public float amount;
}

[Serializable]
public class Tables
{
  public List<Table> tables;
}

[Serializable]
public class Table
{
  public int tableId;
  public int gameId;
  public int tableNo;
  public string active;
  }

[Serializable]
public class Players
{
  public List<Player> players;
}

[Serializable]
public class Player
{
  public int rebuys;
  public int amount;
  public int playerId;
  public int gameId;
  public int tableId;
  public int contactId;
  public string name;
  public string handle;
  public int position;
}

[Serializable]
public class Games
{
  public List<Game> games;
}

[Serializable]
public class Game
{
  public int gameId;
  public string name;
  public string date;
  public string start;
  public string finish;
  public int gameType;
  public int buyIn;
  public int prizePool;
  public int playerCount;
  public int betTime;
  public int blindTime;
  public int breakTime;
  public int playersExp;
  public int durationExp;
  public int chipSet;
  public int chipsStart;
  public int blindStart;
  public int antes;
  public int rebuys;
  public int rebuyExp;
  public int rebuyChips;
  public int rebuyAmount;
  public int addonExp;
  public int addonChips;
  public int addonAmount;
}

[Serializable]
public class Chipsets
{
  public List<Chipset> chipsets;
}

[Serializable]
public class Chipset
{
  public int chipsetId;
  public string description;
  public string denominations;
}

[Serializable]
public class Chips
{
  public List<Chip> chips;
}

[Serializable]
public class Chip
{
  public int chipId;
  public int chipsetId;
  public int denomination;
  public int colorBase;
  public int colorSpoke;
  public int colorDot;
}
