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
        private const string FileName = "vagasmqtt.json";
        private const int NumeroDeVagas = 30;
        private static string FullFileName => $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}{Path.PathSeparator}{FileName}";
        public static IMqttClient Client;

        public static async Task<IMqttClient> Start()
        {
            SetupStorage();

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer(Info.Item1, Info.Item2)
                .Build();

            var client = new MqttFactory().CreateMqttClient();
            await client.ConnectAsync(options);
            

            Client = client;
            Console.WriteLine("Connected.");

            Console.WriteLine("Conectado");
            for (int i = 1; i <= NumeroDeVagas; i++)
            {
                await Client.SubscribeAsync(new TopicFilterBuilder().WithTopic($"sistemas_ciberfisicos_20192/vaga/{i}").Build());
                Console.WriteLine($"Inscrito a vaga {i}.");
            }

            Client.UseApplicationMessageReceivedHandler(async e =>
            {
                var parsedPayload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                var topic = e.ApplicationMessage.Topic;
                Console.WriteLine($"Recebido:\n  Tópico: {topic}\n    Payload: {parsedPayload}");
                await ParseOperation(topic, parsedPayload);
            });

            return client;
        }

        private static void SetupStorage()
        {
            if (!File.Exists(FullFileName))
            {
                var vagas = new bool[NumeroDeVagas];
                for (int i = 0; i<NumeroDeVagas; i++)
                {
                    vagas[i] = false;
                }
                var json = JsonConvert.SerializeObject(vagas, Formatting.Indented);

                Console.WriteLine($"Criado armazenamento em {FullFileName}");
                File.WriteAllText(FullFileName, json);
            }
        }

        public async static Task<bool> UpdateStorage(int index, bool value)
        {
            if (index >= NumeroDeVagas)
                return false;

            var json = await File.ReadAllTextAsync(FullFileName);
            var vagas = JsonConvert.DeserializeObject<bool[]>(json);

            try
            {
                vagas[index - 1] = value;
                var newJson = JsonConvert.SerializeObject(vagas, Formatting.Indented);
                await File.WriteAllTextAsync(FullFileName, newJson);
            } catch
            {
                return false;
            }

            return true;
        }

        public async static Task<bool?> ReadStorage(int index)
        {
            if (index >= NumeroDeVagas)
                return null;

            var json = await File.ReadAllTextAsync(FullFileName);
            var vagas = JsonConvert.DeserializeObject<bool[]>(json);

            try
            {
                return vagas[index - 1];
            }
            catch
            {
                return null;
            }
        }

        public async static Task<bool[]> ReadStorage()
        {
            var json = await File.ReadAllTextAsync(FullFileName);
            var vagas = JsonConvert.DeserializeObject<bool[]>(json);

            try
            {
                return vagas;
            }
            catch
            {
                return null;
            }
        }

        public static async Task Stop()
        {
            await Client.DisconnectAsync();
        }

        public async static Task ParseOperation(string topic, string payload)
        {
            var splitStr = topic.Split("vaga");
            var cleanedStr = splitStr[1].Remove(0, 1);
            int.TryParse(cleanedStr, out int vaga);
            var result = int.TryParse(payload, out int estado);
            
            if (!result)
                return;

            var parsedEstado = estado == 1 ? true : false;
            await UpdateStorage(vaga, parsedEstado);
        }
    }
}
