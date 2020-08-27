using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Gateway.LibroRemote;

namespace TiendaServicios.Api.Gateway.MessageHandler
{
    public class LibroHandler : DelegatingHandler
    {
        private readonly ILogger<LibroHandler> _logger;

        public LibroHandler(ILogger<LibroHandler> logger)
        {

            _logger = logger;


        }




        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {

            var tiempo = Stopwatch.StartNew();
            _logger.LogInformation("Inicia el request");

            var response = await base.SendAsync(request, cancellationToken); 

            if(response.IsSuccessStatusCode)
            {
                var contenido = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions{PropertyNameCaseInsensitive = true};

                var resultado = JsonSerializer.Deserialize<LibroModeloRemote>(contenido, options);
                
            }



            _logger.LogInformation($"Este proceso se hizo en {tiempo.ElapsedMilliseconds}ms");

            return response;
 

        }


    }
}
