using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/login")]
public class UsuarioController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsuarioController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult Login([FromBody] UsuarioLoginDTO usuarioRequest)
    {        
        var usuarioadmin = new UsuarioModel
        {
            Id = 1,
            Nome = "Renan Stein",
            Email = "clover@aliare.co",
            Senha = "clover123"
        };

        var usuario = new UsuarioModel
        {
            Email = usuarioRequest.Email,
            Senha = usuarioRequest.Senha
        };

        if (usuario.Email != usuarioadmin.Email || usuario.Senha != usuarioadmin.Senha)
        {
            return Unauthorized(new { message = "Usuario ou senha inválidos" });
        }

        var token = JwtService.GenerateToken(usuario);

        return Ok(new { token });
    }
}