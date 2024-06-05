namespace Domain.Entities.Interfaces
{
    public interface IUsuario
    {
        int UsuarioId { get; set; }
        string? TipoDeDocumento { get; set; }
        string? NumeroDeDocumento { get; set; }
        int IdRol { get; set; }
        string? Nombres { get; set; }
        string? PrimerApellido { get; set; }
        string? SegundoApellido { get; set; }
        string? Correo { get; set; }
        string? Contrasena { get; set; }
        string? Salt { get; set; }
        bool? Estado { get; set; }
        string? Municipio { get; set; }  // Nueva propiedad para municipio
        string? Departamento { get; set; }  // Nueva propiedad para departamento
       string? CentroDeFormacion { get; set; }
        string? FotoPerfil { get; set; }

        ICollection<Proyectos> Proyectos { get; set; } 


    }

}
