using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiendaServicios.Api.Carrito.RemoteModel;

namespace TiendaServicios.Api.Carrito.RemoteInterface
{
    public interface ILibrosService
    {
       Task<(bool resultado, LibroRemote Libro, string ErrorMessage)>GetLibro(Guid LibroId);
   
    }
}
