using AvondaleIslamicCentre.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AvondaleIslamicCentre.Models;

namespace AvondaleIslamicCentre.Areas.Identity.Data;

public class AICDbContext : IdentityDbContext<AICUser>
{
    public AICDbContext(DbContextOptions<AICDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        /* Customize the ASP.NET Identity model and override the defaults if needed.
        For example, you can rename the ASP.NET Identity table names and more.
        Add your customizations after calling base.OnModelCreating(builder); */
    }

public DbSet<AvondaleIslamicCentre.Models.Booking> Booking { get; set; } = default!;

public DbSet<AvondaleIslamicCentre.Models.Class> Class { get; set; } = default!;

public DbSet<AvondaleIslamicCentre.Models.Teacher> Teachers { get; set; } = default!;

public DbSet<AvondaleIslamicCentre.Models.Hall> Hall { get; set; } = default!;

public DbSet<AvondaleIslamicCentre.Models.Report> Report { get; set; } = default!;

public DbSet<AvondaleIslamicCentre.Models.Student> Students { get; set; } = default!;
}
