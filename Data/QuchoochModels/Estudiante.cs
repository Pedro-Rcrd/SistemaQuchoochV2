using System;
using System.Collections.Generic;

namespace sistemaQuchooch.Data.QuchoochModels;

public partial class Estudiante
{
    public int CodigoEstudiante { get; set; }

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

    public string? CodigoBecario { get; set; }

    public virtual Carrera? CodigoCarreraNavigation { get; set; }

    public virtual Comunidad? CodigoComunidadNavigation { get; set; }

    public virtual Establecimiento? CodigoEstablecimientoNavigation { get; set; }

    public virtual Grado? CodigoGradoNavigation { get; set; }

    public virtual NivelAcademico? CodigoNivelAcademicoNavigation { get; set; }

    public virtual ICollection<EstudiantePatrocinador> EstudiantePatrocinadors { get; set; } = new List<EstudiantePatrocinador>();

    public virtual ICollection<FichaCalificacion> FichaCalificacions { get; set; } = new List<FichaCalificacion>();

    public virtual ICollection<Gasto> Gastos { get; set; } = new List<Gasto>();

    public virtual ICollection<OrdenCompra> OrdenCompras { get; set; } = new List<OrdenCompra>();
}
