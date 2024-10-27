using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace sistemaQuchooch.Data.QuchoochModels;

public partial class FichaCalificacionOutDto
{
    public int CodigoFichaCalificacion { get; set; }

    public string CodigoBecario { get; set; }

    public string Estudiante { get; set; }
    public string ApellidoEstudiante { get; set; }

    public string Establecimiento { get; set; }

    public string NivelAcademico { get; set; }

    public string? Grado { get; set; }

    public string? Carrera { get; set; }

    public DateTime? CicloEscolar { get; set; }
     public string? Estatus { get; set; }
     public string? ModalidadEstudio { get; set; }
}
