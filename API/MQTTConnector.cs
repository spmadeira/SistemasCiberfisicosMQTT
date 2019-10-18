using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Server;
using Newtonsoft.Json;

namespace TrabalhoSistemas.API
{
    public static class MQTTConnector
    {
        private static readonly (string, int) Info = ("broker.hivemq.com", 1883);
        public static IMqttClient Client;
        public static Quarto Quarto { get; set; }
        public static Sala Sala { get; set; }

        public static async Task<IMqttClient> Start()
        {
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer(Info.Item1, Info.Item2)
                .Build();

            var client = new MqttFactory().CreateMqttClient();
            await client.ConnectAsync(options);
            

            Quarto = new Quarto();
            Sala = new Sala();
            Client = client;
            Console.WriteLine("Connected.");

//            Console.WriteLine("Conectado");
//            for (int i = 1; i <= NumeroDeVagas; i++)
//            {
//                await Client.SubscribeAsync(new TopicFilterBuilder().WithTopic($"sistemas_ciberfisicos_20192/vaga/{i}").Build());
//                Console.WriteLine($"Inscrito a vaga {i}.");
//            }

            Client.UseApplicationMessageReceivedHandler(async e =>
            {
                var parsedPayload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                var topic = e.ApplicationMessage.Topic;
                Console.WriteLine($"Recebido:\n  Tópico: {topic}\n    Payload: {parsedPayload}");
                await ParseOperation(topic, parsedPayload);
            });

            return client;
        }

        public static async Task Stop()
        {
            await Client.DisconnectAsync();
        }

        public async static Task ParseOperation(string topic, string payload)
        {
//            var splitStr = topic.Split("vaga");
//            var cleanedStr = splitStr[1].Remove(0, 1);
//            int.TryParse(cleanedStr, out int vaga);
//            var result = int.TryParse(payload, out int estado);
//            
//            if (!result)
//                return;
//
//            var parsedEstado = estado == 1 ? true : false;
//            await UpdateStorage(vaga, parsedEstado);
        }
    }

    public class Quarto
    {
        public bool Luz1 { get; set; }
        public bool Luz2 { get; set; }
        public float Luz3 { get; set; }

        public Quarto()
        {
            Luz1 = false;
            Luz2 = false;
            Luz3 = 0f;
        }
    }

    public class Sala
    {
        public float Luz { get; set; }
        public bool Televisao { get; set; }
        public bool Cortina { get; set; }

        public Sala()
        {
            Luz = 0;
            Televisao = false;
            Cortina = false;
        }
    }
}