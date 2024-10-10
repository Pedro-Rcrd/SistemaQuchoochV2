using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using sistemaQuchooch.Data.QuchoochModels;

namespace sistemaQuchooch.Data;

public partial class QuchoochContext : DbContext
{
    public QuchoochContext()
    {
    }

    public QuchoochContext(DbContextOptions<QuchoochContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Accion> Accions { get; set; }

    public virtual DbSet<Carrera> Carreras { get; set; }

    public virtual DbSet<Comunidad> Comunidads { get; set; }

    public virtual DbSet<Curso> Cursos { get; set; }

    public virtual DbSet<CursoFichaCalificacion> CursoFichaCalificacions { get; set; }

    public virtual DbSet<Establecimiento> Establecimientos { get; set; }

    public virtual DbSet<Estudiante> Estudiantes { get; set; }

    public virtual DbSet<EstudiantePatrocinador> EstudiantePatrocinadors { get; set; }

    public virtual DbSet<FichaCalificacion> FichaCalificacions { get; set; }

    public virtual DbSet<FichaCalificacionDetalle> FichaCalificacionDetalles { get; set; }

    public virtual DbSet<Gasto> Gastos { get; set; }

    public virtual DbSet<GastoDetalle> GastoDetalles { get; set; }

    public virtual DbSet<Grado> Grados { get; set; }

    public virtual DbSet<ModalidadEstudio> ModalidadEstudios { get; set; }

    public virtual DbSet<Modulo> Modulos { get; set; }

    public virtual DbSet<NivelAcademico> NivelAcademicos { get; set; }

    public virtual DbSet<OrdenCompra> OrdenCompras { get; set; }

    public virtual DbSet<OrdenCompraDetalle> OrdenCompraDetalles { get; set; }

    public virtual DbSet<Pai> Pais { get; set; }

    public virtual DbSet<Patrocinador> Patrocinadors { get; set; }

    public virtual DbSet<Permiso> Permisos { get; set; }

    public virtual DbSet<Promedio> Promedios { get; set; }

    public virtual DbSet<Proveedor> Proveedors { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=PEDRO-LàPEZ; Database=quchooch;TrustServerCertificate=True; Trusted_connection=true; TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Accion>(entity =>
        {
            entity.HasKey(e => e.CodigoAccion).HasName("pk_accion_codigoAccion");

            entity.ToTable("accion");

            entity.Property(e => e.CodigoAccion).HasColumnName("codigoAccion");
            entity.Property(e => e.NombreAccion)
                .HasMaxLength(32)
                .HasColumnName("nombreAccion");
        });

        modelBuilder.Entity<Carrera>(entity =>
        {
            entity.HasKey(e => e.CodigoCarrera).HasName("pk_carrera_codigoCarrera");

            entity.ToTable("carrera");

            entity.Property(e => e.CodigoCarrera).HasColumnName("codigoCarrera");
            entity.Property(e => e.CodigoNivelAcademico).HasColumnName("codigoNivelAcademico");
            entity.Property(e => e.Estatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("estatus");
            entity.Property(e => e.NombreCarrera)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("nombreCarrera");

            entity.HasOne(d => d.CodigoNivelAcademicoNavigation).WithMany(p => p.Carreras)
                .HasForeignKey(d => d.CodigoNivelAcademico)
                .HasConstraintName("pf_carrera_nivelAcademico_codigoNivelAcademico");
        });

        modelBuilder.Entity<Comunidad>(entity =>
        {
            entity.HasKey(e => e.CodigoComunidad).HasName("pk_comunidad_codigoComunidad");

            entity.ToTable("comunidad");

            entity.Property(e => e.CodigoComunidad).HasColumnName("codigoComunidad");
            entity.Property(e => e.Estatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("estatus");
            entity.Property(e => e.NombreComunidad)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("nombreComunidad");
        });

        modelBuilder.Entity<Curso>(entity =>
        {
            entity.HasKey(e => e.CodigoCurso).HasName("pk_curso_codigoCurso");

            entity.ToTable("curso");

            entity.Property(e => e.CodigoCurso).HasColumnName("codigoCurso");
            entity.Property(e => e.CodigoNivelAcademico).HasColumnName("codigoNivelAcademico");
            entity.Property(e => e.Estatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("estatus");
            entity.Property(e => e.NombreCurso)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("nombreCurso");

            entity.HasOne(d => d.CodigoNivelAcademicoNavigation).WithMany(p => p.Cursos)
                .HasForeignKey(d => d.CodigoNivelAcademico)
                .HasConstraintName("fk_curso_nivelAcademico_codigoNivelAcademico");
        });

        modelBuilder.Entity<CursoFichaCalificacion>(entity =>
        {
            entity.HasKey(e => e.CodigoCursoFichaCalificacion).HasName("pk_cursoFichaCalificacion_codigoCursoFichaCalificacion");

            entity.ToTable("cursoFichaCalificacion", tb => tb.HasTrigger("trg_ActualizarPromedio"));

            entity.Property(e => e.CodigoCursoFichaCalificacion).HasColumnName("codigoCursoFichaCalificacion");
            entity.Property(e => e.CodigoCurso).HasColumnName("codigoCurso");
            entity.Property(e => e.CodigoFichaCalificacionDetalle).HasColumnName("codigoFichaCalificacionDetalle");
            entity.Property(e => e.Estatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("estatus");
            entity.Property(e => e.Nota).HasColumnName("nota");

            entity.HasOne(d => d.CodigoCursoNavigation).WithMany(p => p.CursoFichaCalificacions)
                .HasForeignKey(d => d.CodigoCurso)
                .HasConstraintName("fk_cursoFichaCalificacion_curs_codigoCurso");

            entity.HasOne(d => d.CodigoFichaCalificacionDetalleNavigation).WithMany(p => p.CursoFichaCalificacions)
                .HasForeignKey(d => d.CodigoFichaCalificacionDetalle)
                .HasConstraintName("fk_cursoFichaCalificacion_fichaCalificacionDetalle_codigoFichaCalificacionDetalle");
        });

        modelBuilder.Entity<Establecimiento>(entity =>
        {
            entity.HasKey(e => e.CodigoEstablecimiento).HasName("pk_establecimiento_codigoEstablecimiento");

            entity.ToTable("establecimiento");

            entity.Property(e => e.CodigoEstablecimiento).HasColumnName("codigoEstablecimiento");
            entity.Property(e => e.Estatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("estatus");
            entity.Property(e => e.NombreEstablecimiento)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("nombreEstablecimiento");
        });

        modelBuilder.Entity<Estudiante>(entity =>
        {
            entity.HasKey(e => e.CodigoEstudiante).HasName("pk_estudiante_codigoEstudiante");

            entity.ToTable("estudiante");

            entity.Property(e => e.CodigoEstudiante).HasColumnName("codigoEstudiante");
            entity.Property(e => e.ApellidoEstudiante)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("apellidoEstudiante");
            entity.Property(e => e.CodigoBecario)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("codigoBecario");
            entity.Property(e => e.CodigoCarrera).HasColumnName("codigoCarrera");
            entity.Property(e => e.CodigoComunidad).HasColumnName("codigoComunidad");
            entity.Property(e => e.CodigoEstablecimiento).HasColumnName("codigoEstablecimiento");
            entity.Property(e => e.CodigoGrado).HasColumnName("codigoGrado");
            entity.Property(e => e.CodigoModalidadEstudio).HasColumnName("codigoModalidadEstudio");
            entity.Property(e => e.CodigoNivelAcademico).HasColumnName("codigoNivelAcademico");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(256)
                .HasColumnName("descripcion");
            entity.Property(e => e.Email)
                .HasMaxLength(128)
                .HasColumnName("email");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("estado");
            entity.Property(e => e.FechaNacimieto)
                .HasColumnType("date")
                .HasColumnName("fechaNacimieto");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("date")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.FotoPerfil).HasColumnName("fotoPerfil");
            entity.Property(e => e.Genero)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("genero");
            entity.Property(e => e.NombreEstudiante)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("nombreEstudiante");
            entity.Property(e => e.NombreMadre)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("nombreMadre");
            entity.Property(e => e.NombrePadre)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("nombrePadre");
            entity.Property(e => e.NumeroCasa)
                .HasMaxLength(16)
                .HasColumnName("numeroCasa");
            entity.Property(e => e.OficioMadre)
                .HasMaxLength(128)
                .HasColumnName("oficioMadre");
            entity.Property(e => e.OficioPadre)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("oficioPadre");
            entity.Property(e => e.Sector).HasColumnName("sector");
            entity.Property(e => e.TelefonoEstudiante)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("telefonoEstudiante");
            entity.Property(e => e.TelefonoMadre)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("telefonoMadre");
            entity.Property(e => e.TelefonoPadre)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("telefonoPadre");

            entity.HasOne(d => d.CodigoCarreraNavigation).WithMany(p => p.Estudiantes)
                .HasForeignKey(d => d.CodigoCarrera)
                .HasConstraintName("fk_estudiante_carrera_codigoCarrera");

            entity.HasOne(d => d.CodigoComunidadNavigation).WithMany(p => p.Estudiantes)
                .HasForeignKey(d => d.CodigoComunidad)
                .HasConstraintName("fk_estudiante_comunidad_codigoComunidad");

            entity.HasOne(d => d.CodigoEstablecimientoNavigation).WithMany(p => p.Estudiantes)
                .HasForeignKey(d => d.CodigoEstablecimiento)
                .HasConstraintName("fk_estudiante_establecimiento_codigoEstablecimiento");

            entity.HasOne(d => d.CodigoGradoNavigation).WithMany(p => p.Estudiantes)
                .HasForeignKey(d => d.CodigoGrado)
                .HasConstraintName("fk_estudiante_grado_codigoGrado");

            entity.HasOne(d => d.CodigoModalidadEstudioNavigation).WithMany(p => p.Estudiantes)
                .HasForeignKey(d => d.CodigoModalidadEstudio)
                .HasConstraintName("fk_estudiante_modalidadEstudio_codigoModalidadEstudio");

            entity.HasOne(d => d.CodigoNivelAcademicoNavigation).WithMany(p => p.Estudiantes)
                .HasForeignKey(d => d.CodigoNivelAcademico)
                .HasConstraintName("fk_estudiante_nivelAcademico_codigoNivelAcademico");
        });

        modelBuilder.Entity<EstudiantePatrocinador>(entity =>
        {
            entity.HasKey(e => e.CodigoEstudiantePatrocinador).HasName("pk_estudiantePatrocinador_codigoestudiantePatrocinador");

            entity.ToTable("estudiantePatrocinador");

            entity.Property(e => e.CodigoEstudiantePatrocinador).HasColumnName("codigoEstudiantePatrocinador");
            entity.Property(e => e.CodigoEstudiante).HasColumnName("codigoEstudiante");
            entity.Property(e => e.CodigoPatrocinador).HasColumnName("codigoPatrocinador");

            entity.HasOne(d => d.CodigoEstudianteNavigation).WithMany(p => p.EstudiantePatrocinadors)
                .HasForeignKey(d => d.CodigoEstudiante)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_estudiantePatrocinador_estudiante_codigoEstudiante");

            entity.HasOne(d => d.CodigoPatrocinadorNavigation).WithMany(p => p.EstudiantePatrocinadors)
                .HasForeignKey(d => d.CodigoPatrocinador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_estudiantePatrocinador_patrocinador_codigoPatrocinador");
        });

        modelBuilder.Entity<FichaCalificacion>(entity =>
        {
            entity.HasKey(e => e.CodigoFichaCalificacion).HasName("pk_fichaCalificacion_codigoFichaCalificacion");

            entity.ToTable("fichaCalificacion");

            entity.Property(e => e.CodigoFichaCalificacion).HasColumnName("codigoFichaCalificacion");
            entity.Property(e => e.CicloEscolar)
                .HasColumnType("date")
                .HasColumnName("cicloEscolar");
            entity.Property(e => e.CodigoCarrera).HasColumnName("codigoCarrera");
            entity.Property(e => e.CodigoEstablecimiento).HasColumnName("codigoEstablecimiento");
            entity.Property(e => e.CodigoEstudiante).HasColumnName("codigoEstudiante");
            entity.Property(e => e.CodigoGrado).HasColumnName("codigoGrado");
            entity.Property(e => e.CodigoModalidadEstudio).HasColumnName("codigoModalidadEstudio");
            entity.Property(e => e.CodigoNivelAcademico).HasColumnName("codigoNivelAcademico");
            entity.Property(e => e.Estatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("estatus");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("date")
                .HasColumnName("fechaRegistro");

            entity.HasOne(d => d.CodigoCarreraNavigation).WithMany(p => p.FichaCalificacions)
                .HasForeignKey(d => d.CodigoCarrera)
                .HasConstraintName("fk_fichaCalificacion_carrera_codigoCarrera");

            entity.HasOne(d => d.CodigoEstablecimientoNavigation).WithMany(p => p.FichaCalificacions)
                .HasForeignKey(d => d.CodigoEstablecimiento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_fichaCalificacion_establecimiento_codigoEstablecimiento");

            entity.HasOne(d => d.CodigoEstudianteNavigation).WithMany(p => p.FichaCalificacions)
                .HasForeignKey(d => d.CodigoEstudiante)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_fichaCalificacion_estudiante_codigoEstudiante");

            entity.HasOne(d => d.CodigoGradoNavigation).WithMany(p => p.FichaCalificacions)
                .HasForeignKey(d => d.CodigoGrado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_fichaCalificacion_grado_codigoGrado");

            entity.HasOne(d => d.CodigoModalidadEstudioNavigation).WithMany(p => p.FichaCalificacions)
                .HasForeignKey(d => d.CodigoModalidadEstudio)
                .HasConstraintName("fk_fichaCalificacion_modalidadEstudio_codigoModalidadEstudio");

            entity.HasOne(d => d.CodigoNivelAcademicoNavigation).WithMany(p => p.FichaCalificacions)
                .HasForeignKey(d => d.CodigoNivelAcademico)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_fichaCalificacion_nivelAcademico_codigoNivelAcademico");
        });

        modelBuilder.Entity<FichaCalificacionDetalle>(entity =>
        {
            entity.HasKey(e => e.CodigoFichaCalificacionDetalle).HasName("pk_fichaCalificacionDetalle_codigoFichaCalificacionDetalle");

            entity.ToTable("fichaCalificacionDetalle");

            entity.Property(e => e.CodigoFichaCalificacionDetalle).HasColumnName("codigoFichaCalificacionDetalle");
            entity.Property(e => e.Bloque).HasColumnName("bloque");
            entity.Property(e => e.CodigoFichaCalificacion).HasColumnName("codigoFichaCalificacion");
            entity.Property(e => e.CodigoPromedio).HasColumnName("codigoPromedio");
            entity.Property(e => e.Desempenio).HasMaxLength(128);
            entity.Property(e => e.Estatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("estatus");
            entity.Property(e => e.ImgCarta).HasColumnName("imgCarta");
            entity.Property(e => e.ImgEstudiante).HasColumnName("imgEstudiante");
            entity.Property(e => e.ImgFichaCalificacion).HasColumnName("imgFichaCalificacion");
            entity.Property(e => e.Promedio)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("promedio");

            entity.HasOne(d => d.CodigoFichaCalificacionNavigation).WithMany(p => p.FichaCalificacionDetalles)
                .HasForeignKey(d => d.CodigoFichaCalificacion)
                .HasConstraintName("fk_fichaCalificacionDetalle_fichaCalificacion_codigoFichaCalificacion");

            entity.HasOne(d => d.CodigoPromedioNavigation).WithMany(p => p.FichaCalificacionDetalles)
                .HasForeignKey(d => d.CodigoPromedio)
                .HasConstraintName("fk_fichaCalificacionDetalle_promedio_desempenio");
        });

        modelBuilder.Entity<Gasto>(entity =>
        {
            entity.HasKey(e => e.CodigoGasto).HasName("pk_gasto_codigoGasto");

            entity.ToTable("gasto");

            entity.Property(e => e.CodigoGasto).HasColumnName("codigoGasto");
            entity.Property(e => e.CodigoEstudiante).HasColumnName("codigoEstudiante");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(1024)
                .HasColumnName("descripcion");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("estado");
            entity.Property(e => e.FechaEntrega)
                .HasColumnType("date")
                .HasColumnName("fechaEntrega");
            entity.Property(e => e.FechaRecibirComprobante)
                .HasColumnType("date")
                .HasColumnName("fechaRecibirComprobante");
            entity.Property(e => e.ImgCheque).HasColumnName("imgCheque");
            entity.Property(e => e.ImgComprobante).HasColumnName("imgComprobante");
            entity.Property(e => e.ImgEstudiante).HasColumnName("imgEstudiante");
            entity.Property(e => e.Monto)
                .HasColumnType("decimal(6, 2)")
                .HasColumnName("monto");
            entity.Property(e => e.NumeroCheque).HasColumnName("numeroCheque");
            entity.Property(e => e.NumeroComprobante)
                .HasMaxLength(128)
                .HasColumnName("numeroComprobante");
            entity.Property(e => e.PersonaRecibe)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("personaRecibe");
            entity.Property(e => e.TipoPago)
                .HasMaxLength(32)
                .HasColumnName("tipoPago");
            entity.Property(e => e.Titulo)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("titulo");

            entity.HasOne(d => d.CodigoEstudianteNavigation).WithMany(p => p.Gastos)
                .HasForeignKey(d => d.CodigoEstudiante)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_gasto_estudiante_codigoEstudiante");
        });

        modelBuilder.Entity<GastoDetalle>(entity =>
        {
            entity.HasKey(e => e.CodigoGastoDetalle).HasName("pk_gastoDetalle_codigoGastoDetalle");

            entity.ToTable("gastoDetalle");

            entity.Property(e => e.CodigoGastoDetalle).HasColumnName("codigoGastoDetalle");
            entity.Property(e => e.Cantidad)
                .HasColumnType("decimal(7, 2)")
                .HasColumnName("cantidad");
            entity.Property(e => e.CodigoGasto).HasColumnName("codigoGasto");
            entity.Property(e => e.NombreProducto)
                .HasMaxLength(128)
                .HasColumnName("nombreProducto");
            entity.Property(e => e.Precio)
                .HasColumnType("decimal(7, 2)")
                .HasColumnName("precio");

            entity.HasOne(d => d.CodigoGastoNavigation).WithMany(p => p.GastoDetalles)
                .HasForeignKey(d => d.CodigoGasto)
                .HasConstraintName("fk_gastoDetalle_gasto_codigoGasto");
        });

        modelBuilder.Entity<Grado>(entity =>
        {
            entity.HasKey(e => e.CodigoGrado).HasName("pk_grado_codigoGrado");

            entity.ToTable("grado");

            entity.Property(e => e.CodigoGrado).HasColumnName("codigoGrado");
            entity.Property(e => e.Estatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("estatus");
            entity.Property(e => e.NombreGrado)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("nombreGrado");
        });

        modelBuilder.Entity<ModalidadEstudio>(entity =>
        {
            entity.HasKey(e => e.CodigoModalidadEstudio).HasName("pk_modalidadEstudio_codigoModalidadEstudio");

            entity.ToTable("modalidadEstudio");

            entity.Property(e => e.CodigoModalidadEstudio).HasColumnName("codigoModalidadEstudio");
            entity.Property(e => e.Estatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("estatus");
            entity.Property(e => e.NombreModalidadEstudio)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("nombreModalidadEstudio");
        });

        modelBuilder.Entity<Modulo>(entity =>
        {
            entity.HasKey(e => e.CodigoModulo).HasName("pk_modulo_codigoModulo");

            entity.ToTable("modulo");

            entity.Property(e => e.CodigoModulo).HasColumnName("codigoModulo");
            entity.Property(e => e.NombreModulo)
                .HasMaxLength(32)
                .HasColumnName("nombreModulo");
        });

        modelBuilder.Entity<NivelAcademico>(entity =>
        {
            entity.HasKey(e => e.CodigoNivelAcademico).HasName("pk_nivelAcademico_codigoNivelAcademico");

            entity.ToTable("nivelAcademico");

            entity.Property(e => e.CodigoNivelAcademico).HasColumnName("codigoNivelAcademico");
            entity.Property(e => e.Estatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("estatus");
            entity.Property(e => e.NombreNivelAcademico)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("nombreNivelAcademico");
        });

        modelBuilder.Entity<OrdenCompra>(entity =>
        {
            entity.HasKey(e => e.CodigoOrdenCompra).HasName("pk_ordenCompra_codigoOrdenCompra");

            entity.ToTable("ordenCompra");

            entity.Property(e => e.CodigoOrdenCompra).HasColumnName("codigoOrdenCompra");
            entity.Property(e => e.CodigoEstudiante).HasColumnName("codigoEstudiante");
            entity.Property(e => e.CodigoProveedor).HasColumnName("codigoProveedor");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(1024)
                .HasColumnName("descripcion");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("estado");
            entity.Property(e => e.FechaCreacion)
                .HasColumnType("date")
                .HasColumnName("fechaCreacion");
            entity.Property(e => e.FechaEntrega)
                .HasColumnType("date")
                .HasColumnName("fechaEntrega");
            entity.Property(e => e.ImgEstudiante).HasColumnName("imgEstudiante");
            entity.Property(e => e.PersonaCreacion)
                .HasMaxLength(128)
                .HasColumnName("personaCreacion");
            entity.Property(e => e.Titulo)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("titulo");
            entity.Property(e => e.Total)
                .HasColumnType("decimal(6, 2)")
                .HasColumnName("total");

            entity.HasOne(d => d.CodigoEstudianteNavigation).WithMany(p => p.OrdenCompras)
                .HasForeignKey(d => d.CodigoEstudiante)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ordenCompra_estudiante_codigoEstudiante");

            entity.HasOne(d => d.CodigoProveedorNavigation).WithMany(p => p.OrdenCompras)
                .HasForeignKey(d => d.CodigoProveedor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ordenCompra_proveedor_codigoProveedor");
        });

        modelBuilder.Entity<OrdenCompraDetalle>(entity =>
        {
            entity.HasKey(e => e.CodigoOrdenCompraDetalle).HasName("pk_ordenCompraDetalle_codigoOrdenCompraDetalle");

            entity.ToTable("ordenCompraDetalle");

            entity.Property(e => e.CodigoOrdenCompraDetalle).HasColumnName("codigoOrdenCompraDetalle");
            entity.Property(e => e.Cantidad)
                .HasColumnType("decimal(7, 2)")
                .HasColumnName("cantidad");
            entity.Property(e => e.CodigoOrdenCompra).HasColumnName("codigoOrdenCompra");
            entity.Property(e => e.NombreProducto)
                .HasMaxLength(128)
                .HasColumnName("nombreProducto");
            entity.Property(e => e.Precio)
                .HasColumnType("decimal(7, 2)")
                .HasColumnName("precio");

            entity.HasOne(d => d.CodigoOrdenCompraNavigation).WithMany(p => p.OrdenCompraDetalles)
                .HasForeignKey(d => d.CodigoOrdenCompra)
                .HasConstraintName("fk_ordenCompraDetalle_ordenCompra_codigoOrdenCompra");
        });

        modelBuilder.Entity<Pai>(entity =>
        {
            entity.HasKey(e => e.CodigoPais).HasName("pk_pais_codigoPais");

            entity.ToTable("pais");

            entity.Property(e => e.CodigoPais).HasColumnName("codigoPais");
            entity.Property(e => e.Estatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("estatus");
            entity.Property(e => e.Nombre)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Patrocinador>(entity =>
        {
            entity.HasKey(e => e.CodigoPatrocinador).HasName("pk_patrocinador_codigoPatrocinador");

            entity.ToTable("patrocinador");

            entity.Property(e => e.CodigoPatrocinador).HasColumnName("codigoPatrocinador");
            entity.Property(e => e.ApellidoPatrocinador)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("apellidoPatrocinador");
            entity.Property(e => e.CodigoPais).HasColumnName("codigoPais");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("estado");
            entity.Property(e => e.FechaCreacion)
                .HasColumnType("date")
                .HasColumnName("fechaCreacion");
            entity.Property(e => e.FechaNacimiento)
                .HasColumnType("date")
                .HasColumnName("fechaNacimiento");
            entity.Property(e => e.FotoPerfil).HasColumnName("fotoPerfil");
            entity.Property(e => e.NombrePatrocinador)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("nombrePatrocinador");
            entity.Property(e => e.Profesion)
                .HasMaxLength(128)
                .HasColumnName("profesion");

            entity.HasOne(d => d.CodigoPaisNavigation).WithMany(p => p.Patrocinadors)
                .HasForeignKey(d => d.CodigoPais)
                .HasConstraintName("fk_patrocinador_pais_codigoPais");
        });

        modelBuilder.Entity<Permiso>(entity =>
        {
            entity.HasKey(e => e.CodigoPermiso).HasName("pk_permiso_codigoPermiso");

            entity.ToTable("permiso");

            entity.Property(e => e.CodigoPermiso).HasColumnName("codigoPermiso");
            entity.Property(e => e.CodigoAccion).HasColumnName("codigoAccion");
            entity.Property(e => e.CodigoModulo).HasColumnName("codigoModulo");
            entity.Property(e => e.CodigoRol).HasColumnName("codigoRol");

            entity.HasOne(d => d.CodigoAccionNavigation).WithMany(p => p.Permisos)
                .HasForeignKey(d => d.CodigoAccion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_permiso_accion_codigoAccion");

            entity.HasOne(d => d.CodigoModuloNavigation).WithMany(p => p.Permisos)
                .HasForeignKey(d => d.CodigoModulo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_permiso_modulo_codigoModulo");

            entity.HasOne(d => d.CodigoRolNavigation).WithMany(p => p.Permisos)
                .HasForeignKey(d => d.CodigoRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_permiso_rol_codigoRol");
        });

        modelBuilder.Entity<Promedio>(entity =>
        {
            entity.HasKey(e => e.CodigoPromedio).HasName("pk_promedio_codigoPromedio");

            entity.ToTable("promedio");

            entity.Property(e => e.CodigoPromedio).HasColumnName("codigoPromedio");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(32)
                .HasColumnName("descripcion");
            entity.Property(e => e.ValorMaximo).HasColumnName("valorMaximo");
            entity.Property(e => e.ValorMinimo).HasColumnName("valorMinimo");
        });

        modelBuilder.Entity<Proveedor>(entity =>
        {
            entity.HasKey(e => e.CodigoProveedor).HasName("pk_proveedor_codigoProveedor");

            entity.ToTable("proveedor");

            entity.Property(e => e.CodigoProveedor).HasColumnName("codigoProveedor");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.Estatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("estatus");
            entity.Property(e => e.NombreEncargado)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("nombreEncargado");
            entity.Property(e => e.NombreProveedor)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("nombreProveedor");
            entity.Property(e => e.Telefono)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("telefono");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.CodigoRol).HasName("pk_rol_codigorol");

            entity.ToTable("rol");

            entity.Property(e => e.CodigoRol).HasColumnName("codigoRol");
            entity.Property(e => e.Estatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("estatus");
            entity.Property(e => e.NombreRol)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("nombreRol");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.CodigoUsuario).HasName("pk_usuario_codigoUsuario");

            entity.ToTable("usuario");

            entity.Property(e => e.CodigoUsuario).HasColumnName("codigoUsuario");
            entity.Property(e => e.CodigoRol).HasColumnName("codigoRol");
            entity.Property(e => e.Contrasenia)
                .HasMaxLength(65)
                .HasColumnName("contrasenia");
            entity.Property(e => e.Email)
                .HasMaxLength(64)
                .HasColumnName("email");
            entity.Property(e => e.Estatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("estatus");
            entity.Property(e => e.FechaCreacion)
                .HasColumnType("date")
                .HasColumnName("fechaCreacion");
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("nombreUsuario");

            entity.HasOne(d => d.CodigoRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.CodigoRol)
                .HasConstraintName("fk_usuario_rol_codigoRol");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
