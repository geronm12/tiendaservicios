using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Carrito.Aplicacion;
using TiendaServicios.Api.Carrito.Persistencia;
using TiendaServicios.Api.Carrito.RemoteInterface;
using TiendaServicios.Api.Carrito.RemoteModel;

namespace TiendaServicios.Api.Carrito.Tests.MockService
{
    public class LibroRemoteMockService : IRequestHandler<Consulta.Ejecuta, CarritoDto>
    {
        private readonly CarritoContexto _context;

        public LibroRemoteMockService(CarritoContexto context)
        {
            _context = context;
        }

        public async Task<CarritoDto> Handle(Consulta.Ejecuta request, CancellationToken cancellationToken)
        {
            var carrito = await _context.CarritoSesion.Where(x => x.CarritoSesionId == request.CarritoSesionId).FirstOrDefaultAsync();

            if (carrito == null)
                throw new ArgumentNullException();


            var carritoDto = new CarritoDto
            {
                CarritoId = request.CarritoSesionId,
                FechaCreacionSeion = carrito.FechaCreacion,
                ListaProductos = null

            };

            return carritoDto;
        
        }
    }
}
