namespace PorjectHorizonSENA.Models.Interfaces
{
    public interface IUsuario
    {
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
        string? CentroDeFormacion { get; set; }
        string? FichaAprendiz { get; set; }
        string? Departamento { get; set; }
        string? Municipio { get; set; }
    }
}
