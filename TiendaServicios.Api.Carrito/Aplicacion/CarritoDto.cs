using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TiendaServicios.Api.Carrito.Aplicacion
{
    public class CarritoDto
    {
        public int CarritoId { get; set; }

        public DateTime? FechaCreacionSeion { get; set; }
    
        
        public List<CarritoDetalleDto> ListaProductos { get; set; }
    }
}
