using PokerPerfect.WebApi;
using PokerPerfect.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using System.Net.NetworkInformation;
using System.Diagnostics;

void MapProperties(object source, object destination)
{
  var sourceType = source.GetType();
  var destinationType = destination.GetType();

  var sourceProperties = sourceType.GetProperties();
  var destinationProperties = destinationType.GetProperties();

  foreach (var sourceProperty in sourceProperties)
  {
    var destinationProperty = destinationProperties.FirstOrDefault(p => p.Name == sourceProperty.Name && p.PropertyType == sourceProperty.PropertyType);

    if (destinationProperty != null && destinationProperty.CanWrite)
    {
      var value = sourceProperty.GetValue(source);
      
      destinationProperty.SetValue(destination, value);
    }
  }
}

Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");

var builder = WebApplication.CreateBuilder(args);

// Get the machine name
//string machineName = Environment.MachineName;
//string connectionStringKey = machineName switch
//{
//  "CHINAMI-A97P7JR" => "SqlServerConnectionTable", // Laptop or work PC
//  "DESKTOP-UNOOTLH" => "SqlServerConnectionHome",  // Home PC
//  _ => "SqlServerConnectionWork"                   // Default to work server if unknown
//};

//// Get the correct connection string
//string selectedConnectionString = builder.Configuration.GetConnectionString(connectionStringKey);

// Configure the database context
//builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(selectedConnectionString));
builder.Services.AddDbContext<ApplicationDbContext>(option => option.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection")));


// Add services to the container.
//builder.Services.AddDbContext<ApplicationDbContext>(option => option.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnectionSQLight")));
//builder.Services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnectionHome")));
//builder.Services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnectionWork")));

//builder.WebHost.UseUrls("https://localhost:7038", "http://localhost:5097");
builder.WebHost.UseUrls("http://0.0.0.0:5097", "https://0.0.0.0:7038");

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapGet("/api/get-ip", () =>
{
  string ipAddress = GetLocalIPAddress();

  if (ipAddress != null)
  {
    // Generate QR code
    QRCodeGenerator qrGenerator = new QRCodeGenerator();
    QRCodeData qrCodeData = qrGenerator.CreateQrCode(ipAddress, QRCodeGenerator.ECCLevel.L);
    PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
    byte[] qrCodeBytes = qrCode.GetGraphic(20);  // Get QR code in PNG format

    // Return the QR code as a PNG image
    return Results.File(qrCodeBytes, "image/png");
  }
  else
  {
    return Results.NotFound("IP address not found");
  }
});

// Function to get the local IP address dynamically
string GetLocalIPAddress()
{
  foreach (var networkInterface in NetworkInterface.GetAllNetworkInterfaces())
  {
    if (networkInterface.OperationalStatus == OperationalStatus.Up)
    {
      foreach (var ipAddress in networkInterface.GetIPProperties().UnicastAddresses)
      {
        if (ipAddress.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
        {
          return ipAddress.Address.ToString(); // Return the first found IPv4 address
        }
      }
    }
  }
  return null; // No valid IP address found
}


//app.MapGet("/api/get-ip", () =>
//{
//  string filePath = @"C:\server_ip.txt";
//  if (System.IO.File.Exists(filePath))
//  {
//    string ipAddress = System.IO.File.ReadAllText(filePath).Trim(); // Remove extra whitespace, including \r\n
//    return Results.Ok(ipAddress); // Return the cleaned IP address
//  }
//  return Results.NotFound("IP address not found");
//});

// **** Blinds *****
app.MapGet("/api/blinds", async ([FromQuery] string? s, ApplicationDbContext db) =>
{
  List<Blind> blinds;

  if (string.IsNullOrWhiteSpace(s))
    blinds = await db.Blinds.ToListAsync();
  else
    blinds = await db.Blinds.Where(x => x.GameId == int.Parse(s)).ToListAsync();

  return Results.Ok(blinds);
});
app.MapGet("/api/blinds/{id}", async (int id, ApplicationDbContext db) =>
{
  var blind = await db.Blinds.FirstOrDefaultAsync(x => x.BlindId == id);
  return Results.Ok(blind);
});
app.MapPost("/api/blinds", async (Blind blind, ApplicationDbContext db) =>
{
  blind.BlindId = 0;

  db.Blinds.Add(blind);

  await db.SaveChangesAsync();
});
app.MapPut("/api/blinds/{id}", async (int id, Blind blind, ApplicationDbContext db) =>
{
  var blindToUpdate = await db.Blinds.FindAsync(id);

  if (blindToUpdate is null)
    return Results.NotFound();

  MapProperties(blind, blindToUpdate);

  await db.SaveChangesAsync();

  return Results.NoContent();
});
app.MapDelete("/api/blinds/{id}", async (int id, ApplicationDbContext db) => {

  var blindToDelete = await db.Blinds.FindAsync(id);

  if (blindToDelete != null)
  {
    db.Blinds.Remove(blindToDelete);
    await db.SaveChangesAsync();
    return Results.Ok(blindToDelete);
  }

  return Results.NotFound();
});

// **** Payout *****
app.MapGet("/api/payouts", async ([FromQuery] string? s, ApplicationDbContext db) =>
{
  List<Payout> payouts;

  if (string.IsNullOrWhiteSpace(s))
    payouts = await db.Payouts.ToListAsync();
  else
    payouts = await db.Payouts.Where(x => x.GameId == int.Parse(s)).ToListAsync();

  return Results.Ok(payouts);
});
app.MapGet("/api/payouts/{id}", async (int id, ApplicationDbContext db) =>
{
  var payout = await db.Payouts.FirstOrDefaultAsync(x => x.PayoutId == id);
  return Results.Ok(payout);
});
app.MapPost("/api/payouts", async (Payout payout, ApplicationDbContext db) =>
{
  payout.PayoutId = 0;

  db.Payouts.Add(payout);

  await db.SaveChangesAsync();
});
app.MapPut("/api/payouts/{id}", async (int id, Payout payout, ApplicationDbContext db) =>
{
  var payoutToUpdate = await db.Payouts.FindAsync(id);

  if (payoutToUpdate is null)
    return Results.NotFound();

  MapProperties(payout, payoutToUpdate);

  await db.SaveChangesAsync();

  return Results.NoContent();
});
app.MapDelete("/api/payouts/{id}", async (int id, ApplicationDbContext db) => {

  var payoutToDelete = await db.Payouts.FindAsync(id);

  if (payoutToDelete != null)
  {
    db.Payouts.Remove(payoutToDelete);
    await db.SaveChangesAsync();
    return Results.Ok(payoutToDelete);
  }

  return Results.NotFound();
});

// **** Tables *****
app.MapGet("/api/tables", async ([FromQuery] string? s, ApplicationDbContext db) =>
{
  List<Table> tables;

  if (string.IsNullOrWhiteSpace(s))
    tables = await db.Tables.ToListAsync();
  else
    tables = await db.Tables.Where(x => x.GameId == int.Parse(s)).ToListAsync();

  return Results.Ok(tables);
});
app.MapGet("/api/tables/{id}", async (int id, ApplicationDbContext db) =>
{
  var table = await db.Tables.FirstOrDefaultAsync(x => x.TableId == id);
  return Results.Ok(table);
});
app.MapPost("/api/tables", async (Table table, ApplicationDbContext db) =>
{
  table.TableId = 0;

  db.Tables.Add(table);

  await db.SaveChangesAsync();
});
app.MapPut("/api/tables/{id}", async (int id, Table table, ApplicationDbContext db) =>
{
  var tableToUpdate = await db.Tables.FindAsync(id);

  if (tableToUpdate is null)
    return Results.NotFound();

  MapProperties(table, tableToUpdate);

  await db.SaveChangesAsync();

  return Results.NoContent();
});
app.MapDelete("/api/tables/{id}", async (int id, ApplicationDbContext db) => {

  var tableToDelete = await db.Tables.FindAsync(id);

  if (tableToDelete != null)
  {
    db.Tables.Remove(tableToDelete);
    await db.SaveChangesAsync();
    return Results.Ok(tableToDelete);
  }

  return Results.NotFound();
});

// **** Players *****
app.MapGet("/api/players", async ([FromQuery] string? s, ApplicationDbContext db) =>
{
  List<Player> players;

  if (int.TryParse(s, out int n) && n > 0)
    players = await db.Players.Where(x => x.GameId == n).ToListAsync();
  else if (string.IsNullOrWhiteSpace(s))
    players = await db.Players.ToListAsync();
  else
    players = await db.Players.Where(x =>
        !string.IsNullOrWhiteSpace(x.Name) && x.Name.ToLower().IndexOf(s.ToLower()) >= 0 ||
        !string.IsNullOrWhiteSpace(x.Handle) && x.Handle.ToLower().IndexOf(s.ToLower()) >= 0).ToListAsync();

  return Results.Ok(players);
});
app.MapGet("/api/players/{id}", async (int id, ApplicationDbContext db) =>
{
  var player = await db.Players.FirstOrDefaultAsync(x => x.PlayerId == id);

  return Results.Ok(player);
});
app.MapPost("/api/players", async (Player player, ApplicationDbContext db) =>
{
  player.PlayerId = 0;

  db.Players.Add(player);

  await db.SaveChangesAsync();
});
app.MapPut("/api/players/{id}", async (int id, Player player, ApplicationDbContext db) =>
{
  var playerToUpdate = await db.Players.FindAsync(id);

  if (playerToUpdate is null)
    return Results.NotFound();

  MapProperties(player, playerToUpdate);

  await db.SaveChangesAsync();

  return Results.NoContent();
});
app.MapDelete("/api/players/{id}", async (int id, ApplicationDbContext db) => {

  var playerToDelete = await db.Players.FindAsync(id);

  if (playerToDelete != null)
  {
    db.Players.Remove(playerToDelete);
    await db.SaveChangesAsync();
    return Results.Ok(playerToDelete);
  }

  return Results.NotFound();
});

// **** Games *****
app.MapGet("/api/games", async ([FromQuery] string? s, ApplicationDbContext db) =>
{
  List<Game> games;

  if (string.IsNullOrWhiteSpace(s))
    games = await db.Games.ToListAsync();
  else
    games = await db.Games.Where(x =>
        !string.IsNullOrWhiteSpace(x.Date) && x.Date.ToLower().IndexOf(s.ToLower()) >= 0).ToListAsync();

  return Results.Ok(games);
});
app.MapGet("/api/games/{id}", async (int id, ApplicationDbContext db) =>
{
  var game = await db.Games.FirstOrDefaultAsync(x => x.GameId == id);
  return Results.Ok(game);
});
app.MapPost("/api/games", async (Game game, ApplicationDbContext db) =>
{
  game.GameId = 0;

  db.Games.Add(game);

  await db.SaveChangesAsync();
});
app.MapPut("/api/games/{id}", async (int id, Game game, ApplicationDbContext db) =>
{
  var gameToUpdate = await db.Games.FindAsync(id);

  if (gameToUpdate is null)
    return Results.NotFound();

  MapProperties(game, gameToUpdate);

  // Set the state to Modified explicitly
  db.Entry(gameToUpdate).State = EntityState.Modified;

  await db.SaveChangesAsync();

  return Results.NoContent();
});
app.MapDelete("/api/games/{id}", async (int id, ApplicationDbContext db) => {

  var gameToDelete = await db.Games.FindAsync(id);

  if (gameToDelete != null)
  {
    db.Games.Remove(gameToDelete);
    await db.SaveChangesAsync();
    return Results.Ok(gameToDelete);
  }

  return Results.NotFound();
});

// **** Contacts *****
app.MapGet("/api/contacts", async ([FromQuery] string? s, ApplicationDbContext db) =>
{
    List<Contact> contacts;

  if (string.IsNullOrWhiteSpace(s))
    contacts = await db.Contacts.ToListAsync();
  else
    contacts = await db.Contacts.Where(x =>
        !string.IsNullOrWhiteSpace(x.Name) && x.Name.ToLower().IndexOf(s.ToLower()) >= 0 ||
        !string.IsNullOrWhiteSpace(x.Email) && x.Email.ToLower().IndexOf(s.ToLower()) >= 0 ||
        !string.IsNullOrWhiteSpace(x.Address) && x.Address.ToLower().IndexOf(s.ToLower()) >= 0 ||
        !string.IsNullOrWhiteSpace(x.Handle) && x.Handle.ToLower().IndexOf(s.ToLower()) >= 0 ||
        !string.IsNullOrWhiteSpace(x.Phone) && x.Phone.ToLower().IndexOf(s.ToLower()) >= 0).ToListAsync();

  return Results.Ok(contacts);
});
app.MapGet("/api/contacts/{id}", async (int id, ApplicationDbContext db) =>
{
  var contact = await db.Contacts.FirstOrDefaultAsync(x => x.ContactId == id);
  return Results.Ok(contact);
});
app.MapPost("/api/contacts", async (Contact contact, ApplicationDbContext db) =>
{
  contact.ContactId = 0;

  db.Contacts.Add(contact);

  await db.SaveChangesAsync();
});
app.MapPut("/api/contacts/{id}", async (int id, Contact contact, ApplicationDbContext db) =>
{
  var contactToUpdate = await db.Contacts.FindAsync(id);

  if (contactToUpdate is null)
    return Results.NotFound();

  MapProperties(contact, contactToUpdate);

  await db.SaveChangesAsync();

  return Results.NoContent();
});
app.MapDelete("/api/contacts/{id}", async (int id, ApplicationDbContext db) => {

  var contactToDelete = await db.Contacts.FindAsync(id);

  if (contactToDelete != null)
  {
    db.Contacts.Remove(contactToDelete);
    await db.SaveChangesAsync();
    return Results.Ok(contactToDelete);
  }

  return Results.NotFound();
});

// **** Chipsets *****
app.MapGet("/api/chipsets", async ([FromQuery] string? s, ApplicationDbContext db) =>
{
  List<Chipset> chipsets;

  chipsets = await db.Chipsets.ToListAsync();

  return Results.Ok(chipsets);
});
app.MapGet("/api/chipsets/{id}", async (int id, ApplicationDbContext db) =>
{
  var chipset = await db.Chipsets.FirstOrDefaultAsync(x => x.ChipsetId == id);
  return Results.Ok(chipset);
});
app.MapPost("/api/chipsets", async (Chipset chipset, ApplicationDbContext db) =>
{
  chipset.ChipsetId = 0;

  db.Chipsets.Add(chipset);

  await db.SaveChangesAsync();
});
app.MapPut("/api/chipsets/{id}", async (int id, Chipset chipset, ApplicationDbContext db) =>
{
  var chipsetToUpdate = await db.Chipsets.FindAsync(id);

  if (chipsetToUpdate is null)
    return Results.NotFound();

  MapProperties(chipset, chipsetToUpdate);

  await db.SaveChangesAsync();

  return Results.NoContent();
});
app.MapDelete("/api/chipsets/{id}", async (int id, ApplicationDbContext db) => {

  var chipsetToDelete = await db.Chipsets.FindAsync(id);

  if (chipsetToDelete != null)
  {
    db.Chipsets.Remove(chipsetToDelete);
    await db.SaveChangesAsync();
    return Results.Ok(chipsetToDelete);
  }

  return Results.NotFound();
});

// **** Chips *****
app.MapGet("/api/chips", async ([FromQuery] string? s, ApplicationDbContext db) =>
{
  List<Chip> chips;

  if (string.IsNullOrWhiteSpace(s))
    chips = await db.Chips.ToListAsync();
  else
    chips = await db.Chips.Where(x => x.ChipsetId == int.Parse(s)).ToListAsync();

  return Results.Ok(chips);
});
app.MapGet("/api/chips/{id}", async (int id, ApplicationDbContext db) =>
{
  var chip = await db.Chips.FirstOrDefaultAsync(x => x.ChipId == id);

  return Results.Ok(chip);
});
app.MapPost("/api/chips", async (Chip chip, ApplicationDbContext db) =>
{
  chip.ChipId = 0;

  db.Chips.Add(chip);

  await db.SaveChangesAsync();
});
app.MapPut("/api/chips/{id}", async (int id, Chip chip, ApplicationDbContext db) =>
{
  var chipToUpdate = await db.Chips.FindAsync(id);

  if (chipToUpdate is null)
    return Results.NotFound();

  MapProperties(chip, chipToUpdate);

  await db.SaveChangesAsync();

  return Results.NoContent();
});
app.MapDelete("/api/chips/{id}", async (int id, ApplicationDbContext db) => {

  var chipToDelete = await db.Chips.FindAsync(id);

  if (chipToDelete != null)
  {
    db.Chips.Remove(chipToDelete);
    await db.SaveChangesAsync();
    return Results.Ok(chipToDelete);
  }

  return Results.NotFound();
});

app.Lifetime.ApplicationStarted.Register(() =>
{
  var url = "https://localhost:7038/api/get-ip"; // Change to your actual URL
  try
  {
    Process.Start(new ProcessStartInfo
    {
      FileName = url,
      UseShellExecute = true
    });
  }
  catch (Exception ex)
  {
    Console.WriteLine($"Failed to open browser: {ex.Message}");
  }
});

app.Run();


