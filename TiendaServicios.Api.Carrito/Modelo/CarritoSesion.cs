using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TiendaServicios.Api.Carrito.Modelo
{
    public class CarritoSesion
    {
        public int CarritoSesionId { get; set; }

        public DateTime? FechaCreacion { get; set; }

        public ICollection<CarritoSesionDetalle> Detalles { get; set; }

    }
}
