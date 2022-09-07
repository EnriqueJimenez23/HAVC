using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web;

namespace CapaServicios.Servicios
{
    public class CorreoElectronico
    {
        public static void Enviar(Dictionary<string, string> configuracion, string correoDestino, string asunto, string mensaje)
        {
            // Carga la configuración del servidor de correo y la cuenta

            string emailServerHost = configuracion["EmailServerHost"];
            string emailServerEnableSsl = configuracion["EmailServerEnableSsl"];
            string emailServerPort = configuracion["EmailServerPort"];
            string emailAddress = configuracion["EmailAddress"];
            string emailAccountUserName = configuracion["EmailAccountUserName"];
            string emailAccountPassword = configuracion["EmailAccountPassword"];
            string emailDisplayName = configuracion["EmailDisplayName"];
            
            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress(emailAddress, emailDisplayName)
            };

            foreach (var correo in correoDestino.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
            {
                mailMessage.To.Add(correo);
            }

            mailMessage.Subject = asunto;
            mailMessage.IsBodyHtml = false;
            mailMessage.Body = mensaje;
            mailMessage.Priority = MailPriority.Normal;

            SmtpClient smtpClient = new SmtpClient(emailServerHost)
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Port = int.Parse(emailServerPort),
                Credentials = new NetworkCredential(emailAccountUserName, emailAccountPassword),
                EnableSsl = Convert.ToBoolean(emailServerEnableSsl)
            };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            smtpClient.Send(mailMessage); 
        }

        public static void EnviarHtml(Dictionary<string, string> configuracion, string correoDestino, string asunto, string mensaje)
        {
            // Carga la configuración del servidor de correo y la cuenta

            string emailServerHost = configuracion["EmailServerHost"];
            string emailServerEnableSsl = configuracion["EmailServerEnableSsl"];
            string emailServerPort = configuracion["EmailServerPort"];
            string emailAddress = configuracion["EmailAddress"];
            string emailAccountUserName = configuracion["EmailAccountUserName"];
            string emailAccountPassword = configuracion["EmailAccountPassword"];
            string emailDisplayName = configuracion["EmailDisplayName"];

            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress(emailAddress, emailDisplayName)
            };

            foreach (var correo in correoDestino.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
            {
                mailMessage.To.Add(correo);
            }

            mailMessage.Subject = asunto;
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = mensaje;
            mailMessage.Priority = MailPriority.Normal;

            SmtpClient smtpClient = new SmtpClient(emailServerHost)
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Port = int.Parse(emailServerPort),
                Credentials = new NetworkCredential(emailAccountUserName, emailAccountPassword),
                EnableSsl = Convert.ToBoolean(emailServerEnableSsl)
            };

            smtpClient.Send(mailMessage);
        }

    }
}
