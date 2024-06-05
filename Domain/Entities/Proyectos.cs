using Domain.Entities.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Proyectos  : IProyecto
    { 
        public Proyectos()
        {
            FechaDeModificacion = DateTime.Now;
            FechaDeCreacion = DateTime.Now;
            ProyectosUsuarios = new HashSet<ProyectosUsuario>();
        }
          public Proyectos(string archivoProyecto, string descripcionProyecto, string nombreProyecto, int idUsuario, string nombres,
                         string primerApellido, int idTipoAportante)
            : this() // Llama al constructor por defecto para inicializar propiedades comunes
        {
            ArchivoProyecto = archivoProyecto;
            DescripcionProyecto = descripcionProyecto;
            NombreProyecto = nombreProyecto;
            IdUsuario = idUsuario;
            Nombres = nombres;
            PrimerApellido = primerApellido;
            IdTipoAportante = idTipoAportante;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdProyectos { get; set; }
        public string? ArchivoProyecto { get; set; }
        public string? DescripcionProyecto { get; set; }
        public string? NombreProyecto { get; set; }
        public int? IdUsuario { get; set; }
        public string? Nombres {  get; set; }
        public string? PrimerApellido { get; set; }
        public int CtdLikes { get; set; }
        public int CtdCalificaciones { get; set; }
        public int CtdComentarios { get; set; }
        public int CtdFeedbacks { get; set; }
        public int IdTipoAportante {  get; set; }
        public DateTime FechaDeCreacion { get; set; }
        public DateTime FechaDeModificacion { get; set; }

        // Cambiar la propiedad Usuarios a una lista de ProyectosUsuario
        public virtual ICollection<ProyectosUsuario> ProyectosUsuarios { get; set; }

        [ForeignKey("IdUsuario")]
        public Usuario Usuario { get; set; }
        
       public ICollection<Comentarios> Comentarios { get; set; }
       
        // Eliminar la anotación ForeignKey, ya que esta relación no necesita una clave foránea en esta clase
    }
}

