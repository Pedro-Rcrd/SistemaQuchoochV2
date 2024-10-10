namespace sistemaQuchooch.Data.DTOs;

public class InformacionNuevoBloqueFichaDto
{ 
    public string? NombreEstudiante { get; set; }
    public string? ApellidoEstudiante { get; set; }
    public string? CodigoBecario { get; set; }
    public string? Establecimiento { get; set; }
    public string? NivelAcademico { get; set; }
      public int CodigoNivelAcademico { get; set; }
    public string? Grado { get; set; }
    public string? Carrera { get; set; }
    public DateTime? CicloEscolar { get; set; }

    public int NumeroBloque {get; set;}

    public string? Modalidad {get; set;}

    public List<BloqueDto> Bloques { get; set; } = new();
     //public string? Estatus { get; set; }
}
 public class BloqueDto
    {
        public int? Bloque { get; set; }
        public float Promedio { get; set; }
        public int? CodigoPromedio {get; set;}
        public Dictionary<string, string> Materias { get; set; } = new();
    }