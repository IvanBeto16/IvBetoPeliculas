namespace PL.Models
{
    public class Root
    {
        public int? page { get; set; }
        public List<Pelicula> results { get; set; }
        //public int total_pages { get; set; }
        //public int total_results { get; set; }
    }
}
