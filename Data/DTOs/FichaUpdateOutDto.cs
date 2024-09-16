using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace sistemaQuchooch.Data.QuchoochModels;

public partial class FichaUpdateOutDto
{
    public int CodigoFichaCalificacion { get; set; }

    public int CodigoEstudiante { get; set; }

public string nombreEstudiante { get; set; }
    public int CodigoEstablecimiento { get; set; }

    public int CodigoNivelAcademico { get; set; }

    public int CodigoGrado { get; set; }

    public int? CodigoCarrera { get; set; }

    public DateTime? CicloEscolar { get; set; }

    public DateTime? FechaRegistro { get; set; }
    }
