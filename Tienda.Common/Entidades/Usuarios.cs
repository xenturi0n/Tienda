using System;
using System.Collections.Generic;
using System.Text;

namespace Tienda.Common.Entidades
{
    public class Usuarios : BaseDTO
    {
        public string NombreDeUsuario { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Password { get; set; }
        public DateTime? FechaHoraInsercion { get; set; }

    }
}
