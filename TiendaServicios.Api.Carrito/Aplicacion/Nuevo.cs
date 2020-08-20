using Google.Protobuf.WellKnownTypes;
using MediatR;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Carrito.Modelo;
using TiendaServicios.Api.Carrito.Persistencia;

namespace TiendaServicios.Api.Carrito.Aplicacion
{
    public class Nuevo
    {

        public class Ejecuta: IRequest
        {
            public DateTime FechaCreacionSesion { get; set; }

            public List<string> ProductoLista { get; set; }
        
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly CarritoContexto _context;
            public Manejador(CarritoContexto context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var carritoSesion = new CarritoSesion
                {
                    FechaCreacion = request.FechaCreacionSesion,
                };

                _context.CarritoSesion.Add(carritoSesion);

                var resp = await _context.SaveChangesAsync();

                if(resp == 0)
                {
                    throw new Exception("Errores en la inserción");
                }

                int id = carritoSesion.CarritoSesionId;
                
                foreach(var obj in request.ProductoLista)
                {
                    var detalleSesion = new CarritoSesionDetalle
                    {
                        CarritoSesionId = id,
                        FechaCreacion = DateTime.Now,
                        ProductoSeleccionado = obj
                    };

                    _context.CarritoSesionDetalle.Add(detalleSesion);

                }

               var resultado = await _context.SaveChangesAsync();

                if(resultado > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo insertar el detalle del carrito de compras"); 
            }
        }

    }
}
