using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using TiendaServicios.Api.Carrito.Aplicacion;
using TiendaServicios.Api.Carrito.Persistencia;
using Xunit;
namespace TiendaServicios.Api.Carrito.Tests
{
    public class CarritosServiceTest
    {

        [Fact]
        public async void CreateCarrito()
        {
            var options = new DbContextOptionsBuilder<CarritoContexto>().UseInMemoryDatabase(databaseName: "DbCarrito").Options;

            var context = new CarritoContexto(options);

            var request = new Nuevo.Ejecuta
            {
                FechaCreacionSesion = DateTime.Now,
                ProductoLista = new System.Collections.Generic.List<string>
                {
                    "Lunes",
                    "Martes",
                    "Miercoles",
                    "Jueves",
                    "Viernes"
                }
            };

            var manejador = new Nuevo.Manejador(context);

            var result = await manejador.Handle(request, new CancellationToken());

            Assert.True(result != null);

        }




    }
}
