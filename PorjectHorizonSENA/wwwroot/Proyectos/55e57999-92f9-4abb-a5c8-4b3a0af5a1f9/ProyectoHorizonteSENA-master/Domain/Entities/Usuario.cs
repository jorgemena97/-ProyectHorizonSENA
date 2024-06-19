using Domain.Entities.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Usuario : IUsuario
{
    public Usuario()
    {
        
    }

    public Usuario(string numeroDeDocumento, string tipoDeDocumento, string nombreCompleto,
                   string correo, string contrasena,
                   string ubicacion,
                   string centroDeFormacion, string fichaDeAprendiz, int idRol)
    {
        NumeroDeDocumento = numeroDeDocumento;
        TipoDeDocumento = tipoDeDocumento;
        NombreCompleto = nombreCompleto;
        Correo = correo;
        Contrasena = contrasena;
        Ubicacion = ubicacion;
        CentroDeFormacion = centroDeFormacion;
        FichaAprendiz = fichaDeAprendiz;
        IdRol = idRol;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UsuarioId { get; set; }
    public string TipoDeDocumento {get; set;}
    public string NumeroDeDocumento { get; set; }
    public int IdRol { get; set; }
    public int? IdProyecto { get; set; }
    public string NombreCompleto { get; set; }
    public string Correo { get; set; }
    public string Contrasena { get; set; }
    public string? Ubicacion { get; set; }
    public string? CentroDeFormacion {get; set;}
    public string? FichaAprendiz {get; set;}

    [ForeignKey("IdRol")]
    public virtual Rol Rol { get; set; }
    [ForeignKey("IdProyecto")]
    public virtual Proyectos Proyecto { get; set; }
}
