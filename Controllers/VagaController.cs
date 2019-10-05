using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TrabalhoSistemas.API;
using Newtonsoft.Json.Linq;

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
                    Vaga = vaga.Value
                });

                return Ok(json);
            }
            else
                return BadRequest();
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var vagas = await MQTTConnector.ReadStorage();

            if (vagas != null)
            {
                var json = JObject.FromObject(new
                {
                    Vagas = vagas
                });

                return Ok(json);
            }
            else
                return new StatusCodeResult(500);
        }
    }
}