using Microsoft.EntityFrameworkCore;
using BettingEntities;
using DatabaseMessages;

public partial class XmlSportsContext : DbContext
{
    public DbSet<Sport> Sports { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Match> Matches { get; set; }
    public DbSet<Bet> Bets { get; set; }
    public DbSet<Odd> Odds { get; set; }
    public DbSet<DatabaseMessage> DatabaseMessages { get; set; }

    public XmlSportsContext(DbContextOptions options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DatabaseMessage>(dbm =>
        {
            dbm.HasKey(x => x.EntitiyId);
        });

        modelBuilder.Entity<Odd>(o =>
        {
            o.HasOne<Bet>().WithMany(b => b.Odds).OnDelete(DeleteBehavior.Cascade);
        });
        modelBuilder.Entity<Bet>(b =>
        {
            b.HasOne<Match>().WithMany(m => m.Bets).OnDelete(DeleteBehavior.Cascade);
        });
        modelBuilder.Entity<Match>(m =>
        {
            m.HasOne<Event>().WithMany(e => e.Matches).OnDelete(DeleteBehavior.Cascade);
        });
        modelBuilder.Entity<Event>(e =>
        {
            e.HasOne<Sport>().WithMany(s => s.Events).OnDelete(DeleteBehavior.Cascade);
        });
    }
}
