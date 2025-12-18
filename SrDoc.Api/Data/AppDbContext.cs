using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SrDoc.Api.Models;

namespace SrDoc.Api.Data;

// Note que IdentityDbContext sem o <IdentityUser> assume o padrão
public class AppDbContext : IdentityDbContext<IdentityUser> 
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Documento> Documentos { get; set; }
    public DbSet<Assinante> Assinantes { get; set; }
}