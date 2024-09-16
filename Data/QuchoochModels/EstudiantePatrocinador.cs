using System;
using System.Collections.Generic;

namespace sistemaQuchooch.Data.QuchoochModels;

public partial class EstudiantePatrocinador
{
    public int CodigoEstudiantePatrocinador { get; set; }

    public int CodigoEstudiante { get; set; }

    public int CodigoPatrocinador { get; set; }

    public virtual Estudiante CodigoEstudianteNavigation { get; set; } = null!;

    public virtual Patrocinador CodigoPatrocinadorNavigation { get; set; } = null!;
}
