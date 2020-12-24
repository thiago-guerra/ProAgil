using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProAgil.Domain;
using ProAgil.Domain.Identity;

namespace ProAgil.Repository
{
    public class ProAgilContext : IdentityDbContext<User, Roles, int, 
                                IdentityUserClaim<int>, UserRoles, IdentityUserLogin<int>,
                                IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
         public ProAgilContext(DbContextOptions<ProAgilContext> options) : base (options)
         {

         }

         public DbSet<Evento> Eventos { get; set; }
         public DbSet<Palestrante> Palestrantes { get; set; }
         public DbSet<PalestranteEvento> PalestranteEventos { get; set; }
         public DbSet<RedeSocial> RedeSociais { get; set; }
         public DbSet<Lote> Lotes { get; set; }

         protected override void OnModelCreating(ModelBuilder modelBuilder)  
         {
             base.OnModelCreating(modelBuilder);

             modelBuilder.Entity<UserRoles>(
                 x => {

                     x.HasKey( y => new { y.UserId, y.RoleId});

                    x.HasOne(r => r.Role)
                    .WithMany(z => z.UserRoles)
                    .HasForeignKey(s => s.RoleId)
                    .IsRequired();

                      x.HasOne(r => r.User)
                    .WithMany(z => z.UserRoles)
                    .HasForeignKey(s => s.UserId)
                    .IsRequired();
                 }); 

            modelBuilder.Entity<PalestranteEvento>().HasKey(x => new { x.EventoId, x.PalestranteId });
         }

    }
}