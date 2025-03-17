using Microsoft.AspNetCore.Mvc;

namespace backend.Models
{
    public class Hotel
    {
        public string? Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool IsBooked { get; set; }

         public string? NearestUser { get; set; }
    }
}
