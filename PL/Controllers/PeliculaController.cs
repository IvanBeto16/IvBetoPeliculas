using Microsoft.AspNetCore.Mvc;
using PL.Models;

namespace PL.Controllers
{
    public class PeliculaController : Controller
    {
        [HttpGet]
        public IActionResult Peliculas(int? page)
        {
            using(var client = new HttpClient())
            {
                Root raiz = new Root();
                raiz.results = new List<Pelicula>();
                client.BaseAddress = new Uri($"https://api.themoviedb.org/3/");
                raiz.page = page == null ? 1 : page; 
                var responseTask = client.GetAsync(client.BaseAddress + $"movie/popular?api_key=71e60de3b27961855c627ef2a6dc1545&page=" + page);
                responseTask.Wait();

                string urlInicio = "https://www.themoviedb.org/t/p/w300_and_h450_bestv2";

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Root>();
                    readTask.Wait();

                    foreach(var item in readTask.Result.results)
                    {
                        //Pelicula? movie = Newtonsoft.Json.JsonConvert.DeserializeObject<Pelicula>(item.ToString());
                        Pelicula movie = new Pelicula();
                        movie.backdrop_path = item.backdrop_path;
                        movie.id = item.id;
                        movie.genre_ids = item.genre_ids;
                        movie.adult = item.adult;
                        movie.original_title = item.original_title;
                        movie.title = item.title;
                        movie.overview = item.overview;
                        movie.poster_path = urlInicio + item.poster_path;
                        movie.original_language = item.original_language;
                        movie.release_date = item.release_date;
                        movie.video = item.video;
                        movie.popularity = item.popularity;
                        movie.vote_average = item.vote_average;
                        movie.vote_count = item.vote_count;

                        raiz.results.Add(movie);
                    }
                }
                ViewBag.Page = page;
                return View(raiz);
            }
            
        }

        [HttpGet]
        public IActionResult Favorites(int idPelicula)
        {
            using( var client = new HttpClient())
            {
                //int accountId = 20522336;
                var json = new
                {
                    media_type = "movie",
                    media_id = (int)idPelicula,
                    favorite = "true"
                };

                client.BaseAddress = new Uri($"https://api.themoviedb.org/3/");
                var postTask = client.PostAsJsonAsync(client.BaseAddress + $"account/20522336/favorite?api_key=71e60de3b27961855c627ef2a6dc1545&session_id=5279334d18791080413e9b7dd825b4e275370c7f", json);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    ViewBag.Message = "Pelicula Añadida a Favoritos";
                }
                else
                {
                    ViewBag.Message = "Error al añadir pelicula a favoritos";
                }
            }
            return PartialView("Modal");
        }

        [HttpGet]
        public ActionResult GetFavorites()
        {
            using ( var client = new HttpClient())
            {
                Root raiz = new Root();
                raiz.results = new List<Pelicula>();
                client.BaseAddress = new Uri($"https://api.themoviedb.org/3/");
                var responseTask = client.GetAsync(client.BaseAddress + $"account/20522336/favorite/movies?api_key=71e60de3b27961855c627ef2a6dc1545&session_id=5279334d18791080413e9b7dd825b4e275370c7f");
                responseTask.Wait();

                string urlInicio = "https://www.themoviedb.org/t/p/w300_and_h450_bestv2";
                var result = responseTask.Result;
                if(result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Root>();
                    readTask.Wait();

                    foreach(var item in readTask.Result.results)
                    {
                        Pelicula movie = new Pelicula();
                        movie.backdrop_path = item.backdrop_path;
                        movie.id = item.id;
                        movie.genre_ids = item.genre_ids;
                        movie.adult = item.adult;
                        movie.original_title = item.original_title;
                        movie.title = item.title;
                        movie.overview = item.overview;
                        movie.poster_path = urlInicio + item.poster_path;
                        movie.original_language = item.original_language;
                        movie.release_date = item.release_date;
                        movie.video = item.video;
                        movie.popularity = item.popularity;
                        movie.vote_average = item.vote_average;
                        movie.vote_count = item.vote_count;

                        raiz.results.Add(movie);
                    }
                }
                return View(raiz);
            }
        }
    }
}
