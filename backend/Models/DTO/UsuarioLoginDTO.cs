using System.ComponentModel;

public class UsuarioLoginDTO
{
    [DefaultValue("clover@aliare.co")]
    public string Email { get; set; } 

    [DefaultValue("clover123")]
    public string Senha { get; set; } 
}