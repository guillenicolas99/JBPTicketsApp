using JBPTicketsApp.Models;
using JBPTicketsApp.Models.Entities;
using JBPTicketsApp.Servicios.Contrato;
using Microsoft.EntityFrameworkCore;

namespace JBPTicketsApp.Servicios.Implementacion
{
    public class UsuarioService : IUsuarioService
    {
        private readonly AppDbContext _context;

        public UsuarioService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Usuario> GetUsuario(string correo, string clave)
        {
            Usuario usuarioEncontrado = await _context.Usuarios.Where(u => u.Correo == correo && u.Clave == clave).
                FirstOrDefaultAsync();

            return usuarioEncontrado;
        }

        public async Task<Usuario> SaveUsuario(Usuario model)
        {
            _context.Usuarios.Add(model);
            await _context.SaveChangesAsync();
            return model;
        }
    }
}
