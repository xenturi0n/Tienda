using System;
using System.Collections.Generic;
using System.Text;
using Tienda.Common.Interfaces;
using Tienda.Common.Entidades;
using System.Linq;

namespace Tienda.BLL
{
    public class ProductosManager : GenericManager<Productos>, IProductosManager
    {
        public ProductosManager(IGenericRepository<Productos> repositorio) : base(repositorio)
        {
        }

        public Productos BuscarProductoPorNombreExacto(string nombre)
        {
            try
            {
                Productos r = _repositorio.Query(p => p.Nombre == nombre).SingleOrDefault();
                this.Error = _repositorio.Error;
                
                if(this.Error != "")
                {
                    return null;
                }
                else
                {
                    return r;
                }
            }
            catch (Exception ex)
            {
                this.Error = ex.Message;
                return null;
            }
        }

        public IEnumerable<Productos> BuscarProductosPorNombre(string criterio)
        {
            throw new NotImplementedException();
        }
    }
}
