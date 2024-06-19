namespace Domain.Entities.Interfaces
{
    public interface IUsuario
    {
        int UsuarioId { get; set; }
        string TipoDeDocumento { get; set; }
        string NumeroDeDocumento { get; set; }
        int IdRol { get; set; }
        public int? IdProyecto { get; set; }
        string NombreCompleto { get; set; }
        string Correo { get; set; }
        string Contrasena { get; set; }
        string Ubicacion { get; set; }
        string CentroDeFormacion { get; set; }
        string FichaAprendiz { get; set; }
    }
}