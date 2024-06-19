using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Rol
{
    [Key]
    public int IdRol { get; set; }

    public string NombreRol {get; set;}
}
