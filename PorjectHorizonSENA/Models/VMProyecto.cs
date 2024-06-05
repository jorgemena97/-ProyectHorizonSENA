namespace ProjectHorizonSENA.Models
{
    public class VMProyecto
    {
        public string NombreProyecto { get; set; }
        public string DescripcionProyecto { get; set; }
        public string ArchivoProyecto { get; set; }
        public string IdUsuario { get; set; }
        public string NombresUsuario { get; set; }
        public string PrimerApellidoUsuario { get; set; }
        public int CantidadLikes { get; set; }
        public int CantidadCalificaciones { get; set; }
        public int CantidadComentarios { get; set; }
        public int CantidadFeedbacks { get; set; }
        public DateTime FechaDeCreacion { get; set; }
        public DateTime FechaDeModificacion { get; set; }
        public int IdTipoAportante { get; set; }
    }
}
