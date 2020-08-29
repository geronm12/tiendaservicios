﻿using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using TiendaServicios.RabbitMQ.Bus.BusRabbit;
using TiendaServicios.RabbitMQ.Bus.Comandos;
using TiendaServicios.RabbitMQ.Bus.Eventos;

namespace TiendaServicios.RabbitMQ.Bus.Implement
{
    public class RabbitEventBus : IRabbitEventBus
    {

        private readonly IMediator _mediator;
        private readonly Dictionary<string, List<Type>> _manejadores;
        private readonly List<Type> _eventoTypos;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public RabbitEventBus(IMediator mediator, IServiceScopeFactory serviceScopeFactory)
        {
            _mediator = mediator;
            _manejadores = new Dictionary<string, List<Type>>();
            _eventoTypos = new List<Type>();
            _serviceScopeFactory = serviceScopeFactory;

        }

        public Task EnviarComando<T>(T comando) where T : Comando
        {
            return _mediator.Send(comando);
        }

        public void Publish<T>(T evento) where T : Evento
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            using (var connection =  factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var eventName = evento.GetType().Name;

                channel.QueueDeclare(eventName, false, false, false, null);

                var message = JsonConvert.SerializeObject(evento);
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish("", eventName,null, body);
                            
            }
        }

        public void Subscriber<T, TH>()
            where T : Evento
            where TH : IEventoManejador<T>
        {

            var eventoNombre = typeof(T).Name;

            var manejadorEventoTipo = typeof(TH);

            if(!_eventoTypos.Contains(typeof(T)))
            {

                _eventoTypos.Add(typeof(T));

            }

            if (!_manejadores.ContainsKey(eventoNombre))
            {
                _manejadores.Add(eventoNombre, new List<Type>());
            }

            if(_manejadores[eventoNombre].Any(x => x.GetType() == manejadorEventoTipo))
            {
                throw new ArgumentException($"El manejador {manejadorEventoTipo.Name} fue registrado anteriormente por {eventoNombre}");

            }

            _manejadores[eventoNombre].Add(manejadorEventoTipo);

            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                DispatchConsumersAsync = true
            };



            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            
            channel.QueueDeclare(eventoNombre, false, false, false,null);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.Received += Consumer_Delegate;

            channel.BasicConsume(eventoNombre, true, consumer);


        }

        private async Task Consumer_Delegate(object sender, BasicDeliverEventArgs e)
        {
            var eventName = e.RoutingKey;
            var message = Encoding.UTF8.GetString(e.Body.ToArray());

            try
            {
                if (_manejadores.ContainsKey(eventName))
                {
                    using(var scope = _serviceScopeFactory.CreateScope())
                    {
                        var subrscriptions = _manejadores[eventName];

                        foreach (var sb in subrscriptions)
                        {

                            var manejador = scope.ServiceProvider.GetService(sb);
                            //Activator.CreateInstance(sb);

                            if (manejador == null) continue;

                            var tipoEvento = _eventoTypos.SingleOrDefault(x => x.Name == eventName);
                            var eventoDS = JsonConvert.DeserializeObject(message, tipoEvento);

                            var concretoTipo = typeof(IEventoManejador<>).MakeGenericType(tipoEvento);

                            await (Task)concretoTipo.GetMethod("Handle").Invoke(manejador, new object[] { eventoDS });

                        }
                    }

                    


                }

            }catch(Exception ex)
            {


            }


        }
    }
}