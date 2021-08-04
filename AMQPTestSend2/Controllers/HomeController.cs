using AMQPTestSend2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMQPTestSend2.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ProcessMessage(Client client)
        {
            var factory = new ConnectionFactory { HostName = "localhost"};
            using (var connection = factory.CreateConnection())
            {
                using var channel = connection.CreateModel();
                string clientSend = client.Queu;
                channel.QueueDeclare(clientSend, false, false, false, null);
                byte[] body = Encoding.UTF8.GetBytes(client.Text);

                channel.BasicPublish("", clientSend, null, body);

            }
            return View("Index");
        }
    }

}
