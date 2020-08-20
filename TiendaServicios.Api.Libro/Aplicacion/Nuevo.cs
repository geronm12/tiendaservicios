using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Libro.Modelo;
using TiendaServicios.Api.Libro.Persistencia;

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
            public Manejador(ContextoLibreria context)
            {
                _context = context;
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

                if (res > 0) {

                    return Unit.Value;
                   
                }


                throw new Exception("Ocurrió un error al agregar el registro");

            }
        }

    }
}
