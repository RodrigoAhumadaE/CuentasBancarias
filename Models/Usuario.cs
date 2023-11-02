#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CuentasBancarias.Models;

public class Usuario{
    [Key]
    public int UsuarioId {get;set;}

    [Required(ErrorMessage = "Debe ingresar este dato.")]
    [MinLength(2, ErrorMessage = "El Nombre debe tener al menos 2 caracteres.")]
    public string Nombre {get;set;}

    [Required(ErrorMessage = "Debe ingresar este dato.")]
    [MinLength(2, ErrorMessage = "El Apellido debe tener al menos 2 caracteres.")]
    public string Apellido {get;set;}

    [Required(ErrorMessage = "Debe ingresar este dato.")]
    [EmailAddress(ErrorMessage = "Formato de correo no válido.")]
    [UniqueEmail]
    public string Email {get;set;}

    [Required(ErrorMessage = "Debe ingresar este dato.")]
    [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
    public string Password {get;set;}

    public DateTime FechaCreacion {get;set;} = DateTime.Now;
    public DateTime FechaActualizacion {get;set;} = DateTime.Now;

    [Required(ErrorMessage = "Debe ingresar este dato.")]
    [NotMapped]
    [Compare("Password", ErrorMessage = "El Password no coincide.")]
    public string PassConfirm {get;set;}

    public List<Transaccion> ListaTransacciones {set;get;} = new List<Transaccion>();
}

public class UniqueEmailAttribute : ValidationAttribute{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext){
        if(value == null){
            return new ValidationResult("Email is required!");
        }   

        MyContext _context = (MyContext)validationContext.GetService(typeof(MyContext));

    	if(_context.Usuarios.Any(e => e.Email == value.ToString())){
            return new ValidationResult("El Email ya está registrado.");
        } else {
            return ValidationResult.Success;
        }
    }
}