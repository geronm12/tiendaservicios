using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TiendaServicios.Mensajeria.Email.SendGridLibreria.Interface;
using TiendaServicios.Mensajeria.Email.SendGridLibreria.Modelo;
using TiendaServicios.RabbitMQ.Bus.BusRabbit;
using TiendaServicios.RabbitMQ.Bus.EventoQueue;

namespace TiendaServicios.Api.Autor.ManejadorRabbit
{
    public class EmailEventoManejador : IEventoManejador<EmailEventoQueue>
    {
        private readonly ILogger<EmailEventoManejador> _logger;

        private readonly ISendGridEnviar _sendGrid;

        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;
        public EmailEventoManejador()
        {

        }

        public EmailEventoManejador(ILogger<EmailEventoManejador> logger, 
            ISendGridEnviar sendGrid, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            _logger = logger;

            _sendGrid = sendGrid;

            _configuration = configuration;

        }

        public async Task Handle(EmailEventoQueue @event)
        {
            _logger.LogInformation($"Este es el valor que consumo desde rabbitMQ {@event.Titulo}");

            var objData = new SendGridData();
            objData.Contenido = @event.Contenido;
            objData.EmailDestinatario = @event.Destinatario;
            objData.NombreDestinatario = @event.Destinatario;
            objData.Titulo = @event.Titulo;
            objData.SendGridApiKey = _configuration["SendGrid:ApiKey"];


            var result = await _sendGrid.EnviarEmail(objData);

            if (result.result)
            {
                await Task.CompletedTask;
                return;
            }

            

        }
    }
}
