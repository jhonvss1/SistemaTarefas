namespace SistemaTarefas.Models
{

    //Tabelas que será usada no DbContext
    public class UsuarioModel
    {
        public int Id  { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }
    }
}
