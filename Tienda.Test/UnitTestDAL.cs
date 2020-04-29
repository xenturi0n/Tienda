using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tienda.Common;
using Tienda.Common.Entidades;
using Tienda.Common.Interfaces;
using Tienda.Common.Validadores;
using Tienda.DAL.SQLServer;
using System.Linq;

namespace Tienda.Test
{
    //TODO: capa biz
    [TestClass]
    public class UnitTestDAL
    {
        IGenericRepository<Productos> RepositorioProductos;
        IGenericRepository<Usuarios> RepositorioUsuarios;
        IGenericRepository<Ventas> RepositorioVentas;
        IGenericRepository<ProductosVendidos> RepositorioProductosVendidos;

        Random r;

        public UnitTestDAL()
        {
            RepositorioProductos = new GenericRepository<Productos>(new ProductosValidator(), "FechaHoraInsercion", true);
            RepositorioUsuarios = new GenericRepository<Usuarios>(new UsuariosValidator(), "FechaHoraInsercion", false);
            RepositorioVentas = new GenericRepository<Ventas>(new VentasValidator(), "FechaHoraInsercion", true);
            RepositorioProductosVendidos = new GenericRepository<ProductosVendidos>(new ProductosVendidosValidator(), "FechaHoraInsercion", true);
            

            r = new Random();
        }

        [TestMethod]
        public void TestMethodProductos()
        {
            //Creamos un producto de prueba
            Productos p = CrearProductoDePrueba();

            //Obtenemos la cantidad de productos existentes actualmente en la base de datos
            int cantidadProductos = RepositorioProductos.Count;

            //Insertamos un producto a la base de datos y checamos que la insercion se halla producido correctamente
            Assert.IsTrue(RepositorioProductos.Create(p), RepositorioProductos.Error);

            //Verificamos que efectivamente exista un producto mas en la base de datos
            Assert.AreEqual(cantidadProductos + 1, RepositorioProductos.Count, "No se inserto el registro");

            //Obtenemos el Id mas reciente (el del ultimo producto insertado)
            int ultimoId = RepositorioProductos.Last.IdProducto;            

            //Obtenemos el ultimo producto insertado de la base de datos
            Productos productoModificado = RepositorioProductos.SearchById(ultimoId.ToString());

            //modificamos el producto
            productoModificado.Nombre = "MODIFICADO";

            //actualizamos en la base de datos y verificamos que la actualizacion se produzca correctamente
            Assert.IsTrue(RepositorioProductos.Update(productoModificado), RepositorioProductos.Error);

            //Obtenemos el ultimo producto modificado en la base de datos
            Productos productoModificado2 = RepositorioProductos.SearchById(ultimoId.ToString());

            //Verificamos que la propiedad nombre del producto coincida con la modificacion que hicimos
            Assert.AreEqual("MODIFICADO", productoModificado2.Nombre, "No se actualizo el producto correctamente");

            //Eliminamos el producto que acabamos de insertar y modificar
            Assert.IsTrue(RepositorioProductos.Delete(ultimoId.ToString()));

            //Verificamos que la cantidad de productos quede igual que al inicio, comprobando que no se afecto el numero de registro totales
            //ya que eliminamos el el mismo producto que se habia insertado anteriormente
            Assert.IsTrue(cantidadProductos == RepositorioProductos.Count, "La cantidad de productos no coincide");

        }

        [TestMethod]
        public void TestMethodUsuarios()
        {
            Usuarios u = CrearUsuariosDePrueba();

            int cantidadUsuarios = RepositorioUsuarios.Count;

            Assert.IsTrue(RepositorioUsuarios.Create(u), RepositorioUsuarios.Error);

            Assert.AreEqual(cantidadUsuarios + 1, RepositorioUsuarios.Count, "No se inserto el registro");

            Usuarios ultimoUsuarioParaModificar = RepositorioUsuarios.Last;

            ultimoUsuarioParaModificar.Nombres = "MODIFICADO";

            Assert.IsTrue(RepositorioUsuarios.Update(ultimoUsuarioParaModificar), RepositorioProductos.Error);

            Usuarios ultimoUsuarioParaModificar2 = RepositorioUsuarios.Last;

            Assert.AreEqual("MODIFICADO", ultimoUsuarioParaModificar2.Nombres, "El campo nombres no fue modificado");

            Assert.IsTrue(RepositorioUsuarios.Delete(ultimoUsuarioParaModificar.NombreDeUsuario), RepositorioUsuarios.Error);

            Assert.AreEqual(cantidadUsuarios, RepositorioUsuarios.Count, "La cantidad de usuarios es diferente, algo salio mal");

        }

        [TestMethod]
        public void TestMethodVentas()
        {
            Usuarios u = CrearUsuariosDePrueba();

            int cantidadVentas = RepositorioVentas.Count;
            int cantidadUsuarios = RepositorioUsuarios.Count;

            Assert.IsTrue(RepositorioUsuarios.Create(u), RepositorioUsuarios.Error);

            for (int i = 1; i <= 10; i++)
            {
                string usuario = RepositorioUsuarios.Last.NombreDeUsuario;
                Ventas v = CrearVentasDePrueba(usuario);

                Assert.IsTrue(RepositorioVentas.Create(v), RepositorioVentas.Error);

                Assert.AreEqual(cantidadVentas + i, RepositorioVentas.Count, "No se inserto el registro");
            }

            

            Ventas ventaModificar = RepositorioVentas.Last;

            ventaModificar.Cliente = "Pito Perez";

            Assert.IsTrue(RepositorioVentas.Update(ventaModificar), RepositorioVentas.Error);

            Ventas ventaModificada = RepositorioVentas.Last;

            Assert.IsTrue("Pito Perez" == ventaModificada.Cliente);

            for (int i = 0; i < 10; i++)
            {
                string ventaEliminar = RepositorioVentas.Last.IdVenta.ToString();
                Assert.IsTrue(RepositorioVentas.Delete(ventaEliminar), RepositorioVentas.Error);
            }            

            Assert.AreEqual(cantidadVentas, RepositorioVentas.Count, "Hay diferente cantidad de registros, algo salio mal");

            Assert.IsTrue(RepositorioUsuarios.Delete(u.NombreDeUsuario), RepositorioUsuarios.Error);

            Assert.AreEqual(cantidadUsuarios, RepositorioUsuarios.Count, "Hay diferente cantidad de usuarios");
        }

        [TestMethod]
        public void TestMethodProductosVendidos()
        {
            int cantidadVentas = RepositorioVentas.Count;
            int cantidadProductos = RepositorioProductos.Count;
            int cantidadUsuarios = RepositorioUsuarios.Count;
            int cantidadProductosVendidos = RepositorioProductosVendidos.Count;

            Assert.IsTrue(RepositorioUsuarios.Create(CrearUsuariosDePrueba()), RepositorioUsuarios.Error);
            Assert.IsTrue(cantidadUsuarios + 1 == RepositorioUsuarios.Count, "No se inserto correctamente el usuario vendedor");
            Assert.IsTrue(RepositorioUsuarios.Read.Count() == RepositorioUsuarios.Count, "No coincide la cantidad de usuarios");

            Assert.IsTrue(RepositorioVentas.Create(CrearVentasDePrueba(RepositorioUsuarios.Last.NombreDeUsuario)), RepositorioVentas.Error);
            Assert.IsTrue(cantidadVentas + 1 == RepositorioVentas.Count, "No se inserto correctamente la venta");
            Assert.IsTrue(RepositorioVentas.Read.Count() == RepositorioVentas.Count, "No coincide la cantidad de ventas");

            Ventas v = RepositorioVentas.Last;

            for (int i = 1; i <= 10; i++)
            {
                Productos p = CrearProductoDePrueba();

                Assert.IsTrue(RepositorioProductos.Create(p), RepositorioProductos.Error);
                Assert.IsTrue(cantidadProductos + i == RepositorioProductos.Count, "No se inserto correctamente el producto");
                Assert.IsTrue(RepositorioProductos.Read.Count() == RepositorioProductos.Count, "No coincide la cantidad de productos");

                ProductosVendidos pv = CrearProductosVendidos(v.IdVenta, RepositorioProductos.Last.IdProducto);

                Assert.IsTrue(RepositorioProductosVendidos.Create(pv), RepositorioProductosVendidos.Error);
                Assert.IsTrue(cantidadProductosVendidos + i == RepositorioProductosVendidos.Count, "No se inserto correctamente el producto vendido");
                Assert.IsTrue(RepositorioProductosVendidos.Read.Count() == RepositorioProductosVendidos.Count, "No coincide la cantidad de productos vendidos");

                ProductosVendidos productoOriginal = RepositorioProductosVendidos.Last;

                productoOriginal.PrecioDeVenta = (decimal)7777.77;

                Assert.IsTrue(RepositorioProductosVendidos.Update(productoOriginal), RepositorioProductosVendidos.Error);
                Assert.IsTrue(cantidadProductosVendidos + i == RepositorioProductosVendidos.Count, "No se inserto correctamente el producto vendido");
                Assert.IsTrue(RepositorioProductosVendidos.Read.Count() == RepositorioProductosVendidos.Count, "No coincide la cantidad de productos vendidos");
                Assert.IsTrue((decimal)7777.77 == RepositorioProductosVendidos.Last.PrecioDeVenta, "No coincide el precio");
            }

            for (int i = 9; i >= 0; i--)
            {
                string idProductoVendido = RepositorioProductosVendidos.Last.IdProductoVendido.ToString();

                Assert.IsTrue(RepositorioProductosVendidos.Delete(idProductoVendido), RepositorioProductosVendidos.Error);
                Assert.IsTrue(cantidadProductosVendidos + i == RepositorioProductosVendidos.Count, "No se elimino correctamente el producto vendido");
                Assert.IsTrue(RepositorioProductosVendidos.Read.Count() == RepositorioProductosVendidos.Count, "No coincide la cantidad de productos vendidos");
            }

            for (int i = 9; i >= 0; i--)
            {
                string idProducto = RepositorioProductos.Last.IdProducto.ToString();

                Assert.IsTrue(RepositorioProductos.Delete(idProducto), RepositorioProductos.Error);
                Assert.IsTrue(cantidadProductos + i == RepositorioProductos.Count, "No se elimino correctamente el producto");
                Assert.IsTrue(RepositorioProductos.Read.Count() == RepositorioProductos.Count, "No coincide la cantidad de productos");
            }


            Assert.IsTrue(RepositorioVentas.Delete(RepositorioVentas.Last.IdVenta.ToString()), RepositorioVentas.Error);
            Assert.IsTrue(cantidadVentas == RepositorioVentas.Count, "No se elimino correctamente la venta");
            Assert.IsTrue(RepositorioVentas.Read.Count() == RepositorioVentas.Count, "No coincide la cantidad de ventas");

            Assert.IsTrue(RepositorioUsuarios.Delete(RepositorioUsuarios.Last.NombreDeUsuario), RepositorioUsuarios.Error);
            Assert.IsTrue(cantidadUsuarios == RepositorioUsuarios.Count, "No se elimino el usuario correctamente");
            Assert.IsTrue(RepositorioUsuarios.Read.Count() == RepositorioUsuarios.Count, "No coincide la cantidad de usuarios");
        }

        private Productos CrearProductoDePrueba()
        {
            decimal decimales = r.Next(1, 99) / 100;

            return new Productos()
            {
                Nombre = $"Producto de prueba, {r.Next(3, 100)}",
                Precio = r.Next(5, 100) + decimales
            };
        }

        private Usuarios CrearUsuariosDePrueba()
        {
            return new Usuarios()
            {
                Apellidos = "Perez Lopez",
                Nombres = "Andres Manuel",
                NombreDeUsuario = "Lopitoz5",
                Password = "123456"
            };
        }

        private Ventas CrearVentasDePrueba(string vendedor)
        {
            return new Ventas()
            {
                Cliente = "Casimiro Rocha",
                FechaHora = DateTime.Now,
                Vendedor = vendedor               
            };
        }

        private ProductosVendidos CrearProductosVendidos(int idVenta, int idProducto)
        {
            decimal decimales = r.Next(1, 99) / 100;

            return new ProductosVendidos()
            {
                IdVenta = idVenta,
                PrecioDeVenta = (decimal)(r.Next(1, 100) + decimales),
                Cantidad = r.Next(1, 20),
                IdProducto = idProducto
            };
        }
    }
}
