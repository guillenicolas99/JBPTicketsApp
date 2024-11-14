using System.ComponentModel.DataAnnotations;

namespace JBPTicketsApp.Models.Entities
{
    public class Rol
    {
        [Key]
        public int IdRol { get; set; }
        [Required]
        public string NombreRol { get; set; }

        // Relación con Usuario
        public ICollection<Usuario> Usuarios { get; set; }
    }

}
