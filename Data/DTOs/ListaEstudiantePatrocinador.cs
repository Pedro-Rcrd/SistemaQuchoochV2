using System;
using System.Collections.Generic;

namespace sistemaQuchooch.Data.DTOs;

public partial class ListaEstudiantePatrocinador
{
     public string? CodigoBecario { get; set; }
     public int CodigoEstudiante { get; set; }

    public string? NombreEstudiante { get; set; }

    public string? ApellidoEstudiante { get; set; }
   
    public int? CantidadPatrocinadores { get; set; }
        public string? Estatus { get; set; }
}
