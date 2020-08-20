using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Libro.Modelo;
using TiendaServicios.Api.Libro.Persistencia;

namespace TiendaServicios.Api.Libro.Aplicacion
{
    public class Consulta
    {

        public class ListLibros: IRequest<List<LibroDto>>
        { 

        }

        public class Manejador : IRequestHandler<ListLibros, List<LibroDto>>
        {
            private readonly IMapper _mapper;
            private readonly ContextoLibreria _context;

            public Manejador(IMapper mapper, ContextoLibreria context)
            {
                _context = context;

                _mapper = mapper;

            }


            public async Task<List<LibroDto>> Handle(ListLibros request, CancellationToken cancellationToken)
            {
                var _listaLibros = await _context.LibreriaMaterial.ToListAsync();


                if (_listaLibros == null)
                    throw new Exception("No se encontró ningún libro en la base de datos");


                var _listaDto = _mapper.Map<List<LibreriaMaterial>, List<LibroDto>>(_listaLibros);

                return _listaDto;
                

            }
        }

    }
}
