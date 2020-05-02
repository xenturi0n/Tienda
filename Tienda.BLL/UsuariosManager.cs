using System;
using System.Collections.Generic;
using System.Text;
using Tienda.Common.Entidades;
using Tienda.Common.Interfaces;
using System.Linq;


namespace Tienda.BLL
{
    //public class ProductosManager : GenericManager<Productos>, IProductosManager
    public class UsuariosManager: GenericManager<Usuarios>, IUsuariosManager
    {
        public UsuariosManager(IGenericRepository<Usuarios> repositorio) : base(repositorio)
        {
        }

        public Usuarios Login(string nombreDeUsuario, string password)
        {
            try
            {
                Usuarios r = _repositorio.Query(u => u.NombreDeUsuario == nombreDeUsuario && u.Password == password).SingleOrDefault();
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
