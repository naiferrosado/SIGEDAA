using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SIGEDAA.Data;
using SIGEDAA.Models;
using System.Linq;

namespace SIGEDAA.Controllers
{
    public class AuthController : Controller
    {

        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }


        // MOSTRAR FORMULARIO LOGIN
        public IActionResult Login()
        {
            return View();
        }


        // PROCESAR LOGIN
        [HttpPost]
        public IActionResult Login(string Email, string password)
        {

            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.Email == Email && u.Password == password);

            if (usuario != null)
            {

                // GUARDAR DATOS EN SESION
                HttpContext.Session.SetInt32("idusuario", usuario.Id);
                HttpContext.Session.SetString("usuario", usuario.Nombre);
                HttpContext.Session.SetString("rol", usuario.Rol);

                return RedirectToAction("Index", "Dashboard");
            }

            ViewBag.Error = "Usuario o contraseña incorrectos";

            return View();
        }


        // CERRAR SESION
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Login", "Auth");
        }

    }
}