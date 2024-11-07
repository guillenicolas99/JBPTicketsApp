using System.ComponentModel.DataAnnotations;

namespace JBPTicketsApp.Models.Entities
{
    public class Red
    {
        [Key]
        public int IdRed { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        public string Nombre { get; set; }

        public ICollection<Persona> Personas { get; set; } = new List<Persona>();
    }

}
