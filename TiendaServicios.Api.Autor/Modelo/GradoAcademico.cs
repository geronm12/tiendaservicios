using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TiendaServicios.Api.Autor.Modelo
{
    public class GradoAcademico
    {
        public int GradoAcademicoId { get; set; }

        public string Nombre { get; set; }

        public string CentroAcademico { get; set; }

        public DateTime? FechaGrado { get; set; }
    
        public int AutorLibroId { get; set; }


        //Propiedades de Navegacion

        public AutorLibro AutorLibro { get; set; }

        public string GradoAcademicoGuid { get; set; }
   
    }
}
