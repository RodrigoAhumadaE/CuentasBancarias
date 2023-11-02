#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace CuentasBancarias.Models;

public class Transaccion{

    [Key]
    public int TransID {set;get;}

    [Required(ErrorMessage = "Debe ingresar una cantidad.")]
    public double Cantidad {set;get;}

    public DateTime FechaCreacion {set;get;} = DateTime.Now;

    public int UsuarioId {set;get;}

    public Usuario? Creador {set;get;}
}