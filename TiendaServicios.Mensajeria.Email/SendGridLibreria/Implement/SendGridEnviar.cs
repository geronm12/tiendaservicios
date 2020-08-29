using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;
using TiendaServicios.Mensajeria.Email.SendGridLibreria.Interface;
using TiendaServicios.Mensajeria.Email.SendGridLibreria.Modelo;

namespace TiendaServicios.Mensajeria.Email.SendGridLibreria.Implement
{
    public class SendGridEnviar : ISendGridEnviar
    {
        public async Task<(bool result, string errorMessage)> EnviarEmail(SendGridData data)
        {
            try
            {
                var sendGridCliente = new SendGridClient(data.SendGridApiKey);
                var destinatario = new EmailAddress(data.EmailDestinatario, data.NombreDestinatario);
                var titulo = data.Titulo;
                var sender = new EmailAddress("gerolpz01@gmail.com", "Gero");
                var contenido = data.Contenido;


                var objMensaje = MailHelper.CreateSingleEmail(sender, destinatario, titulo, contenido, contenido);

                await sendGridCliente.SendEmailAsync(objMensaje);

                return (true, null);
            }catch(Exception e)
            {
                
                
                
                return (false, e.Message);



            }
        
        }
    }
}
