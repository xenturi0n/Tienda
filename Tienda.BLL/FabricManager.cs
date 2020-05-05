using System;
using System.Collections.Generic;
using System.Text;
using Tienda.Common.Entidades;
using Tienda.Common.Interfaces;
using Tienda.Common.Validadores;

namespace Tienda.BLL
{
    public static class FabricManager
    {
        public enum OrigenesDeDatos
        {
            MySQL=1,
            SQLServer=2
        }

        public static IUsuariosManager UsuariosManager(FabricManager.OrigenesDeDatos origen)
        {
            switch (origen)
            {
                case OrigenesDeDatos.MySQL:
                    return null;
                case OrigenesDeDatos.SQLServer:
                    return new UsuariosManager(new DAL.SQLServer.GenericRepository<Usuarios>(new UsuariosValidator(), "FechaHoraInsercion", false));
                default:
                    return null;
            }
        }

        public static IProductosManager productosManager(FabricManager.OrigenesDeDatos origen)
        {
            switch (origen)
            {
                case OrigenesDeDatos.MySQL:
                    return null;
                case OrigenesDeDatos.SQLServer:
                    return new ProductosManager(new DAL.SQLServer.GenericRepository<Productos>(new ProductosValidator(), "FechaHoraInsercion", true));
                default:
                    return null;
            }
        }

        public static IVentasManager Ventasmanager(FabricManager.OrigenesDeDatos origen)
        {
            switch (origen)
            {
                case OrigenesDeDatos.MySQL:
                    return null;
                case OrigenesDeDatos.SQLServer:
                    return new VentasManager(new DAL.SQLServer.GenericRepository<Ventas>(new VentasValidator(), "FechaHoraInsercion", true));
                default:
                    return null;
            }
        }

        public static IProductosVendidosManager ProductosvenidosManager(FabricManager.OrigenesDeDatos origen)
        {
            switch (origen)
            {
                case OrigenesDeDatos.MySQL:
                    return null;
                case OrigenesDeDatos.SQLServer:
                    return new ProductosVendidosManager(new DAL.SQLServer.GenericRepository<ProductosVendidos>(new ProductosVendidosValidator(), "FechaHoraInsercion", true));
                default:
                    return null;
            }
        }
    }
}
