using System;
using System.Collections.Generic;
using System.Text;
using Tienda.Common.Entidades;

namespace Tienda.Common.Interfaces
{
    public interface IGenericManager<T> where T:BaseDTO
    {
        string Error { get; }

        bool Insertar(T entidad);

        IEnumerable<T> ObtenerTodos { get; }

        bool Actualizar(T entidad);

        bool Eliminar(T entidad);

        T BuscarPorId(string id);

    }
}
