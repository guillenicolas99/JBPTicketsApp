using System.ComponentModel.DataAnnotations;

namespace JBPTicketsApp.Models.Entities
{
    public class NivelLiderazgo
    {
        [Key]
        public int IdNivelLiderazgo { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        public string Nombre { get; set; }

        public ICollection<Persona> Personas { get; set; } = new List<Persona>();
    }

}
