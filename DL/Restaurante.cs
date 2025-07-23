using System;
using System.Collections.Generic;

namespace DL;

public partial class Restaurante
{
    public int IdRestaurante { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Slogan { get; set; }

    public byte[]? Imagen { get; set; }

    public string Descripcion { get; set; } = null!;
}
