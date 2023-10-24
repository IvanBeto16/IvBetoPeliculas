using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CineController : ControllerBase
    {
        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAllCines()
        {
            ML.Result result = BL.Cine.GetAll();
            if (result.Correct)
            {
                return StatusCode(200, result);
            }
            else
            {
                return StatusCode(400, result);
            }

        }

        [HttpGet]
        [Route("GetById/{idCine}")]
        public IActionResult GetById(int idCine)
        {
            ML.Result result = BL.Cine.GetById(idCine);
            if (result.Correct)
            {
                return StatusCode(200, result);
            }
            else
            {
                return StatusCode(400, result);
            }
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult Add([FromBody]ML.Cine cine)
        {
            ML.Result result = BL.Cine.Add(cine);
            if (result.Correct)
            {
                return StatusCode(200, result);
            }
            else
            {
                return StatusCode(400, result);
            }

        }

        [HttpPut]
        [Route("Update")]
        public IActionResult Update([FromBody]ML.Cine cine)
        {
            ML.Result result = BL.Cine.Update(cine);
            if (result.Correct)
            {
                return StatusCode(200, result);
            }
            else
            {
                return StatusCode(400, result);
            }
        }

        [HttpDelete]
        [Route("Delete/{idCine}")]
        public IActionResult Delete(int idCine)
        {
            ML.Result result = BL.Cine.Delete(idCine);
            if (result.Correct)
            {
                return StatusCode(200, result);
            }
            else
            {
                return StatusCode(400, result);
            }
        }
    }
}
