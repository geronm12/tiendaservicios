using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Libro.Modelo;
using TiendaServicios.Api.Libro.Persistencia;
using TiendaServicios.RabbitMQ.Bus.BusRabbit;
using TiendaServicios.RabbitMQ.Bus.EventoQueue;

namespace TiendaServicios.Api.Libro.Aplicacion
{
    public class Nuevo { 

        public class Ejecuta: IRequest
        {
            public string Titulo { get; set; }
             
            public DateTime? FechaPublicacion { get; set; }

            public Guid? AutorLibro { get; set; }

        }

        public class EjecutaValidacion: AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.Titulo).NotEmpty().NotNull();

                RuleFor(x => x.FechaPublicacion).NotEmpty().NotNull();

                RuleFor(x => x.AutorLibro).NotEmpty();

            }
        }


        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly ContextoLibreria _context;
            private readonly IRabbitEventBus _eventBus;
            public Manejador(ContextoLibreria context, IRabbitEventBus eventBus)
            {
                _context = context;

                _eventBus = eventBus;

            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {

                var libro = new LibreriaMaterial
                {
                    Titulo = request.Titulo,
                    FechaPublicacion = request.FechaPublicacion,
                    AutorLibro = request.AutorLibro,
             
                };

                _context.LibreriaMaterial.Add(libro);

                var res = await _context.SaveChangesAsync();

                _eventBus.Publish(new EmailEventoQueue("gerolpz01@gmail.com", request.Titulo, "Contenido de ejemplo"));


                if (res > 0) {

                    return Unit.Value;
                }

             
                throw new Exception("Ocurrió un error al agregar el registro");

            }
        }

    }
}
