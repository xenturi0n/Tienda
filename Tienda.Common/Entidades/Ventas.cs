using System;
using System.Collections.Generic;
using System.Text;

namespace Tienda.Common.Entidades
{
    public class Ventas : BaseDTO
    {
        public int IdVenta { get; set; }
        public DateTime FechaHora { get; set; }
        public string Vendedor { get; set; }

        public string Cliente { get; set; }
        public DateTime? FechaHoraInsercion { get; set; }
    }
}
