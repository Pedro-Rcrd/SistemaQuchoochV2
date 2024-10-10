using System;
using System.Collections.Generic;

namespace sistemaQuchooch.Data.QuchoochModels;

public partial class FichaCalificacion
{
    public int CodigoFichaCalificacion { get; set; }

    public int CodigoEstudiante { get; set; }

    public int CodigoEstablecimiento { get; set; }

    public int CodigoNivelAcademico { get; set; }

    public int CodigoGrado { get; set; }

    public int? CodigoCarrera { get; set; }

    public DateTime? CicloEscolar { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public int? CodigoModalidadEstudio { get; set; }

    public string? Estatus { get; set; }

    public virtual Carrera? CodigoCarreraNavigation { get; set; }

    public virtual Establecimiento CodigoEstablecimientoNavigation { get; set; } = null!;

    public virtual Estudiante CodigoEstudianteNavigation { get; set; } = null!;

    public virtual Grado CodigoGradoNavigation { get; set; } = null!;

    public virtual ModalidadEstudio? CodigoModalidadEstudioNavigation { get; set; }

    public virtual NivelAcademico CodigoNivelAcademicoNavigation { get; set; } = null!;

    public virtual ICollection<FichaCalificacionDetalle> FichaCalificacionDetalles { get; set; } = new List<FichaCalificacionDetalle>();
}
