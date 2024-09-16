using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace sistemaQuchooch.Data.QuchoochModels;

public partial class EstudiantePatrocinadorInputDto
{

    public int CodigoEstudiante { get; set; }

    public int CodigoPatrocinador { get; set; }
  
}
