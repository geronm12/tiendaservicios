﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TiendaServicios.Mensajeria.Email.SendGridLibreria.Modelo
{
    public class SendGridData
    {

        public string SendGridApiKey { get; set; }

        public string EmailDestinatario { get; set; }

        public string NombreDestinatario { get; set; }

        public string Titulo { get; set; }

        public string Contenido { get; set; }

    }

}
