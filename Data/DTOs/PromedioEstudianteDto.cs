

namespace sistemaQuchooch.Data.DTOs;
public class PromedioEstudianteDto
{
    public int CodigoFichaCalificacion { get; set; }
    public int CodigoEstudiante { get; set; }
    public string NombreEstudiante { get; set; }
    public double PromedioGeneral { get; set; }
    public string? NivelAcademico { get; set; }
    public string? Grado { get; set; }
}
