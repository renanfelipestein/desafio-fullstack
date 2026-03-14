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

        try
        {
            var climaCidade = await _climaService.ConsultarPorCidadeAsync(dto.Cidade);    

            var resultado = new ConsultaClimaModel
            {
                Cidade = climaCidade.Cidade,
                Latitude = climaCidade.Latitude,
                Longitude = climaCidade.Longitude,
                Temperatura = climaCidade.Temperatura,
                Usuario = usuario
            };

            _context.ConsultasClima.Add(resultado);
            await _context.SaveChangesAsync(); 

            return Ok(resultado);
        }catch (Exception ex)    
            {
            return NotFound("Cidade não encontrada");
        }
    }

    [Authorize]
    [HttpPost("latlong")]
    public async Task<IActionResult> ConsultarPorLatLong([FromBody] ConsultaLatLogDTO dto)
    {

        var emailusuario = User.FindFirstValue(ClaimTypes.Email);

        if (emailusuario is null)
            return Unauthorized("Token inválido.");

        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == emailusuario);

        if (usuario is null)
            return Unauthorized("Usuário não encontrado.");

        if (dto.Latitude is null && dto.Longitude is null || dto.Latitude is null || dto.Longitude is null)
            return BadRequest("Latitude e Longitude devem ser fornecidos.");

        try
        {
            var climaCordenadas = await _climaService.ConsultarPorCoordenadasAsync(dto.Latitude.Value, dto.Longitude.Value);
            var resultado = new ConsultaClimaModel
        {
            Cidade = climaCordenadas.Cidade,
            Latitude = climaCordenadas.Latitude,
            Longitude = climaCordenadas.Longitude,
            Temperatura = climaCordenadas.Temperatura,
            Usuario = usuario
        };

        _context.ConsultasClima.Add(resultado);
        await _context.SaveChangesAsync(); 

        return Ok(resultado);
        
        }catch (Exception ex)
            {
                return NotFound("Cordenadas não encontrada.");
            } 
    }


    [Authorize]
    [HttpGet("consultaclima")]
    public async Task<IActionResult> GetConsultasClima([FromQuery] string? cidade,
                                                        [FromQuery] double? lat,
                                                        [FromQuery] double? lon)
    {
        
        var emailusuario = User.FindFirstValue(ClaimTypes.Email);
        
        if (emailusuario is null)
            return Unauthorized("Token inválido.");

        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == emailusuario);
        if (usuario is null)
            return Unauthorized("Usuário não autorizado.");

        var trintaDiasAtras = DateTime.UtcNow.AddDays(-30).Date;

        var consultas = await _context.ConsultasClima
            .Where(c => c.DataConsulta.Date > trintaDiasAtras)
            .Where(c => c.Cidade.ToLower().Contains(cidade.ToLower()) ||
                  (c.Latitude == lat && c.Longitude == lon))
            .OrderBy(c => c.Cidade)
            .ThenByDescending(c => c.DataConsulta)
            .Select(c => new ConsultaClimaDTO
            {
                Cidade = c.Cidade,
                Latitude = c.Latitude,
                Longitude = c.Longitude,
                Temperatura = c.Temperatura,
                DataConsulta = c.DataConsulta
            })
            .ToListAsync();   

        if (consultas == null || consultas.Count == 0)
            return NotFound("Nenhuma consulta encontrada para os critérios fornecidos.");

        return Ok(consultas);
        }

        
    

}