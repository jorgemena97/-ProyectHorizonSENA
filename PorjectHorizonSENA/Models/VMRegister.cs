namespace PorjectHorizonSENA.Models
{
    public class VMRegister
    {
        public string NumeroDeDocumento { get; set; }
        public string TipoDeDocumento { get; set; }
        public string Nombres { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string Correo { get; set; }
        public string Contrasena { get; set; }
        public int IdRol { get; set; }
        public bool Estado {  get; set; } = true;
        public string Municipio { get; set; }  // Nueva propiedad para Municipio
        public string Departamento { get; set; }  // Nueva propiedad para Departamento
        public string CentroDeFormacion { get; set; }
        public string FichaAprendiz { get; set; }
    }
}
