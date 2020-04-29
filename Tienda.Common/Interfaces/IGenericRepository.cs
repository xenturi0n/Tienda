using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Tienda.Common.Entidades;

namespace Tienda.Common.Interfaces
{
    /// <summary>
    /// Proporciona los metodos basicos de acceso a una base de datos, Create(Insert), Read(Select), Update(Update), Delete(Delete)
    /// </summary>
    /// <typeparam name="T">Tipo de entidad (clase) a la que se refiere una tabla</typeparam>
    public interface IGenericRepository<T> where T:BaseDTO
    {
        /// <summary>
        /// Proporciona Informacion sobre el error ocurrido en alguna de las operaciones
        /// </summary>
        string Error { get; }

        bool Create(T entidad );

        IEnumerable<T> Read { get; }

        bool Update(T entidad);

        bool Delete(string id);

        bool EjecutaComando(string sql);

        IEnumerable<T> Query(Expression<Func<T, bool>> predicado);

        T SearchById(string id);

        T Last { get;}

        int Count { get; }

    }
}
