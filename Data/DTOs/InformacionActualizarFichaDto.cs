namespace sistemaQuchooch.Data.DTOs;

public class InformacionActualizarFichaDto
{ 

    public int CodigoEstudiante { get; set; }
    public string? NombreEstudiante {get; set;}
    public string? ApellidoEstudiante {get; set;}

    public int CodigoEstablecimiento { get; set; }

    public int CodigoNivelAcademico { get; set; }

    public int CodigoGrado { get; set; }

    public int? CodigoCarrera { get; set; }

    public DateTime? CicloEscolar { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public int? CodigoModalidadEstudio { get; set; }

    public string? Estatus { get; set; }

    public int CantidadBloque {get; set;}

    public List<BloquesDto> Bloques { get; set; } = new();

}


public class BloquesDto
{
    public int CodigoFichaCalificacionDetalle { get; set; }
     public int? Bloque { get; set; }
    public float Promedio { get; set; }
    public int? CodigoPromedio { get; set; }
    public List<MateriaDto> Materias { get; set; } = new(); // Cambiado a List
}

public class MateriaDto
{
    public string NombreCurso { get; set; }
    public string Nota { get; set; }
    public int CodigoCursoFichaCalificacion { get; set; }
}

