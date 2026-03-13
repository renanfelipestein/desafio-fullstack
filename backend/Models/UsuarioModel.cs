

public class UsuarioModel
{
    public int Id { get; set; }
    public string Nome { get; set; } 
    public string Email { get; set; } 
    public string Senha { get; set; }

    public UsuarioModel()
    {
    }
    public UsuarioModel(string nome, string email, string senha)
    {
        Nome = nome;
        Email = email;
        Senha = senha;
    }
}