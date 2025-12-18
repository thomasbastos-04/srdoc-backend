using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SrDoc.Api.Models;

namespace SrDoc.Api.Data;

public class AppDbContext : IdentityDbContext<IdentityUser> {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Document> Documents { get; set; }
    public DbSet<Signatory> Signatories { get; set; }
}