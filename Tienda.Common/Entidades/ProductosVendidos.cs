using System;
using System.Collections.Generic;
using System.Text;

namespace Tienda.Common.Entidades
{
    public class ProductosVendidos : BaseDTO
    {
        public int IdProductoVendido { get; set; }
        public int IdVenta { get; set; }
        public decimal PrecioDeVenta { get; set; }
        public int Cantidad { get; set; }
        public int IdProducto { get; set; }
        public DateTime? FechaHoraInsercion { get; set; }
    }
}
