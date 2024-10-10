using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace sistemaQuchooch.Data.DTOs;

public partial class ImagenesFichaDto
{
    public int? NumeroBloque { get; set; }
    public string? ImgEstudiante { get; set; }
    public string? ImgCarta { get; set; }
    public string? ImgFichaCalificacion { get; set; }
}