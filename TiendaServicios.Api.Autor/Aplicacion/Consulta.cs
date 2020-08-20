using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Autor.Modelo;
using TiendaServicios.Api.Autor.Persistencia;

namespace TiendaServicios.Api.Autor.Aplicacion
{
    public class Consulta
    {
        public class ListAutor: IRequest<List<AutorDto>>
        {

        }

        public class Manejador : IRequestHandler<ListAutor, List<AutorDto>>
        {
            private readonly ContextoAutor _context;

            private readonly IMapper _mapper;
            public Manejador(ContextoAutor context, IMapper mapper)
            {

                _context = context;

                _mapper = mapper;

            }

            public async Task<List<AutorDto>> Handle(ListAutor request, CancellationToken cancellationToken)
            {
                var _listaAutores = await _context.AutorLibro.ToListAsync();

                var autoresDto = _mapper.Map<List<AutorLibro>, List<AutorDto>>(_listaAutores);

                return autoresDto;
            }

        }
    }
}
