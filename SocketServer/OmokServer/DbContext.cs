using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmokServer;
public class HiveDbContext : DbContext
{
    public HiveDbContext(DbContextOptions<HiveDbContext> options) : base(options) { }
}

public class GameDbContext : DbContext
{
    public GameDbContext(DbContextOptions<GameDbContext> options) : base(options) { }
}

