using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JBPTicketsApp.Models.Entities
{
    public class Persona
    {
        [Key]
        public int IdPersona { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        public string Nombre { get; set; }

        [ForeignKey("Red")]
        public int IdRed { get; set; }
        public virtual Red? Red { get; set; }

        [ForeignKey("NivelLiderazgo")]
        public int IdNivelLiderazgo { get; set; }
        public virtual NivelLiderazgo? NivelLiderazgo { get; set; }

        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }

}
