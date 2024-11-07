using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace JBPTicketsApp.Models.Entities
{
    public class Ticket
    {
        [Key]
        public int IdTicket { get; set; }

        public string Codigo { get; set; }

        public double? Abono { get; set; }

        [Required(ErrorMessage = "Ingrese el precio de la ticket")]
        public double Precio { get; set; }

        public double? Descuento { get; set; }

        public DateTime? FechaDescuento { get; set; }

        public string Estado { get; set; }

        [ForeignKey("Evento")]
        public int IdEvento { get; set; }
        public virtual Evento Evento { get; set; }

        [ForeignKey("Categoria")]
        public int IdCategoria { get; set; }
        public virtual Categoria Categoria { get; set; }

        [ForeignKey("Persona")]
        public int IdPersona { get; set; }
        public virtual Persona Persona { get; set; }
    }

}
