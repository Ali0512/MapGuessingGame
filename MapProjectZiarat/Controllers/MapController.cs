using MapProjectZiarat.Data;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System;

namespace MapProjectZiarat.Controllers
{
    public class MapController : Controller
    {
        
        private AppDbContext _context;
        public MapController(AppDbContext context)
        {
            _context= context;
        }
        public IActionResult Index()
        {
            var regions = new[] { "Asia", "Africa", "Europe", "North America", "South America", "Australia" };
            var maxLongitude = 77.0;
            var minLongitude = 60.5;
            var maxLatitude = 37.1;
            var minLatitude = 23.7;
            var random = new Random();
            var longitude = random.NextDouble() * (maxLongitude - minLongitude) + minLongitude;
            var latitude = random.NextDouble() * (maxLatitude - minLatitude) + minLatitude;
            var region = regions[random.Next(0, regions.Length)];

            // Pass the random coordinates and region to the view
            ViewBag.Longitude = longitude;
            ViewBag.Latitude = latitude;
            ViewBag.Region = region;

            return View();
        }

        [HttpPost]
        public IActionResult CheckGuess(double guessLongitude, double guessLatitude, double actualLongitude, double actualLatitude, string actualRegion)
        {
            var distance = CalculateDistance(guessLatitude, guessLongitude, actualLatitude, actualLongitude);

            // Get the actual location name from the database based on the actual latitude and longitude
            var actualLocation = _context.maps.FirstOrDefault(m => m.Latitude == actualLatitude && m.Longitude == actualLongitude);
            var actualLocationName = actualLocation?.Name ?? "Unknown location";

            if (distance < 50)
            {
                // Calculate the score based on the distance and time taken to make the guess
                var score = CalculateScore(distance, 0);

                // Redirect to the GuessResult action with the score and actualLocationName parameters
                return RedirectToAction("GuessResult", new { score = score, actualLocationName = actualLocationName });
            }
            else
            {
                return Content($"Sorry, your guess was {distance}km away from the actual location ({actualLocationName}). Hint: {actualRegion}");
            }
        }


        public IActionResult GuessResult(string locationName, int score)
        {
            ViewBag.LocationName = locationName;
            ViewBag.Score = score;

            return View();
        }
        public IActionResult Score(int score, string location)
        {
            ViewBag.Score = score;
            ViewBag.Location = location;
            return View();
        }


        private int CalculateScore(double distance, int timeTakenInSeconds)
        {
            // Calculate the score based on the distance and time taken to make the guess
            var baseScore = 10000;
            var distanceScore = (int)(baseScore - distance * 200);
            var timeScore = (int)(baseScore - timeTakenInSeconds * 100);
            var score = distanceScore + timeScore;

            return score;
        }


        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            var R = 6371; // Radius of the earth in km
            var dLat = deg2rad(lat2 - lat1); // deg2rad below
            var dLon = deg2rad(lon2 - lon1);
            var a =
                Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2)
                ;
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c; // Distance in km
            return d;
        }

        private double deg2rad(double deg)
        {
            return deg * (Math.PI / 180);
        }

    }
}
