using JBPTicketsApp.Models;
using JBPTicketsApp.Models.Entities;
using JBPTicketsApp.Recursos;
using JBPTicketsApp.Servicios.Contrato;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace JBPTicketsApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RegisterController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly AppDbContext _context;

        public RegisterController(IUsuarioService usuarioService, AppDbContext context)
        {
            _usuarioService = usuarioService;
            _context = context;
        }
        public IActionResult Registrarse()
        {
            ViewData["IdRol"] = new SelectList(_context.Roles, "IdRol", "NombreRol");

            ViewData["Usuarios"] = _context.Usuarios
                .Include(u => u.Rol)
                .ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrarse(Usuario model)
        {
            model.Clave = Utilidades.EncriptarClave(model.Clave);
            Usuario usuarioCreado = await _usuarioService.SaveUsuario(model);
            if (usuarioCreado.IdUsuario > 0)
            {
                return View();
            }

            ViewData["Mensaje"] = "No se pudo crear el ususario";
            return View();
        }
    }
}
