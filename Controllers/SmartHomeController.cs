using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TrabalhoSistemas.API;
using Newtonsoft.Json.Linq;

namespace TrabalhoSistemas.Controllers
{
    [Route("home")]
    public class SmartHomeController : ControllerBase
    {
//        [HttpGet("{id}")]
//        public async Task<ActionResult> Get(int id)
//        {
//            var vaga = await MQTTConnector.ReadStorage(id);
//    
//            if (vaga.HasValue)
//            {
//                var json = JObject.FromObject(new
//                {
//                    Vaga = vaga.Value
//                });
//
//                return Ok(json);
//            }
//            else
//                return BadRequest();
//        }
//
//        [HttpGet]
//        public async Task<ActionResult> Get()
//        {
//            var vagas = await MQTTConnector.ReadStorage();
//
//            if (vagas != null)
//            {
//                var json = JObject.FromObject(new
//                {
//                    Vagas = vagas
//                });
//
//                return Ok(json);
//            }
//            else
//                return new StatusCodeResult(500);
//        }

        [HttpGet("quarto/luz")]
        public IActionResult GetLuzQuarto()
        {
            var json = JObject.FromObject(new
            {
                luz1 = MQTTConnector.Quarto.Luz1,
                luz2 = MQTTConnector.Quarto.Luz2,
                luz3 = MQTTConnector.Quarto.Luz3
            });

            return Ok(json);
        }

        [HttpGet("sala")]
        public IActionResult GetSala()
        {
            var json = JObject.FromObject(new
            {
                luz = MQTTConnector.Sala.Luz,
                televisao = MQTTConnector.Sala.Televisao,
                cortina = MQTTConnector.Sala.Cortina
            });

            return Ok(json);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var json = JObject.FromObject(new
            {
                quarto = new
                {
                    luz1 = MQTTConnector.Quarto.Luz1,
                    luz2 = MQTTConnector.Quarto.Luz2,
                    luz3 = MQTTConnector.Quarto.Luz3
                },
                sala = new
                {
                    luz = MQTTConnector.Sala.Luz,
                    televisao = MQTTConnector.Sala.Televisao,
                    cortina = MQTTConnector.Sala.Cortina
                }
            });

            return Ok(json);
        }

        [HttpPut("quarto/luz/1")]
        public IActionResult PutLuzQuarto1(bool value)
        {
            MQTTConnector.Quarto.Luz1 = value;
            return Ok();
        }
        
        [HttpPut("quarto/luz/2")]
        public IActionResult PutLuzQuarto2(bool value)
        {
            MQTTConnector.Quarto.Luz2 = value;
            return Ok();
        }
        
        [HttpPut("quarto/luz/3")]
        public IActionResult PutLuzQuarto3(float value)
        {
            if (value < 0 || value > 255)
                return StatusCode(400);
            MQTTConnector.Quarto.Luz3 = value;
            return Ok();
        }

        [HttpPut("sala/luz")]
        public IActionResult PutLuzSala(float value)
        {
            if (value < 0 || value > 255)
                return StatusCode(400);

            MQTTConnector.Sala.Luz = value;
            return Ok();
        }

        [HttpPut("sala/televisao")]
        public IActionResult PutTelevisaoSala(bool value)
        {
            MQTTConnector.Sala.Televisao = value;
            return Ok();
        }

        [HttpPut("sala/cortina")]
        public IActionResult Cortina(bool value)
        {
            MQTTConnector.Sala.Cortina = value;
            return Ok();
        }
    }
}