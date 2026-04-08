using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SIGEDAA.Data;
using SIGEDAA.Models;
using System.Linq;
using System.Security.Claims; //librerías para Claims
using Microsoft.AspNetCore.Authentication; // Para SignInAsync
using Microsoft.AspNetCore.Authentication.Cookies; // Para las Cookies
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using SIGEDAA.Services;
using System.Threading.Tasks;

namespace SIGEDAA.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IPasswordService _passwordService;
        private readonly IAuditTrailService _auditTrailService;

        public AuthController(AppDbContext context, IPasswordService passwordService, IAuditTrailService auditTrailService)
        {
            _context = context;
            _passwordService = passwordService;
            _auditTrailService = auditTrailService;
        }

        // MOSTRAR
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(string Email, string password)
        {
            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.CorreoElectronico == Email);

            if (usuario != null)
            {
                PasswordVerificationResult passwordResult = _passwordService.VerifyPassword(usuario, usuario.ClaveAcceso, password);
                if (passwordResult == PasswordVerificationResult.Failed)
                {
                    await _auditTrailService.RecordAsync("LoginFallido", $"Intento fallido para el correo {Email}.", Email);
                    ViewBag.Error = "Usuario o contrasena incorrectos";
                    return View();
                }

                if (!usuario.EstadoActivo)
                {
                    await _auditTrailService.RecordAsync("LoginBloqueado", $"Acceso bloqueado para el usuario {usuario.CorreoElectronico} por estado inactivo.", usuario.NombreCompleto);
                    ViewBag.Error = "Tu cuenta esta inactiva. Solicita activacion al administrador.";
                    return View();
                }

                if (passwordResult == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    usuario.ClaveAcceso = _passwordService.HashPassword(usuario, password);
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }

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

                await _auditTrailService.RecordAsync("LoginExitoso", $"Inicio de sesion correcto con rol {usuario.Rol}.", usuario.NombreCompleto);

                return RedirectToAction("Index", "Dashboard");
            }

            await _auditTrailService.RecordAsync("LoginFallido", $"Intento fallido para el correo {Email}.", Email);
            ViewBag.Error = "Usuario o contrasena incorrectos";

            return View();
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        // CERRAR SESIÓN (Ahora es async Task)
        public async Task<IActionResult> Logout()
        {
            string actor = User.Identity?.Name ?? HttpContext.Session.GetString("usuario") ?? "Usuario";

            // Limpiamos tu sesión actual
            HttpContext.Session.Clear();

            // NUEVO: DESTRUIR LA COOKIE DE SEGURIDAD
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await _auditTrailService.RecordAsync("Logout", "Sesion cerrada.", actor);

            return RedirectToAction("Login", "Auth");
        }
    }
}
