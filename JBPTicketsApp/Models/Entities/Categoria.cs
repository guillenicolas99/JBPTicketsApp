using System.ComponentModel.DataAnnotations;

namespace JBPTicketsApp.Models.Entities
{
    public class Categoria
    {
        [Key]
        public int IdCategoria { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        public string Nombre { get; set; }

        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }

}
