using System;
using System.Collections.Generic;
using System.Text;
using Tienda.Common.Entidades;
using Tienda.Common.Interfaces;

namespace Tienda.BLL
{
    public abstract class GenericManager<T> : IGenericManager<T> where T : BaseDTO
    {
        internal IGenericRepository<T> _repositorio;

        public GenericManager(IGenericRepository<T> repositorio)
        {
            this._repositorio = repositorio;
        }

        public string Error { get; internal set; }

        public IEnumerable<T> ObtenerTodos { 
            get 
            {
                try
                {
                    IEnumerable<T> r = _repositorio.Read;
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
        }

        public bool Actualizar(T entidad)
        {
            try
            {
                bool r = _repositorio.Update(entidad);
                Error = _repositorio.Error;
                return r;

            }
            catch (Exception ex)
            {

                this.Error = ex.Message;
                return false;
            }
        }

        public T BuscarPorId(string id)
        {            
            try
            {
                T r = _repositorio.SearchById(id);
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

        public bool Eliminar(string id)
        {
            try
            {
                bool r = _repositorio.Delete(id);
                this.Error = _repositorio.Error;
                return r;
            }
            catch (Exception ex)
            {
                this.Error = ex.Message;
                return false;
            }
        }

        public bool Insertar(T entidad)
        {
            try
            {
                bool r = _repositorio.Create(entidad);
                this.Error = _repositorio.Error;
                return r;
            }
            catch (Exception ex)
            {
                this.Error = ex.Message;
                return false;
            }
        }
    }
}
