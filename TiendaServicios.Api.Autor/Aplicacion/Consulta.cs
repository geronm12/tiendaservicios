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
        public class ListAutor: IRequest<List<AutorLibro>>
        {

        }

        public class Manejador : IRequestHandler<ListAutor, List<AutorLibro>>
        {
            private readonly ContextoAutor _context;
            public Manejador(ContextoAutor context)
            {
                _context = context;
            }

            public async Task<List<AutorLibro>> Handle(ListAutor request, CancellationToken cancellationToken)
            {
                var _listaAutores = await _context.AutorLibro.ToListAsync();

                return _listaAutores;
            }

        }
    }
}
