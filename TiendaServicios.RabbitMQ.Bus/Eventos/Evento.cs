using System;
using System.Collections.Generic;
using System.Text;

namespace TiendaServicios.RabbitMQ.Bus.Eventos
{
    public abstract class Evento
    {
        public DateTime TimepStamp { get; protected set; }

        protected Evento()
        {
            TimepStamp = DateTime.Now;
        }

    }
}
