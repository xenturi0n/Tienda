using System;
using System.Collections.Generic;
using System.Text;

namespace Tienda.Common.Entidades
{
    public class Productos : BaseDTO
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public DateTime? FechaHoraInsercion { get; set; }
    }
}
