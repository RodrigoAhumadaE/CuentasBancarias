#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace CuentasBancarias.Models;

public class Login{
    [Required(ErrorMessage = "Debe ingresar este dato.")]
    [EmailAddress(ErrorMessage = "Formato de correo no válido.")]
    [CheckEmail]
    public string EmailLogin {get;set;}

    [Required(ErrorMessage = "Debe ingresar este dato.")]
    public string PasswordLogin {get;set;}
}

public class CheckEmailAttribute : ValidationAttribute{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext){
        if(value == null){
            return new ValidationResult("Debe ingresar este dato.");
        }   

        MyContext _context = (MyContext)validationContext.GetService(typeof(MyContext));

    	if(_context.Usuarios.Any(e => e.Email == value.ToString())){
            return ValidationResult.Success;
        } else {
            return new ValidationResult("El Email no está registrado.");
        }
    }
}