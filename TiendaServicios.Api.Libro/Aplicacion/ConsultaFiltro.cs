using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Libro.Persistencia;

namespace TiendaServicios.Api.Libro.Aplicacion
{
    public class ConsultaFiltro
    {

        public class LibroUnico: IRequest<LibroDto>
        {

            public Guid? LibroId { get; set; }

        }

        public class Manejador : IRequestHandler<LibroUnico, LibroDto>
        {
            private readonly ContextoLibreria _context;
            private readonly IMapper _mapper;

            public Manejador(ContextoLibreria context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;

            }

            public async Task<LibroDto> Handle(LibroUnico request, CancellationToken cancellationToken)
            {
                var libro = await _context.LibreriaMaterial.Where(x => x.LibreriaMaterialId == request.LibroId).FirstOrDefaultAsync();

                if (libro == null)
                    throw new Exception("No se encontró ningún libro con ese Id");

                return _mapper.Map<LibroDto>(libro);


            }
        }
    }
}
