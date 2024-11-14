using System.ComponentModel.DataAnnotations;

namespace JBPTicketsApp.Models.Entities
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }
        [Required(ErrorMessage = "El nombre del usuario es requerido"), MinLength(3, ErrorMessage = "El usuario debe contener al menos 3 caracteres")]
        public string NombreUsuario { get; set; }
        [Required(ErrorMessage = "El correo es requerido")]
        public string Correo { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria"), MinLength(8, ErrorMessage = "La contraseña debe contener al menos 8 caracteres")]
        public string Clave { get; set; }

        // Relación con Rol
        public int IdRol { get; set; }
        public Rol Rol { get; set; }
    }

}
