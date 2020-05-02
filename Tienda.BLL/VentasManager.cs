using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Tienda.Common.Entidades;
using Tienda.Common.Interfaces;

namespace Tienda.BLL
{
    public class VentasManager : GenericManager<Ventas>, IVentasManager
    {
        public VentasManager(IGenericRepository<Ventas> repositorio) : base(repositorio)
        {
        }

        IEnumerable<Ventas> IVentasManager.VentasDeClienteEnIntervalo(string nombreCliente, DateTime fechaInicial, DateTime fechaFinal)
        {
            try
            {
                DateTime inicio = new DateTime(fechaInicial.Year, fechaInicial.Month, fechaInicial.Day, 0, 0, 0);
                DateTime final = new DateTime(fechaFinal.Year, fechaFinal.Month, fechaFinal.Day, 0, 0, 0).AddDays(1);

                IEnumerable<Ventas> r = _repositorio.Query(vi => vi.FechaHora >= inicio && vi.FechaHora < final && vi.Cliente == nombreCliente);
                Error = "";
                return r;
            }
            catch (Exception ex)
            {

                Error = ex.Message;
                return null;
            }
        }

        IEnumerable<Ventas> IVentasManager.VentasEnIntervalo(DateTime fechaInicial, DateTime fechaFinal)
        {
            try
            {
                DateTime inicio = new DateTime(fechaInicial.Year, fechaInicial.Month, fechaInicial.Day, 0, 0, 0);
                DateTime final = new DateTime(fechaFinal.Year, fechaFinal.Month, fechaFinal.Day, 0, 0, 0).AddDays(1);

                IEnumerable<Ventas> r = _repositorio.Query(vi => vi.FechaHora >= inicio && vi.FechaHora < final);
                Error = "";
                return r;
            }
            catch (Exception ex)
            {

                Error = ex.Message;
                return null;
            }
        }
    }
}
