using System;
using System.Collections.Generic;

namespace GestionLibrosApi.Models;

public partial class Estudiante
{
    public int EstudianteId { get; set; }

    public string Nombres { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateTime FechaNacimiento { get; set; }
}
