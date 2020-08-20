using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TiendaServicios.Api.Libro.Aplicacion;

namespace TiendaServicios.Api.Libro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibroController : ControllerBase
    {
        private readonly IMediator _mediator;
        public LibroController(IMediator mediator)
        {
            _mediator = mediator;

        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecuta data)
        {
            return await _mediator.Send(data);
        }

        [HttpGet]
        public async Task<ActionResult<List<LibroDto>>> GetLibros()
        {

            return await _mediator.Send(new Consulta.ListLibros());

        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<LibroDto>> GetById(string Id)
        {
            return await _mediator.Send(new ConsultaFiltro.LibroUnico
            {

                LibroId = Guid.Parse(Id)

            });
        }

    }
}
