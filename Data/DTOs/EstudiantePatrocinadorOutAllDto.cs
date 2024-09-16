using System;
using System.Collections.Generic;

namespace sistemaQuchooch.Data.QuchoochModels;

public partial class EstudiantePatrocinadorOutAllDto
{
    public int CodigoEstudiantePatrocinador { get; set; }

    public int CodigoEstudiante { get; set; }

    public string NombreEstudiante { get; set; }
    public string ApellidoEstudiante { get; set; }

    public string CodigoBecario { get; set; }


    public int CodigoPatrocinador { get; set; }
    public string NombrePatrocinador { get; set; }
}
