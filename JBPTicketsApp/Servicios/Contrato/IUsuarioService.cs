using JBPTicketsApp.Models.Entities;

namespace JBPTicketsApp.Servicios.Contrato
{
    public interface IUsuarioService
    {
        Task<Usuario> GetUsuario(string correo, string clave);
        Task<Usuario> SaveUsuario(Usuario model);

    }
}
