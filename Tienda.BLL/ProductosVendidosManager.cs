using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Tienda.Common.Entidades;
using Tienda.Common.Interfaces;

namespace Tienda.BLL
{
    public class ProductosVendidosManager : GenericManager<ProductosVendidos>, IProductosVendidosManager
    {
        public ProductosVendidosManager(IGenericRepository<ProductosVendidos> repositorio) : base(repositorio)
        {
        }

        public IEnumerable<ProductosVendidos> ProductosDeUnaVenta(int idVenta)
        {
            try
            {
                IEnumerable<ProductosVendidos> r = _repositorio.Query(pv => pv.IdVenta == idVenta);
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
