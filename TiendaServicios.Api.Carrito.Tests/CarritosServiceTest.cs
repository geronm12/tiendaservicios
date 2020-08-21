using AutoMapper;
using Castle.Core.Logging;
using GenFu;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using TiendaServicios.Api.Carrito.Aplicacion;
using TiendaServicios.Api.Carrito.Modelo;
using TiendaServicios.Api.Carrito.Persistencia;
using TiendaServicios.Api.Carrito.RemoteInterface;
using TiendaServicios.Api.Carrito.RemoteService;
using TiendaServicios.Api.Carrito.Tests.MockService;
using Xunit;
namespace TiendaServicios.Api.Carrito.Tests
{
    public class CarritosServiceTest
    {

        private IEnumerable<CarritoSesion> GetCarritos()
        {
            A.Configure<CarritoSesion>()
            .Fill(x => x.FechaCreacion, () =>
            {
                return DateTime.Now;
            }).Fill(x => x.CarritoSesionId, () =>
            {
                return new Random().Next(0, 300);
            }).Fill(x => x.Detalles, () =>
            {
                A.Configure<CarritoSesionDetalle>()
               .Fill(x => x.CarritoSesionDetalleId, () =>
               {
                   return new Random().Next(0, 300);
               }).Fill(x => x.FechaCreacion, () =>
               {
                   
                   return DateTime.Now;

               }).Fill(x => x.ProductoSeleccionado).AsArticleTitle()
               .Fill(x => x.CarritoSesionId, () =>
               {
                   return new Random().Next(0, 300);
               });

                return A.ListOf<CarritoSesionDetalle>(10);
                

            });


            var lista = A.ListOf<CarritoSesion>(30);

            lista[0].CarritoSesionId = 1;

            return lista;
        }


        public Mock<CarritoContexto> CrearContext()
        {
          
            var data = GetCarritos().AsQueryable();

            var dbSet = new Mock<DbSet<CarritoSesion>>();

            dbSet.As<IQueryable<CarritoSesion>>().Setup(x => x.Provider).Returns(data.Provider);
            dbSet.As<IQueryable<CarritoSesion>>().Setup(x => x.Expression).Returns(data.Expression);
            dbSet.As<IQueryable<CarritoSesion>>().Setup(x => x.ElementType).Returns(data.ElementType);
            dbSet.As<IQueryable<CarritoSesion>>().Setup(x => x.GetEnumerator()).Returns(data.GetEnumerator());

            dbSet.As<IAsyncEnumerable<CarritoSesion>>().Setup(x => x.GetAsyncEnumerator(new System.Threading.CancellationToken()))
                .Returns(new AsyncEnumerator<CarritoSesion>(data.GetEnumerator()));

            dbSet.As<IQueryable<CarritoSesion>>().Setup(x => x.Provider).Returns(new AsyncQueryProvider<CarritoSesion>(data.Provider));

            var context = new Mock<CarritoContexto>();
            context.Setup(x => x.CarritoSesion).Returns(dbSet.Object);

            return context;
        }

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

        [Fact]
        public async void ObtenerCarritoId()
        {
            var mock = CrearContext();

            var mockHttp = new Mock<IHttpClientFactory>();
 

            var request = new Consulta.Ejecuta();
            request.CarritoSesionId = 1;

            var manejador = new LibroRemoteMockService(mock.Object);

            var result = await manejador.Handle(request, new CancellationToken());

            Assert.True(result != null);

        }




    }
}
