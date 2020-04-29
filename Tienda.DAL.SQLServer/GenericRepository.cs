using System;
using System.Collections.Generic;
using System.Text;
using Tienda.Common.Interfaces;
using Tienda.Common.Entidades;
using System.Linq.Expressions;
using System.Linq;
using FluentValidation;
using System.Data.SqlClient;
using System.Reflection;
using FluentValidation.Results;

namespace Tienda.DAL.SQLServer
{
    //TODO: implementar metodo para obtener el ultimo T insertado
    //TODO: implementar metodo para obtener la cantidad total de registros en la tabla T de la base de datos
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseDTO
    {
        private DBSQLServer _db;

        private bool _idEsAutoNumerico;

        private string _columnaFechaHoraInsercion;

        private AbstractValidator<T> _validator;

        /// <summary>
        /// Constructor qu inicializa la clase generic repositorio
        /// </summary>
        /// <param name="validator">es necesario nuget FluentValidation</param>
        /// <param name="idEsAutoNumerico">por defecto es true</param>
        /// <param name="columnaFechaHoraInsercion">Nombre de la columna en donde se genera automaticamente la fecha y hora de la insercion del registro</param>
        public GenericRepository(AbstractValidator<T> validator, string columnaFechaHoraInsercion, bool idEsAutoNumerico = true)
        {
            this._validator = validator;
            this._idEsAutoNumerico = idEsAutoNumerico;
            this._columnaFechaHoraInsercion = columnaFechaHoraInsercion;
            this._db = new DBSQLServer();
        }

        public string Error { get; private set; }

        public IEnumerable<T> Read
        {
            get
            {
                try
                {
                    string sql = $"SELECT * FROM {typeof(T).Name}";
                    SqlDataReader r = (SqlDataReader)_db.Consulta(sql);
                    List<T> datos = new List<T>();

                    var campos = typeof(T).GetProperties();
                    T dato;
                    Type Ttypo = typeof(T);

                    while (r.Read())
                    {
                        dato = (T)(Activator.CreateInstance(typeof(T)));

                        for (int i = 0; i < campos.Length; i++)
                        {
                            PropertyInfo prop = Ttypo.GetProperty(campos[i].Name);
                            prop.SetValue(dato, r[i]);
                        }

                        datos.Add(dato);
                    }
                    r.Close();
                    Error = "";
                    return datos;
                }
                catch (Exception ex)
                {
                    Error = ex.Message;
                    return null;
                }

            }
        }

        public bool Create(T entidad)
        {

            ValidationResult resultadoDeLaValidacion = _validator.Validate(entidad);

            if (resultadoDeLaValidacion.IsValid)
            {
                string sql1 = $"INSERT INTO {typeof(T).Name} (";
                string sql2 = $") VALUES (";

                var campos = typeof(T).GetProperties();
                T dato = (T)(Activator.CreateInstance(typeof(T)));

                Type Ttypo = typeof(T);

                for (int i = 0; i < campos.Length; i++)
                {
                    if ((_idEsAutoNumerico && i == 0) || campos[i].Name == _columnaFechaHoraInsercion)
                    {
                        continue;
                    }

                    sql1 += $" {campos[i].Name}";
                    var propiedad = Ttypo.GetProperty(campos[i].Name);
                    var valor = propiedad.GetValue(entidad);

                    switch (propiedad.PropertyType.Name)
                    {
                        case "String":
                            sql2 += $"'{valor}'";
                            break;
                        case "DateTime":
                            DateTime v = (DateTime)valor;
                            sql2 += $"'{v.Year}-{v.Month}-{v.Day} {v.Hour}:{v.Minute}:{v.Second}'";
                            break;
                        default:
                            sql2 += $" {valor}";
                            break;
                    }

                    if (i != campos.Length - 2)
                    {
                        sql1 += " ,";
                        sql2 += " ,";
                    }
                }

                return EjecutaComando($"{sql1}{sql2});");
            }
            else
            {
                Error = "Error de validacion:";
                foreach (var item in resultadoDeLaValidacion.Errors)
                {
                    Error += $"{item.ErrorMessage}. ";
                }

                return false;
            }
        }

        public bool Delete(string id)
        {
            try
            {
                var campos = typeof(T).GetProperties();
                Type Ttypo = typeof(T);
                string sql = $"DELETE FROM {typeof(T).Name} WHERE {campos[0].Name} = ";

                switch (Ttypo.GetProperty(campos[0].Name).PropertyType.Name)
                {
                    case "String":
                        sql += $"'{id}'";
                        break;
                    default:
                        sql += $"{id}";
                        break;
                }

                if (_db.Comando(sql + ";"))
                {
                    Error = "";
                    return true;
                }
                else
                {
                    Error = _db.Error;
                    return false;
                }

            }
            catch (Exception ex)
            {

                Error = ex.Message;
                return false;
            }
        }

        public bool EjecutaComando(string sql)
        {
            if (_db.Comando(sql))
            {
                Error = "";
                return true;
            }
            else
            {
                Error = _db.Error + "========> " + sql;
                return false;
            }
        }

        public IEnumerable<T> Query(Expression<Func<T, bool>> predicado)
        {
            return Read.Where(predicado.Compile());
        }

        public T SearchById(string id)
        {
            try
            {
                var campos = typeof(T).GetProperties();
                Type Ttypo = typeof(T);
                string sql = $"SELECT * FROM {typeof(T).Name} WHERE {campos[0].Name} = ";

                switch (Ttypo.GetProperty(campos[0].Name).PropertyType.Name)
                {
                    case "String":
                        sql += $"'{id}'";
                        break;
                    default:
                        sql += $"{id}";
                        break;
                }

                SqlDataReader r = (SqlDataReader)(_db.Consulta(sql));
                T dato = (T)Activator.CreateInstance(typeof(T));
                int j = 0;

                while (r.Read())
                {
                    dato = (T)Activator.CreateInstance(typeof(T));
                    for (int i = 0; i < campos.Length; i++)
                    {
                        PropertyInfo prop = Ttypo.GetProperty(campos[i].Name);
                        prop.SetValue(dato, r[i]);
                    }
                    j++;
                }
                r.Close();

                if (j > 0)
                {
                    Error = "";
                    return dato;
                }
                else
                {
                    Error = $"Id no existe en la tabla {typeof(T).Name}";
                    return null;
                }
            }
            catch (Exception ex)
            {

                Error = ex.Message;
                return null;
            }
        }

        public bool Update(T entidad)
        {
            try
            {
                string sql1 = $"UPDATE {typeof(T).Name} SET ";
                string sql2 = " WHERE ";
                string sql = "";

                var campos = typeof(T).GetProperties();
                T dato = (T)(Activator.CreateInstance(typeof(T)));
                Type Ttypo = typeof(T);

                for (int i = 0; i < campos.Length; i++)
                {
                    var propiedad = Ttypo.GetProperty(campos[i].Name);
                    var valor = propiedad.GetValue(entidad);

                    if ((i != 0 || _idEsAutoNumerico == false) && campos[i].Name != _columnaFechaHoraInsercion)
                    {
                        sql1 += $"{propiedad.Name} = ";
                    }


                    switch (propiedad.PropertyType.Name)
                    {
                        case "String":
                            sql += $"'{valor}'";
                            break;
                        case "DateTime":
                            DateTime v = (DateTime)valor;
                            sql += $"'{v.Year}-{v.Month}-{v.Day} {v.Hour}:{v.Minute}:{v.Second}'";
                            break;
                        default:
                            sql += $" {valor}";
                            break;
                    }

                    if (i == 0)
                    {
                        sql2 += $"{propiedad.Name} = {sql}";
                    }

                    if (i != campos.Length - 2)
                    {
                        sql += ", ";
                    }

                    if ((i != 0 || _idEsAutoNumerico == false) && campos[i].Name != _columnaFechaHoraInsercion)
                    {
                        sql1 += sql;
                    }

                    sql = "";
                }

                if (_db.Comando(sql1 + sql2))
                {
                    Error = "";
                    return true;
                }
                else
                {
                    //Borrar el extra que se le agrego al mensaje de error por motivos de depuracion
                    Error = _db.Error + $" ===> {sql1 + sql2}";
                    return false;
                }
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                return false;
            }
        }

        //T IGenericRepository<T>.Ultimo()
        //{
        //    throw new NotImplementedException();
        //}

        public T Last
        {
            //WITH x AS (SELECT IdProducto, Nombre, Precio, r = RANK() OVER(ORDER BY FechaHoraInsercion DESC)
            //FROM dbo.Productos)
            //SELECT IdProducto, Nombre, Precio FROM x WHERE r = 1;

            get
            {
                try
                {
                    string sql1 = $"WITH x AS (SELECT ";
                    string sql2 = $"r = RANK() OVER(ORDER BY {_columnaFechaHoraInsercion} DESC) FROM {typeof(T).Name}) SELECT ";
                    string sql3 = " FROM x WHERE r = 1";

                    var campos = typeof(T).GetProperties();
                    Type Ttypo = typeof(T);
                    T dato;

                    for (int i = 0; i < campos.Length; i++)
                    {
                        sql1 += $"{campos[i].Name}, ";

                        sql2 += $"{campos[i].Name}";

                        if (i != campos.Length - 1)
                        {
                            sql2 += ", ";
                        }
                    }

                    sql1 += sql2 + sql3;

                    SqlDataReader r = (SqlDataReader)_db.Consulta(sql1);

                    r.Read();

                    dato = (T)(Activator.CreateInstance(typeof(T)));

                    for (int i = 0; i < campos.Length; i++)
                    {
                        PropertyInfo prop = Ttypo.GetProperty(campos[i].Name);
                        prop.SetValue(dato, r[i]);
                    }
                    r.Close();
                    Error = "";
                    return dato;
                }
                catch (Exception ex)
                {
                    Error = ex.Message;
                    return null;
                }
            }
        }

        public int Count
        {
            get
            {
                try
                {
                    var campos = typeof(T).GetProperties();
                    string tabla = typeof(T).Name;

                    string sql = $"SELECT COUNT({campos[0].Name}) FROM {tabla}";

                    int resultado =(int)(_db.Scalar(sql));
                    Error = "";
                    return resultado;
                }
                catch (Exception ex)
                {
                    Error = ex.Message;
                    return -1;                    
                }
            }
        }
    }
}
