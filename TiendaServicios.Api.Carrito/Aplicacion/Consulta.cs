using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Carrito.Persistencia;
using TiendaServicios.Api.Carrito.RemoteInterface;

namespace TiendaServicios.Api.Carrito.Aplicacion
{
    public class Consulta
    {
        public class Ejecuta: IRequest<CarritoDto>
        {
            public int CarritoSesionId { get; set; }

        }

        public class Manejador : IRequestHandler<Ejecuta, CarritoDto>
        {
            private readonly CarritoContexto _context;

            private readonly ILibrosService _libroService;

            public Manejador(CarritoContexto context)
            {
                _context = context;
            }


            public Manejador(CarritoContexto context, 
                 ILibrosService service)
            {
                _context = context;

                _libroService = service;

            }
            public async Task<CarritoDto> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var carritoSesion = await _context.CarritoSesion.FirstOrDefaultAsync(x => x.CarritoSesionId == request.CarritoSesionId);

                var carritoSesionDetalle = await _context.CarritoSesionDetalle.Where(x => x.CarritoSesionId == request.CarritoSesionId).ToListAsync();

                var listaCarritoDto = new List<CarritoDetalleDto>();

                foreach(var libro in carritoSesionDetalle)
                {
                    var response =  await _libroService.GetLibro(Guid.Parse(libro.ProductoSeleccionado));
                
                    if(response.resultado)
                    {
                        var objetoLibro = response.Libro;
                        var carritoDetalle = new CarritoDetalleDto
                        {
                            LibroId = objetoLibro.LibreriaMaterialId,
                            TituloLibro = objetoLibro.Titulo,
                            FechaPublicacion = objetoLibro.FechaPublicacion
                        };

                        listaCarritoDto.Add(carritoDetalle);

                    }
                }


                var carritoSesionDto = new CarritoDto
                {
                    
                    CarritoId = carritoSesion.CarritoSesionId,
                    FechaCreacionSeion = carritoSesion.FechaCreacion,
                    ListaProductos = listaCarritoDto
                
                };

                return carritoSesionDto;
            }
        }
    }
}
