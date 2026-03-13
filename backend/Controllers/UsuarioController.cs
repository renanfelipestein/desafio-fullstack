using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/login")]
public class UsuarioController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly JwtService _jwtService;

    public UsuarioController(AppDbContext context, JwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] UsuarioLoginDTO usuarioRequest)
    {     
        var verificausuario = await _context.Usuarios
                .AnyAsync(u => u.Email == "clover@aliare.co");           

        if (!verificausuario)
        {
            _context.Usuarios.Add(new UsuarioModel    
        {
            Nome = "Usuario Teste",
            Email = "clover@aliare.co",
            Senha = "clover123"
        }            
        );
            await _context.SaveChangesAsync(); 
        }

        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == usuarioRequest.Email 
                                && u.Senha == usuarioRequest.Senha);

        if (usuario is null) {
            return Unauthorized(new { message = "Usuário ou senha inválidos." });
        }                 

        var token = JwtService.GenerateToken(usuario);

        return Ok(new { token });
    }
}