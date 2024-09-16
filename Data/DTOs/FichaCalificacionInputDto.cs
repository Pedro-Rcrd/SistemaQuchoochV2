using System;
using System.Collections.Generic;

namespace sistemaQuchooch.Data.QuchoochModels;

public partial class FichaCalificacionInputDto
{
    public int CodigoEstudiante { get; set; }

    public int CodigoEstablecimiento { get; set; }

    public int CodigoNivelAcademico { get; set; }

    public int CodigoGrado { get; set; }

    public int? CodigoCarrera { get; set; }

    public DateTime? CicloEscolar { get; set; }

    public DateTime? FechaRegistro { get; set; }

    
    //Fica de calificación detalle

    public int? Bloque { get; set; }

    public string? ImgEstudiante { get; set; }

    public string? ImgFichaCalificacion { get; set; }

    public string? ImgCarta { get; set; }

    //Cursos de fichas de calificiaciones
    public int? CodigoCurso { get; set; }

    public int? Nota { get; set; }

}
