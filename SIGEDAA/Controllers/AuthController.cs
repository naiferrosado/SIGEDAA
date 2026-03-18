using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SIGEDAA.Data;
using SIGEDAA.Models;
using System.Linq;
using System.Security.Claims; //librerías para Claims
using Microsoft.AspNetCore.Authentication; // Para SignInAsync
using Microsoft.AspNetCore.Authentication.Cookies; // Para las Cookies
using System.Threading.Tasks;

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

        // PROCESAR LOGIN (Ahora es async Task)
        [HttpPost]
        public async Task<IActionResult> Login(string Email, string password)
        {
            var usuario = _context.Usuarios
                .FirstOrDefault(u =>
                    u.CorreoElectronico == Email &&
                    u.ClaveAcceso == password);

            if (usuario != null)
            {
                // GUARDAR DATOS EN SESIÓN
                HttpContext.Session.SetInt32("idusuario", usuario.IdUsuario);
                HttpContext.Session.SetString("usuario", usuario.NombreCompleto);
                HttpContext.Session.SetString("rol", usuario.Rol);

                // NUEVO: CREAR LA CREDENCIAL DE SEGURIDAD (COOKIE)
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                    new Claim(ClaimTypes.Name, usuario.NombreCompleto),
                    new Claim(ClaimTypes.Email, usuario.CorreoElectronico),
                    new Claim(ClaimTypes.Role, usuario.Rol)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Iniciar sesión oficialmente para activar el candado [Authorize]
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Dashboard");
            }

            ViewBag.Error = "Usuario o contraseña incorrectos";

            return View();
        }

        // CERRAR SESIÓN (Ahora es async Task)
        public async Task<IActionResult> Logout()
        {
            // Limpiamos tu sesión actual
            HttpContext.Session.Clear();

            // NUEVO: DESTRUIR LA COOKIE DE SEGURIDAD
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Auth");
        }
    }
}