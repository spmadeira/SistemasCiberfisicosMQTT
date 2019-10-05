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
        public async Task<ActionResult<bool>> Get(int id)
        {
            var vaga = await MQTTConnector.ReadStorage(id);
    
            if (vaga.HasValue)
            {
                var json = JObject.FromObject(new
                {
                    Vaga = vaga.Value.ToString()
                });

                return Ok(json);
            }
            else
                return BadRequest();
        }
    }
}