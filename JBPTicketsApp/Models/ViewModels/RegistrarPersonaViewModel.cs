namespace JBPTicketsApp.Models.ViewModels
{
    public class RegistrarPersonaViewModel
    {
        public List<PersonaViewModel> Persona { get; set; }
    }

    public class PersonaViewModel
    {
        public string Nombre { get; set; }
        public int IdTitulo { get; set; }
        public int IdRed { get; set; }

    }
}
