using System;
using System.Collections.Generic;

namespace sistemaQuchooch.Data.QuchoochModels;

public partial class EstudianteDtoInput
{

    public int? CodigoComunidad { get; set; }

    public int? CodigoNivelAcademico { get; set; }

    public int? CodigoGrado { get; set; }

    public int? CodigoCarrera { get; set; }

    public int? CodigoEstablecimiento { get; set; }

    public string NombreEstudiante { get; set; } = null!;

    public string ApellidoEstudiante { get; set; } = null!;

    public DateTime FechaNacimieto { get; set; }

    public string Genero { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public string? TelefonoEstudiante { get; set; }

    public string? Email { get; set; }

    public byte? Sector { get; set; }

    public string? NumeroCasa { get; set; }

    public string? Descripcion { get; set; }

    public string NombrePadre { get; set; } = null!;

    public string? TelefonoPadre { get; set; }

    public string? OficioPadre { get; set; }

    public string NombreMadre { get; set; } = null!;

    public string? TelefonoMadre { get; set; }

    public string? OficioMadre { get; set; }

    public string? FotoPerfil { get; set; }

    public DateTime? FechaRegistro { get; set; }

}
