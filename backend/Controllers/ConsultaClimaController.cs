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

        var resultado = await _climaService.ConsultarPorCidadeAsync(dto.Cidade);

        if (resultado is null)
            return NotFound("Cidade não encontrada.");

        return Ok(resultado);
    }
}