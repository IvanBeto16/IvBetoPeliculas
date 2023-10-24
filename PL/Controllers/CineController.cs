using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class CineController : Controller
    {
        [HttpGet]
        public IActionResult GetAllCines()
        {
            ML.Cine cine = new ML.Cine();
            cine.Zona = new ML.Zona();
            cine.Cines = new List<object>();

            //ML.Result result = BL.Cine.GetAll();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5242/api/Cine" + $"/GetAll");
                var responseTask = client.GetAsync(client.BaseAddress);

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ML.Result>();
                    readTask.Wait();
                    foreach (var item in readTask.Result.Objects)
                    {
                        ML.Cine axiliar = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Cine>(item.ToString());
                        cine.Cines.Add(axiliar);
                    }
                }
                else
                {
                    ViewBag.Message = "Se ha producido un error, no hay cines registrados";
                }
                return View(cine);
            }
        }

        [HttpGet]
        public ActionResult Form(int? idCine)
        {
            ML.Cine cine = new ML.Cine();
            cine.Zona = new ML.Zona();

            ML.Result resultZona = BL.Zona.GetAll();
            if (idCine == 0 || idCine == null) //Add
            {
                cine.Zona.Zonas = resultZona.Objects.ToList();
            }
            else  //Update
            {
                ML.Result result = BL.Cine.GetById(idCine.Value);
                if (result.Correct)
                {
                    cine = (ML.Cine)result.Object;
                    cine.Zona.Zonas = resultZona.Objects.ToList();
                }
            }
            return View(cine);
        }

        public ActionResult Form(ML.Cine cine)
        {
            //cine.Zona.Zonas = new List<object>();
            //cine.Zona.Zonas.Add(null);
            //cine.Cines = new List<object>();
            //cine.Cines.Add(null);
            if (cine.IdCine == 0)  //Add
            {
                //if (cine.Zona.IdZona == 1)
                //{
                //    cine.Zona.Nombre = "Norte";
                //}
                //if (cine.Zona.IdZona == 2)
                //{
                //    cine.Zona.Nombre = "Sur";
                //}
                //if (cine.Zona.IdZona == 3)
                //{
                //    cine.Zona.Nombre = "Este";
                //}
                //if (cine.Zona.IdZona == 4)
                //{
                //    cine.Zona.Nombre = "Oeste";
                //}
                //ML.Result result = BL.Cine.Add(cine);
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"http://localhost:5242/api/Cine");
                    var responseTask = client.PostAsJsonAsync<ML.Cine>(client.BaseAddress + $"/Add/", cine);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        ViewBag.Message = "Se ha agregado la sucursal correctamente";
                    }
                    else
                    {
                        ViewBag.Message = "No se ha agregado la sucursal, ha ocurrido un error";
                    }
                }

            }
            else        //Update
            {
                //ML.Result result = BL.Cine.Update(cine);
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"http://localhost:5242/api/Cine");
                    var updateTask = client.PutAsJsonAsync<ML.Cine>(client.BaseAddress + $"/Update/", cine);
                    updateTask.Wait();

                    var result = updateTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        ViewBag.Message = "Se ha actualizado la información de la sucursal";
                    }
                    else
                    {
                        ViewBag.Message = "No se ha podido actualizar la información, ha ocurrido un error";
                    }
                }
            }
            return PartialView("Modal");
        }

        public IActionResult Delete(int? idCine)
        {
            ML.Cine cine = new ML.Cine();
            cine.IdCine = idCine.Value;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"http://localhost:5242/api/Cine");
                var responseTask = client.DeleteAsync(client.BaseAddress + "/Delete/" + cine.IdCine);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    ViewBag.Message = "Se ha eliminado correctamente la sucursal de Cine";
                }
                else
                {
                    ViewBag.Message = "Ha ocurrido un error al eliminar la sucursal";
                }
            }
            return PartialView("Modal");
        }

        public IActionResult Estadisticas()
        {
            ML.Cine cine = new ML.Cine();
            cine.Cines = new List<object>();
            using( var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"http://localhost:5242/api/Cine");
                var responseTask = client.GetAsync(client.BaseAddress + $"/GetAll");
                responseTask.Wait();
                var result = responseTask.Result;
                if(result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ML.Result>();
                    readTask.Wait();
                    foreach (var item in readTask.Result.Objects)
                    {
                        ML.Cine axiliar = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Cine>(item.ToString());
                        cine.Cines.Add(axiliar);
                    }
                }
                else
                {
                    ViewBag.Message = "Se ha producido un error, no hay cines registrados";
                }
                return View(cine);
            }
        }

        
    }
}
