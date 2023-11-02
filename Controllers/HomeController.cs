using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CuentasBancarias.Models;
using Microsoft.AspNetCore.Identity;

namespace CuentasBancarias.Controllers;

public class HomeController : Controller{
    private readonly ILogger<HomeController> _logger;
    private MyContext _context;

    public HomeController(ILogger<HomeController> logger, MyContext context){
        _logger = logger;
        _context = context;
    }

    [HttpGet("")]
    public IActionResult Index(){
        return View("Index");
    }

    [HttpGet("logout")]
    public IActionResult Logout(){
        HttpContext.Session.Clear();
        return View("Index");
    }

    [HttpGet("detalle/cuenta")]
    public IActionResult DetalleCuenta(){
        string? email = HttpContext.Session.GetString("email");
        if(email != null){
            Usuario? usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email);
            double saldo = 0;
            HttpContext.Session.SetString("Nombre", $"{usuario.Nombre} {usuario.Apellido}");
            HttpContext.Session.SetInt32("Id", usuario.UsuarioId);
            List<Transaccion>? listaTrans = _context.Transacciones.Where(e => e.UsuarioId == usuario.UsuarioId).ToList();
            ViewBag.trans = listaTrans;
            if(listaTrans == null){
                HttpContext.Session.SetInt32("Saldo", (Int32)saldo);
                return View("DetalleCuenta");
            }else{
                foreach(Transaccion trans in listaTrans){
                    saldo += trans.Cantidad;
                }
                HttpContext.Session.SetInt32("Saldo", (Int32)saldo);
                return View("DetalleCuenta");
            }
        }
        return View("Index");
    }

    // POST

    [HttpPost("procesa/registro")]
    public IActionResult ProcesaRegistro(Usuario usuario){
        if(ModelState.IsValid){
            PasswordHasher<Usuario> Hasher = new PasswordHasher<Usuario>();
            usuario.Password = Hasher.HashPassword(usuario, usuario.Password);
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
            HttpContext.Session.SetString("email", usuario.Email);
            return RedirectToAction("DetalleCuenta");
        }
        return View("Index");
    }

    [HttpPost("procesa/login")]
    public IActionResult ProcesaLogin(Login login){
        if(ModelState.IsValid){
            Usuario? usuario = _context.Usuarios.FirstOrDefault(u => u.Email == login.EmailLogin);
            if(usuario != null){
                PasswordHasher<Login> Hasher = new PasswordHasher<Login>();
                var result = Hasher.VerifyHashedPassword(login, usuario.Password, login.PasswordLogin);
                if(result != 0){
                    HttpContext.Session.SetString("email", login.EmailLogin);
                    return RedirectToAction("DetalleCuenta");
                }
            }
            ModelState.AddModelError("PasswordLogin", "Credenciales incorrectas");
            return View("Index");
        }
        return View("Index");
    }

    [HttpPost("procesa/movimiento")]
    public IActionResult ProcesaMovimiento(Transaccion trans){
        int? saldo = HttpContext.Session.GetInt32("Saldo");
        if((saldo + (int)trans.Cantidad) < 0){
            ModelState.AddModelError("Cantidad", "No puede retirar una cantidad superior a su saldo.");
            return View("DetalleCuenta");
        }
        _context.Transacciones.Add(trans);
        _context.SaveChanges();
        return RedirectToAction("DetalleCuenta");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(){
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
