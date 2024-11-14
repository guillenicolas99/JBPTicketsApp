using JBPTicketsApp.Models;
using JBPTicketsApp.Models.Entities;
using JBPTicketsApp.Recursos;
using JBPTicketsApp.Servicios.Contrato;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace JBPTicketsApp.Controllers
{
    public class InicioController : Controller
    {
        private readonly IUsuarioService _usuarioService;

        private readonly AppDbContext _context;

        public InicioController(IUsuarioService usuarioService, AppDbContext context)
        {
            _usuarioService = usuarioService;
            _context = context;
        }

        public IActionResult IniciarSesion()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                // Si ya ha iniciado sesión, redirige al área principal
                return RedirectToAction("Index", "Home"); // Cambia "Dashboard" y "Index" según tu aplicación
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> IniciarSesion(string correo, string clave)
        {
            Usuario usuarioEncontrado = await _usuarioService.GetUsuario(correo, Utilidades.EncriptarClave(clave));
            var usuario = _context.Usuarios.Include(u => u.Rol) // Incluye el rol si está relacionado
                               .FirstOrDefault(u => u.Correo == correo && u.Clave == Utilidades.EncriptarClave(clave));

            if (usuario == null)
            {
                // Redirigir o mostrar un mensaje de error indicando que el usuario no existe
                ViewData["Mensaje"] = "Usuario o contraseña incorrectos.";
                return View();
            }


            if (usuarioEncontrado == null)
            {
                ViewData["Mensaje"] = "No se encontraron coincidencias";
                return View();
            }

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, usuarioEncontrado.NombreUsuario),
                new Claim(ClaimTypes.Role, usuario.Rol.NombreRol)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties
                );

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("IniciarSesion", "Inicio");
        }
    }
}
