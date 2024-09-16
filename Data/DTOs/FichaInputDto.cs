using System;
using System.Collections.Generic;

namespace sistemaQuchooch.Data.QuchoochModels;

public partial class FichaInputDto
{
    public int CodigoEstudiante { get; set; }

    public int CodigoEstablecimiento { get; set; }

    public int CodigoNivelAcademico { get; set; }

    public int CodigoGrado { get; set; }

    public int? CodigoCarrera { get; set; }

    public DateTime? CicloEscolar { get; set; }

    public DateTime? FechaRegistro { get; set; }
}