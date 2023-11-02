#pragma warning disable CS8618
using Microsoft.EntityFrameworkCore;
namespace CuentasBancarias.Models;

public class MyContext : DbContext{
    public DbSet<Usuario> Usuarios {get;set;}
    public DbSet<Transaccion> Transacciones {get;set;}

    public MyContext(DbContextOptions options) : base(options){}
}