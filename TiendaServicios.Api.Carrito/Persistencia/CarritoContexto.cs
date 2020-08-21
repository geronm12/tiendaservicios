using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiendaServicios.Api.Carrito.Modelo;

namespace TiendaServicios.Api.Carrito.Persistencia
{
    public class CarritoContexto: DbContext
    {

        public CarritoContexto()
        {
                
        }

        public CarritoContexto(DbContextOptions<CarritoContexto> options): base(options)
        {

        }

       public virtual DbSet<CarritoSesion> CarritoSesion { get; set; }

       public virtual DbSet<CarritoSesionDetalle> CarritoSesionDetalle { get; set; }

    }
}
