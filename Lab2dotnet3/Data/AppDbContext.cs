using Microsoft.EntityFrameworkCore;
using Lab2dotnet3.Models;

namespace Lab2dotnet3;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Card> Cards { get; set; }
    public virtual DbSet<Deck> Decks { get; set; }
    public virtual DbSet<DeckCard> DeckCards { get; set; } // ✅ Match existing table

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Finnish_Swedish_CI_AS");

        modelBuilder.Entity<Card>(entity =>
        {
            entity.HasKey(e => e.CardId).HasName("PK__Cards__55FECDAE7607C791");

            entity.Property(e => e.ManaCost).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Type).HasMaxLength(50);
        });

        modelBuilder.Entity<Deck>(entity =>
        {
            entity.HasKey(e => e.DeckId).HasName("PK__Decks__76B5446CE0194D97");

            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasMany(d => d.Cards).WithMany(p => p.Decks)
                .UsingEntity<DeckCard>(
                    j => j.HasOne(dc => dc.Card)
                          .WithMany()
                          .HasForeignKey(dc => dc.CardId),
                    j => j.HasOne(dc => dc.Deck)
                          .WithMany()
                          .HasForeignKey(dc => dc.DeckId),
                    j =>
                    {
                        j.HasKey(dc => new { dc.DeckId, dc.CardId }).HasName("PK__DeckCard__83EAA8B630652EB9");
                        j.ToTable("DeckCards"); // ✅ Match database table
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
