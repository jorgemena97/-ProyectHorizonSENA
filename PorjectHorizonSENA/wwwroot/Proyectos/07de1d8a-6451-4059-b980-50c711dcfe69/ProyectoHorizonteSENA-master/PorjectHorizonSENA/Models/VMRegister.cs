using PorjectHorizonSENA.Models.Interfaces;

namespace PorjectHorizonSENA.Models
{
    public class VMRegister : IUsuario
    {
        public string NumeroDeDocumento { get; set; }
        public string TipoDeDocumento { get; set; }
        public string Nombres { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string Correo { get; set; }
        public string Contrasena { get; set; }
        public int IdRol { get; set; }
        public string MunicipioCentroDeFormacion { get; set; }
        public string DepartamentoCentroDeFormacion { get; set; }
        public string CentroDeFormacion { get; set; }
        public string FichaAprendiz { get; set; }
        public string Ubicacion { get; set; }
    }
}
