using Microsoft.EntityFrameworkCore;
using Lab2dotnet3;
using Lab2dotnet3.Models;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Database connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Database connection string is missing!");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ Add this
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});


var app = builder.Build();

// ✅ Add this
app.UseCors("AllowAll");

app.UseSwagger();
app.UseSwaggerUI();

app.UseSwagger();
app.UseSwaggerUI();

// Global exception handling middleware
app.UseExceptionHandler("/error");

app.Map("/error", (HttpContext httpContext) =>
{
    var exception = httpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
    return Results.Problem(detail: exception?.Message, statusCode: 500);
});

// ✅ Get all DeckCards
app.MapGet("/deckcards", async (AppDbContext db) =>
{
    try
    {
        var deckCards = await db.DeckCards.ToListAsync();
        return Results.Ok(deckCards);
    }
    catch (Exception ex)
    {
        return Results.Problem(detail: ex.Message, statusCode: 500);
    }
});

// ✅ Get a single DeckCard by DeckId & CardId
app.MapGet("/deckcards/{deckId}/{cardId}", async (int deckId, int cardId, AppDbContext db) =>
{
    try
    {
        var deckCard = await db.DeckCards.FindAsync(deckId, cardId);
        return deckCard is not null ? Results.Ok(deckCard) : Results.NotFound();
    }
    catch (Exception ex)
    {
        return Results.Problem(detail: ex.Message, statusCode: 500);
    }
});

// ✅ Add a new DeckCard
app.MapPost("/deckcards", async (DeckCard deckCard, AppDbContext db) =>
{
    try
    {
        db.DeckCards.Add(deckCard);
        await db.SaveChangesAsync();
        return Results.Created($"/deckcards/{deckCard.DeckId}/{deckCard.CardId}", deckCard);
    }
    catch (Exception ex)
    {
        return Results.Problem(detail: ex.Message, statusCode: 500);
    }
});

// ✅ Delete a DeckCard
app.MapDelete("/deckcards/{deckId}/{cardId}", async (int deckId, int cardId, AppDbContext db) =>
{
    try
    {
        var deckCard = await db.DeckCards.FindAsync(deckId, cardId);
        if (deckCard is null) return Results.NotFound();

        db.DeckCards.Remove(deckCard);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }
    catch (Exception ex)
    {
        return Results.Problem(detail: ex.Message, statusCode: 500);
    }
});

// ✅ Get all Cards
app.MapGet("/cards", async (AppDbContext db) =>
{
    try
    {
        var cards = await db.Cards.ToListAsync();
        return Results.Ok(cards);
    }
    catch (Exception ex)
    {
        return Results.Problem(detail: ex.Message, statusCode: 500);
    }
});

// ✅ Get a single Card by Id
app.MapGet("/cards/{id}", async (int id, AppDbContext db) =>
{
    try
    {
        var card = await db.Cards.FindAsync(id);
        return card is not null ? Results.Ok(card) : Results.NotFound();
    }
    catch (Exception ex)
    {
        return Results.Problem(detail: ex.Message, statusCode: 500);
    }
});

// ✅ Add a new Card
app.MapPost("/cards", async (Card card, AppDbContext db) =>
{
    try
    {
        db.Cards.Add(card);
        await db.SaveChangesAsync();
        return Results.Created($"/cards/{card.Id}", card);
    }
    catch (Exception ex)
    {
        return Results.Problem(detail: ex.Message, statusCode: 500);
    }
});

// ✅ Update a Card
app.MapPut("/cards/{id}", async (int id, Card updatedCard, AppDbContext db) =>
{
    try
    {
        var card = await db.Cards.FindAsync(id);
        if (card is null) return Results.NotFound();

        card.Name = updatedCard.Name;
        card.Description = updatedCard.Description;
        await db.SaveChangesAsync();
        return Results.NoContent();
    }
    catch (Exception ex)
    {
        return Results.Problem(detail: ex.Message, statusCode: 500);
    }
});

// ✅ Delete a Card
app.MapDelete("/cards/{id}", async (int id, AppDbContext db) =>
{
    try
    {
        var card = await db.Cards.FindAsync(id);
        if (card is null) return Results.NotFound();

        db.Cards.Remove(card);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }
    catch (Exception ex)
    {
        return Results.Problem(detail: ex.Message, statusCode: 500);
    }
});

// ✅ Get all Decks
app.MapGet("/decks", async (AppDbContext db) =>
{
    try
    {
        var decks = await db.Decks.ToListAsync();
        return Results.Ok(decks);
    }
    catch (Exception ex)
    {
        return Results.Problem(detail: ex.Message, statusCode: 500);
    }
});

// ✅ Get a single Deck by Id
app.MapGet("/decks/{id}", async (int id, AppDbContext db) =>
{
    try
    {
        var deck = await db.Decks.FindAsync(id);
        return deck is not null ? Results.Ok(deck) : Results.NotFound();
    }
    catch (Exception ex)
    {
        return Results.Problem(detail: ex.Message, statusCode: 500);
    }
});

// ✅ Add a new Deck
app.MapPost("/decks", async (Deck deck, AppDbContext db) =>
{
    try
    {
        db.Decks.Add(deck);
        await db.SaveChangesAsync();
        return Results.Created($"/decks/{deck.Id}", deck);
    }
    catch (Exception ex)
    {
        return Results.Problem(detail: ex.Message, statusCode: 500);
    }
});

// ✅ Update a Deck
app.MapPut("/decks/{id}", async (int id, Deck updatedDeck, AppDbContext db) =>
{
    try
    {
        var deck = await db.Decks.FindAsync(id);
        if (deck is null) return Results.NotFound();

        deck.Name = updatedDeck.Name;
        deck.Description = updatedDeck.Description;
        await db.SaveChangesAsync();
        return Results.NoContent();
    }
    catch (Exception ex)
    {
        return Results.Problem(detail: ex.Message, statusCode: 500);
    }
});

// ✅ Delete a Deck
app.MapDelete("/decks/{id}", async (int id, AppDbContext db) =>
{
    try
    {
        var deck = await db.Decks.FindAsync(id);
        if (deck is null) return Results.NotFound();

        db.Decks.Remove(deck);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }
    catch (Exception ex)
    {
        return Results.Problem(detail: ex.Message, statusCode: 500);
    }
});

app.Run();
