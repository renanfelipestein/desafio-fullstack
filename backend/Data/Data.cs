using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<UsuarioModel> Usuarios { get; set; }
    public DbSet<ConsultaClimaModel> ConsultasClima { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ConsultaClimaModel>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Temperatura)
                  .HasPrecision(6, 2);

            entity.Property(e => e.DataConsulta)
                  .HasDefaultValueSql("NOW()");

            entity.HasOne(e => e.Usuario)
                  .WithMany()
                  .HasForeignKey(e => e.UsuarioId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}