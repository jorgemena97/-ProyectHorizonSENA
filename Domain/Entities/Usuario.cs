using Domain.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Usuario : IUsuario
    {
        public Usuario()
        {

        }


        public Usuario(string numeroDeDocumento, string tipoDeDocumento, string nombres, string primerApellido, string segundoApellido,
              string correo, string contrasena,
              string municipio, string departamento,  
              string centroDeFormacion, string fichaDeAprendiz, int idRol)
        {
            NumeroDeDocumento = numeroDeDocumento;
            TipoDeDocumento = tipoDeDocumento;
            Nombres = nombres;
            PrimerApellido = primerApellido;
            SegundoApellido = segundoApellido;
            Correo = correo;
            Contrasena = contrasena;
            Municipio = municipio;  
            Departamento = departamento;  // 
            CentroDeFormacion = centroDeFormacion;
            FichaAprendiz = fichaDeAprendiz;
            IdRol = idRol;
            
        }


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UsuarioId { get; set; }
        public string? TipoDeDocumento { get; set; }
        public string? NumeroDeDocumento { get; set; }
        public int IdRol { get; set; }
        public string? Nombres { get; set; }
        public string? PrimerApellido { get; set; }
        public string? SegundoApellido { get; set; }
        public string Correo { get; set; }
        public string Contrasena { get; set; }
        public string? Salt { get; set; }
        public bool? Estado { get; set; }
        public string? Municipio { get; set; }  // Nueva propiedad para municipio
        public string? Departamento { get; set; }  // Nueva propiedad para departamento
        public string? CentroDeFormacion { get; set; }
        public string? FichaAprendiz { get; set; }

        public string? FotoPerfil {  get; set; }

        // Propiedad de navegación para la relación con la tabla intermedia ProyectosUsuario
        public virtual ICollection<ProyectosUsuario> ProyectosUsuarios { get; set; }

        public ICollection<Proyectos> Proyectos { get; set; } = new List<Proyectos>();

        // Propiedad de navegación para la relación con la tabla Rol
        [ForeignKey("IdRol")]
        public virtual Rol Rol { get; set; }
       
    }
}
