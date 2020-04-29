using System;
using System.Collections.Generic;
using System.Text;
using Tienda.Common.Entidades;

namespace Tienda.Common.Interfaces
{
    public interface IUsuariosManager : IGenericManager<Usuarios>
    {
        Usuarios Login(string nombreDeUsuario, string password);

    }
}
