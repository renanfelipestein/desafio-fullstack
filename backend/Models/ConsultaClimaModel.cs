using System.Runtime.CompilerServices;

public class ConsultaClimaModel
{
    public int Id { get; set; }
    public string Cidade { get; set; } 
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public decimal Temperatura  { get; set; } 
    public DateTime DataConsulta { get; set; }  = DateTime.UtcNow;
    public int UsuarioId { get; set; }
    public required UsuarioModel Usuario { get; set; } 

    public ConsultaClimaModel()
    {
    }
}