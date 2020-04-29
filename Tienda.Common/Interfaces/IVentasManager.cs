using System;
using System.Collections.Generic;
using System.Text;
using Tienda.Common.Entidades;

namespace Tienda.Common.Interfaces
{
    public interface IVentasManager : IGenericManager<Ventas>
    {
        IEnumerable<Ventas> VentasEnIntervalo(DateTime fechaInicial, DateTime fechaFinal);
        IEnumerable<Ventas> VentasDeClienteEnIntervalo(string nombreCliente, DateTime fechaInicial, DateTime fechaFinal);
    }
}
