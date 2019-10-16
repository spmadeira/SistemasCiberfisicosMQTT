using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TrabalhoSistemas.API;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace TrabalhoSistemas.Controllers
{
    [Route("vaga/")]
    public class VagaController : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var vaga = await MQTTConnector.ReadStorage(id);

            if (vaga.HasValue)
            {
                var json = JObject.FromObject(new
                {
                    vaga = vaga.Value
                });

                return Ok(json);
            }
            else
                return BadRequest();
        }

        [HttpGet]
        public async Task<ActionResult> GetObj()
        {
            var vagas = await MQTTConnector.ReadStorage();

            if (vagas != null)
            {
                var vagasJson = new List<JObject>();

                foreach (var vaga in vagas)
                {
                    var vagaJson = JObject.FromObject(new
                    {
                        vaga
                    });
                    vagasJson.Add(vagaJson);
                }

                var json = JArray.FromObject(vagasJson.ToArray());

                return Ok(json);
            }
            else
                return new StatusCodeResult(500);
        }
    }
}
