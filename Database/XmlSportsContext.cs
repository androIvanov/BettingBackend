using Microsoft.EntityFrameworkCore;
using Entities;

public partial class XmlSportsContext : DbContext
{
    public virtual DbSet<Sport> Sports { get; set; }
    public virtual DbSet<Event> Events { get; set; }
    public virtual DbSet<Match> Matches { get; set; }
    public virtual DbSet<Bet> Bets { get; set; }
    public virtual DbSet<Odd> Odds { get; set; }

    protected readonly IConfiguration Configuration;
    //public XmlSportsContext(IConfiguration configuration)
    //{
    //    Configuration = configuration;
    //}

    public XmlSportsContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Odd>(o =>
        {
            o.HasOne<Bet>().WithMany(b => b.Odds).OnDelete(DeleteBehavior.Cascade);
        });
        modelBuilder.Entity<Bet>(b => { 
            b.HasOne<Match>().WithMany(m => m.Bets).OnDelete(DeleteBehavior.Cascade);
        });
        modelBuilder.Entity<Match>(m => { 
            m.HasOne<Event>().WithMany(e => e.Matches).OnDelete(DeleteBehavior.Cascade);
        });
        modelBuilder.Entity<Event>(e => { 
            e.HasOne<Sport>().WithMany(s => s.Events).OnDelete(DeleteBehavior.Cascade);
        });
    }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    optionsBuilder.UseSqlServer(Configuration.GetConnectionString("ConnectionString"));
    //}
}
