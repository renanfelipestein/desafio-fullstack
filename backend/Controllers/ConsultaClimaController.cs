using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/consulta-clima")]
public class ConsultaClimaController : ControllerBase
{
    
    private readonly ClimaService _climaService;
    private readonly AppDbContext _context;

    public ConsultaClimaController(ClimaService climaService, AppDbContext context)
    {
        _climaService = climaService;
        _context = context;
    }

    [Authorize]
    [HttpPost("cidade")]
    public async Task<IActionResult> ConsultarPorCidade([FromBody] ConsultaCidadeDTO dto)
    {

        // Verificar se o usuário existe no banco de dados para salvar na consulta
        // Verificar para passar o pais apos a cidade toledo esta buscando na espanha

        var emailusuario = User.FindFirstValue(ClaimTypes.Email);

        if (emailusuario is null)
            return Unauthorized("Token inválido.");

        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == emailusuario);

        if (usuario is null)
            return Unauthorized("Usuário não encontrado.");

        var climaCidade = await _climaService.ConsultarPorCidadeAsync(dto.Cidade);
        
        if (climaCidade is null)
            return NotFound("Cidade não encontrada.");

        var resultado = new ConsultaClimaModel
        {
            Cidade = climaCidade.Cidade,
            Latitude = 0,
            Longitude = 0,
            Temperatura = climaCidade.Temperatura,
            Usuario = usuario
        };

        _context.ConsultasClima.Add(resultado);
        await _context.SaveChangesAsync(); 

        return Ok(resultado);
    }

    [Authorize]
    [HttpPost("latlong")]
    public async Task<IActionResult> ConsultarPorLatLong([FromBody] ConsultaLatLogDTO dto)
    {

        // Verificar se o usuário existe no banco de dados para salvar na consulta
        // Verificar para passar o pais apos a cidade toledo esta buscando na espanha

        var resultado = await _climaService.ConsultarPorCoordenadasAsync(dto.Latitude, dto.Longitude);

        if (resultado is null)
            return NotFound("Coordenadas não encontradas.");

        return Ok(resultado);
    }


}