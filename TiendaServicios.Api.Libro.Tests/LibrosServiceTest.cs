
using AutoMapper;
using Castle.Components.DictionaryAdapter;
using GenFu;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TiendaServicios.Api.Libro.Aplicacion;
using TiendaServicios.Api.Libro.Modelo;
using TiendaServicios.Api.Libro.Persistencia;
using Xunit;

namespace TiendaServicios.Api.Libro.Tests
{
    public class LibrosServiceTest
    {

        private IEnumerable<LibreriaMaterial> ObtenerDataPrueba()
        {
            A.Configure<LibreriaMaterial>()
                .Fill(x => x.Titulo).AsArticleTitle()
                .Fill(x => x.LibreriaMaterialId, () =>
                {
                    return Guid.NewGuid();
                });

            var list = A.ListOf<LibreriaMaterial>(30);

            list[0].LibreriaMaterialId = Guid.Empty;

            return list;
        
        }


        private Mock<ContextoLibreria> CrearContexto()
        {
            var data = ObtenerDataPrueba().AsQueryable();

            var dbSet = new Mock<DbSet<LibreriaMaterial>>();
            
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.Provider).Returns(data.Provider);
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.Expression).Returns(data.Expression);
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.ElementType).Returns(data.ElementType);
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.GetEnumerator()).Returns(data.GetEnumerator());

            dbSet.As<IAsyncEnumerable<LibreriaMaterial>>().Setup(x => x.GetAsyncEnumerator(new System.Threading.CancellationToken()))
                .Returns(new AsyncEnumerator<LibreriaMaterial>(data.GetEnumerator()));

            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.Provider).Returns(new AsyncQueryProvider<LibreriaMaterial>(data.Provider));

            var context = new Mock<ContextoLibreria>();
            context.Setup(x => x.LibreriaMaterial).Returns(dbSet.Object);

            return context;


        }

        [Fact]
        public async void GetLibroPorId()
        {
            var mock = CrearContexto();

            var mapConfig = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));

            var mapper = mapConfig.CreateMapper();

            var request = new ConsultaFiltro.LibroUnico();
            request.LibroId = Guid.Empty;

            var manejador = new ConsultaFiltro.Manejador(mock.Object, mapper);

            var result = await manejador.Handle(request, new CancellationToken ());

            Assert.NotNull(result);
            Assert.True(result.LibreriaMaterialId == Guid.Empty);

        }


        [Fact]
        public async void GetLibros()
        { 

            var mockContexto = CrearContexto();

            var mapConfig = new MapperConfiguration(cfg => cfg.AddProfile(new MappingTest()));

            var mapper = mapConfig.CreateMapper();

            Consulta.Manejador manejador = new Consulta.Manejador (mapper, mockContexto.Object);

            Consulta.ListLibros request = new Consulta.ListLibros();

            var result = await manejador.Handle(request, new CancellationToken());

            Assert.True(result.Any());
            
        }
    
    
    
        [Fact]
        public async void GuardarLibro()
        {
            var options = new DbContextOptionsBuilder<ContextoLibreria>()
                .UseInMemoryDatabase(databaseName: "BaseDatosLibro")
                .Options;

            var context = new ContextoLibreria(options);

            var request = new Nuevo.Ejecuta
            {
                Titulo = "La naranja mecánica",
                AutorLibro = Guid.Empty,
                FechaPublicacion = DateTime.Now
            };

            var manejador = new Nuevo.Manejador(context);

            var result = await manejador.Handle(request, new CancellationToken());

            Assert.True(result != null);

        }
    
    
    }
}
