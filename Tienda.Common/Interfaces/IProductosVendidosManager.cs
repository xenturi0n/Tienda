using System;
using System.Collections.Generic;
using System.Text;
using Tienda.Common.Entidades;

namespace Tienda.Common.Interfaces
{
    public interface IProductosVendidosManager : IGenericManager<ProductosVendidos>
    {
        IEnumerable<ProductosVendidos> ProductosDeUnaVenta(int idVenta);
    }
}
