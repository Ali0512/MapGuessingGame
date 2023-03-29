using Microsoft.EntityFrameworkCore;

namespace MapProjectZiarat.Models
{
    public class Map
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Region { get; set; }
    }
}
