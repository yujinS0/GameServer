using Microsoft.EntityFrameworkCore;

namespace OmokServer;
public class HiveDbContext : DbContext
{
    public HiveDbContext(DbContextOptions<HiveDbContext> options) : base(options) { }
}

public class GameDbContext : DbContext
{
    public GameDbContext(DbContextOptions<GameDbContext> options) : base(options) { }
}

