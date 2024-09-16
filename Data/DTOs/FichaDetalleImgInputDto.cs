using System;
using System.Collections.Generic;

namespace sistemaQuchooch.Data.QuchoochModels;

public partial class FichaDetalleImgInputDto
{
    public int? CodigoFichaCalificacion { get; set; }

    public int? Bloque { get; set; }

    public IFormFile? ImgEstudiante { get; set; }

    public IFormFile? ImgFicha { get; set; }

    public IFormFile? ImgCarta { get; set; }
}
