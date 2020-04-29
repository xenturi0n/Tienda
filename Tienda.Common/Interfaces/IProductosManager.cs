using System;
using System.Collections.Generic;
using System.Text;
using Tienda.Common.Entidades;

namespace Tienda.Common.Interfaces
{
    public interface IProductosManager : IGenericManager<Productos>
    {
        IEnumerable<Productos> BuscarProductosPorNombre(string criterio);

        Productos BuscarProductoPorNombreExacto(string nombre);
    }
}
